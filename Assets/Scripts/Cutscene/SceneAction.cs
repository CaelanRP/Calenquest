using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAction : MonoBehaviour {
	public float actionFloat = 0;
	public string actionString, actionString2 = "";
	public bool actionBool, actionBool2, actionBool3;
	public Vector3 actionVector3 = Vector3.zero;
	public enum Type{DialogueLine = 0, SetCameraTarget = 1, UnlockCamera = 2, Wait = 3, EnableObject = 4, InvokeMethod = 5, FlipObject = 6, 
	TriggerAnimation = 7, PlaySound = 8, Warp = 9}
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
			DialogueBox.instance.active = false;
			yield return StartCoroutine(Wait());
		}

		else if (actionType == Type.DialogueLine){
			yield return StartCoroutine(DisplayDialogue());
		}

		else if (actionType == Type.SetCameraTarget){
			CameraController.instance.LockOnTarget(actionVector3);
			CameraController.instance.trackX = actionBool;
			CameraController.instance.trackY = actionBool2;
			if (actionBool3){
				CameraController.instance.transform.position = CameraController.instance.target;
			}
		}

		else if (actionType == Type.UnlockCamera){
			CameraController.instance.UnlockTarget();
			CameraController.instance.trackX = actionBool;
			CameraController.instance.trackY = actionBool2;
		}

		else if (actionType == Type.EnableObject){
			((GameObject)actionObject).SetActive(actionBool);
		}

		else if (actionType == Type.InvokeMethod){
			if (actionBool){
				DialogueBox.instance.active = false;
			}
			((MonoBehaviour)actionObject).Invoke(actionString, 0);
		}
		else if (actionType == Type.FlipObject){
			GameObject obj = (GameObject)actionObject;
			obj.transform.localScale = new Vector3(obj.transform.localScale.x * -1, obj.transform.localScale.y, obj.transform.localScale.z);
		}
		else if (actionType == Type.TriggerAnimation){
			Animator anim = (Animator)actionObject;
			anim.SetTrigger(actionString);
		}
		else if (actionType == Type.PlaySound){
			AudioClip clip = (AudioClip)actionObject; 
			AudioManager.source.PlayOneShot(clip);
		}
		else if (actionType == Type.Warp){
			Calen.instance.transform.position = actionVector3;
		}
		Finish();
	}

	IEnumerator Wait(){
		Debug.Log("Waiting...");
		yield return new WaitForSecondsRealtime(actionFloat);
	}

	IEnumerator DisplayDialogue(){
		Debug.Log("Playing cutscene...");
		if (actionString2 != null && actionString2 != ""){
			DialogueBox.instance.speakerText.text = actionString2;
		}
		else if (actionObject != null){
			DialogueBox.instance.speakerText.text = ((Speaker)actionObject).speakerName;
		}
		else{
			DialogueBox.instance.speakerText.text = "";
		}

		if (actionObject != null){
			Debug.Log("setting speaker.");
			DialogueBox.instance.speaker = (Speaker)actionObject;
		}

		yield return DialogueBox.instance.DisplayDialogue(actionString);
	}

	private void Finish(){
		Debug.Log("Action Finished.");
		finished = true;
		playing = false;
	}

	private void ClearVars(){
		actionFloat = 0;
		actionString = actionString2 = "";
		actionBool = false;
		actionVector3 = Vector3.zero;
		actionObject = null;
	}
}
