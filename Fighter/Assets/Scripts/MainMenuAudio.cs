﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuAudio : MonoBehaviour
{
    [SerializeField]
    Sound[] sounds = null;
    //[HideInInspector]
    public float masterVolume;

    void Awake()
    {
        //for each sound in array, 
        foreach (Sound s in sounds)
        {
            //assign the AudioSource variable in Sound an instance of AudioSource
            //and assign the properties saved in Sound obj to the AudioSource obj
            s.source = gameObject.AddComponent<AudioSource>();
            //s.source.volume = s.volume;
            s.source.clip = s.clip;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        masterVolume = SystemSettings.systemSettings.masterVolume;
        foreach (Sound s in sounds)
        {
            s.source.volume = masterVolume;
        }
        PlaySound("Theme");
    }

    public void PlaySound(string name)
    {
        Sound found = null;
        foreach (Sound s in sounds)
        {
            if (s.soundName == name)
            {
                found = s;
            }
        }
        if (found == null)
        {
            Debug.Log("Sound with " + name + " was not found");
            return;
        }
        found.source.Play();
    }

    public void ChangeAllVolume(float value)
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = value;
        }
        //masterVolume = value;
        SystemSettings.systemSettings.masterVolume = value;
        GameObject.Find("VolumeValue").GetComponent<Text>().text = value.ToString("0.##");
    }
}
