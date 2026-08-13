using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackHitbox : MonoBehaviour
{
    private Collider hitboxCollider;

    private HashSet<GameObject> hitTargets =
        new HashSet<GameObject>();

    private void Awake()
    {
        hitboxCollider = GetComponent<Collider>();

        if (hitboxCollider == null)
        {
            

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

        // Allow targets to be hit again during a new attack.
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
        if (!other.CompareTag("Player"))
        {
            return;
        }

        GameObject playerObject = other.transform.root.gameObject;

        if (hitTargets.Contains(playerObject))
        {
            return;
        }

        hitTargets.Add(playerObject);

        PlayerAnimatorController player =
            playerObject.GetComponent<PlayerAnimatorController>();

        if (player == null)
        {
            Debug.LogError(
                $"Could not find PlayerAnimatorController on {playerObject.name}."
            );

            return;
        }

        player.TakeHit();
    }
}