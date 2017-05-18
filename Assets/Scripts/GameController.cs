using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 *  Manages current score + score UI text
 */
public class GameController : MonoBehaviour
{

    // Maintain current game score
    private int score = 0;

    // GUI current score component
    private Text scoreText;

    void Start()
    {
        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
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

    // End the game
    public void GameOver()
    {
        // todo 
    }
}
