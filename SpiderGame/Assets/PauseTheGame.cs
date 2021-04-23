using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Add this to game manager script

public class PauseTheGame : MonoBehaviour
{
    public bool gameIsPaused;
    public Image image;
    public TextMeshProUGUI gamePausedText;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void PauseGame()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;
            image.enabled = true;
            gamePausedText.enabled = true;


        }
        else
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
            image.enabled = false;
            gamePausedText.enabled = false;
        }
    }
}