using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Will show the image for our quest. When spider steps on platform - show questwindow - open up accept.

public class UIAppear : MonoBehaviour
{
    [SerializeField] private GameObject window;

    public Player player;
/*
    void Update()
    {
        //Press the space bar to apply no locking to the Cursor
        if (Input.GetKey(KeyCode.Q))
        {
            Cursor.lockState = CursorLockMode.Confined;

        }

    }*/

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && player.quest.isCompleted == false && player.quest.isAccepted == false)
        {

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Debug.Log("In");
            window.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Cursor.visible = false;
            Debug.Log("Out");
            window.SetActive(false);
        }
    }
}
