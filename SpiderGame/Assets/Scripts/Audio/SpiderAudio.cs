using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAudio : MonoBehaviour
{
    AudioSource audioSourceSpider;
    public AudioSource pickUpSound;

    void Start()
    {
        audioSourceSpider = GetComponent<AudioSource>();
    }

    public void Burn()
    {
        audioSourceSpider.clip = Resources.Load<AudioClip>("Audio/Burn");
        audioSourceSpider.Play();
    }

    public void WebShoot()
    {
        audioSourceSpider.clip = Resources.Load<AudioClip>("Audio/WebShoot");
        audioSourceSpider.Play();
    }

    public void VacuumSuck()
    {
        audioSourceSpider.clip = Resources.Load<AudioClip>("Audio/VacuumPlayer");
        audioSourceSpider.Play();
    }

    public void PickUpSound()
    {
        audioSourceSpider.clip = Resources.Load<AudioClip>("Audio/ItemPickUp");
        audioSourceSpider.Play();
    }

    public void LightSwitch()
    {
        audioSourceSpider.clip = Resources.Load<AudioClip>("Audio/LightSwitchNEW");
        audioSourceSpider.Play();
    }

    public void KillFlies()
    {
        audioSourceSpider.clip = Resources.Load<AudioClip>("Audio/FlyKillGood");
        audioSourceSpider.Play();
    }
}
