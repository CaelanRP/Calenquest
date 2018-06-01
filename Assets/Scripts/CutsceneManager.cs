using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CutsceneManager : MonoBehaviour {
	public static CutsceneManager instance;
	public bool busy;
	public List<SceneAction> queuedActions;
	// Use this for initialization
	void Awake () {
		instance = this;
	}

	void Start(){

	}

	public void StartNewScene(SceneAction[] actions){
		if (!busy){
			queuedActions = actions.ToList();
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
			if (currentAction.finished){
				queuedActions.RemoveAt(0);
				return;
			}
			if (!currentAction.playing){
				currentAction.Play();
			}
		}
		else{
			busy = false;
		}
	}
}
