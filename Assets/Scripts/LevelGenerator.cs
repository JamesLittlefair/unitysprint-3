using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

	private int levelSize = 250;
	public int noTraps = 10;
	public int noChests = 20;

	private Transform startingModule;
	private Random rnd;
	private bool genLevel = false;
	private bool genFinished = false;
	private Transform lastPlacedModule;
	private Transform lastUsedExit;
	private List<Transform> lastModuleExits;
	private int Iterations = 0;
	private int n = 0;
	private int frames = 0;

	public Transform startRoom1;
	public Transform endRoom1;
	public Transform world;

	public List<Transform> modules;
	public List<Transform> corridors;
	public List<Transform> traps;

	private List<Transform> placedModules;
	private List<Transform> freeExits;

	// Use this for initialization
	void Start () {
		placedModules = new List<Transform> ();
		freeExits = new List<Transform> ();
		startingModule = Instantiate (startRoom1);
		if (!(startingModule == null)) {
			freeExits.AddRange (getExits(startingModule));
			lastPlacedModule = startingModule;
			lastUsedExit = null;
			genLevel = true;
		}
	}

	void GenerateLevel() {
		 
	}

	void Update(){
		frames++;
		if (frames % 5 == 0) {
			if (genLevel) {
				if (checkClip (lastPlacedModule)) {
					placedModules.Add (lastPlacedModule);
					lastPlacedModule = null;
					if (!(lastModuleExits == null)) {
						freeExits.AddRange (lastModuleExits);
					}
					if (freeExits.Count == 0) {
						Debug.Log ("ERROR no free exits");
						genLevel = false;
						resetGen ();
						return;
					}
					if (Iterations < levelSize) {
					
						Transform exit = GetRandomExit (freeExits);
						Transform newModule = Instantiate (GetRandomModule (exit.parent.tag));
						newModule.name = newModule.name + " MODULE " + n;
						newModule.SetParent (world);
						n++;
						Debug.Log ("Placing Module " + newModule);
						List<Transform> moduleExits = getExits (newModule);
						Transform newExit = GetRandomExit (moduleExits);
						matchExits (exit, newExit);
						lastModuleExits = moduleExits;
						moduleExits.Remove (newExit);

						lastUsedExit = exit;
						freeExits.Remove (exit);
						lastPlacedModule = newModule;
						Iterations++;

					} else if (Iterations >= levelSize) {
						genLevel = false;
						genFinished = true;
						populateModules ();
					}
				} else {
					Debug.Log ("Module Destroyed " + lastPlacedModule);
					DestroyImmediate (lastPlacedModule.gameObject);
					lastPlacedModule = null;
					lastModuleExits = null;
					if (!(lastUsedExit == null)) {
						freeExits.Add (lastUsedExit);
					}
					Iterations--;
				}
			}

			if (genFinished) {
				if (freeExits.Count == 0) {
					Debug.Log ("ERROR no free exits");
					genFinished = false;
					resetGen ();
					return;
				}
				Transform furtherestExit = GetRandomExit (freeExits);
				float distance = (furtherestExit.position - startRoom1.position).magnitude;
				foreach (Transform exit in freeExits) {
					float newDistance = (exit.position - startRoom1.position).magnitude;
					if (newDistance > distance) {

					}
				}
			}
		}
	}

	void populateModules(){

	}

	void resetGen(){

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
		if (e.Count == 0) {
			Debug.Log ("ERROR no free exits");
			return null;
		}
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
		if (newModule == null) {
			return true;
		}

		//GameObject c = newModule.gameObject;
		//CheckCollision check = (CheckCollision) c.GetComponent (typeof(CheckCollision));
		//if (check == null) {
		//	Debug.Log ("ERROR cannot find collision script");
		//	return false;
		//} else {

		if (checkModuleClip(newModule)) {
				Debug.Log ("collision avoided"  + newModule.gameObject);
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

	bool checkModuleClip(Transform newModule){
		List<Transform> modulesToCheck = placedModules;
		modulesToCheck.Add (newModule);
		foreach (Transform module in modulesToCheck) {
			if (!(module == null)) {
				GameObject c = module.gameObject;
				CheckCollision check = (CheckCollision)c.GetComponent (typeof(CheckCollision));
				if (check.checkClipping ()) {
					resetClip ();
					return true;
				}
			}
		}

		return false;
	}

	void resetClip(){
		foreach (Transform module in placedModules) {
			if (!(module == null)) {
				GameObject c = module.gameObject;
				CheckCollision check = (CheckCollision)c.GetComponent (typeof(CheckCollision));
				check.setClipping (false);
			}
		}
	}

}
