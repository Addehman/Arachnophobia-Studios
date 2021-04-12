using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebSwitchTether : MonoBehaviour
{
    public Transform newTether;
    public WebSwing swing;

    void Start()
    {
        
    }


    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            swing.pendulum.SwitchTether(newTether.transform.position);
        }
    }
}
