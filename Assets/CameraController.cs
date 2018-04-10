﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public Vector2 offset;
	public float trackingSpeedX, trackingSpeedY;
	public Vector2 target;
	public GameObject trackingObject;
	private static Camera cam;
	public Vector3 initPos;
	// Use this for initialization
	void Awake(){
		cam = Camera.main;
		initPos = transform.position;
	}
	void Start () {
		trackingObject = GameObject.FindObjectOfType<Calen>().gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		target = (Vector2)trackingObject.transform.position + offset;
		float x = Mathf.Lerp(transform.position.x, target.x, trackingSpeedX * Time.deltaTime);
		float y = Mathf.Lerp(transform.position.y, target.y, trackingSpeedY * Time.deltaTime);

		transform.position = new Vector3(x, y, -10);
	}

	public static Camera GetCamera(){
		return cam;
	}
}