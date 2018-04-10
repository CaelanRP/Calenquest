using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animSpeed : MonoBehaviour {
	public float speed;
	private Animator anim;
	// Use this for initialization
	void Awake () {
		anim = GetComponent<Animator>();
		if (anim){
			anim.speed = speed;
		}
	}
}
