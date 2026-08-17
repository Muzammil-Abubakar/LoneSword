using System.Collections.Generic;
using UnityEngine;

public sealed class AttackHitbox : MonoBehaviour
{
    private Collider hitboxCollider;

    private readonly HashSet<IHitReceiver> hitTargets =
        new HashSet<IHitReceiver>();

    private void Awake()
    {
        hitboxCollider = GetComponent<Collider>();

        if (hitboxCollider == null)
        {
            Debug.LogError(
                $"{nameof(AttackHitbox)} requires a Collider.",
                this
            );

            return;
        }

        hitboxCollider.enabled = false;
    }

    public void EnableHitbox()
    {
        if (hitboxCollider == null)
        {
            return;
        }

        hitTargets.Clear();
        hitboxCollider.enabled = true;
    }

    public void DisableHitbox()
    {
        if (hitboxCollider == null)
        {
            return;
        }

        hitboxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        IHitReceiver hitReceiver =
            other.GetComponentInParent<IHitReceiver>();

        if (hitReceiver == null)
        {
            return;
        }

        if (hitTargets.Contains(hitReceiver))
        {
            return;
        }

        hitTargets.Add(hitReceiver);

        hitReceiver.ReceiveHit();
    }
}