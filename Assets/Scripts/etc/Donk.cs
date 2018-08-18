using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Donk : MonoBehaviour {
	public int damage = 1;
	public AudioClip clip;
	public float minAngle = 90;

	public GameObject enableWound;

	void OnCollisionEnter2D(Collision2D coll){
		if (!enabled){
			return;
		}
		Calen c = coll.gameObject.GetComponent<Calen>();
		if (c){
			if (Vector2.Angle(c.transform.up, Vector2.up) > minAngle){
				c.TakeDamageFake(damage);
				AudioManager.source.PlayOneShot(clip);
				if (enableWound){
					enableWound.SetActive(true);
				}
				
			}
			this.enabled = false;
		}
	}
}
