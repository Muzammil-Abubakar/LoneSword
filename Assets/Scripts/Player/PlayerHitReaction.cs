using UnityEngine;

public sealed class PlayerHitReaction : MonoBehaviour, IHitReceiver
{
    [Header("References")]
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private PlayerAnimator playerAnimator;

    private void Awake()
    {
        if (playerManager == null)
        {
            playerManager = GetComponent<PlayerManager>();
        }

        if (playerCombat == null)
        {
            playerCombat = GetComponent<PlayerCombat>();
        }

        if (playerAnimator == null)
        {
            playerAnimator = GetComponent<PlayerAnimator>();
        }

        ValidateReferences();
    }

    /// <summary>
    /// Called when the player receives a hit.
    /// Atomically enters hit state, canceling any in-progress attack
    /// and locking the player in hit reaction.
    /// </summary>
    public void ReceiveHit()
    {
        // Enter hit state first, which cancels any in-progress attack
        playerCombat.EnterHitState();

        // Disable movement
        playerManager.SetMovementEnabled(false);

        // Play hit animation
        playerAnimator.PlayHit();
    }

    /// <summary>
    /// Called by an Animation Event approximately 25%
    /// through the PlayerHit animation.
    /// Exits hit state and restores movement control.
    /// </summary>
    public void RestoreControl()
    {
        // Exit hit state, allowing attacks to resume
        playerCombat.ExitHitState();

        // Restore movement
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

        if (playerCombat == null)
        {
            Debug.LogError(
                $"{nameof(PlayerHitReaction)} requires a {nameof(PlayerCombat)} reference.",
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