﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Calen : MonoBehaviour {

	public CalenStats stats;
	private Rigidbody2D rb;
	public Collider2D body;
	public LayerMask terrainLayers;
	public Transform groundRaycastPos;
	private bool jumping;

	public bool jumpEnabled;

	public SpriteRenderer exclamation;

	public static Rewired.Player player;
	// Use this for initialization
	void Awake(){
		rb = GetComponent<Rigidbody2D>();
	}
	void Start () {
		player = ReInput.players.GetPlayer(0);
	}
	
	// Update is called once per frame
	void Update () {
		if (controlsActive){
			HandleInput();
		}
	}

	void FixedUpdate(){
		UpdateGravity();
		UpdateDrag();
		UpdateLock();
		rb.angularVelocity = Mathf.Clamp(rb.angularVelocity,-stats.maxTorque,stats.maxTorque);
		if (controlsActive){
			HandleInputFixed();
		}
	}

	public bool controlsActive{
		get{
			return !CutsceneManager.busy;
		}
	}

	public void UpdateGravity(){
		rb.gravityScale = stats.defaultGrav;
	}

	public void UpdateDrag(){
		if (grounded || touchingGround){
			rb.drag = stats.defaultDrag;
		}
		else{
			rb.drag = stats.airDrag;
		}
	}

	public void UpdateLock(){
		if (CutsceneManager.busy){
			rb.constraints = RigidbodyConstraints2D.FreezePositionX;
		}
		else{
			rb.constraints = RigidbodyConstraints2D.None;
		}
	}

	public void HandleInput(){
		if (grounded && player.GetButtonDown("Jump")){
			Jump();
			Debug.Log("Jumping");
		}
		TestInteract();
	}

	public void HandleInputFixed(){
		if (!strafing){
			rb.AddForce(Vector2.right * player.GetAxis("MoveHorizontal") * stats.walkForceGrounded);
		}
		if (touchingGround){
			rb.AddTorque(player.GetAxis("MoveHorizontal") * -stats.rotateForceGround);
		}
		else{
			rb.AddTorque(player.GetAxis("MoveHorizontal") * -stats.rotateForceAir);
		}
		rb.angularVelocity = Mathf.Clamp(rb.angularVelocity,-stats.maxTorque,stats.maxTorque);
	}

	public void Jump(){
		if (!jumping){
			StartCoroutine(JumpRoutine());
		}
	}

	void TestInteract(){
		Triggerable trigger = ActiveTrigger();
		if (trigger != null){
			if (player.GetButtonDown("Interact")){
				trigger.Trigger();
			}
		}
		exclamation.enabled = (trigger != null) && !CutsceneManager.busy;
	}

	// Slow method, don't call this every frame
	Triggerable ActiveTrigger(){
		Physics2D.queriesHitTriggers = true;
		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, stats.interactionRange, Vector2.up, 0.01f);
		if (hits.Length > 0){
			RaycastHit2D closestHit = hits[0];
			for(int i = 0; i < hits.Length; i++){
				Triggerable c = hits[i].collider.gameObject.GetComponent<Triggerable>();
				if (c && ((hits[i].distance < closestHit.distance) || (closestHit.collider.gameObject.GetComponent<Cutscene>() == null))) {
					closestHit = hits[i];
				}
			}
			return closestHit.collider.gameObject.GetComponent<Triggerable>();
		}
		return null;
	}

	IEnumerator JumpRoutine(){
		jumping = true;
		float timer = stats.bigJumpDelay;
		while (timer > 0){
			yield return null;
			timer -= Time.deltaTime;
			if (!Input.GetButton("Jump")){
				rb.AddForce(transform.up * stats.jumpForce, ForceMode2D.Impulse);
				jumping = false;
				yield break;
			}
		}
		rb.AddForce(transform.up * stats.jumpForceBig, ForceMode2D.Impulse);
		jumping = false;
	}

	public bool strafing{
		get{
			return Input.GetButton("Strafe");
		}
	}

	public bool grounded{
		get{
			
			RaycastHit2D hit = Physics2D.Raycast(groundRaycastPos.position, -transform.up, stats.groundRaycastDistance, terrainLayers);
			Debug.DrawLine(groundRaycastPos.position,groundRaycastPos.position + (-transform.up * stats.groundRaycastDistance),Color.red);
			if (hit.collider && Vector2.Angle(hit.normal,transform.up) < stats.maxJumpAngle){
				return true;
			}
			return false;
		}
	}

	public bool touchingGround{
		get{
			return body.IsTouchingLayers(terrainLayers);
		}
	}
}
