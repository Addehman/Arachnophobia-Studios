using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Add me to object that will push light. This will talk to EmissionPulser script and make object "glow" / halo effect.

[RequireComponent(typeof(Light))]
public class LightPulser : MonoBehaviour
{
    public float duration;
    public Light lt;
    void Start()
    {
        lt = GetComponent<Light>();
    }

    void Update()
    {
        float phi = (Time.time / duration) * 2 * Mathf.PI;
        float amplitude = Mathf.Cos(phi) * 0.5f * 0.5f;
        lt.intensity = amplitude;
    }
}
