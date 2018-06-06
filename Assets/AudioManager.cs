using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	[HideInInspector]
	public AudioSource source;
	public static AudioManager instance;
	// Use this for initialization
	void Awake(){
		source = GetComponent<AudioSource>();
		instance = this;
	}
}
