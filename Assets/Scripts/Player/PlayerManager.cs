using UnityEngine;

/// <summary>
/// Coordinates high-level player state.
///
/// This class does not implement movement, input,
/// animation, combat, health, or hitbox behaviour.
///
/// Its responsibility is to control whether the player
/// is currently permitted to perform certain actions.
/// </summary>
public sealed class PlayerManager : MonoBehaviour
{
    [Header("Player Components")]
    [SerializeField] private PlayerMovement playerMovement;

    /// <summary>
    /// Whether the player is currently allowed to move.
    /// </summary>
    public bool CanMove { get; private set; } = true;

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
    ///
    /// Other systems can use this later for:
    /// - Attacks
    /// - Stuns
    /// - Hit reactions
    /// - Dialogue
    /// - Cutscenes
    /// - Death
    /// </summary>
    public void SetMovementEnabled(bool enabled)
    {
        CanMove = enabled;
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