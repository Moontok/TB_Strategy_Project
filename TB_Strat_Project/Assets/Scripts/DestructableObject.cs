using System;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    public static event EventHandler OnAnyDestroyed;

    [SerializeField] Transform destroyedObjectPrefab = null;

    HealthSystem healthSystem = null;
    GridPosition gridPosition = new GridPosition();

    void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        healthSystem.OnDead += HealthSystem_OnDead;
    }

    void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Transform destroyedObject = Instantiate(destroyedObjectPrefab, transform.position, transform.rotation);
        ApplyExplosionToChildren(destroyedObject, 150f, transform.position, 10f);

        Destroy(gameObject);
        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }    
    
    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    void ApplyExplosionToChildren(Transform root, float force, Vector3 position, float range)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(force, position, range);
            }

            ApplyExplosionToChildren(child, force, position, range);
        }
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
}
