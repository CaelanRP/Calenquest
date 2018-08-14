using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveZone : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D coll){
		Calen c = coll.GetComponent<Calen>();
		if (c){
			c.active = false;
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		Calen c = coll.GetComponent<Calen>();
		if (c){
			c.active = true;
		}
	}
}
