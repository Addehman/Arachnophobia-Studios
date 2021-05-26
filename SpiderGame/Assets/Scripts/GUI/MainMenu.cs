using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
	public GameObject fadeIn;
	public GameObject howToPlay;
	public GameObject credits;
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

	public void PlayButton()
	{
		ClickingSound.clickSound();
		SceneManager.LoadScene("Cutscene");
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
	}

	public void GenericClickButton()
	{
		ClickingSound.clickSound();
	}
}
