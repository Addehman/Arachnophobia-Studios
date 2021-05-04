using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Makes it possible to pick up and check the object via the Inventory

public class PickUpObject : MonoBehaviour
{
    bool canPickUp = false;
    public bool isAllItemsCollected = false;

    public int itemID;
    [SerializeField]
    int numberOfItemsPickedUp;

    private Inventory inventoryOnPlayer;
    private GameObject thisObjectThatWeStandOn;

    public event Action pickedUpItem;

    private void Start()
    {
        inventoryOnPlayer = FindObjectOfType<Inventory>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canPickUp == true)
        {
            numberOfItemsPickedUp++;
            Debug.Log("number of items picked up: " + numberOfItemsPickedUp);

            inventoryOnPlayer.GiveItem(itemID);

            if (pickedUpItem != null)
            {
                pickedUpItem(); 
            }

            if (thisObjectThatWeStandOn != null)
            {
                Destroy(thisObjectThatWeStandOn);
                thisObjectThatWeStandOn = null;
            }

            canPickUp = false;
        }

        if(numberOfItemsPickedUp >= 4)
        {
            isAllItemsCollected = true;
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

