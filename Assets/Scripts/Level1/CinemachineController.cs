using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineController : MonoBehaviour
{
    public static CinemachineController instance;
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    private CinemachineComposer virtualComposer;
    public float shakeDuration = 0.3f;
    public float shakeAmplitude = 7f;
    public float shakeFrequency = 7f;
    private float shakeElapsedTime = 0f;


    void Start()
    {
        instance = this;
        virtualCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
        virtualCameraNoise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        virtualComposer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();
    }

    
    void Update()
    {  
        if (virtualCamera != null && virtualCameraNoise != null)
        {
            if (shakeElapsedTime > 0)
            {
                virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
                virtualCameraNoise.m_FrequencyGain = shakeFrequency;
                
                shakeElapsedTime -= Time.deltaTime;
            }
            else
            {
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