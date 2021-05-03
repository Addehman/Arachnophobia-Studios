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
    public UIAppear uiAppear;

    public GameObject questWindow;
    public GameObject deliverWindow;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    // No reference in script - used in Inspector for UI-buttons.
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
        ClearInventory();
        uiAppear.isFinished = true;
    }

    // Resets color on Images for Inventory and clear UiItems list so its refresh for new quest.
    public void ClearInventory()
    {
        for (int i = 0; i < uiInventory.uIItems.Count; i++)
        {
            uiInventory.uIItems[i].item = null;
            uiInventory.uIItems[i].spriteImage.color = Color.clear;
        }
    }
}
