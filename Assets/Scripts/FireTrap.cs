using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour {

	private GameObject fire;
	private int i;
	private ParticleSystem[] emissions;
	private GameObject fireLight;
	private bool fireEnabled;
	public int delay = 0;
    private GameController gc;

	// Use this for initialization
	void Start () {
		fire = this.transform.Find ("FireComplex").gameObject;
		fireLight = fire.transform.Find ("Light").gameObject;
		fireEnabled = true;
		i = delay;
		emissions = fire.GetComponentsInChildren<ParticleSystem> ();
        // Get reference to gameController to end the game
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}

	void Update (){
		i++;
		if (i == 300) {
			i = 0;
			foreach (ParticleSystem e in emissions) {
				if (fireEnabled) {
					e.Stop ();
					//e.gameObject.SetActive(false);
				} else {
					e.Play ();
					//e.gameObject.SetActive(true);
				}
			}
			if (fireEnabled) {
				fireLight.SetActive (false);
			} else {
				fireLight.SetActive (true);
			}
			fireEnabled = !fireEnabled;
		}
	}

	void OnTriggerEnter (Collider other){
		if (other.tag == "Player" && fireEnabled) {
            gc.GameOver();
		}
	}
}
