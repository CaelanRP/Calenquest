using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swivel : MonoBehaviour {
	public float period, amplitude;

	private float initRotation;

	private float randomOffset;

	void Start(){
		initRotation = transform.eulerAngles.z;
		StartCoroutine(SwivelRoutine());
		randomOffset = Random.Range(0f,4f);
	}
	
	IEnumerator SwivelRoutine(){
		while(true){
			if (period > 0 && Time.time > 0){
				float angle = initRotation + (Mathf.Sin(((Time.time + randomOffset) / period)) * amplitude);
				transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle);
			}
			yield return null;
		}
	}
}
