using UnityEngine;

public sealed class EnemyCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemyAnimator enemyAnimator;

    [SerializeField] private AttackHitbox attackHitbox;

    [Header("Combat")]
    [SerializeField, Min(0f)] private float attackRange = 1.5f;

    private bool isAttacking;
    private bool isHitReacting;

    public bool IsAttacking => isAttacking;
    public bool IsHitReacting => isHitReacting;

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

        if (attackHitbox == null)
        {
            attackHitbox = GetComponentInChildren<AttackHitbox>(true);
        }

        ValidateReferences();
    }

    private void Update()
    {
        if (isAttacking || isHitReacting)
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
        if (!isAttacking || isHitReacting)
        {
            return;
        }

        isAttacking = false;

        DisableHitbox();

        enemyManager.SetMovementEnabled(true);
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

    public void CancelAttack()
    {
        if (!isAttacking)
        {
            return;
        }

        isAttacking = false;
        DisableHitbox();
    }

    public void EnterHitState()
    {
        isHitReacting = true;
        isAttacking = false;
        
        DisableHitbox();
    }

    public void ExitHitState()
    {
        isHitReacting = false;
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