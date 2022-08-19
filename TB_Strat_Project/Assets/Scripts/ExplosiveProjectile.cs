using System;
using UnityEngine;

public class ExplosiveProjectile : MonoBehaviour
{
    public static event EventHandler OnAnyExplosion;

    [SerializeField] float moveSpeed = 15f;
    [SerializeField] int damage = 30;
    [SerializeField] float detonateRange = .2f;
    [SerializeField] float damageRadius = 4f;
    [SerializeField] bool hurtFriendly = false;
    [SerializeField] Transform explosionPrefab = null;
    [SerializeField] TrailRenderer trailRenderer = null;

    Action onExplosiveBehaviorComplete;
    Vector3 targetPos = Vector3.zero;

    void Update()
    {
        Vector3 moveDir = (targetPos - transform.position).normalized;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        if(Vector3.Distance(transform.position, targetPos) < detonateRange)
        {
            Collider[] hits = Physics.OverlapSphere(targetPos, damageRadius);

            foreach (Collider hit in hits)
            {
                hit.TryGetComponent(out Unit target);

                if (target != null)
                {
                    if(hurtFriendly || target.IsEnemy())
                        target.Damage(damage);
                }
            }

            OnAnyExplosion?.Invoke(this, EventArgs.Empty);

            trailRenderer.transform.parent = null;
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            onExplosiveBehaviorComplete();
        }
    }

    public void Setup(GridPosition targetGridPos, Action onExplosiveBehaviorComplete)
    {
        this.onExplosiveBehaviorComplete = onExplosiveBehaviorComplete;
        targetPos = LevelGrid.Instance.GetWorldPosition(targetGridPos);
    }
}
