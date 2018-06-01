using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAction : MonoBehaviour {
	public float waitTime = 0;
	public string dialogueLine = "";
	public Vector3 cameraTarget = Vector3.zero;
	public enum Type{DialogueLine = 0, SetCameraTarget = 1, Wait = 2}
	public Type actionType;


	public bool finished, playing;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Play(){
		if (!playing){
			playing = true;
		}

		if (actionType == Type.Wait){
			StartCoroutine(Wait(waitTime));
		}

		else if (actionType == Type.DialogueLine){
			StartCoroutine(DisplayDialogue(dialogueLine));
		}
	}

	IEnumerator Wait(float seconds){
		Debug.Log("Waiting...");
		yield return new WaitForSeconds(seconds);
		Finish();
	}

	IEnumerator DisplayDialogue(string text){
		Debug.Log("Playing cutscene...");
		yield return DialogueBox.instance.DisplayDialogue(text);
		Finish();
	}

	private void Finish(){
		Debug.Log("Finished.");
		finished = true;
	}
}
