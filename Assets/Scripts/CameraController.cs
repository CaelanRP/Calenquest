using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public Vector2 offset;
	public bool trackX = true;
	public bool trackY = true;
	public float trackingSpeedX, trackingSpeedY;
	public Vector2 target;
	public GameObject trackingObject;
	public Calen trackingCalen;
	private static Camera cam;
	public Vector3 initPos;
	public bool lockedOnTarget;
	public bool trackingActive = true;

	public Vector2 minPos, maxPos;

	public static CameraController instance;
	// Use this for initialization
	void Awake(){
		instance = this;
		cam = Camera.main;
		initPos = transform.position;
		target = initPos;
	}
	void Start () {
		trackingCalen = GameObject.FindObjectOfType<Calen>();
		trackingObject = trackingCalen.gameObject;

	}
	
	// Update is called once per frame
	void Update () {
		if (!lockedOnTarget && trackingActive){
			UpdateTarget();
		}

		/* 
		target.x = Mathf.Max(Mathf.Min(target.x, maxPos.x), minPos.x);
		target.y = Mathf.Max(Mathf.Min(target.y, maxPos.y), minPos.y);
		*/
		float x = transform.position.x;
		float y = transform.position.y;
		if (trackX){
			x = Mathf.Lerp(transform.position.x, target.x, trackingSpeedX * Time.deltaTime);
		}
		if (trackY){
			y = Mathf.Lerp(transform.position.y, target.y, trackingSpeedY * Time.deltaTime);
		}

		transform.position = new Vector3(x, y, -10);
	}

	void UpdateTarget(){
		if (trackingCalen){
			target = (Vector2)trackingCalen.GetCameraSpot();
		}
		else{
			target = (Vector2)trackingObject.transform.position;
		}
		target += offset;
	}

	public void LockOnTarget(Vector2 pos){
		lockedOnTarget = true;
		target = pos;
	}

	public void UnlockTarget(){
		print("unlocking camera.");
		lockedOnTarget = false;
	}

	public static Camera GetCamera(){
		return cam;
	}
}
