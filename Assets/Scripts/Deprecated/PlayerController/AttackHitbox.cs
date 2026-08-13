/*using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private Collider hitboxCollider;

    private HashSet<SkeletonCombat> hitTargets =
        new HashSet<SkeletonCombat>();

    private void Awake()
    {
        hitboxCollider = GetComponent<Collider>();

        hitboxCollider.enabled = false;
    }

    public void EnableHitbox()
    {
        hitTargets.Clear();

        hitboxCollider.enabled = true;
        
    }

    public void DisableHitbox()
    {
        hitboxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy"))
        {
            return;
        }

        SkeletonCombat skeleton =
            other.GetComponentInParent<SkeletonCombat>();

        if (skeleton == null)
        {
            return;
        }

        if (hitTargets.Contains(skeleton))
        {
            return;
        }

        hitTargets.Add(skeleton);

        skeleton.TakeHit();
    }
}*/