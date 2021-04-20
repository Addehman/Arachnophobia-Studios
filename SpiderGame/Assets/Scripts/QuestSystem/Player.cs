using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public int health = 5;

    public Quest quest;

    public void GoExploreTheWorld()
    {
        // First quest - go from A to B || place 3 spiderwebs
        health = 1;

        if (quest.isActive)
        {
            quest.finished.FruitCollected();
            if (quest.finished.IsReached())
            {
                quest.Complete();
            }
        }
    }
}
