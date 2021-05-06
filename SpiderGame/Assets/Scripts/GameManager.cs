using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	// private void Awake()
	// {
	// 	DontDestroyOnLoad(this);
	// }

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}
}
