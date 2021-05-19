using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject howToPlay;
    public GameObject credits;

    private void Start()
    {
        AudioListener.volume = 1f;
    }

    public void PlayButton()
    {
        GUISound.clickSound();
        SceneManager.LoadScene("GameScene");
    }

    public void HowToPlayButton()
    {
        GUISound.clickSound();
        SceneManager.LoadScene("TutorialScene");
    }

    public void CreditsButton()
    {
        GUISound.clickSound();
        credits.SetActive(true);
    }

    public void QuitGameButton()
    {
        GUISound.clickSound();
        Application.Quit();
    }

    public void Back()
    {
        GUISound.clickSound();
        credits.SetActive(false);
        howToPlay.SetActive(false);
    }
}
