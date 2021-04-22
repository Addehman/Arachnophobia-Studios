using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Player - Instead of GetKeyDown P - place things on collection or spider web to find out how to make it.

public class Player : MonoBehaviour
{
    public Quest quest;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GoDoQuest();
        }
    }

    public void GoDoQuest() // First quest - go from A to B || place 3 spiderwebs
    {
        if (quest.isAccepted)
        {
            quest.finished.FruitCollected();
            if (quest.finished.IsReached())
            {
                quest.Complete();
            }
        }
    }
    // Make sure that if the required amount is reached for the gathering  quest - it should say finish.
}
