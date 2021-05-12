using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookQuest : MonoBehaviour
{
    [SerializeField] private GameObject window;
    public Player player;
    public UIInventory uiInventory;
    public GameObject check;
    public GameObject helpText;
    public GameObject questCircle;
    public PickUpObject pickUpObject;
    public bool isFinished = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isFinished == false && pickUpObject.isAllItemsCollected == true)
        {
            helpText.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            window.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isFinished == false && pickUpObject.isAllItemsCollected == true)
        {
            helpText.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            window.SetActive(false);
        }
    }

    public void CookButton()
    {
        helpText.SetActive(false);
        Winstate.AddCompletedQuest();
        isFinished = true;
        check.SetActive(true);
        window.SetActive(false);
        questCircle.SetActive(false);
        ClearInventory();
    }

    public void ClearInventory()
    {
        for (int i = 0; i < uiInventory.uIItems.Count; i++)
        {
            uiInventory.uIItems[i].item = null;
            uiInventory.uIItems[i].spriteImage.color = Color.clear;
        }
    }
}
