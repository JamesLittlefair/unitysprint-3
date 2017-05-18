using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

	private int levelSize = 15;

	private Transform finalRoom;
	private Transform startingModule;
	private Random rnd;

	private bool genLevel = false;
	private bool genFinished = false;
	private bool checkFinalRoom = false;

	private Transform furtherestExit;
	private Transform lastPlacedModule;
	private Transform lastUsedExit;
	private List<Transform> lastModuleExits;

	private int Iterations = 0;
	private int n = 0;
	private int frames = 0;

	public Transform startRoom1;
	public Transform endRoom1;
	public Transform world;
	public Transform endCap;

	public List<Transform> modules;
	public List<Transform> corridors;
	public List<Transform> traps;

	public Transform chest;
	public Transform chestLarge;
	public Transform chestTrap;

	private List<Transform> placedModules;
	private List<Transform> freeExits;
	private List<Transform> finalExits;

	private List<Transform> trapLocations;
	private List<Transform> chestLocations;

	// Use this for initialization
	void Start () {
		GenerateLevel ();
	}

	void GenerateLevel() {
		placedModules = new List<Transform> ();
		freeExits = new List<Transform> ();
		finalExits = new List<Transform> ();
		startingModule = Instantiate (startRoom1);
		startingModule.name = startingModule.name + " MODULE " + n;
		startingModule.SetParent (world);
		n++;
		Debug.Log ("Placing Module " + startingModule);
		if (!(startingModule == null)) {
			lastModuleExits = (getExits(startingModule));
			lastPlacedModule = startingModule;
			lastUsedExit = null;
			genLevel = true;
		}
	}

	void Update(){
		frames++;
		if (frames % 5 == 0) {
			if (genLevel) {
				if (checkClip (lastPlacedModule)) {
					if (!(lastPlacedModule == null)) {
						placedModules.Add (lastPlacedModule);
					}
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
						finalExits = freeExits;
						genLevel = false;
						genFinished = true;
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
				if (finalExits.Count == 0) {
					Debug.Log ("ERROR cannot place final exit");
					genFinished = false;
					resetGen ();
					return;
				}
				furtherestExit = GetRandomExit (finalExits);
				float distance = (furtherestExit.position - startingModule.position).magnitude;
				foreach (Transform exit in finalExits) {
					float newDistance = (exit.position - startingModule.position).magnitude;
					if (newDistance > distance) {
						furtherestExit = exit;
						distance = newDistance;
					}
				}
				finalRoom = Instantiate (endRoom1);
				finalRoom.name = finalRoom.name + " MODULE " + n;
				finalRoom.SetParent (world);
				n++;
				Debug.Log ("Placing Module " + finalRoom);
				matchExits (furtherestExit, GetRandomExit(getExits(finalRoom)));

				finalExits.Remove (furtherestExit);
				genFinished = false;
				checkFinalRoom = true;
			}

			if (checkFinalRoom) {
				if (checkClip (finalRoom)) {
					placedModules.Add (finalRoom);
					freeExits.Remove (furtherestExit);
					checkFinalRoom = false;
					populateModules ();
				} else {
					DestroyImmediate (finalRoom.gameObject);
					genFinished = true;
					checkFinalRoom = false;
				}
			}
		}
	}

	void populateModules(){

		foreach (Transform module in placedModules) {
			if (!(module == null)) {
				Transform m = module.Find ("Walls");
				if (m == null) {
					Debug.Log ("Error could not find walls " + module);
				}
				m.gameObject.SetActive (true);
			}
		}

		foreach (Transform exit in freeExits) {
			if (!(exit == null)) {
				Transform cap = Instantiate (endCap);
				cap.name = cap.name + " MODULE " + n;
				cap.SetParent (world);
				n++;
				Debug.Log ("Placing Module " + cap);
				matchExits (exit, GetRandomExit(getExits(cap)));
			}
		}

		trapLocations = new List<Transform>();
		GameObject[] array1 = GameObject.FindGameObjectsWithTag ("Trap");
		for (int i = 0; i < array1.Length; i++) {
			trapLocations.Add (array1[i].transform);
		}

		chestLocations = new List<Transform>();
		GameObject[] array2 = GameObject.FindGameObjectsWithTag ("Chest");
		for (int i = 0; i < array2.Length; i++) {
			chestLocations.Add (array2[i].transform);
		}

		int noTraps = (trapLocations.Count * 100) / 40;
		for (int i = 0; i < noTraps; i++) {
			if (!(trapLocations.Count == 0)) {
				Transform randomTrap = trapLocations [Random.Range (0, trapLocations.Count)];
				Transform trap = Instantiate (GetRandomTrap ());
				matchExits (randomTrap, GetRandomExit (getExits (trap)));
				trapLocations.Remove (randomTrap);
			}
		}

		int noChests = (chestLocations.Count * 100) / 70;
		for (int i = 0; i < noChests; i++) {
			if (!(chestLocations.Count == 0)) {
				Transform randomChest = chestLocations [Random.Range (0, chestLocations.Count)];
				Transform chestModule = Instantiate (chest);
				matchExits (randomChest, GetRandomExit (getExits (chestModule)));
				chestLocations.Remove (randomChest);
			}
		}
	}

	void resetGen(){
		foreach(Transform module in placedModules){
			DestroyImmediate (module.gameObject);
			GenerateLevel ();
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

	Transform GetRandomTrap(){
		return traps [Random.Range (0, traps.Count)];
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
