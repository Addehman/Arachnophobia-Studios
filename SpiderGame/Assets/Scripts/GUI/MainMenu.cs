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
        ClickSound.PlayClickSFX();
        SceneManager.LoadScene("GameScene");
    }

    public void HowToPlayButton()
    {
        ClickSound.PlayClickSFX();
        SceneManager.LoadScene("TutorialScene");
    }

    public void CreditsButton()
    {
        ClickSound.PlayClickSFX();
        credits.SetActive(true);
    }
    
    public void GenericClickButton()
    {
        ClickSound.PlayClickSFX();
        
    }

    public void QuitGameButton()
    {
        ClickSound.PlayClickSFX();
        Application.Quit();
    }

    public void Back()
    {
        ClickSound.PlayClickSFX();
        credits.SetActive(false);
        howToPlay.SetActive(false);
    }

}
