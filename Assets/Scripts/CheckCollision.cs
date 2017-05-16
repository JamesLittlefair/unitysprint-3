using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour {
	
	private bool isClipping;

	// Use this for initialization
	void Start () {
		isClipping = false;
	}

	public bool checkClipping () {
		return isClipping;
	}

	void OnTriggerEnter (Collider collisionInfo) {
		if ((collisionInfo.tag == "FloorPlan") && !(collisionInfo.transform.IsChildOf(this.transform))) {
			isClipping = true;
			Debug.Log("Collision detected");
		}
	}
}
