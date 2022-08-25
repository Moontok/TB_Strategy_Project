using System;
using UnityEditor.Profiling;
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
    [SerializeField] AnimationCurve arcYAnimCurve = null;

    Action onExplosiveBehaviorComplete;
    Vector3 targetPos = Vector3.zero;
    float totalDistance = 0f;
    Vector3 posXZ = Vector3.zero;

    void Update()
    {
        Vector3 moveDir = (targetPos - posXZ).normalized;
        posXZ += moveDir * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(posXZ, targetPos);
        float distanceNormalized = 1 - distance / totalDistance;


        float maxHeight = totalDistance / 4f;
        float posY = arcYAnimCurve.Evaluate(distanceNormalized);
        transform.position = new Vector3(posXZ.x, posY, posXZ.z);


        if(Vector3.Distance(posXZ, targetPos) < detonateRange)
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
                else
                {
                    hit.TryGetComponent(out DestructableObject destructable);

                    if (destructable != null)
                        destructable.Damage(damage);
                }
            }

            OnAnyExplosion?.Invoke(this, EventArgs.Empty);

            trailRenderer.transform.parent = null;
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            onExplosiveBehaviorComplete();
        }
    }

    public void Setup(GridPosition targetGridPos, Action onExplosiveBehaviorComplete, float height)
    {
        this.onExplosiveBehaviorComplete = onExplosiveBehaviorComplete;
        targetPos = LevelGrid.Instance.GetWorldPosition(targetGridPos);

        posXZ = transform.position;
        posXZ.y = 0;
        totalDistance = Vector3.Distance(posXZ, targetPos);

        Keyframe[] keys = arcYAnimCurve.keys;
        keys[0].value = height;

        arcYAnimCurve.keys = keys;
    }
}
