using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public Player player;
/*    public int currentAmountOut;*/

    public QuestFinished questfinished;

    public GameObject questWindow;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            questfinished.currentAmount++;
            questfinished.FruitCollected();
            print(questfinished.currentAmount);
        }
 /*       currentAmountOut = questfinished.currentAmount;*/ 
    }

    public void OpenQuestWindow()
    {
        questWindow.SetActive(true);
        titleText.text = quest.title;
        descriptionText.text = quest.description;
    }

    public void AcceptQuest()
    {
        questWindow.SetActive(false);
        quest.isActive = true;
        player.quest = quest;
    }

}
