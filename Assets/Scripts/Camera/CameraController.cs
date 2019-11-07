﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    private CinemachineComposer virtualComposer;
    public float shakeDuration = 0.3f;
    public float shakeAmplitude = 7f;
    public float shakeFrequency = 7f;
    private float shakeElapsedTime = 0f;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        virtualCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
        virtualCameraNoise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        virtualComposer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the Cinemachine componet is not set, avoid update
        if (virtualCamera != null && virtualCameraNoise != null)
        {
            // If Camera Shake effect is still playing
            if (shakeElapsedTime > 0)
            {
                virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
                virtualCameraNoise.m_FrequencyGain = shakeFrequency;
                // Update Shake Timer
                shakeElapsedTime -= Time.deltaTime;
            }
            else
            {
                // If Camera Shake effect is over, reset variables
                virtualCameraNoise.m_AmplitudeGain = 0f;
                shakeElapsedTime = 0f;
            }
        }
    }

    public void Shake(float sec)
    {
        if (sec == 0)
            shakeElapsedTime = shakeDuration;
        else
            StartCoroutine(WaitForShake(sec));
    }

    IEnumerator WaitForShake(float sec)
    {
        yield return new WaitForSeconds(sec);
        shakeElapsedTime = shakeDuration;
    }

    public void ModifyZoom(float value)
    {
        virtualCamera.m_Lens.OrthographicSize = value;
    }
}