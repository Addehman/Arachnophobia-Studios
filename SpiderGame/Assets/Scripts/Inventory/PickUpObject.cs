using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Makes it possible to pick up and check the object via the Inventory

public class PickUpObject : MonoBehaviour
{
    public bool canPickUp = false;
    public bool isAllItemsCollected = false;

    public int itemID;
    [SerializeField]
    public int numberOfItemsPickedUp;

    private Inventory inventoryOnPlayer;
    private GameObject thisObjectThatWeStandOn;
    public GameObject helpText;

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
            helpText.SetActive(false);

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

        if(numberOfItemsPickedUp >= 9)
        {
            isAllItemsCollected = true;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("PickUpAble"))
        {
            helpText.SetActive(true);
            canPickUp = true;
            thisObjectThatWeStandOn = collider.gameObject;
            itemID = thisObjectThatWeStandOn.GetComponent<ItemID>().itemID;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("PickUpAble")) // Player
        {
            helpText.SetActive(false);
            thisObjectThatWeStandOn = null;
            canPickUp = false;
        }
    }
}

