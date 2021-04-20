using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public bool isActive;

    public string title;
    public string description;


    public QuestFinished finished;

    public void Complete()
    {
        isActive = false;
        Debug.Log(title + " was completed!");
    }
    // Add here if we want more things for our quest.
}
