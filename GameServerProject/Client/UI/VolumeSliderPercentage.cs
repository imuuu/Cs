using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class VolumeSliderPercentage : MonoBehaviour
{


    TextMeshProUGUI percentageText;
    public AudioMixer audioMixer;

    void Start()
    {
        percentageText = GetComponent<TextMeshProUGUI>();
    }

    public void textUpdate (float value)
    {
        percentageText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    public void SetVolume(float value)
    {
        audioMixer.SetFloat("volume", value);
    }
}
