using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureWalkover : MonoBehaviour
{
    public bool endTreasure = false;

    private GameController gc;
    private AudioSource audioSource;

    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        audioSource = GetComponent<AudioSource>();
    }

    // Increment the score and remove the object when player walks over
    void OnTriggerEnter(Collider collisionInfo)
    { 
        if (collisionInfo.tag == "Player")
        {
            gc.IncrementScore();
            audioSource.Play();
            GameObject.Destroy(gameObject);

            // If the end treasure, end the game also
            if (endTreasure)
            {
                Debug.Log("End treasure reached");
                gc.GameOver(true);
            }
        }
    }
}
