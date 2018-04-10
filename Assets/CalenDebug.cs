using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalenDebug : MonoBehaviour {
	private Calen calen;
	public bool grounded;
	// Use this for initialization
	void Awake(){
		calen = GetComponent<Calen>();
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		grounded = calen.grounded;
	}
}
