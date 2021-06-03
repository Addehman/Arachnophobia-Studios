using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
	public GameObject pauseMenu;
	public GameObject howToPlay;
	public GameObject resumeBtn;
	public GameObject pauseMenuMain;
	public GameObject settingsMenu;
	public GameObject settingsBtn;
	public static bool isPaused;

	void Start()
	{
		pauseMenu.SetActive(false);
	}


	void Update()
	{
		if (Input.GetButtonDown("PauseButton"))
		{
			if(isPaused)
			{
				ResumeGame();
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
				AudioListener.volume = 1f;
				
			}
			else
			{
				PauseGame();
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.Confined;
				AudioListener.volume = 0f;
				EventSystem.current.SetSelectedGameObject(null);
				EventSystem.current.SetSelectedGameObject(resumeBtn);
			}
		}

		if (Input.GetButtonDown("Back"))
		{
			settingsMenu.SetActive(false);
			pauseMenuMain.SetActive(true);
			EventSystem.current.SetSelectedGameObject(null);
			EventSystem.current.SetSelectedGameObject(settingsBtn);
		}
	}

	public void PauseGame()
	{
		pauseMenu.SetActive(true);
		pauseMenuMain.SetActive(true);
		Time.timeScale = 0f;
		isPaused = true;
	}


	public void ResumeGame()
	{
		pauseMenu.SetActive(false);
		settingsMenu.SetActive(false);
		Time.timeScale = 1f;
		isPaused = false;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		AudioListener.volume = 1f;
	}

	public void GoToMainMenu()
	{
		Time.timeScale = 1f;
		isPaused = false;
		SceneManager.LoadScene("MainMenu");
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void OpenHowToPlay()
	{
		howToPlay.SetActive(true);
	}

	public void CloseHowToPlay()
	{
		howToPlay.SetActive(false);
	}
}
