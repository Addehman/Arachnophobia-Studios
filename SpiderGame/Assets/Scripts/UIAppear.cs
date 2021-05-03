using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Will show the image for our quest. When spider steps on platform - show questwindow - open up accept.

public class UIAppear : MonoBehaviour
{
    [SerializeField] private GameObject window;

    public Player player;
    public bool isFinished = false;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && player.quest.isCompleted == false && player.quest.isAccepted == false)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Debug.Log("In");
            window.SetActive(true);
        }

        if (other.CompareTag("Player") && player.quest.isCompleted == true && isFinished == false)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
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
