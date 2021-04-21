using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Quest quest;

    public void GoDoQuest()
    {
        // First quest - go from A to B || place 3 spiderwebs

        if (quest.isActive)
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
