using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SetVolume : MonoBehaviour
{
    public AudioMixer AudioMixer;
    public Slider masterSlider;
    public string faderName;

    void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat(faderName + "Volume", 0.75f);
    }

    public void SetLevel(float sliderValue)
    {
        sliderValue = Mathf.Clamp(sliderValue, 0.0001f, 1);
        AudioMixer.SetFloat(faderName, Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat(faderName + "Volume", sliderValue);
    }
}
//CHECK