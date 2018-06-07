using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneStarter : MonoBehaviour {
	public Cutscene cutscene;
	// Use this for initialization
	void Start () {
		if (!cutscene){
			cutscene = GetComponentInChildren<Cutscene>();
		}

		if (cutscene){
			CutsceneManager.instance.StartNewScene(cutscene.actions);
		}
	}
}
