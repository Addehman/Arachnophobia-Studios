using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        AudioListener.volume = 1f;
    }
    public void MainMenuButtonVictory()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
