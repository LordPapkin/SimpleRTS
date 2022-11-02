using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemamachineShake : MonoBehaviour
{
    public static CinemamachineShake Instance => instance;
    private static CinemamachineShake instance;

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin channelPerlin;

    private float timer;
    private float timerMax;
    private float intensity;

    public void ShakeCamera(float intensity, float timer)
    {
        timerMax = timer;
        this.timer = timer;
        this.intensity = intensity;
        channelPerlin.m_AmplitudeGain = intensity;
    }

    private void Awake()
    {
        if(instance == null)
            instance = this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        channelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        HandleShake();
    }

    private void HandleShake()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            float amplitude = intensity * (timer / timerMax);
            channelPerlin.m_AmplitudeGain = amplitude;
        }
        else
        {
            channelPerlin.m_AmplitudeGain = 0f;
        }
    }
}
