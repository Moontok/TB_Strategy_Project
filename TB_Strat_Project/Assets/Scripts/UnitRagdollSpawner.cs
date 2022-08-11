using System;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] Transform ragdollPrefab = null;
    [SerializeField] Transform originalRootBone = null;

    HealthSystem healthSystem = null;

    void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();

        healthSystem.OnDead += HealthSystem_OnDead;
    }

    void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, Quaternion.identity);
        UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(originalRootBone);
    }
}
