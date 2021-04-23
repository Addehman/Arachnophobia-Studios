using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    public void GoDoQuest() //This function hold the quest that is needed to be done right now. At the moment, fruitcollected.
    {
        if (quest.isAccepted)
        {
            quest.finished.GatherFood();
            if (quest.finished.IsReached())
            {
                quest.Complete();
            }
        }
    }
}
