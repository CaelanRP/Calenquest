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

	void OnTriggerStay2D(Collider2D coll){
		Calen c = coll.GetComponent<Calen>();
		if (c){
			Quaternion targetAngle = Quaternion.Euler(0,0,180);
			c.transform.rotation = Quaternion.Slerp(c.transform.rotation, targetAngle, 0.5f);
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		Calen c = coll.GetComponent<Calen>();
		if (c){
			c.active = true;
		}
	}
}
