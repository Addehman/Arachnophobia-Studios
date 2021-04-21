using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public bool isActive;
    public bool isCompleted;

    public string title;
    public string description;

    public QuestFinished finished;

    public void Complete()
    {
        isActive = false;
        isCompleted = true;
        Debug.Log(title + " was completed!");
    }
    // Add here if we want more things for our quest.

    public void SetQuest(QuestFinished questRequirements)
    {
        isActive = true;
        finished = questRequirements;
        isCompleted = false;
        Debug.Log($"QuestRequirements{questRequirements.questType}");
    }
}
