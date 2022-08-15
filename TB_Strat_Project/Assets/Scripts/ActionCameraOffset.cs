using UnityEngine;

public class ActionCameraOffset : MonoBehaviour
{
    [SerializeField] Vector3 cameraOffset = Vector3.zero;

    public Vector3 GetCameraOffset()
    {
        return cameraOffset;
    }
}
