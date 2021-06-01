using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
	public EventSystem eventSystem;
	public GameObject mainMenu;
	public GameObject playButton;
	public GameObject howToPlay;
	public GameObject options;
	public GameObject credits;
	public Animator fadeOut;
	public AudioMixer audioMixer;


	private void Start()
	{
		Winstate.RemoveCompletedQuests();
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.Confined;

		float volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
		volume = Mathf.Log10(volume) * 20;
		audioMixer.SetFloat("Master", volume);

		Time.timeScale = 1f;
		AudioListener.volume = 1f;
	}

	private void Update()
	{
		if (Input.GetButtonDown("Back"))
		{
			Back();
		}

		if (fadeOut.GetCurrentAnimatorStateInfo(0).IsName("FastFadeOut") && fadeOut.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
		{
			FadeToPlay();
		}
	}

	public void PlayButton()
	{
		ClickingSound.clickSound();
		fadeOut.Play("FastFadeOut");
		// SceneManager.LoadScene("Cutscene");
	}

	public void HowToPlayButton()
	{
		ClickingSound.clickSound();
		SceneManager.LoadScene("TutorialScene");
	}

	public void CreditsButton()
	{
		ClickingSound.clickSound();
		credits.SetActive(true);
	}

	public void QuitGameButton()
	{
		ClickingSound.clickSound();
		Application.Quit();
	}

	public void Back()
	{
		ClickingSound.clickSound();
		credits.SetActive(false);
		howToPlay.SetActive(false);
		options.SetActive(false);
		mainMenu.SetActive(true);

		eventSystem.SetSelectedGameObject(playButton);
	}

	public void GenericClickButton()
	{
		ClickingSound.clickSound();
	}

	public void FadeToPlay()
	{
		SceneManager.LoadScene("Cutscene");
	}
}
