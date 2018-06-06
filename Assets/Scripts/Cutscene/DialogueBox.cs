using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBox : MonoBehaviour {
	public Vector3 activePos, hiddenPos;
	private Vector3 initArrowPos;
	private RectTransform rectTransform;
	[HideInInspector]
	public bool active;
	public float moveLerp;
	public static DialogueBox instance;
	public float charDelay, initDelay, arrowBoopAmount, arrowBlinkDelay;
	[HideInInspector]
	public Speaker speaker;
	public TextMeshProUGUI dialogueText, speakerText;
	public float minBlipTime;
	public RectTransform arrow;
	// Use this for initialization
	void Awake () {
		rectTransform = GetComponent<RectTransform>();
		instance = this;
		initArrowPos = arrow.anchoredPosition;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = active ? activePos : hiddenPos;
		rectTransform.anchoredPosition = Vector3.Lerp(rectTransform.anchoredPosition, pos, moveLerp * Time.deltaTime);
	}

	public IEnumerator DisplayDialogue(string text){
		dialogueText.text = "";
		int length = 0;
		int maxLength = text.Length;
		
		if (!active){
			active = true;
			yield return new WaitForSeconds(initDelay);
		}

		if (speaker){
			speaker.FlapMouth();
		}
		float charTime = charDelay;
		float blipTime = minBlipTime;

		// Do an initial blip
		Blip();
		while (length < maxLength){
		
			charTime -= Time.unscaledDeltaTime;
			blipTime -= Time.unscaledDeltaTime;
			if (charTime <= 0){
				length ++;
				charTime = charDelay;

				if (blipTime <= 0){
					Blip();
					blipTime = minBlipTime;
				}
			}
				yield return null;
			dialogueText.text = text.Substring(0, length);
		}
		if (speaker){
			speaker.StopFlapping();
		}
		IEnumerator blink = BlinkArrow();
		StartCoroutine(blink);

		while (true){
			if (Calen.player.GetButtonDown("Interact") || (Calen.player.GetButtonDown("Jump"))){
				break;
			}
			yield return null;
		}

		arrow.gameObject.SetActive(false);
		StopCoroutine(blink);
		
		speaker = null;
	}

	IEnumerator BlinkArrow(){
		while(true){
			arrow.gameObject.SetActive(true);
			yield return new WaitForSeconds(arrowBlinkDelay);
			arrow.gameObject.SetActive(false);
			yield return new WaitForSeconds(arrowBlinkDelay);
		}
	}

	void Blip(){
		if (speaker && speaker.blipSound != null){
			AudioManager.instance.source.PlayOneShot(speaker.blipSound);
		}
	}
}
