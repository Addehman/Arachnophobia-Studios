using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
//    SpiderAudio spiderAudio;
    public EricAlert ericAlert;
    public SpiderMovement spiderMovement;
    public VacuumKillPlayer vacuumKillPlayer;
    public GameObject gameOverScreen;
    public GameObject restartButton;
    public GameObject spider;
    public Animator spiderAnimator;
    public bool diedByEric = false;
    float gameOverTimer = 0f;
    float soundFadeOutTimer = 0f;
    bool itIsGameOver = false;

    private void Start()
    {
        Time.timeScale = 1f;
        AudioListener.volume = 1f;
        Winstate.isVictory = false;
    }

    private void Update()
    {
        if(itIsGameOver == true)
        {
            gameOverTimer += Time.deltaTime;
            StartCoroutine(soundFadeOut(0.005f));

            if (gameOverTimer >= 3f)
            {
                soundFadeOutTimer += Time.deltaTime;
                Time.timeScale = 0f;
            }
        }
    }

    IEnumerator soundFadeOut(float speed)
    {
        float audioVolume = AudioListener.volume;

        while(audioVolume >= speed)
        {
            audioVolume -= speed;
            AudioListener.volume = audioVolume;

            yield return new WaitForSeconds(0.1f);
        }
    }


    public void GameOverScreen()
    {
        itIsGameOver = true;
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
        GUISound.clickSound();
        SceneManager.LoadScene("GameScene");
    }

    public void BackToMainMenu()
    {
        GUISound.clickSound();
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitButton()
    {
        GUISound.clickSound();
        Application.Quit();
    }
}
