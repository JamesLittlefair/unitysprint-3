using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureWalkover : MonoBehaviour
{

    private GameController gc;

    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Increment the score and remove the object when player walks over
    void OnTriggerEnter(Collider collisionInfo)
    { 
        if (collisionInfo.tag == "Player")
        {
            gc.IncrementScore();
            GameObject.Destroy(gameObject);
        }
    }
}
