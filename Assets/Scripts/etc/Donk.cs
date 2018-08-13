using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Donk : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D coll){
		Calen c = coll.GetComponent<Calen>();
		if (c){
			if (Vector2.Angle(c.transform.up, Vector2.up) > 90){
				c.TakeDamageFake();
				gameObject.SetActive(false);
			}
		}
	}
}
