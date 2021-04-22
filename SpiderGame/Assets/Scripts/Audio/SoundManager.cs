using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioClip[] soundClip;
    public AudioSource audioSource;
    public AudioSource vacuumSource;
    public AudioSource music;

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
        soundClip = new AudioClip[8];
        soundClip[0] = Resources.Load<AudioClip>("Audio/EricFootStep");
        soundClip[1] = Resources.Load<AudioClip>("Audio/Detected");
        soundClip[2] = Resources.Load<AudioClip>("Audio/EricDoor");
        soundClip[3] = Resources.Load<AudioClip>("Audio/EricEnterRoom");
        soundClip[4] = Resources.Load<AudioClip>("Audio/Vacuum");
        soundClip[5] = Resources.Load<AudioClip>("Audio/WebShoot");
        soundClip[6] = Resources.Load<AudioClip>("Audio/GameMusic");
        soundClip[7] = Resources.Load<AudioClip>("Audio/EricCough");
        soundClip[8] = Resources.Load<AudioClip>("Audio/EricCloseDoor");
    }

    public void EricFootStep()
    {
        audioSource.clip = soundClip[0];
        audioSource.Play();
    }

    public void Detected()
    {
        audioSource.clip = soundClip[1];
        audioSource.Play();
    }

    public void Door()
    {
        audioSource.clip = soundClip[2];
        audioSource.Play();
    }

    public void EricHmm()
    {
        audioSource.clip = soundClip[3];
        audioSource.Play();
    }

    public void WebShoot()
    {
        audioSource.clip = soundClip[5];
        audioSource.Play();
    }

    public void EricCough()
    {
        audioSource.clip = soundClip[7];
        audioSource.Play();
    }

    public void CloseDoor()
    {
        audioSource.clip = soundClip[8];
        audioSource.Play();
    }


    public enum ClipEnum : int
    {
        FootStep,
        Detected,
        Door,
        EnterRoom,
        Vacuum,
        WebShoot,
        GameMusic
    }
}
