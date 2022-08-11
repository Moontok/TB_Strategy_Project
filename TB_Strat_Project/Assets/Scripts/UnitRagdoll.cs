using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] Transform ragdollRootBone = null;

    public void Setup(Transform originalRootBone)
    {
        MatchAllChildTransforms(originalRootBone, ragdollRootBone);

        ApplyExplosionToRagdoll(ragdollRootBone, 300f, transform.position, 10f);
    }

    void ApplyExplosionToRagdoll(Transform root, float force, Vector3 position, float range)
    {
        foreach (Transform child in root)
        {
            if (TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(force, position, range);
            }

            ApplyExplosionToRagdoll(child, force, position, range);
        }
    }

    void MatchAllChildTransforms(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if (cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;

                MatchAllChildTransforms(child, cloneChild);
            }
        }
    }
}
