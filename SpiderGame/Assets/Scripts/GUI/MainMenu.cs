using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject howToPlay;
    public GameObject credits;
    public void PlayButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void HowToPlayButton()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void CreditsButton()
    {
        credits.SetActive(true);
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }

    public void Back()
    {
        credits.SetActive(false);
        howToPlay.SetActive(false);
    }
}
