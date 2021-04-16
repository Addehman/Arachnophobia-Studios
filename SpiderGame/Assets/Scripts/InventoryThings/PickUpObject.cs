using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    bool canPickUp = false;
    public int itemID;
    private Inventory inventoryOnPlayer;


    private void Start()
    {
        inventoryOnPlayer = FindObjectOfType<Inventory>();
    }
    private void Update()
    {
        if (canPickUp && Input.GetKeyDown(KeyCode.E))
        {
            inventoryOnPlayer.GiveItem(itemID);
            print("Picked Up Object");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            canPickUp = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            canPickUp = false;
        }
    }
}



