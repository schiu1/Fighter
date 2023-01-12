using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VolumeSlider : MonoBehaviour
{
    Slider volume;

    void Start()
    {
        volume = gameObject.GetComponent<Slider>();
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Fight_Scene"))
        {
            volume.value = AudioManager.audioManager.masterVolume;
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main_Menu"))
        {
            volume.value = GameObject.Find("AudioManager").GetComponent<MainMenuAudio>().masterVolume;
        }
    }

    public void ChangeVolume()
    {
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Fight_Scene"))
        {
            AudioManager.audioManager.ChangeAllVolume(volume.value);
        }
        else if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main_Menu"))
        {
            GameObject g = GameObject.Find("AudioManager");
            g.GetComponent<MainMenuAudio>().ChangeAllVolume(volume.value);
        }
    }
}
