using UnityEngine;

public sealed class EnemyCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemyAnimator enemyAnimator;

    [Header("Combat")]
    [SerializeField, Min(0f)] private float attackRange = 1.5f;

    private bool isAttacking;

    public bool IsAttacking => isAttacking;

    private void Awake()
    {
        if (enemyManager == null)
        {
            enemyManager = GetComponent<EnemyManager>();
        }

        if (enemyMovement == null)
        {
            enemyMovement = GetComponent<EnemyMovement>();
        }

        if (enemyAnimator == null)
        {
            enemyAnimator = GetComponent<EnemyAnimator>();
        }

        ValidateReferences();
    }

    private void Update()
    {
        if (isAttacking)
        {
            return;
        }

        Transform target = enemyManager.Target;

        if (target == null)
        {
            return;
        }

        float distance = Vector3.Distance(
            transform.position,
            target.position
        );

        if (distance <= attackRange)
        {
            StartAttack();
        }
    }

    private void StartAttack()
    {
        if (isAttacking)
        {
            return;
        }

        isAttacking = true;

        enemyManager.SetMovementEnabled(false);

        enemyMovement.FaceTarget(
            enemyManager.Target
        );

        enemyAnimator.PlayAttack();
    }

    public void EndAttack()
    {
        if (!isAttacking)
        {
            return;
        }

        isAttacking = false;

        enemyManager.SetMovementEnabled(true);
    }

    public void CancelAttack()
    {
        if (!isAttacking)
        {
            return;
        }

        isAttacking = false;
    }

    private void ValidateReferences()
    {
        if (enemyManager == null)
        {
            Debug.LogError(
                $"{nameof(EnemyCombat)} requires an {nameof(EnemyManager)} reference.",
                this
            );
        }

        if (enemyMovement == null)
        {
            Debug.LogError(
                $"{nameof(EnemyCombat)} requires an {nameof(EnemyMovement)} reference.",
                this
            );
        }

        if (enemyAnimator == null)
        {
            Debug.LogError(
                $"{nameof(EnemyCombat)} requires an {nameof(EnemyAnimator)} reference.",
                this
            );
        }
    }
}