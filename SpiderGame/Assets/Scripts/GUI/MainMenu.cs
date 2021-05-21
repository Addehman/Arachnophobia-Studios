using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public GameObject howToPlay;
    public GameObject credits;
    public AudioMixer audioMixer;

    private void Start()
    {
        float volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        volume = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("Master", volume);
    }

    public void PlayButton()
    {
        GUISound.clickSound();
        SceneManager.LoadScene("GameScene");
    }

    public void HowToPlayButton()
    {
        GUISound.clickSound();
        SceneManager.LoadScene("TutorialScene");
    }

    public void CreditsButton()
    {
        GUISound.clickSound();
        credits.SetActive(true);
    }

    public void QuitGameButton()
    {
        GUISound.clickSound();
        Application.Quit();
    }

    public void Back()
    {
        GUISound.clickSound();
        credits.SetActive(false);
        howToPlay.SetActive(false);
    }
}
