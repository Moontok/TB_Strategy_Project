using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] bool isOpen = false;
    [SerializeField] Transform leftDoor = null;
    [SerializeField] Transform rightDoor = null;

    GridPosition gridPosition = new GridPosition();
    Animator animator = null;
    Action onInteractComplete = null;
    bool isActive = false;
    float timer = 0;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isActive)
            return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            isActive = false;
            onInteractComplete();
        }
    }

    void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetDoorAtGridPosition(gridPosition, this);

        if (isOpen)
            OpenDoor();
        else
            CloseDoor();
    }

    public void Interact(Action onInteractComplete)
    {
        this.onInteractComplete = onInteractComplete;
        isActive = true;
        timer = 0.5f;
        if (isOpen)
            CloseDoor();
        else
            OpenDoor();
    }
    
    void OpenDoor()
    {
        isOpen = true;
        animator.SetBool("IsOpen", isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);
    }

    void CloseDoor()
    {
        isOpen = false;
        animator.SetBool("IsOpen", isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
    }
}
