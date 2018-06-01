using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBox : MonoBehaviour {
	public Vector3 activePos, hiddenPos;
	private RectTransform rectTransform;
	public bool active;
	public float moveLerp;
	// Use this for initialization
	void Awake () {
		rectTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = active ? activePos : hiddenPos;
		rectTransform.anchoredPosition = Vector3.Lerp(rectTransform.anchoredPosition, pos, moveLerp * Time.deltaTime);
	}
}
