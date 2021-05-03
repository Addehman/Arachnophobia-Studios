using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Winstate 
{
    private static int numberOfQuestCompleted = 0;
    private static int questToComplete = 2;

    public static void AddCompletedQuest()
    {
        numberOfQuestCompleted++;
        if (numberOfQuestCompleted >= questToComplete)
        {
            Debug.Log("Victory"); 
        }
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
