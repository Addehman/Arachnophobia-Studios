using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    float timer;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= 8f)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
