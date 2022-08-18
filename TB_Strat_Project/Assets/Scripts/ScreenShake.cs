using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance { get; private set; }

    CinemachineImpulseSource cmInpulseSource = null;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"There is more than one Screen Shake! {transform} - {Instance}");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        cmInpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            cmInpulseSource.GenerateImpulse();
    }

    public void Shake(float intensity = 1f)
    {
        cmInpulseSource?.GenerateImpulse(intensity);
    }
}
