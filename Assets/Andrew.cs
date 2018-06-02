using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Andrew : MonoBehaviour {
	private Animator anim;

	void Awake(){
		anim = GetComponent<Animator>();
	}
	public void DestroySelf(){
		Destroy(gameObject);
	}

	public void Leap(){
		anim.SetTrigger("leap");
	}
}
