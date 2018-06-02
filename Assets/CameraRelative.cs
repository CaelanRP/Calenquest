using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRelative : MonoBehaviour {

	private Vector3 offset;
	void Awake () {
		offset = transform.position - CameraController.GetCamera().transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = CameraController.GetCamera().transform.position + offset;
	}
}
