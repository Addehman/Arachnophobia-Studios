using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// TODO: Player - Instead of GetKeyDown P - place things on collection or spider web to find out how to make it.

// When quest picked up - go for pick up food - check so that pickup collecatable puts in inventory & quest.

public class Player : MonoBehaviour
{
    public Quest quest;
    public PickUpObject pickUpObject;

    private void Start()
    {
        pickUpObject = GetComponent<PickUpObject>();
        pickUpObject.pickedUpItem += PickUpObject_pickedUpItem;
    }

    private void PickUpObject_pickedUpItem()
    {
        GoDoQuest();
    }



    public void GoDoQuest() // First quest - go from A to B || place 3 spiderwebs
    {
        if (quest.isAccepted)
        {
            quest.finished.FruitCollected();
            if (quest.finished.IsReached())
            {
                quest.Complete();
            }
        }
    }
    // Make sure that if the required amount is reached for the gathering  quest - it should say finish.
}
