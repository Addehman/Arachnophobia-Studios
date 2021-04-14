using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WebString : MonoBehaviour
{
    public Transform spider;
    public Transform target1;
    private Transform curTarget;
    LineRenderer stringRenderer;
    //private bool isTarget1 = true;
    public bool hasParent;

    // Use this for initialization
    void Start()
    {
        stringRenderer = GetComponent<LineRenderer>();
        if (hasParent)
        {
            stringRenderer.SetPosition(1, transform.InverseTransformPoint(spider.position));
        }
        else
        {
            stringRenderer.SetPosition(1, spider.position);
        }

        curTarget = target1;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                print(hit.transform.name);
                stringRenderer.enabled = true;
            }
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            stringRenderer.enabled = false;
        }

        stringRenderer = GetComponent<LineRenderer>();
        if (hasParent)
        {
            stringRenderer.SetPosition(1, transform.InverseTransformPoint(spider.position));
        }
        else
        {
            stringRenderer.SetPosition(1, spider.position);
        }

        if (hasParent)
        {
            stringRenderer.SetPosition(0, transform.InverseTransformPoint(curTarget.position));
        }
        else
        {
            stringRenderer.SetPosition(0, curTarget.position);
        }
    }
}
