using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneCollider : MonoBehaviour {
	public Cutscene cutscene;
	public bool disableAfterPlay = true;

	void OnTriggerStay2D(Collider2D coll){
		Calen c = coll.GetComponent<Calen>();
		if (c){
			if (cutscene && !CutsceneManager.busy){
				CutsceneManager.instance.StartNewScene(cutscene.actions);
				if (disableAfterPlay){
					GetComponent<BoxCollider2D>().enabled = false;
				}
			}
		}
	}
}
