using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerCountdown : MonoBehaviour
{
    public Image timerCircle;
    float maxTime = 60f;
    float currentTime;

    void Start()
    {
        currentTime = maxTime;
    }

    void Update()
    {
        if(currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            timerCircle.fillAmount = currentTime / maxTime;
        }
    }
}
