using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Transform cameraTransform = null;

    void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.LookAt(cameraTransform);
    }
}
