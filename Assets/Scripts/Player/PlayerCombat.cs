using UnityEngine;

public sealed class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private AttackHitbox attackHitbox;

    private bool isAttacking;
    private bool isHitReacting;

    public bool IsAttacking => isAttacking;
    public bool IsHitReacting => isHitReacting;

    private void Awake()
    {
        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }

        if (playerManager == null)
        {
            playerManager = GetComponent<PlayerManager>();
        }

        if (playerAnimator == null)
        {
            playerAnimator = GetComponent<PlayerAnimator>();
        }

        if (attackHitbox == null)
        {
            attackHitbox = GetComponentInChildren<AttackHitbox>(true);
        }

        ValidateReferences();
    }

    private void OnEnable()
    {
        if (playerInput != null)
        {
            playerInput.AttackRequested += StartAttack;
        }
    }

    private void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.AttackRequested -= StartAttack;
        }
    }

    private void StartAttack()
    {
        if (isAttacking || isHitReacting)
        {
            return;
        }

        if (playerManager == null || !playerManager.CanMove)
        {
            return;
        }

        isAttacking = true;

        playerManager.SetMovementEnabled(false);

        playerAnimator.PlayAttack();
    }

    public void EndAttack()
    {
        if (!isAttacking || isHitReacting)
        {
            return;
        }

        isAttacking = false;

        DisableHitbox();

        playerManager.SetMovementEnabled(true);
    }

    public void CancelAttack()
    {
        if (!isAttacking)
        {
            return;
        }

        isAttacking = false;

        DisableHitbox();
    }

    /// <summary>
    /// Enters hit reaction state, canceling any in-progress attack.
    /// This is called by PlayerHitReaction when the player is hit.
    /// </summary>
    public void EnterHitState()
    {
        isHitReacting = true;
        isAttacking = false;
        
        DisableHitbox();
    }

    /// <summary>
    /// Exits hit reaction state, allowing attacks to resume.
    /// This is called at the end of the hit reaction animation.
    /// </summary>
    public void ExitHitState()
    {
        isHitReacting = false;
    }

    public void EnableHitbox()
    {
        if (!isAttacking || attackHitbox == null)
        {
            return;
        }

        attackHitbox.EnableHitbox();
    }

    public void DisableHitbox()
    {
        if (attackHitbox == null)
        {
            return;
        }

        attackHitbox.DisableHitbox();
    }

    private void ValidateReferences()
    {
        if (playerInput == null)
        {
            Debug.LogError(
                $"{nameof(PlayerCombat)} requires a {nameof(PlayerInput)} reference.",
                this
            );
        }

        if (playerManager == null)
        {
            Debug.LogError(
                $"{nameof(PlayerCombat)} requires a {nameof(PlayerManager)} reference.",
                this
            );
        }

        if (playerAnimator == null)
        {
            Debug.LogError(
                $"{nameof(PlayerCombat)} requires a {nameof(PlayerAnimator)} reference.",
                this
            );
        }

        if (attackHitbox == null)
        {
            Debug.LogError(
                $"{nameof(PlayerCombat)} requires an {nameof(AttackHitbox)} reference.",
                this
            );
        }
    }
}