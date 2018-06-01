using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calen : MonoBehaviour {

	public CalenStats stats;
	private Rigidbody2D rb;
	public Collider2D body;
	public LayerMask terrainLayers;
	public Transform groundRaycastPos;
	public bool jumping;
	// Use this for initialization
	void Awake(){
		rb = GetComponent<Rigidbody2D>();
	}
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		HandleInput();
	}

	void FixedUpdate(){
		UpdateGravity();
		UpdateDrag();
		rb.AddTorque(Input.GetAxis("Horizontal") * -stats.rotateForceAir);
		rb.angularVelocity = Mathf.Clamp(rb.angularVelocity,-stats.maxTorque,stats.maxTorque);
		HandleInputFixed();
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

	public void HandleInput(){
		if (grounded && Input.GetButtonDown("Jump")){
			Jump();
			Debug.Log("Jumping");
		}

		TestInteract();
	}

	public void HandleInputFixed(){
		if (!strafing){
			rb.AddForce(Vector2.right * Input.GetAxis("Horizontal") * stats.walkForceGrounded);
		}
		if (touchingGround){
			rb.AddTorque(Input.GetAxis("Horizontal") * -stats.rotateForceGround);
		}
		else{
			rb.AddTorque(Input.GetAxis("Horizontal") * -stats.rotateForceAir);
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
			if (Input.GetButtonDown("Interact")){
				trigger.Trigger();
			}
		}
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
