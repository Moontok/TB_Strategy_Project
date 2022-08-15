using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] GameObject actionCamera = null;

    void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;

        HideActionCamera();
    }

    void ShowActionCamera()
    {
        actionCamera.SetActive(true);
    }

    void HideActionCamera()
    {
        actionCamera.SetActive(false);
    }

    public void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        switch(sender)
        {
            case ShootAction shootAction:
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();

                Vector3 aimDirection = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;
                Vector3 cameraOffset = shooterUnit.GetComponent<ActionCameraOffset>().GetCameraOffset();
                Vector3 yOffset = Vector3.up * cameraOffset.y;
                Vector3 xOffset = Quaternion.Euler(0, 90, 0) * aimDirection * cameraOffset.x;
                Vector3 zOffset = aimDirection * cameraOffset.z;
                Vector3 offset = xOffset + yOffset + zOffset;

                actionCamera.transform.position = shooterUnit.GetWorldPosition() + offset;
                actionCamera.transform.LookAt(targetUnit.GetWorldPosition() + yOffset);
                ShowActionCamera();
                break;
        }
    }

    public void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {

        switch (sender)
        {
            case ShootAction shootAction:
                HideActionCamera();
                break;
        }
    }
}
