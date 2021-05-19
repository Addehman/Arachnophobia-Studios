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
        SceneManager.LoadScene("MainMenu");
    }

    public void CreditsButtonVictory()
    {
        creditsUI.SetActive(true);
    }

    public void xButtonVictory()
    {
        creditsUI.SetActive(false);
    }

    public void exiButtonVictory()
    {
        Application.Quit();
    }
}
