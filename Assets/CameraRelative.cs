using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRelative : MonoBehaviour {

	private Vector3 offset;
	public bool lockX = true;
	public bool lockY = true;
	void Awake () {
		offset = transform.position - CameraController.GetCamera().transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = CameraController.GetCamera().transform.position + offset;
		float x = lockX ? pos.x : transform.position.x;
		float y = lockY ? pos.y : transform.position.y;

		transform.position = new Vector3(x,y,transform.position.z);
	}
}
