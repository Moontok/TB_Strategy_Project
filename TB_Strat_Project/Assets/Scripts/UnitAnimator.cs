using System;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] Animator animator = null;
    [SerializeField] Transform rangeWeapon = null;
    [SerializeField] Transform meleeWeapon = null;

    MoveAction moveAction = null;
    ShootAction shootAction = null;
    MeleeAction swordAction = null;

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

        if (TryGetComponent(out swordAction))
        {
            swordAction.OnSwordActionStart += SwordAction_OnSwordActionStart;
            swordAction.OnSwordActionComplete += SwordAction_OnSwordActionComplete;
        }
    }

    void Start()
    {
        EquipRangeWeapon();
    }

    private void SwordAction_OnSwordActionStart(object sender, EventArgs e)
    {
        EquipMeleeWeapon();
        animator.SetTrigger("SwordSlash");
    }

    private void SwordAction_OnSwordActionComplete(object sender, EventArgs e)
    {
        EquipRangeWeapon();
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

    void EquipMeleeWeapon()
    {
        meleeWeapon.gameObject.SetActive(true);
        rangeWeapon.gameObject.SetActive(false);
    }

    void EquipRangeWeapon()
    {
        rangeWeapon.gameObject.SetActive(true);
        meleeWeapon.gameObject.SetActive(false);
    }
}
