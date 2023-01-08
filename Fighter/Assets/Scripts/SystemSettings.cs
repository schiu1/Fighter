using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSettings : MonoBehaviour
{
    public static SystemSettings systemSettings { get; private set; }

    public float masterVolume;

    void Awake()
    {
        if(systemSettings != null && systemSettings != this)
        {
            Destroy(this);
        }
        else
        {
            systemSettings = this;
            DontDestroyOnLoad(systemSettings);
        }

        masterVolume = 0.2f;
    }
}
