using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSphere : MonoBehaviour, IInteractable
{
    [SerializeField] Material greenMat = null;
    [SerializeField] Material redMat = null;
    [SerializeField] MeshRenderer meshRenderer = null;

    MatColor colorTrack = MatColor.Green;
    GridPosition gridPosition = new GridPosition();
    Action onInteractionComplete = null;
    bool isActive = false;
    float timer = 0;

    enum MatColor
    {
        Red,
        Green
    }

    void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);

        SetColorGreen();
    }

    void Update()
    {
        if (!isActive)
            return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            isActive = false;
            onInteractionComplete();
        }
    }

    void SetColorGreen()
    {
        colorTrack = MatColor.Green;
        meshRenderer.material = greenMat;
    }

    void SetColorRed()
    {
        colorTrack = MatColor.Red;
        meshRenderer.material = redMat;
    }

    public void Interact(Action onInteractionComplete)
    {

        this.onInteractionComplete = onInteractionComplete;

        isActive = true;
        timer = 0.5f;

        if (colorTrack == MatColor.Green)
            SetColorRed();
        else
            SetColorGreen();
    }
}
