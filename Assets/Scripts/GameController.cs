using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

/**
 *  Manages current score + score UI text
 */
public class GameController : MonoBehaviour
{

    // Maintain current game score
    private int score = 0;

    // Player
    private FirstPersonController player;
    // GUI current score component
    private Text scoreText;
    // Game over screen
    private GameObject gameOver;
    // Game win screen
    private GameObject gameWin;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
        gameOver = GameObject.FindGameObjectWithTag("GameOver");
        gameOver.SetActive(false);
        gameWin = GameObject.FindGameObjectWithTag("GameWin");
        gameWin.SetActive(false);
    }

    // Update score GUI text after change
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    // Reset the score
    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }

    // Increase the score by one
    public void IncrementScore()
    {
        score++;
        UpdateScoreText();
    }

    public int GetScore()
    {
        return score;
    }

    // Start the game
    public void NewGame()
    {
        SceneManager.LoadScene("Main");
    }

    // End the game (dead)
    public void GameOver()
    {
        GameOver(false);
    }

    public void GameOver(bool win)
    {
        // Disable movement component
        player.enabled = false;
        if (win)
        {
            // Show game win UI
            gameWin.SetActive(true);
        } else
        {
            // Show game over UI
            gameOver.SetActive(true);
        }
    }
}
