using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebSelector : MonoBehaviour
{
    //Make selectedWeb public if we want to listen to it from the spring-/hook-/climbWeb scripts instead.
    private int selectedWeb = 0;
    SpringJointWeb springJointWeb;
    HookWeb hookWeb;
    ClimbWeb climbWeb;

    void Start()
    {
        SelectWeb();
        springJointWeb = FindObjectOfType<SpringJointWeb>();
        hookWeb = FindObjectOfType<HookWeb>();
        climbWeb = FindObjectOfType<ClimbWeb>();
    }

    void Update()
    {
        int previousSelectedWeb = selectedWeb;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeb = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeb = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedWeb = 2;
        }

        if (previousSelectedWeb != selectedWeb)
        {
            SelectWeb();
        }

        if (selectedWeb == 0)
        {
            springJointWeb.enabled = true;
            hookWeb.enabled = false;
            climbWeb.enabled = false;
        }
        else if (selectedWeb == 1)
        {
            springJointWeb.enabled = false;
            hookWeb.enabled = true;
            climbWeb.enabled = false;
        }
        else
        {
            springJointWeb.enabled = false;
            hookWeb.enabled = false;
            climbWeb.enabled = true;
        }

    }

    private void SelectWeb()
    {
        int i = 0;

        foreach (Transform web in transform)
        {
            if (i == selectedWeb)
            {
                web.gameObject.SetActive(true);
            }
            else
            {
                web.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
