using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickingSound : MonoBehaviour
{
    static AudioSource audioSourceGUI;
    private static ClickingSound guiSound;
    public static ClickingSound Instance
    {
        get
        {
            if (guiSound == null)
            {
                Debug.LogError("GameManager is Null.");
            }
            return guiSound;
        }
    }

    public static void clickSound()
    {
        audioSourceGUI = GameObject.Find("ClickAudio").GetComponent<AudioSource>();
        audioSourceGUI.clip = Resources.Load<AudioClip>("Audio/ClickButton");
        audioSourceGUI.Play();
    }
}
