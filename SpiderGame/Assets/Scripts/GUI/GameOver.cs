using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
//    SpiderAudio spiderAudio;
	public EricAlert ericAlert;
	public VacuumKillPlayer vacuumKillPlayer;
	public GameObject gameOverScreen;
	public GameObject restartButton;
	public GameObject spider;
	public Animator spiderAnimator;
	public bool diedByEric = false;
	
	private SpiderMovement spiderMovement;
	private float gameOverTimer = 0f;
	private float soundFadeOutTimer = 0f;
	private bool itIsGameOver = false;
	

	private void Start()
	{
		spiderMovement = FindObjectOfType<SpiderMovement>();
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
		ClickingSound.clickSound();
		SceneManager.LoadScene("GameScene");
	}

	public void BackToMainMenu()
	{
		ClickingSound.clickSound();
		SceneManager.LoadScene("MainMenu");
	}

	public void ExitButton()
	{
		ClickingSound.clickSound();
		Application.Quit();
	}
}
