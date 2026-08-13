using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private PlayerManager playerManager;
    private AttackHitbox attackHitbox;

    private bool isAttacking;

    private void Awake()
    {
        playerManager =
            GetComponent<PlayerManager>();

        attackHitbox =
            GetComponentInChildren<AttackHitbox>(true);

        if (playerManager == null)
        {
            Debug.LogError(
                "PlayerCombat could not find PlayerManager."
            );
        }

        if (attackHitbox == null)
        {
            Debug.LogError(
                "PlayerCombat could not find AttackHitbox."
            );
        }
    }

    public void TryAttack()
    {
        if (isAttacking)
        {
            return;
        }

        StartAttack();
    }

    private void StartAttack()
    {
        isAttacking = true;

        playerManager.PlaySlash();
    }

    public void EndAttack()
    {
        isAttacking = false;

        DisableHitbox();
    }

    public void CancelAttack()
    {
        isAttacking = false;

        DisableHitbox();
    }

    public void EnableHitbox()
    {
        if (attackHitbox == null)
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

    public bool IsAttacking()
    {
        return isAttacking;
    }
}