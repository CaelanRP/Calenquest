using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAnimator : MonoBehaviour {
	private Animator anim;
	void Awake(){
		anim = GetComponent<Animator>();
	}

	public void DisableAnimation(){
		anim.enabled = false;
	}
}
