using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastToggler : MonoBehaviour
{
    public GameObject ericRaycast1;
    public GameObject ericRaycast2;

    public EricAlert ericAlert;

    public void Start()
    {
        if(ericAlert.ericSpawnPosition == 0)
        {
            ericRaycast2.SetActive(false);
            ericRaycast1.SetActive(true);
        }
        else if(ericAlert.ericSpawnPosition == 1)
        {
            ericRaycast1.SetActive(false);
            ericRaycast2.SetActive(true);
        }
        else
        {
            Debug.LogError(ericAlert.ericSpawnPosition);
        }
    }
}
