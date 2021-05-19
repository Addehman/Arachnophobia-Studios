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
        GUISound.clickSound();
        SceneManager.LoadScene("MainMenu");
    }

    public void CreditsButtonVictory()
    {
        GUISound.clickSound();
        creditsUI.SetActive(true);
    }

    public void xButtonVictory()
    {
        GUISound.clickSound();
        creditsUI.SetActive(false);
    }

    public void exiButtonVictory()
    {
        GUISound.clickSound();
        Application.Quit();
    }
}
