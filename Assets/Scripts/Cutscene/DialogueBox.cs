using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBox : MonoBehaviour {
	public Vector3 activePos, hiddenPos;
	private RectTransform rectTransform;
	public bool active;
	public float moveLerp;
	public static DialogueBox instance;
	public float charDelay, initDelay;

	public TextMeshProUGUI dialogueText, speakerText;
	// Use this for initialization
	void Awake () {
		rectTransform = GetComponent<RectTransform>();
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = active ? activePos : hiddenPos;
		rectTransform.anchoredPosition = Vector3.Lerp(rectTransform.anchoredPosition, pos, moveLerp * Time.deltaTime);
	}

	public IEnumerator DisplayDialogue(string text){
		dialogueText.text = "";
		active = true;
		int length = 0;
		int maxLength = text.Length;
		yield return new WaitForSeconds(initDelay);
		while (length < maxLength){
			length ++;
			yield return new WaitForSeconds(charDelay);
			dialogueText.text = text.Substring(0, length);
		}

		while (true){
			if (Calen.player.GetButtonDown("Interact") || (Calen.player.GetButtonDown("Jump"))){
				break;
			}
			yield return null;
		}
		active = false;
	}
}
