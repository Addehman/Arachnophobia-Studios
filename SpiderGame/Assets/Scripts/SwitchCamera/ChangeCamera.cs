using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    public GameObject thirdPersonCamera;
    public GameObject firstPersonCamera;
    public int camMode;

    private void Update()
    {
        if(Input.GetButtonDown("Camera"))
        {
            if(camMode == 1)
            {
                camMode = 0;
            }
            else
            {
                camMode += 1;
            }

            StartCoroutine(CamChange());
        }
    }

    IEnumerator CamChange()
    {
        yield return new WaitForSeconds(0.01f);
        if(camMode == 0)
        {
            thirdPersonCamera.SetActive(true);
            firstPersonCamera.SetActive(false);
        }
        if(camMode == 1)
        {
            thirdPersonCamera.SetActive(false);
            firstPersonCamera.SetActive(true);
        }
    }
}
