using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// TODO: Check so the quest we want is added here.

public class QuestNPC : MonoBehaviour
{
    public Quest quest;
    public Player player;
    public Inventory inventory;
    public UIInventory uiInventory;
    public ItemInfo itemInfo;
    

    public GameObject questWindow;
    public GameObject deliverWindow;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    public void OpenQuestWindow()
    {
        questWindow.SetActive(true);
        titleText.text = quest.title;
        descriptionText.text = quest.description;
    }

    public void AcceptQuest()
    {
        questWindow.SetActive(false);
        quest.SetQuest(quest.finished);
        player.quest = quest;
        gameObject.SetActive(false);
    }

    public void DeliverQuest()
    {
        deliverWindow.SetActive(true);
        quest.Complete();
        player.quest = quest;
        gameObject.SetActive(false);
/*        ClearInventory();*/
        //Add so that AcceptQuest will be accepted and finished instead of going through entire process.
    }
/*
    public void ClearInventory()
    {
        uiInventory.DestroySlotPrefab();
        uiInventory.InstantiateSlotPrefab(); // add new - not multiply



        *//*uiInventory.uIItems.Clear();
        inventory.RemoveItem("Banana");*//*
    }*/

}
