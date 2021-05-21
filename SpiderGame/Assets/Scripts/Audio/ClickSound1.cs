/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSound : MonoBehaviour
{
    static AudioSource audioSourceGUI;
    private static ClickSound guiSound;
    public static ClickSound Instance
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

    public static void PlayClickSFX()
    {
        audioSourceGUI = GameObject.Find("ClickAudio").GetComponent<AudioSource>();
        audioSourceGUI.clip = Resources.Load<AudioClip>("Audio/ClickButton");
        audioSourceGUI.Play();
    }
}
*/