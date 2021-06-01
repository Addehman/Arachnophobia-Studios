using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	private static GameManager gameManager;
	public static GameManager Instance {
		get
		{
			if (gameManager == null)
			{
				Debug.LogError("GameManager is Null.");
			}
			return gameManager;
		}
	}

	public GameObject fadeScreen;
	
	private Image fadeScreenImage;


	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		PauseMenu.isPaused = false;

		if (GameObject.Find("FadeOut"))
		{
			fadeScreen = GameObject.Find("FadeOut");
			fadeScreenImage = fadeScreen.GetComponent<Image>();
			fadeScreenImage.color = Color.black;
		}
	}
}
