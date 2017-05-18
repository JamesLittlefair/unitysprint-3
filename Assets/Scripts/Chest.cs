using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

	private GameController controller;

	// Use this for initialization
	void Start () {
		controller = GameObject.Find ("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if(other.CompareTag("Player")) {
			controller.addScore (10 + Random.Range (-2, 20));
			Destroy (this.gameObject);
		}
	}
}
