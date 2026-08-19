using UnityEngine;

public sealed class EnemyHitReaction : MonoBehaviour, IHitReceiver
{
    [Header("References")]
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private EnemyCombat enemyCombat;
    [SerializeField] private EnemyAnimator enemyAnimator;

    private void Awake()
    {
        if (enemyManager == null)
        {
            enemyManager = GetComponent<EnemyManager>();
        }

        if (enemyCombat == null)
        {
            enemyCombat = GetComponent<EnemyCombat>();
        }

        if (enemyAnimator == null)
        {
            enemyAnimator = GetComponent<EnemyAnimator>();
        }

        ValidateReferences();
    }

    public void ReceiveHit()
    {
        enemyCombat.EnterHitState();

        enemyManager.SetMovementEnabled(false);

        enemyAnimator.PlayHit();
    }

    public void EndHitReaction()
    {
        enemyCombat.ExitHitState();

        enemyManager.SetMovementEnabled(true);
    }

    private void ValidateReferences()
    {
        if (enemyManager == null)
        {
            Debug.LogError(
                $"{nameof(EnemyHitReaction)} requires an {nameof(EnemyManager)} reference.",
                this
            );
        }

        if (enemyCombat == null)
        {
            Debug.LogError(
                $"{nameof(EnemyHitReaction)} requires an {nameof(EnemyCombat)} reference.",
                this
            );
        }

        if (enemyAnimator == null)
        {
            Debug.LogError(
                $"{nameof(EnemyHitReaction)} requires an {nameof(EnemyAnimator)} reference.",
                this
            );
        }
    }
}