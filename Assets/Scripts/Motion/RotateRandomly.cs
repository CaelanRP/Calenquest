using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRandomly : MonoBehaviour {
	public float interval;
	void Start(){
		StartCoroutine(AngleRoutine());
	}
	
	IEnumerator AngleRoutine(){
		while(true){
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Random.Range(0,360));
			yield return new WaitForSeconds(interval);
		}
	}
}
