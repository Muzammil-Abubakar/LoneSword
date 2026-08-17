using UnityEngine;

public sealed class PlayerAnimator : MonoBehaviour
{
    private static readonly int SpeedHash =
        Animator.StringToHash("Speed");

    private static readonly int SlashHash =
        Animator.StringToHash("Slash1");

    private static readonly int HitHash =
        Animator.StringToHash("Hit");

    [SerializeField] private PlayerMovement playerMovement;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (playerMovement == null)
        {
            playerMovement = GetComponent<PlayerMovement>();
        }

        ValidateReferences();
    }

    private void Update()
    {
        UpdateLocomotion();
    }

    public void PlayAttack()
    {
        animator.SetTrigger(SlashHash);
    }

    public void PlayHit()
    {
        animator.SetTrigger(HitHash);
    }

    private void UpdateLocomotion()
    {
        animator.SetFloat(
            SpeedHash,
            playerMovement.CurrentSpeed
        );
    }

    private void ValidateReferences()
    {
        if (animator == null)
        {
            Debug.LogError(
                $"{nameof(PlayerAnimator)} requires an Animator component.",
                this
            );
        }

        if (playerMovement == null)
        {
            Debug.LogError(
                $"{nameof(PlayerAnimator)} requires a {nameof(PlayerMovement)} reference.",
                this
            );
        }
    }
}