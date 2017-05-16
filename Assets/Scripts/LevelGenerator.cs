using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

	private int levelSize = 5;
	private Transform startingModule;
	private Random rnd;
	private bool started;

	public Transform startRoom1;
	public Transform endRoom1;

	public List<Transform> modules;
	public List<Transform> corridors;

	public List<Transform> placedModules;
	private List<Transform> freeExits;

	// Use this for initialization
	void Start () {
		placedModules = new List<Transform> ();
		freeExits = new List<Transform> ();
		startingModule = Instantiate (startRoom1);
		if (!(startingModule == null)) {
			freeExits.AddRange (getExits(startingModule));
			GenerateLevel();
		}
	}

	void GenerateLevel() {
		int Iterations = 0;
		while (Iterations < levelSize) {
			List<Transform> newExits = new List<Transform> ();
			foreach (Transform exit in freeExits) {
				bool checkClipping = false;
				int i = 0;
				while ((!checkClipping) && (i < 10)) {
					Transform newModule = Instantiate (GetRandomModule (exit.parent.tag));
					List<Transform> moduleExits = getExits (newModule);
					Transform newExit = GetRandomExit (moduleExits);
					matchExits (exit, newExit);
					bool b = checkClip (newModule);
					if (b) {
						moduleExits.Remove (newExit);
						newExits.AddRange (moduleExits);
						placedModules.Add (newModule);
						checkClipping = true;
					}
					i++;
				}
			}
			freeExits = newExits;
			Iterations++;
		}
	}

	IEnumerator wait(){
		yield return new WaitForSeconds(5);
	}

	Transform GetRandomModule(string s){
		if (s == "Corridor") {
			return modules [Random.Range (0, modules.Count)];
		} else {
			return corridors [Random.Range (0, corridors.Count)];
		}
	}

	Transform GetRandomExit(List<Transform> e){
		Transform randomExit = e [Random.Range (0, e.Count)];
		return randomExit;
	}

	bool matchExits(Transform oldExit, Transform newExit){
		Transform newModule = newExit.parent;
		Vector3 forwardVMatch = -oldExit.transform.forward;
		float rotation = (Vector3.Angle (Vector3.forward, forwardVMatch) * Mathf.Sign (forwardVMatch.x)) - (Vector3.Angle (Vector3.forward, newExit.transform.forward) * Mathf.Sign (newExit.transform.forward.x));
		newModule.RotateAround (newExit.transform.position, Vector3.up, rotation);
		Vector3 translation = oldExit.transform.position - newExit.transform.position;
		newModule.transform.position += translation;
		return true;
	}

	bool checkClip(Transform newModule){
		//GameObject c = newModule.gameObject;
		//CheckCollision check = (CheckCollision) c.GetComponent (typeof(CheckCollision));
		//if (check == null) {
		//	Debug.Log ("ERROR cannot find collision script");
		//	return false;
		//} else {

		if (checkModuleClip ()) {
				Destroy (newModule.gameObject);
				Debug.Log ("collision avoided");
				return false;
			} else {
				Debug.Log ("collision Fine, collision check: " + newModule.gameObject);
				return true;
			}
		//}
	}

	List<Transform> getExits(Transform module){
		List<Transform> newExits = new List<Transform> ();
		for (int i = 0; i < module.childCount; i++) {
			Transform e = module.GetChild (i);
			if(e.CompareTag("Exit")){
				newExits.Add(e);
			}
		}
		return newExits;
	}

	bool checkModuleClip(){
		foreach (Transform module in placedModules) {
			GameObject c = module.gameObject;
			CheckCollision check = (CheckCollision) c.GetComponent (typeof(CheckCollision));
			if (check.checkClipping ()) {
				Debug.Log ("Collision found woo");
				resetClip ();
				return true;
			}
		}
		Debug.Log ("No collision found lol");
		return false;
	}

	void resetClip(){
		foreach (Transform module in placedModules) {
			GameObject c = module.gameObject;
			CheckCollision check = (CheckCollision)c.GetComponent (typeof(CheckCollision));
			check.setClipping (false);
		}
	}

}
