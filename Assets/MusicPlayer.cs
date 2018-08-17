using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {
	[HideInInspector]
	public AudioSource source;
	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource>();
	}
	
	public void Pause(){
		source.Pause();
	}

	public void UnPause(){
		source.UnPause();
	}

	public void Mute(){
		source.mute = true;
	}

	public void Unmute(){
		source.mute = false;
	}

	public void Stop(){
		source.Stop();
	}

	public void Play(){
		source.Play();
	}
}
