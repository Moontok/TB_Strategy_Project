using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    static MouseWorld instance = null;

    [SerializeField] LayerMask mousePlaneLayerMask = new LayerMask();

    void Awake()
    {
        instance = this;
    }

    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.mousePlaneLayerMask);

        return raycastHit.point;
    }
}
