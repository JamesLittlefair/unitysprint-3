using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	private int health;
	private FirstPersonController controller;
	public GameObject gameControllerObj;
	private GameController gameController;

	public Text healthMeter;

	// Use this for initialization
	void Start () {
		health = 100;
		controller = this.GetComponentInParent<FirstPersonController> ();
		gameController = gameControllerObj.GetComponentInParent<GameController> ();
		controller.freeze = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0) {
			die ();
		}
	}

	public void damage(int n) {
		health -= n;
		healthMeter.text = health.ToString();
	}

	void die () {
		health = 100;
		freeze(true);
		gameController.playerDeath ();
	}

	public void freeze(bool b){
		controller.freeze = b;
	}

	public void reset () {
		health = 100;
		controller.freeze = false;
		this.gameObject.transform.position = new Vector3 (0f, 2f, 0f);
		healthMeter.text = health.ToString();
	}
}
