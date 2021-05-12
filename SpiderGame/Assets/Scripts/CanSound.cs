using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSound : MonoBehaviour
{
    public AudioSource canSound;

    private void OnCollisionEnter(Collision sodaCanSound)
    {
        if (sodaCanSound.relativeVelocity.magnitude > 2) // Apply higher or lower value  pending on height on cans.
        {
            canSound.Play();
        }
    }
}
