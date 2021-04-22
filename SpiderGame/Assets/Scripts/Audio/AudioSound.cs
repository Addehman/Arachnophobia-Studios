using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSound : MonoBehaviour
{
    public AudioClip music;
    public AudioClip vacuum;
    public AudioSource audioSource;
    public AudioSource vacuumAudioSource;

    void Start()
    {
        audioSource.clip = music;
        audioSource.Play();
    }
}
