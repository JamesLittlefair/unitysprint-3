using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour {
	
	public bool clipping;

	// Use this for initialization
	void Start () {
		clipping = false;
	}

	public bool checkClipping () {
		return clipping;
	}

	public void setClipping(bool b){
		clipping = b;
	}

	void OnTriggerEnter (Collider collisionInfo) {
		if ((collisionInfo.tag == "FloorPlan") && !(collisionInfo.transform.parent.Equals(this.transform))) {
			clipping = true;
			Debug.Log("Collision detected: " + this.transform + " with " + collisionInfo.transform.parent);
		}
	}
}
