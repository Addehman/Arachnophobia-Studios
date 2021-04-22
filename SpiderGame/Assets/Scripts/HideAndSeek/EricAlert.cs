using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EricAlert : MonoBehaviour
{
    public SoundManager soundManager;
    public SpiderMovement spiderMovement;
    public GameOver gameOver;
    public Animator animator;

    float currentTime = 0f;
    float currentRaycastTimer = 0f;

    float ericSpawnTimer;
    float ericWarningTimer = 10f;
    float ericHmmTimer = 10f;
    float ericOpenDoorTimer = 5f;
    float ericBeginRaycastTimer = 2f;
    float ericExitTimer = 5f;
    float detectionTimer;

    public bool playerDetected = false;
    bool isRaycasting = false;

    public Text timer;

    State currentState;

    enum State
    {
        EricNotInRoom,
        EricInc,
        EricOpenDoor,
        EricRaycast,
        EricHmm,
        EricExit
    }

    void Start()
    {
        currentState = State.EricNotInRoom;
        ericSpawnTimer = Random.Range(7f, 10f);
    }

    private void FixedUpdate()
    {
        Debug.DrawLine(transform.position, spiderMovement.currentPosition, Color.red, 1.0f);
        
        RaycastHit hit;

        if (isRaycasting == true)
        {
            if (Physics.Linecast(transform.position, spiderMovement.currentPosition, out hit))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    playerDetected = true;
                    Debug.Log("Player Detected");
                }
                else
                {
                    playerDetected = false;
                    Debug.Log("No detection");
                }
            }
        }
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if(playerDetected == true)
        {
            detectionTimer += Time.deltaTime;

            if (detectionTimer >= 3f)
            {
                //not playing 
                soundManager.Detected();
                gameOver.GameOverScreen();
            }
        }
        else if(playerDetected == false)
        {
            detectionTimer = 0f;
        }

        //Eric is not active and not in room
        if (currentTime >= ericSpawnTimer && currentState == State.EricNotInRoom)
        {
            Debug.Log("Eric not room");

            animator.SetTrigger("IdleDoor");

            currentTime = 0f;
            currentState = State.EricInc;
        }

        //Eric incomming, player should find a hide spot during this timer
        if (currentTime >= ericWarningTimer && currentState == State.EricInc)
        {
            Debug.Log("Eric inc");

            soundManager.audioSource.loop = true;
            soundManager.EricFootStep();

            currentTime = 0f;
            currentState = State.EricOpenDoor; 
        }

        //Eric Open Door
        if(currentTime >= ericOpenDoorTimer && currentState == State.EricOpenDoor)
        {
            Debug.Log("Eric Open door");

            soundManager.audioSource.loop = false;
            soundManager.Door();
            animator.SetTrigger("OpenDoor");

            currentTime = 0f;
            currentState = State.EricRaycast;
        }

        //Eric is entering room and starting raycasting
        if (currentTime >= ericBeginRaycastTimer && currentState == State.EricRaycast)
        {
            Debug.Log("Eric enter, doing Raycast");
            isRaycasting = true;

            currentTime = 0f;
            currentState = State.EricHmm;
        }

        //Eric turn around and exiting room, player dont have to worry anymore
        if (currentTime >= ericHmmTimer && currentState == State.EricHmm)
        {
            Debug.Log("Eric no detection");

            playerDetected = false;
            isRaycasting = false;
            soundManager.EricHmm();

            currentTime = 0f;
            currentState = State.EricExit;
        }

        //Nothing happens here, door is closed
        if(currentTime >= ericExitTimer && currentState == State.EricExit)
        {
            Debug.Log("Eric exit room");

            soundManager.Door();
            animator.SetTrigger("CloseDoor");

            ericSpawnTimer = Random.Range(3f, 5f);

            currentTime = 0f;
            currentState = State.EricNotInRoom;
        }


        timer.text = currentTime.ToString();
    }
}
