using System;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] Animator animator = null;

    MoveAction moveAction = null;
    ShootAction shootAction = null;

    void Awake()
    {
        if (TryGetComponent(out moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent(out shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }
    }

    void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }

    void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }

    void ShootAction_OnShoot(object sender, EventArgs e)
    {
        animator.SetTrigger("Shoot");
    }
}
