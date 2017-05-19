using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : MonoBehaviour {

	private GameController controller;

	// Use this for initialization
	void Start () {
		controller = GameObject.Find ("GameController").GetComponent<GameController>();
	}
	
	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Player")) {
			controller.win ();
		}
	}
}
