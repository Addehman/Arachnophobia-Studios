using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Create more QuestGoals, FruitCollected only checks for pickup. Placement for webs, bevare of things etc - check and fix this

[System.Serializable]
public class QuestFinished
{
    public QuestGoals questType;
    public Player player;
    public QuestNPC questgiver;

    public int requiredAmount;
    public int currentAmount;


    public bool IsReached()
    {
        return (currentAmount >= requiredAmount);
    }

    public void FruitCollected()
    {
        Debug.Log($"QuestType: {questType} ");
        if (questType == QuestGoals.GatherFood)
        {
            currentAmount++;
            Debug.Log("Collected");
        }
        // Make sure this is hooked up to collecatles and player - easy to expand to other quests aswell. 
        Debug.Log($" currentamount:{currentAmount}");
    }

    public enum QuestGoals
    {
        AtoB,
        GatherFood,
        GoExploreTheWorld,
        SpiderwebPlacement,
        Hide,
        GordonRamsey

     // Several different quest we want to achieve in the game
    }
}
