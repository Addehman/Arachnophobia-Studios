using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    AudioClip[] soundClip;
    AudioSource music;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        GetSoundComponents();
    }

    void GetSoundComponents()
    {
        soundClip = new AudioClip[11];
        soundClip[0] = Resources.Load<AudioClip>("Audio/EricFootStep");
        soundClip[1] = Resources.Load<AudioClip>("Audio/Detected");
        soundClip[2] = Resources.Load<AudioClip>("Audio/EricDoor");
        soundClip[3] = Resources.Load<AudioClip>("Audio/EricEnterRoom");
        soundClip[4] = Resources.Load<AudioClip>("Audio/Vacuum");
        soundClip[5] = Resources.Load<AudioClip>("Audio/WebShoot");
        soundClip[6] = Resources.Load<AudioClip>("Audio/GameMusic");
        soundClip[7] = Resources.Load<AudioClip>("Audio/EricCough");
        soundClip[8] = Resources.Load<AudioClip>("Audio/EricCloseDoor");
        soundClip[9] = Resources.Load<AudioClip>("Audio/Burn");
        soundClip[10] = Resources.Load<AudioClip>("Audio/HotHob");
    }
}
