using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Flies : MonoBehaviour
{
    public enum FlyType { Fly1, Fly2, Fly3 }
    public FlyType flytype;

    [Range(-2f, 2f)] // Restricted to a certain range, shown as a slider.
    public float horizontalSpeed;

    [Range(0, 5)]
    public float verticalSpeed;

    [Range(0, 5)]
    public float depthSpeed;

    [Range(0, 10)]
    public float amplitude;

    private Vector3 originalPos;
    private Vector3 tempPosition;

    void Start()
    {
        tempPosition = originalPos = transform.position;
    }

    void FixedUpdate()
    {
        if (flytype == FlyType.Fly1)
        {
            Flies1();
        }
        else if (flytype == FlyType.Fly2)
        {
            Flies2();
        }
    }

    void Flies1()
    {
        tempPosition = originalPos;
        tempPosition.x += Mathf.Sin(Time.realtimeSinceStartup * horizontalSpeed) * amplitude;
        tempPosition.y += Mathf.Sin(Time.realtimeSinceStartup * verticalSpeed) * amplitude;
        tempPosition.z += Mathf.Sin(Time.realtimeSinceStartup * depthSpeed) * amplitude;
        transform.position = tempPosition;
    }

    void Flies2()
    {
        tempPosition = originalPos;
        tempPosition.x += Mathf.Cos(Time.realtimeSinceStartup * horizontalSpeed) * amplitude;
        tempPosition.y += Mathf.Cos(Time.realtimeSinceStartup * verticalSpeed) * amplitude;
        tempPosition.z += Mathf.Cos(Time.realtimeSinceStartup * depthSpeed) * amplitude;
        transform.position = tempPosition;
    }

}