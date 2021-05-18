using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EricAlert : MonoBehaviour
{
    public RaycastToggler raycastToggler;
    SoundManager soundManager;
    public SpiderMovement spiderMovement;
    public GameOver gameOver;

    public Animator animatorDoor1;
    public Animator animatorDoor2;
    public Animator ericAnimator1;
    public Animator ericAnimator2;
    public Animator ericWalking1;
    public Animator ericWalking2;
    public Animator spiderAnimator;
    public Animator flySwatterAnimator;

    AudioSource audioSourceEric;
    public AudioSource audioSourceFlySwatter;

    public GameObject eric1;
    public GameObject eric2;
    public GameObject ericsVision;
    public GameObject flySwatter;
    public GameObject ericIncWarning;
    public GameObject ericDetectedWarning;

    public int ericSpawnPosition;

    float currentTime = 0f;
    float currentRaycastTimer = 0f;
    float deadAnimationTimer = 0f;
    float footStepAnimationTimer = 0f;

    float ericSpawnTimer;
    float ericWarningTimer = 50f;
    float ericHmmTimer = 10f;
    float ericOpenDoorTimer = 15f;
    float ericBeginRaycastTimer = 2f;
    float ericExitTimer = 5f;
    float detectionTimer;
    float footStepCounter;

    public bool playerDetected = false;
    bool isRaycasting = false;
    bool showedGameOver = false;
    bool animationSwatterPlayed = false;
    bool ericExitRoomFirstTime = false;

    Coroutine currentCoroutine;

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
    //    animatorDoor = GetComponent<Animator>();
        audioSourceEric = GetComponent<AudioSource>();
        soundManager = FindObjectOfType<SoundManager>();
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
                    ericsVision.SetActive(true);
                    playerDetected = true;
                    ericDetectedWarning.SetActive(true);
                    Debug.Log("Player Detected");
                }
                else
                {
                    ericsVision.SetActive(false);
                    playerDetected = false;
                    ericDetectedWarning.SetActive(false);
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
            deadAnimationTimer += Time.deltaTime;

            if (detectionTimer >= 3f)
            {
                if(showedGameOver == false)
                {
                    audioSourceEric.clip = Resources.Load<AudioClip>("Audio/Detected");
                    audioSourceEric.PlayOneShot(Resources.Load<AudioClip>("Audio/Detected"));

                    flySwatter.SetActive(true);
                    flySwatterAnimator.SetTrigger("Kill");
                    gameOver.GameOverScreen();
                    showedGameOver = true;
                    audioSourceFlySwatter.mute = true;

                    ericAnimator1.SetTrigger("Detected");
                    ericAnimator2.SetTrigger("Detected");
                }
            }

            if (deadAnimationTimer >= 3.6f)
            {
                {
                    if(animationSwatterPlayed == false)
                    {
                        audioSourceFlySwatter.mute = false;
                        animationSwatterPlayed = true;
                        spiderAnimator.SetBool("Dead", true);
                        audioSourceFlySwatter.Play();
                    }
                }
            }
        }
        else if(playerDetected == false)
        {
            deadAnimationTimer = 0f;
            detectionTimer = 0f;
        }

        //Eric is not active and not in room
        if (currentTime >= ericSpawnTimer && currentState == State.EricNotInRoom)
        {
            Debug.Log("Eric is not in room/respawning");

            ericWalking1.SetBool("Inc", false);
            ericWalking2.SetBool("Inc", false);

            if (ericExitRoomFirstTime == true)
            {
                StopCoroutine(currentCoroutine);
            }

            raycastToggler.RandomEricPosition();
            
            if (raycastToggler.ericSpawnPosition == 0)
            {
                animatorDoor1.SetTrigger("IdleDoor");
            }
            else if (raycastToggler.ericSpawnPosition == 1)
            {
                animatorDoor2.SetTrigger("IdleDoor");
            }

            currentTime = 0f;
            currentState = State.EricInc;
        }

        //Eric incomming, player should find a hide spot during this timer
        if (currentTime >= ericWarningTimer && currentState == State.EricInc)
        {
            Debug.Log("Eric inc");

            eric1.SetActive(false);
            eric2.SetActive(false);

            audioSourceEric.clip = Resources.Load<AudioClip>("Audio/EricIncStep2");
            audioSourceEric.loop = true;
            audioSourceEric.Play();

            ericIncWarning.SetActive(true);

            /*            StartCoroutine(FootStep());
                        currentCoroutine = StartCoroutine(FootStep());*/

            if (raycastToggler.ericSpawnPosition == 0)
            {
                ericWalking1.SetBool("Inc", true);
            }
            else if (raycastToggler.ericSpawnPosition == 1)
            {
                ericWalking2.SetBool("Inc", true);
            }

            currentTime = 0f;
            currentState = State.EricOpenDoor; 
        }

        //Eric Open Door
        if(currentTime >= ericOpenDoorTimer && currentState == State.EricOpenDoor)
        {
            Debug.Log("Eric Open door");

/*            StopCoroutine(currentCoroutine);*/
            audioSourceEric.clip = Resources.Load<AudioClip>("Audio/KickDoor");
            audioSourceEric.loop = false;
            audioSourceEric.Play();


            if (raycastToggler.ericSpawnPosition == 0)
            {
                animatorDoor1.SetTrigger("OpenDoor");
                eric1.SetActive(true);
            }
            else if (raycastToggler.ericSpawnPosition == 1)
            {
                animatorDoor2.SetTrigger("OpenDoor");
                eric2.SetActive(true);
            }

            ericAnimator1.SetBool("OpenDoor", true);
            ericAnimator2.SetBool("OpenDoor", true);

            currentTime = 0f;
            currentState = State.EricRaycast;
        }

        //Eric is entering room and starting raycasting
        if (currentTime >= ericBeginRaycastTimer && currentState == State.EricRaycast)
        {
            Debug.Log("Eric enter, doing Raycast");

            StopAllCoroutines();

            ericAnimator1.SetBool("OpenDoor", false);
            ericAnimator1.SetBool("LookAround", true);
            ericAnimator2.SetBool("OpenDoor", false);
            ericAnimator2.SetBool("LookAround", true);

            isRaycasting = true;

            currentTime = 0f;
            currentState = State.EricHmm;
        }

        //Eric turn around and exiting room, player dont have to worry anymore
        if (currentTime >= ericHmmTimer && currentState == State.EricHmm)
        {
            Debug.Log("Eric no detection");

            ericAnimator1.SetBool("LookAround", false);
            ericAnimator1.SetBool("Hmm", true);
            ericAnimator2.SetBool("LookAround", false);
            ericAnimator2.SetBool("Hmm", true);

            ericDetectedWarning.SetActive(false);
            ericsVision.SetActive(false);
            playerDetected = false;
            isRaycasting = false;

            ericIncWarning.SetActive(false);

            audioSourceEric.clip = Resources.Load<AudioClip>("Audio/EricEnterRoom");
            audioSourceEric.Play();

            currentTime = 0f;
            currentState = State.EricExit;
        }

        //Nothing happens here, door is closed
        if(currentTime >= ericExitTimer && currentState == State.EricExit)
        {
            Debug.Log("Eric exit room");

            ericAnimator1.SetBool("false", true);
            ericAnimator2.SetBool("false", true);

            StartCoroutine(CloseDoor());
            currentCoroutine = StartCoroutine(CloseDoor());
            ericExitRoomFirstTime = true;

            if (raycastToggler.ericSpawnPosition == 0)
            {
                animatorDoor1.SetTrigger("CloseDoor");
            }
            else if (raycastToggler.ericSpawnPosition == 1)
            {
                animatorDoor2.SetTrigger("CloseDoor");
            }        

            ericSpawnTimer = Random.Range(3f, 5f);

            currentTime = 0f;
            currentState = State.EricNotInRoom;
        }

        timer.text = currentTime.ToString();
    }

/*    IEnumerator FootStep()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.6f);

            audioSourceEric.clip = Resources.Load<AudioClip>("Audio/EricFootStep");
            audioSourceEric.Play();
        }
    }*/

    IEnumerator CloseDoor()
    {
        {
            yield return new WaitForSeconds(0.8f);

            audioSourceEric.clip = Resources.Load<AudioClip>("Audio/EricCloseDoor");
            audioSourceEric.Play();
        }
    }
}
