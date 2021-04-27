using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public EricAlert ericAlert;
    public SpiderMovement spiderMovement;
    public GameObject gameOverScreen;
    public GameObject restartButton;

    public void GameOverScreen()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined; //None
        gameOverScreen.SetActive(true);
        spiderMovement.rb.isKinematic = true;
        spiderMovement.enabled = false;
    }

    public void RestartButton()
    {
        /*SceneManager.LoadScene("GameScene");*/
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
