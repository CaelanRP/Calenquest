using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneWarp : Triggerable {
	public string scene;
	public AudioClip sound;
	public override void Trigger(){
		if (scene != null && scene != ""){
			if (sound != null){
				AudioManager.source.PlayOneShot(sound);
			}
			SceneManager.LoadScene(scene);
		}
	}
}
