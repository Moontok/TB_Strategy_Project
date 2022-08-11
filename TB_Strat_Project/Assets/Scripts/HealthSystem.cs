using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnDamaged;

    [SerializeField] int healthMax = 100;

    int health = 0;


    void Awake()
    {
        health = healthMax;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        if (health < 0)
        {
            health = 0;
        }

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (health == 0)
        {
            Die();
        }
    }

    void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float)health / healthMax;
    }
}
