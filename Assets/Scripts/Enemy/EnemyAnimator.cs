using UnityEngine;

public sealed class EnemyAnimator : MonoBehaviour
{
    private static readonly int SpeedHash =
        Animator.StringToHash("Speed");

    private static readonly int AttackHash =
        Animator.StringToHash("Attack");

    private static readonly int HitHash =
        Animator.StringToHash("Hit");

    [Header("References")]
    [SerializeField] private EnemyMovement enemyMovement;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (enemyMovement == null)
        {
            enemyMovement = GetComponent<EnemyMovement>();
        }

        ValidateReferences();
    }

    private void Update()
    {
        UpdateLocomotion();
    }

    public void PlayAttack()
    {
        animator.SetTrigger(AttackHash);
    }

    public void PlayHit()
    {
        animator.SetTrigger(HitHash);
    }

    private void UpdateLocomotion()
    {
        animator.SetFloat(
            SpeedHash,
            enemyMovement.CurrentSpeed
        );
    }

    private void ValidateReferences()
    {
        if (animator == null)
        {
            Debug.LogError(
                $"{nameof(EnemyAnimator)} requires an Animator component.",
                this
            );
        }

        if (enemyMovement == null)
        {
            Debug.LogError(
                $"{nameof(EnemyAnimator)} requires an {nameof(EnemyMovement)} reference.",
                this
            );
        }
    }
}