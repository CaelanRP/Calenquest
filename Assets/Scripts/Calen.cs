using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using TMPro;

public class Calen : MonoBehaviour {

	public CalenStats stats;
	public CalenVisualStats aesthetics;
	private Rigidbody2D rb;
	public Collider2D body;
	public LayerMask terrainLayers;
	public Transform groundRaycastPos, shellSpawnPos, bulletSpawnPos;
	private bool jumping, canWalk;

	public bool jumpEnabled, hasGun;
	public bool active = true;

	public SpriteRenderer exclamation;
	public GameObject bullet, muzzleFlash, shotgunShell;
	public static Rewired.Player player;
	public LayerMask bulletHitMask;
	public Sprite normalSprite, gunSprite;
	private SpriteRenderer sr;
	private int currentBullets;
	// Use this for initialization
	void Awake(){
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
	}
	void Start () {
		player = ReInput.players.GetPlayer(0);
		currentBullets = stats.numBullets;

		StartCoroutine(Reload());
	}
	
	// Update is called once per frame
	void Update () {
		if (controlsActive){
			HandleInput();
		}
		UpdateSprite();
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
			return !CutsceneManager.busy && active;
		}
	}

	void UpdateSprite(){
		sr.sprite = hasGun ? gunSprite : normalSprite;
		sr.color = Color.Lerp(sr.color, Color.white, 8 * Time.deltaTime);
	}

	public void UpdateGravity(){
		rb.gravityScale = stats.defaultGrav;
	}

	// You can only use horizontal force on the ground.
	// But it's not mega consistent, so you have a few frames of grace period.

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
		if (jumpEnabled && grounded && player.GetButtonDown("Jump")){
			Jump();
			Debug.Log("Jumping");
		}
		if (hasGun){
			TestShoot();
		}
		TestInteract();
	}

	public void SetJumpEnabled(){
		jumpEnabled = true;
	}

	public void TestShoot(){
		if (player.GetButtonDown("Fire") && currentBullets > 0){
			CancelHorizontalVelocity();
			rb.AddForce(-transform.right * stats.shotgunKnockback, ForceMode2D.Impulse);
			if (touchingGround){
				rb.AddForce(transform.up * stats.shotgunKnockbackUp, ForceMode2D.Impulse);
			}

			currentBullets = Mathf.Max (currentBullets - 1, 0);

			StartCoroutine(ShootFX());
		}
	}

	public void SpawnBullets(){

	}

	void CancelHorizontalVelocity(){
		Vector2 locVel = transform.InverseTransformDirection(rb.velocity);
   		locVel.x = 0;
   		rb.velocity = transform.TransformDirection(locVel);
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

	IEnumerator ShootFX(){
		AudioManager.source.PlayOneShot(aesthetics.shotgunShot);
		muzzleFlash.SetActive(true);
		yield return new WaitForSeconds(0.1f);
		muzzleFlash.SetActive(false);
	}

	IEnumerator Reload(){
		while(true){
			if (currentBullets < stats.numBullets){
				yield return new WaitForSeconds(stats.reloadTime);
				LoadBullet();
			}
			yield return null;
		}
	}

	void LoadBullet(){
		currentBullets = Mathf.Min(currentBullets + 1, stats.numBullets);
		var shell = Instantiate(shotgunShell, shellSpawnPos.position, transform.rotation);
		AudioManager.source.PlayOneShot(aesthetics.shotgunReload);
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

	//Raycast down and find the ground spot to track, otherwise just use my position
	public Vector3 GetCameraSpot(){
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 4, terrainLayers);
		if (hit && hit.collider){
			return hit.point;
		}
		return transform.position;
	}
	[ContextMenu("Take Damage")]
	public void TakeDamageFake(int damage = 1){
		sr.color = Color.red;
		StartCoroutine(SpawnDamageNum(damage));
	}

	IEnumerator SpawnDamageNum(int damage){
		GameObject obj = Instantiate(aesthetics.damageNumPrefab, transform.position + (Vector3)aesthetics.damageNumOffset, Quaternion.identity);
		obj.GetComponentInChildren<TextMeshProUGUI>().text = ("-"+damage);
		float time = 0f;
		while (time < aesthetics.damageNumTime){
			time += Time.deltaTime;
			obj.transform.Translate((Vector2.up * aesthetics.damageNumDistance * Time.deltaTime) / aesthetics.damageNumTime);
			yield return null;
		}
		Destroy(obj);
	}
}
