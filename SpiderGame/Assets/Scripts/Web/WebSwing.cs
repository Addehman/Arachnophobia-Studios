using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebSwing : MonoBehaviour
{
    [SerializeField]
    public WebPendulum pendulum;
    void Start()
    {
        pendulum.Initialise();
    }

    void Update()
    {
        transform.localPosition = pendulum.MoveSpider(transform.localPosition, Time.deltaTime);
    }
}
