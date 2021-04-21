using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void HowToPlayButton()
    {
        Debug.Log("How to play");
    }

    public void CreditsButton()
    {
        Debug.Log("Credits");
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }
}
