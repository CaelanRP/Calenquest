using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAction : MonoBehaviour {
	public float actionFloat = 0;
	public string actionString, actionString2 = "";
	public bool actionBool;
	public Vector3 actionVector3 = Vector3.zero;
	public enum Type{DialogueLine = 0, SetCameraTarget = 1, UnlockCamera = 2, Wait = 3, EnableObject = 4}
	public Object actionObject;
	public Type actionType;




	public bool finished, playing;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Play(){
		StartCoroutine(PlayRoutine());
	}

	IEnumerator PlayRoutine(){
		if (!playing){
			playing = true;
		}

		if (actionType == Type.Wait){
			yield return StartCoroutine(Wait());
		}

		else if (actionType == Type.DialogueLine){
			yield return StartCoroutine(DisplayDialogue());
		}

		else if (actionType == Type.SetCameraTarget){
			CameraController.instance.LockOnTarget(actionVector3);
		}

		else if (actionType == Type.UnlockCamera){
			CameraController.instance.UnlockTarget();
		}

		else if (actionType == Type.EnableObject){
			((GameObject)actionObject).SetActive(actionBool);
		}
		Finish();
	}

	IEnumerator Wait(){
		Debug.Log("Waiting...");
		yield return new WaitForSeconds(actionFloat);
	}

	IEnumerator DisplayDialogue(){
		Debug.Log("Playing cutscene...");
		DialogueBox.instance.speakerText.text = actionString2;
		yield return DialogueBox.instance.DisplayDialogue(actionString);
	}

	private void Finish(){
		Debug.Log("Action Finished.");
		finished = true;
		playing = false;
	}
}
