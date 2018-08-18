using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour {
	public float period, amplitude;

	public Vector3 dir;

	private Vector3 initPos;

	void Start(){
		initPos = transform.position;
		StartCoroutine(TranslateRoutine());
	}
	
	IEnumerator TranslateRoutine(){
		while(true){
			if (period > 0 && Time.time > 0){
				Vector3 pos = initPos + (dir * (Mathf.Sin(((Time.time) / period)) * amplitude));
				transform.position = pos;
			}
			yield return null;
		}
	}
}
