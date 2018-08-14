using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour {
	public string speakerName;
	private Animator anim;
	public AudioClip blipSound;

	public float volume = 1;
	public float pitchVariance = 0.1f;

	void Awake(){
		anim = GetComponent<Animator>();
	}

	public void FlapMouth(){
		if (anim)
			anim.SetBool("DialogueActive", true);
	}

	public void StopFlapping(){
		if (anim)
			anim.SetBool("DialogueActive", false);
	}

	public void TalkOver(){
		if (anim)
			anim.SetTrigger("FinishedTalking");
	}
}
