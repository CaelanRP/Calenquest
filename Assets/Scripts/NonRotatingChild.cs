using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonRotatingChild : MonoBehaviour {

	private Quaternion initRot;
	private Vector3 initOffset;

	public float initialRotation;
	// Use this for initialization
	void Awake () {
		if (!transform.parent){
			this.enabled = false;
			return;
		}
		transform.eulerAngles = new Vector3(0,0,initialRotation);
		initRot = transform.rotation;
		initOffset = transform.position - transform.parent.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = initRot;
		if (transform.parent){
			transform.position = transform.parent.position + initOffset;
		}
	}
}
