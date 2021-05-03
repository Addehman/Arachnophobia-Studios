using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookQuest : MonoBehaviour
{
    [SerializeField] private GameObject window;
    public Player player;
    public QuestFinished questFinished;
    public GameObject check;
    public bool isFinished = false;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(questFinished.currentAmount);
        if (other.CompareTag("Player") && isFinished == false && questFinished.currentAmount == 1)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            window.SetActive(true);
            check.SetActive(true);
        }
    }
}
