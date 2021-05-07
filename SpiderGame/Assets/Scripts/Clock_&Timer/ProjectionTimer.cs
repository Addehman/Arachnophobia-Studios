using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionTimer : MonoBehaviour
{
    Projector proj;
    Color myColor;
    float colorIncreaseValue = 0.33f;
    float increaseValue = 0.05f; //Use 20 for proj.fieldOfView

    private void Awake()
    {
        proj = GetComponent<Projector>();
        myColor = new Color(0, 0, 0, 1);
    }

    void Update()
    {
        if (proj.orthographicSize < 0.15)
        {
            proj.material.color = myColor;

            myColor.r += colorIncreaseValue * Time.deltaTime;

            proj.orthographicSize += increaseValue * Time.deltaTime;
        }
    }
    
    private void OnDisable()
    {
        proj.orthographicSize = 0.01f;
        proj.material.color = new Color(0, 0, 0, 1);
    }
}
