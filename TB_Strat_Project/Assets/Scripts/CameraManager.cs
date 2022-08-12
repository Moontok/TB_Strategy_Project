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
