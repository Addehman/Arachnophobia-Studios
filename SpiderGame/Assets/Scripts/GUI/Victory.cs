using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
    public GameObject heart;
    public GameObject victoryUI;
    float vcitoryTimer = 0f;
    float soundFadeOutTimer = 0f;
    bool itIsGameOver = false;

    private void Start()
    {
        victoryUI.SetActive(false);
        AudioListener.volume = 1f;
    }

    private void Update()
    {
        if(Winstate.isVictory == true)
        {
            VictoryScreen();
        }
    }
    public void VictoryScreen()
    {
        victoryUI.SetActive(true);
        Winstate.RemoveCompletedQuests();

        {
            vcitoryTimer += Time.deltaTime;
            StartCoroutine(soundFadeOut(0.005f));
            soundFadeOutTimer += Time.deltaTime;

            if (vcitoryTimer >= 3f)
            {
                heart.SetActive(true);

                if(vcitoryTimer >= 4.5f)
                {
                    SceneManager.LoadScene("VictoryScene");
                }
            }
        }
    }

    IEnumerator soundFadeOut(float speed)
    {
        float audioVolume = AudioListener.volume;

        while (audioVolume >= speed)
        {
            audioVolume -= speed;
            AudioListener.volume = audioVolume;

            yield return new WaitForSeconds(0.02f);
        }
    }
}
