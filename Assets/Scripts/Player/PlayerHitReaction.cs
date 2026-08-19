using UnityEngine;

public sealed class PlayerHitReaction : MonoBehaviour, IHitReceiver
{
    [Header("References")]
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private PlayerAnimator playerAnimator;

    private void Awake()
    {
        if (playerManager == null)
        {
            playerManager = GetComponent<PlayerManager>();
        }

        if (playerAnimator == null)
        {
            playerAnimator = GetComponent<PlayerAnimator>();
        }

        ValidateReferences();
    }

    /// <summary>
    /// Called when the player receives a hit.
    /// </summary>
    public void ReceiveHit()
    {
        playerManager.SetMovementEnabled(false);
        playerAnimator.PlayHit();
    }

    /// <summary>
    /// Called by an Animation Event approximately 25%
    /// through the PlayerHit animation.
    /// </summary>
    public void RestoreControl()
    {
        playerManager.SetMovementEnabled(true);
    }

    private void ValidateReferences()
    {
        if (playerManager == null)
        {
            Debug.LogError(
                $"{nameof(PlayerHitReaction)} requires a {nameof(PlayerManager)} reference.",
                this
            );
        }

        if (playerAnimator == null)
        {
            Debug.LogError(
                $"{nameof(PlayerHitReaction)} requires a {nameof(PlayerAnimator)} reference.",
                this
            );
        }
    }
}