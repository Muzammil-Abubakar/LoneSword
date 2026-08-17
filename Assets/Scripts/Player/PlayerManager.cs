using UnityEngine;

/// <summary>
/// Coordinates high-level player state and control.
/// </summary>
public sealed class PlayerManager : MonoBehaviour
{
    [Header("Player Components")]
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Movement State")]
    [SerializeField] private bool canMove = true;

    /// <summary>
    /// Whether the player is currently allowed to move.
    /// </summary>
    public bool CanMove => canMove;

    private void Awake()
    {
        if (playerMovement == null)
        {
            playerMovement = GetComponent<PlayerMovement>();
        }

        ValidateReferences();
    }

    /// <summary>
    /// Enables or disables player movement.
    /// </summary>
    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;

        Debug.Log(
            $"[{nameof(PlayerManager)}] CanMove changed to: {canMove}",
            this
        );

        if (!canMove)
        {
            playerMovement.StopMovement();
        }
    }

    private void ValidateReferences()
    {
        if (playerMovement == null)
        {
            Debug.LogError(
                $"{nameof(PlayerManager)} requires a {nameof(PlayerMovement)} reference.",
                this
            );
        }
    }
}