using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Shake : MonoBehaviour
{
    //get the vcam
    CinemachineVirtualCamera vcam;
    float shakeTimer;

    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();

    }

    void Update()
    {
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if(shakeTimer <= 0) //if time is up
            {
                //get the component of the vcam
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                    vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        //get the component of the vcam
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        //change amplitude
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }
}
