using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebQuest : MonoBehaviour
{
    public ChangeCamera changeCamera;
    public GameObject web;
    public GameObject particle;

    int questCompleted;

    bool isOnWebSpot = false;
    bool isWebPlaced = false;

    private void Start()
    {
        questCompleted = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && isOnWebSpot == true && isWebPlaced == false)
        {
            questCompleted++;
            web.SetActive(true);
            isWebPlaced = true;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log(questCompleted);
            Debug.Log("Enter webspot");
            isOnWebSpot = true;

            if(isWebPlaced == false)
            {
                particle.SetActive(true);
            }
            else
            {
                particle.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider exit)
    {
        if (exit.gameObject.tag == "Player")
        {
            Debug.Log("Exit webspot");
            isOnWebSpot = false;
            particle.SetActive(false);
        }
    }
}
