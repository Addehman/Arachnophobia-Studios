using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EricAlert : MonoBehaviour
{
    float currentTime = 0f;

    float ericSpawnTimer;
    float ericWarningTimer = 10f;
    float ericExitTimer = 5f;
    float ericDelayTimer = 5f;

    IEnumerator ericWarningCoroutine;
    IEnumerator ericExitCoroutine;

    public Text timer;

    State currentState;

    enum State
    {
        EricNotInRoom,
        EricInc,
        EricEnter,
        EricExit
    }

    bool isEricInc = false;
    bool isEricInRoom = true;
    void Start()
    {
        currentState = State.EricNotInRoom;
        ericSpawnTimer = Random.Range(4f, 10f);
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if(currentTime >= ericSpawnTimer && currentState == State.EricNotInRoom)
        {
            Debug.Log("Eric inc");
            currentTime = 0f;
            currentState = State.EricInc;
        }

        if(currentTime >= ericWarningTimer && currentState == State.EricInc)
        {
            Debug.Log("Eric enter room");
            currentTime = 0f;
            currentState = State.EricEnter;
        }

        if(currentTime>= ericExitTimer && currentState == State.EricEnter)
        {
            Debug.Log("Eric exit room");
            currentTime = 0f;
            currentState = State.EricExit;
        }

        if(currentTime >= ericDelayTimer && currentState == State.EricExit)
        {
            Debug.Log("Eric timer reset");
            currentTime = 0f;
            currentState = State.EricNotInRoom;
        }

        timer.text = currentTime.ToString();
    }
}
