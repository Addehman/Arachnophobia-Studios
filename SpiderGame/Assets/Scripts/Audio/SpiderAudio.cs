using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAudio : MonoBehaviour
{
    AudioSource audioSourceSpider;

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
}
