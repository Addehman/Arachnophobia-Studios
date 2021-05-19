using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Winstate 
{
    private static int numberOfQuestCompleted = 0;
    private static int questToComplete = 1;  //Update this if a new quest is added
    public static bool isVictory = false;

    public static void AddCompletedQuest()
    {
        numberOfQuestCompleted++;
        if (numberOfQuestCompleted >= questToComplete)
        {
            isVictory = true;
            Debug.Log("Victory"); 
        }
    }

    public static void RemoveCompletedQuests()
    {
        numberOfQuestCompleted = 0;
    }

/*
    public static void Init(int questToComplete)
    {
        Winstate.questToComplete = questToComplete;
        numberOfQuestCompleted = 0;
    }*/
    //Add new function for quest to complete pending on how many queest needed to win.
    //Function that clears number of quest completed.
}
