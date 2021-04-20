using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Quest quest;
    int fruit = 1;

    public void GoExploreTheWorld()
    {
        int fruit = 1;
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
}
