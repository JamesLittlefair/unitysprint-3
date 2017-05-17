using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour {

	private GameObject fire;
	private int i;
	private ParticleSystem[] emissions;
	private GameObject fireLight;
	private bool fireEnabled;

	// Use this for initialization
	void Start () {
		fire = this.transform.Find ("FireComplex").gameObject;
		fireLight = fire.transform.Find ("Light").gameObject;
		fireEnabled = true;
		i = 0;
		emissions = fire.GetComponentsInChildren<ParticleSystem> ();
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
		if (other.tag == "Player") {
			
		}
	}
}
