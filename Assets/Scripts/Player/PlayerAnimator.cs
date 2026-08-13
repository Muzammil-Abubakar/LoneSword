using UnityEngine;

/// <summary>
/// Responsible only for communicating player state
/// to the Animator.
///
/// This class does not move the player and does not
/// read input directly.
/// </summary>
public sealed class PlayerAnimator : MonoBehaviour
{
    private static readonly int SpeedHash =
        Animator.StringToHash("Speed");

    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Animator animator;

    [Header("Animation")]
    [SerializeField, Min(0f)] private float speedDamping = 0.15f;

    private void Awake()
    {
        if (playerMovement == null)
        {
            playerMovement = GetComponent<PlayerMovement>();
        }

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        ValidateReferences();
    }

    private void LateUpdate()
    {
        UpdateLocomotion();
    }

    private void UpdateLocomotion()
    {
        if (animator == null || playerMovement == null)
        {
            return;
        }

        animator.SetFloat(
            SpeedHash,
            playerMovement.CurrentSpeed,
            speedDamping,
            Time.deltaTime
        );
    }

    private void ValidateReferences()
    {
        if (playerMovement == null)
        {
            Debug.LogError(
                $"{nameof(PlayerAnimator)} requires a {nameof(PlayerMovement)} reference.",
                this
            );
        }

        if (animator == null)
        {
            Debug.LogError(
                $"{nameof(PlayerAnimator)} requires an Animator reference.",
                this
            );
        }
    }
}