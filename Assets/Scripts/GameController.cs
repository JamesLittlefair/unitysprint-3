using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public int Score;
	public float time = 0;
	public GameObject introPanel;
	public GameObject restartPanel;
	public GameObject winPanel;
	public GameObject blackOut;
	public Text scoreCounter;
	public LevelGenerator gen;
	public GameObject loading;
	public GameObject playerObj;
	private Player player;
	public GameObject stats;
	public Text winDescription;
	public Text timeText;
	private bool gameStarted;
	private float timef;
	public AudioClip hurt;
	public AudioClip treasure;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
		gameStarted = false;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		player = playerObj.GetComponent<Player> ();
		stats.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (gameStarted) {
			time += Time.deltaTime;
			timef = Mathf.Round (time * 10) / 10;
			timeText.text = timef.ToString();
		}
	}

	public void addScore(int n) {
		audioSource.clip = treasure;
		audioSource.Play();
		Score += n;
		scoreCounter.text = Score.ToString();
	}

	public void playerHurt(){
		audioSource.clip = hurt;
		audioSource.Play();
	}

	void reset(){
		Score = 0;
		scoreCounter.text = Score.ToString();
	}

	public void startGame (int size) {
		if (gen.levelGenerated == true) {
			gen.resetGen ();
		}
		Cursor.visible = false;
		loading.SetActive (true);
		blackOut.SetActive (true);
		introPanel.SetActive (false);
		restartPanel.SetActive (false);
		winPanel.SetActive (false);
		gen.GenerateLevel (size);

	}

	public void genFinished() {
		stats.SetActive (true);
		loading.SetActive (false);
		blackOut.SetActive (false);
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		time = 0f;
		gameStarted = true;
		player.reset ();
	}

	public void playerDeath(){
		gameStarted = false;
		restartPanel.SetActive (true);
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	public void win(){
		gameStarted = false;
		player.freeze (true);
		winDescription.text = "Well done, you have escaped the dungeon in " + timef.ToString() + " seconds, with a score of " + Score.ToString() + ". To restart, click a difficulty option below";
		winPanel.SetActive (true);
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
}
