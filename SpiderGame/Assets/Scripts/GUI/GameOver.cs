using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public EricAlert ericAlert;
    public SpiderMovement spiderMovement;
    public VacuumKillPlayer vacuumKillPlayer;
    public GameObject gameOverScreen;
    public GameObject restartButton;
    public GameObject spider;
    public Animator spiderAnimator;
    public bool diedByEric = false;

    public void GameOverScreen()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined; //None
        gameOverScreen.SetActive(true);
        spiderMovement.rb.isKinematic = true;
        spiderMovement.enabled = false;
        Winstate.RemoveCompletedQuests();

        if(vacuumKillPlayer.diedByVacuum == true)
        {
            spider.SetActive(false);
        }
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("GameScene");
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
