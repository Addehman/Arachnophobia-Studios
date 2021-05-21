using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ClickSound
{

    private static AudioSource source;

    private static AudioClip clip;
    private const string ClickPath = "Audio/ClickButton";

    public static void PlayClickSFX()
    {
        if (source == null)
        {
            source = new GameObject().AddComponent<AudioSource>();
            MonoBehaviour.DontDestroyOnLoad(source.gameObject);
        }

        if (clip == null)
        {
            clip = Resources.Load<AudioClip>(ClickPath);
        }
        source.PlayOneShot(clip);
    }
}