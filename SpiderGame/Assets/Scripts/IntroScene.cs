using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= 7f)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
