using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    void Start()
    {
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
        ExplosiveProjectile.OnAnyExplosion += ExplosiveProjectile_OnAnyExplosion;
        MeleeAction.OnAnyMeleeHit += MeleeAction_OnAnyMeleeHit;
    }

    private void MeleeAction_OnAnyMeleeHit(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake(2f);
    }

    void ExplosiveProjectile_OnAnyExplosion(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake(5f);
    }

    void ShootAction_OnAnyShoot(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake();
    }
}
