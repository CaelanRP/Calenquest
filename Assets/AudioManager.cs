using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	[HideInInspector]
	private AudioSource source_;
	public static AudioManager instance;
	// Use this for initialization
	void Awake(){
		source_ = GetComponent<AudioSource>();
		instance = this;
	}

	public static AudioSource source {
		get{
			return instance.source_;
		}
	}

	public void StopAll(){
		source_.Stop();
	}
}
