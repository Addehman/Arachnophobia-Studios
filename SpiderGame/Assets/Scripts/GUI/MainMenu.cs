using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject howToPlay;
    public void PlayButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void HowToPlayButton()
    {
        howToPlay.SetActive(true);
    }

    public void CreditsButton()
    {
        Debug.Log("Credits");
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }

    public void Back()
    {
        howToPlay.SetActive(false);
    }
}
