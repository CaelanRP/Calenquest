using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : Triggerable {
	public Cutscene cutscene;

	public override void Trigger(){
		if (cutscene){
			CutsceneManager.instance.StartNewScene(cutscene.actions);
		}
	}
}
