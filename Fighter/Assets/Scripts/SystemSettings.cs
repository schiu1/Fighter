using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSettings : MonoBehaviour
{
    public static SystemSettings systemSettings { get; private set; }

    public float masterVolume;

    void Awake()
    {
        masterVolume = 0.5f;
    }
}
