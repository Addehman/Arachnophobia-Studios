using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Makes it possible to pick up and check the object via the Inventory

public class PickUpObject : MonoBehaviour
{
    bool canPickUp = false;
    public int itemID;
    private Inventory inventoryOnPlayer;
    private GameObject thisObjectThatWeStandOn;

    public event Action pickedUpItem;

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

            if (pickedUpItem != null)
            {
                pickedUpItem(); 
            }

            if (thisObjectThatWeStandOn != null)
            {
                Destroy(thisObjectThatWeStandOn);
                canPickUp = false;
                thisObjectThatWeStandOn = null;
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("PickUpAble"))
        {
            canPickUp = true;
            thisObjectThatWeStandOn = collider.gameObject;
            itemID = thisObjectThatWeStandOn.GetComponent<ItemID>().itemID;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("PickUpAble")) // Player
        {
            thisObjectThatWeStandOn = null;
            canPickUp = false;
        }
    }
}

