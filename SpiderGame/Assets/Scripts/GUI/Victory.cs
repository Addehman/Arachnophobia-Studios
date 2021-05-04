using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour
{
    public GameObject victoryText;

    private void Update()
    {
        if(Winstate.isVictory == true)
        {
            VictoryScreen();
        }
    }
    public void VictoryScreen()
    {
        victoryText.SetActive(true);
    }
}
