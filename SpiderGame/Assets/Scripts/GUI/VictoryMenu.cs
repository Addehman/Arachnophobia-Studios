using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour
{
    public GameObject creditsUI;
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        AudioListener.volume = 1f;
    }
    public void MainMenuButtonVictory()
    {
        ClickSound.PlayClickSFX();
        SceneManager.LoadScene("MainMenu");
    }

    public void CreditsButtonVictory()
    {
        ClickSound.PlayClickSFX();
        creditsUI.SetActive(true);
    }

    public void xButtonVictory()
    {
        ClickSound.PlayClickSFX();
        creditsUI.SetActive(false);
    }

    public void exiButtonVictory()
    {
        ClickSound.PlayClickSFX();
        Application.Quit();
    }
}
