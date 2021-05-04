using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastToggler : MonoBehaviour
{
    public GameObject ericPosition1;
    public GameObject ericPosition2;

    public int ericSpawnPosition;

    public EricAlert ericAlert;

    public void RandomEricPosition()
    {
        ericSpawnPosition = Random.Range(0, 2);
     //   Debug.Log("RandomPositionIs: " + ericSpawnPosition);

        if (ericSpawnPosition == 0)
        {
            ericPosition2.SetActive(false);
            ericPosition1.SetActive(true);
        }
        else
        {
            ericPosition1.SetActive(false);
            ericPosition2.SetActive(true);
        }
    }
}
