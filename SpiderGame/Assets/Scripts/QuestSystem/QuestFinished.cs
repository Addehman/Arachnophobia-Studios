using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestFinished
{
    public QuestGoals questType;

    public int requiredAmount;
    public int currentAmount;

    public Player player;

    public QuestGiver questgiver;

    public bool IsReached()
    {
        return (currentAmount >= requiredAmount);
    }

    public void FruitCollected()
    {
        if (questType == QuestGoals.GatherFood)
        {
            currentAmount++;
            /*player.GoDoQuest();*/
        }
        // Make sure this is hooked up to collecatles and player - easy to expand to other quests aswell. 
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
