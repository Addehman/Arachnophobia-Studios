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

	private void Update()
	{
		if (Input.GetButtonDown("Back"))
		{
			xButtonVictory();
		}
	}

	public void MainMenuButtonVictory()
	{
		ClickingSound.clickSound();
		SceneManager.LoadScene("MainMenu");
	}

	public void CreditsButtonVictory()
	{
		ClickingSound.clickSound();
		creditsUI.SetActive(true);
	}

	public void xButtonVictory()
	{
		ClickingSound.clickSound();
		creditsUI.SetActive(false);
	}

	public void exiButtonVictory()
	{
		ClickingSound.clickSound();
		Application.Quit();
	}
}
