using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumKillPlayer : MonoBehaviour
{
    public GameOver gameOverScript;

    void Start()
    {
        gameOverScript = FindObjectOfType<GameOver>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameOverScript.GameOverScreen();
        }
    }
}
