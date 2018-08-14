using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunShell : MonoBehaviour {
	public Vector2 minEjectForce, maxEjectForce;
	public float minTorque, maxTorque;
	private Rigidbody2D rb;
	float timeSpawned;
	// Use this for initialization
	void Awake(){
		rb = GetComponent<Rigidbody2D>();
		timeSpawned = Time.time;
	}
	void Start () {

		Vector2 force = CalenUtil.RandomBetweenVectors(minEjectForce,maxEjectForce);
		//force = transform.InverseTransformDirection(force);
		rb.AddForce(force,ForceMode2D.Impulse);
		var torque = Random.Range(minTorque, maxTorque);
		if (CalenUtil.Coinflip()){
			torque = -torque;
		}
		rb.AddTorque(torque,ForceMode2D.Impulse);
		Destroy(gameObject, 15);
	}



}
