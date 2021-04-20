using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EricAlert : MonoBehaviour
{
    public SpiderMovement spiderMovement;
    public Animator animator;

    float currentTime = 0f;

    float ericSpawnTimer;
    float ericWarningTimer = 10f;
    float ericExitTimer = 5f;
    float ericDelayTimer = 5f;

    bool isDoorOpen = false;

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

    void Start()
    {
        currentState = State.EricNotInRoom;
        ericSpawnTimer = Random.Range(4f, 10f);
    }

    private void FixedUpdate()
    {
        Debug.DrawLine(transform.position, spiderMovement.currentPosition, Color.red, 1.0f);

        RaycastHit hit;

        if (currentState == State.EricEnter)
        {
            if (Physics.Linecast(transform.position, spiderMovement.currentPosition, out hit))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    Debug.Log("Player Detected");
                }

                else
                {
                    Debug.Log("No detection");
                }
            }
        }
    }

    void Update()
    {
        if (isDoorOpen == true)
        {
           // door.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 45f, 0f), 2 * Time.deltaTime);
        }
        else
        {
           // door.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, 0f), 2 * Time.deltaTime);
        }

        currentTime += Time.deltaTime;

        //Eric incomming, player should find a hide spot during this timer
        if (currentTime >= ericSpawnTimer && currentState == State.EricNotInRoom)
        {
            Debug.Log("Eric inc");

            currentTime = 0f;
            animator.SetTrigger("IdleDoor");
            currentState = State.EricInc;
        }

        //Eric is entering room and starting raycasting
        if (currentTime >= ericWarningTimer && currentState == State.EricInc)
        {
            Debug.Log("Eric enter room");

            currentTime = 0f;
            animator.SetTrigger("OpenDoor");
            currentState = State.EricEnter;
        }

        //Eric turn around and exiting room, player dont have to worry anymore
        if (currentTime >= ericExitTimer && currentState == State.EricEnter)
        {
            Debug.Log("Eric exit room");

            currentTime = 0f;
            currentState = State.EricExit;
        }

        //Nothing happens here, door is closed
        if(currentTime >= ericDelayTimer && currentState == State.EricExit)
        {
            Debug.Log("Eric timer reset");

            currentTime = 0f;
            animator.SetTrigger("CloseDoor");
            currentState = State.EricNotInRoom;
        }


        timer.text = currentTime.ToString();
    }
}
