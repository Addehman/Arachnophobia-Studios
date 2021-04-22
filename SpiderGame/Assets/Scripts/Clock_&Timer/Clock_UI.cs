using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock_UI : MonoBehaviour
{
    //Real seconds, 5 in this case will be one day in game
    private const float REAL_SECONDS_PER_INGAME_DAY = 60f;

    public Transform ClockHourHandTransform;
    public Transform ClockMinuteHandTransform;
    private float day;

    private void Update()
    {
        day += Time.deltaTime / REAL_SECONDS_PER_INGAME_DAY;

        //8 hour inGameDay, 480 sek, 8 min
        float dayNormal = day % 1f;

        float rotationDegreesPerDay = 360f;
        ClockHourHandTransform.eulerAngles = new Vector3(0, 0, -dayNormal * rotationDegreesPerDay);

        float hoursPerDay = 12f;
        ClockMinuteHandTransform.eulerAngles = new Vector3(0, 0, -dayNormal * rotationDegreesPerDay * hoursPerDay);
    }
}
