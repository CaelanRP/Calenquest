using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birb : MonoBehaviour {

	// Use this for initialization
	public float hopSpeed, hopDelay, maxDistance;

	private Rigidbody2D rb;
	private Vector3 initPos;
	private Animator anim;
	private SpriteRenderer sr;
	void Awake () {
		rb = GetComponent<Rigidbody2D>();
		initPos = transform.position;
		anim = GetComponentInChildren<Animator>();
		sr = GetComponentInChildren<SpriteRenderer>();
	}

	void Start(){
		StartCoroutine(Hop());
	}
	
	// Update is called once per frame
	void Update () {
		if (rb.velocity.x > 0){
			sr.flipX = true;
		}
		else if (rb.velocity.x < 0){
			sr.flipX = false;
		}
	}

	bool TooFarRight(){
		return (Vector3.Distance(initPos, transform.position) > maxDistance && transform.position.x > initPos.x);
	}

	bool TooFarLeft(){
		return (Vector3.Distance(initPos, transform.position) > maxDistance && transform.position.x < initPos.x);
	}

	IEnumerator Hop(){
		while (true){
			yield return new WaitForSeconds(hopDelay);
			int rando = Random.Range(0,3);

			if (rando == 0){
				rb.velocity = Vector2.zero;
				anim.SetBool("Hopping", false);

			}
			else if (rando == 1){
				anim.SetBool("Hopping", true);
				if (TooFarRight()){
					rb.velocity = Vector2.left * hopSpeed;
				}
				else{
					rb.velocity = Vector2.right * hopSpeed;
				}
			}
			else if (rando == 2){
				anim.SetBool("Hopping", true);
				if (TooFarLeft()){
					rb.velocity = Vector2.right * hopSpeed;
				}
				else{
					rb.velocity = Vector2.left * hopSpeed;
				}
			}
		}
	}
}
