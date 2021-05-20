using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock_2Script : MonoBehaviour
{
    public Transform SecondHand;
    public Transform MinuteHand;
    public Transform HourHand;

    void Update()
    {
        DateTime currentTime = DateTime.Now;
        float second = (float)currentTime.Second;
        float minute = (float)currentTime.Minute;
        float hour = (float)currentTime.Hour%12;

        float secondAngle = 360 * (second/60);
        float minuteAngle = 360 * (minute/60);
        float hourAngle = 360 * (hour/12);

        SecondHand.localRotation = Quaternion.Euler(0, 0, -secondAngle);
        MinuteHand.localRotation = Quaternion.Euler(0, 0, -minuteAngle);
        HourHand.localRotation = Quaternion.Euler(0, 0, -hourAngle);
    }
}
