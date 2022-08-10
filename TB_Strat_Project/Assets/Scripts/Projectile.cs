using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] int damageAmount = 40;
    [SerializeField] float moveSpeed = 100f;
    [SerializeField] float destroyDelay = 0f;
    [SerializeField] TrailRenderer trailRenderer = null;
    [SerializeField] Transform projectileHitPrefab = null; 


    Vector3 targetPosition = Vector3.zero;

    void Update()
    {
        transform.LookAt(targetPosition);

        float distanceToTargetBefore = Vector3.Distance(transform.position, targetPosition);
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        float distanceToTargetAfter = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTargetBefore < distanceToTargetAfter && !Mathf.Approximately(moveSpeed, 0f))
        {
            moveSpeed = 0f;
            transform.position = targetPosition;
            trailRenderer.transform.parent = null;
            Destroy(gameObject, destroyDelay);
            Instantiate(projectileHitPrefab, transform.position, Quaternion.identity);
        }
    }

    public void SetTarget(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    public int GetDamageAmount()
    {
        return damageAmount;
    }
}
