using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    Slider volume;

    void Start()
    {
        volume = gameObject.GetComponent<Slider>();
        volume.value = AudioManager.audioManager.masterVolume;
    }

    public void ChangeVolume()
    {
        AudioManager.audioManager.masterVolume = volume.value;
        AudioManager.audioManager.ChangeAllVolume(volume.value);
    }
}
