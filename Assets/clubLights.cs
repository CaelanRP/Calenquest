using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class clubLights : MonoBehaviour {
	private List<SpriteRenderer> sprites;
	public float interval, proportionOn;
	// Use this for initialization
	void Start () {
		sprites = GetComponentsInChildren<SpriteRenderer>().ToList();
		StartCoroutine(Blink());
	}

	public void StopLights(){
		StopAllCoroutines();
		sprites.ForEach(s => s.enabled = false);
	}

	public void StartLights(){
		StartCoroutine(Blink());
	}
	
	IEnumerator Blink(){
		while (true){
			var oldList = new List<SpriteRenderer>(sprites);
			var newList = new List<SpriteRenderer>();
			while(newList.Count < sprites.Count * proportionOn){
				int index = Random.Range(0, oldList.Count);
				newList.Add(oldList[index]);
				oldList.RemoveAt(index);

				newList.ForEach(e => e.enabled = true);
				oldList.ForEach(e => e.enabled = false);
			}
			yield return new WaitForSeconds(interval);
		}
	}
}
