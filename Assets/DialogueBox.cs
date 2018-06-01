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
	public float charDelay;

	public TextMeshProUGUI dialogueText;
	// Use this for initialization
	void Awake () {
		rectTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = active ? activePos : hiddenPos;
		rectTransform.anchoredPosition = Vector3.Lerp(rectTransform.anchoredPosition, pos, moveLerp * Time.deltaTime);
	}

	public IEnumerator DisplayDialogue(string text){
		int length = 0;
		int maxLength = text.Length;
		while (length < maxLength){
			length ++;
			yield return charDelay;
			dialogueText.text = text.Substring(0, length);
		}
	}
}
