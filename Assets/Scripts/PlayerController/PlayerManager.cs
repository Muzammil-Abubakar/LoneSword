using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;
    private PlayerHealth playerHealth;
    private PlayerAnimator playerAnimator;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCombat = GetComponent<PlayerCombat>();
        playerHealth = GetComponent<PlayerHealth>();
        playerAnimator = GetComponent<PlayerAnimator>();

        ValidateComponents();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (playerInput.SlashPressed)
        {
            playerCombat.TryAttack();
        }
    }

    // --------------------------------------------------
    // MOVEMENT
    // --------------------------------------------------

    public Vector2 GetMoveInput()
    {
        return playerInput.MoveInput;
    }

    public bool IsRunning()
    {
        return playerInput.IsRunning;
    }

    public bool CanMove()
    {
        return !playerCombat.IsAttacking()
            && !playerHealth.IsHit();
    }

    // --------------------------------------------------
    // COMBAT
    // --------------------------------------------------

    public void EndAttack()
    {
        playerCombat.EndAttack();
    }

    public void EnableAttackHitbox()
    {
        playerCombat.EnableHitbox();
    }

    public void DisableAttackHitbox()
    {
        playerCombat.DisableHitbox();
    }

    // --------------------------------------------------
    // DAMAGE
    // --------------------------------------------------

    public void TakeHit()
    {
        playerCombat.CancelAttack();
        playerMovement.StopMovement();
        playerHealth.TakeHit();
    }

    public void EndHitReaction()
    {
        playerHealth.EndHitReaction();
    }

    // --------------------------------------------------
    // ANIMATION
    // --------------------------------------------------

    public void PlaySlash()
    {
        playerAnimator.PlaySlash();
    }

    public void PlayHit()
    {
        playerAnimator.PlayHit();
    }

    public void SetMovementAnimation(float speed)
    {
        playerAnimator.SetMovementSpeed(speed);
    }

    // --------------------------------------------------
    // DEBUG
    // --------------------------------------------------

    private void ValidateComponents()
    {
        if (playerInput == null)
        {
            Debug.LogError(
                "PlayerManager could not find PlayerInput."
            );
        }

        if (playerMovement == null)
        {
            Debug.LogError(
                "PlayerManager could not find PlayerMovement."
            );
        }

        if (playerCombat == null)
        {
            Debug.LogError(
                "PlayerManager could not find PlayerCombat."
            );
        }

        if (playerHealth == null)
        {
            Debug.LogError(
                "PlayerManager could not find PlayerHealth."
            );
        }

        if (playerAnimator == null)
        {
            Debug.LogError(
                "PlayerManager could not find PlayerAnimator."
            );
        }
    }
}