using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBlooper : MonoBehaviour {

	public Animator text_anim;
	public TextMeshProUGUI text;

	private string[] texts = new string[]{
		"In the year 201X, the ethically questionable\ncorporation known as MANAZONE",
		"purchased full property rights to the city\nof SEATTLE, reshaping it",
		"into a bustling metropolis\nknown as NEO SEATTLE.",
		"This high-tech capitalist utopia is the\nperfect place for a young man",
		"known as CALEN to fulfill his dreams of\nbecoming an INDEPENDENT GAME DEVELOPER..."
	};

	public void SetText(int index){
		Debug.Log("Setting text to " + index);
		text.text = texts[index];
	}

	public void BloopText(){
		Debug.Log("Blooping text.");
		text_anim.SetTrigger("ShowText");
	}
}
