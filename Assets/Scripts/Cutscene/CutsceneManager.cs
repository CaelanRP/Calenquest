﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CutsceneManager : MonoBehaviour {
	public static CutsceneManager instance;
	public static bool busy;
	public List<SceneAction> queuedActions;
	public Cutscene currentScene;
	// Use this for initialization
	void Awake () {
		instance = this;
	}

	void Start(){

	}

	public void StartNewScene(Cutscene scene){
		if (!busy){
			currentScene = scene;
			queuedActions = scene.actions.ToList();
			busy = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		UpdateScene();
	}

	void UpdateScene(){
		if (queuedActions.Count > 0){
			SceneAction currentAction = queuedActions[0];
			if (currentAction == null){
				queuedActions.RemoveAt(0);
				return;
			}
			else if (currentAction.finished){
				currentAction.finished = false;
				queuedActions.RemoveAt(0);
				return;
			}
			if (!currentAction.playing){
				currentAction.Play();
			}
		}
		else{
			if (busy){
				FinishCutscene();
			}
		}
	}

	void FinishCutscene(){
		Debug.Log("Cutscene finished.");
		if (!currentScene.keepCameraLocked){
			CameraController.instance.UnlockTarget();
		}
		DialogueBox.instance.active = false;
		busy = false;
	}
}
