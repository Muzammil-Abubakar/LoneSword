/*using System.Collections.Generic;
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
            Debug.LogError(
                $"SkeletonAttackHitbox on {gameObject.name} could not find a Collider."
            );

            return;
        }

        hitboxCollider.enabled = false;
    }

    // --------------------------------------------------
    // HITBOX
    // --------------------------------------------------

    public void EnableHitbox()
    {
        if (hitboxCollider == null)
        {
            return;
        }

        // Allow the player to be hit again
        // during the next attack.
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

    // --------------------------------------------------
    // COLLISION
    // --------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        GameObject playerObject =
            other.transform.root.gameObject;

        // Prevent the same player from being hit
        // multiple times during one attack.
        if (hitTargets.Contains(playerObject))
        {
            return;
        }

        PlayerManager playerManager =
            playerObject.GetComponent<PlayerManager>();

        if (playerManager == null)
        {
            Debug.LogError(
                $"Could not find PlayerManager on {playerObject.name}."
            );

            return;
        }

        hitTargets.Add(playerObject);

        playerManager.TakeHit();
    }
}*/