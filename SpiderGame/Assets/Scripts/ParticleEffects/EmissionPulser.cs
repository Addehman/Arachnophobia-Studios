using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Place me on Object that you want to be able to glow. need to have LightPulser on the other object and fix material so it works on items.
public class EmissionPulser : MonoBehaviour
{
    public float duration;
    Material myMat;


    private void Start()
    {
        myMat = GetComponent<Renderer>().material;
    }
    // Update is called once per frame
    void Update()
    {
        float phi = Time.time / duration * 2 * Mathf.PI;
        float amplitude = Mathf.Cos(phi) * 0.5f + 0.5f;
        float G = amplitude;
        float B = amplitude;
        myMat.SetColor("EmissionColor", new Color(0f, G, B));
    }
}
