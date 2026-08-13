/*using UnityEngine;
using UnityEngine.AI;

public class SkeletonAI : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("Detection")]
    [SerializeField] private float detectionRange = 10f;

    [Header("Combat Distance")]
    [SerializeField] private float chaseDistance = 1.8f;
    [SerializeField] private float stopDistance = 1.3f;
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("Movement")]
    [SerializeField] private float closeSpeed = 1.5f;
    [SerializeField] private float farSpeed = 4f;

    [Tooltip("Distance at which the skeleton switches from close speed to far speed.")]
    [SerializeField] private float fastSpeedDistance = 4f;

    [SerializeField] private float acceleration = 12f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 8f;

    private NavMeshAgent agent;
    private Animator animator;

    private float attackTimer;

    private bool isHit;
    private bool isChasing;
    private bool isAttacking;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.speed = closeSpeed;
        agent.acceleration = acceleration;
        agent.stoppingDistance = stopDistance;
        agent.autoBraking = true;

        // Rotation is handled manually.
        agent.updateRotation = false;
    }

    private void Update()
    {
        if (player == null)
        {
            StopMoving();
            return;
        }

        // ---------------------------------------------
        // HIT REACTION
        // ---------------------------------------------

        if (isHit)
        {
            StopMoving();
            return;
        }

        // ---------------------------------------------
        // ATTACK
        // ---------------------------------------------

        if (isAttacking)
        {
            // Completely stop movement while attacking.
            StopMoving();
            return;
        }

        attackTimer -= Time.deltaTime;

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        // ---------------------------------------------
        // OUTSIDE DETECTION RANGE
        // ---------------------------------------------

        if (distance > detectionRange)
        {
            isChasing = false;
            StopMoving();
            return;
        }

        // ---------------------------------------------
        // COMBAT DISTANCE
        // ---------------------------------------------

        if (distance <= stopDistance)
        {
            isChasing = false;

            StopMoving();

            TryAttack();

            return;
        }

        // ---------------------------------------------
        // CHASE STATE
        // ---------------------------------------------

        if (!isChasing && distance > chaseDistance)
        {
            isChasing = true;
        }

        if (isChasing)
        {
            ChasePlayer(distance);
        }
        else
        {
            StopMoving();
        }
    }

    // --------------------------------------------------
    // CHASE
    // --------------------------------------------------

    private void ChasePlayer(float distance)
    {
        agent.isStopped = false;
        agent.stoppingDistance = stopDistance;

        // ---------------------------------------------
        // DYNAMIC SPEED
        // ---------------------------------------------

        if (distance >= fastSpeedDistance)
        {
            agent.speed = farSpeed;
        }
        else
        {
            agent.speed = closeSpeed;
        }

        agent.SetDestination(player.position);

        RotateTowardsMovement();

        float speed = agent.velocity.magnitude;

        animator.SetFloat(
            "Speed",
            speed,
            0.1f,
            Time.deltaTime
        );
    }

    private void RotateTowardsMovement()
    {
        Vector3 direction = agent.desiredVelocity;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
        {
            return;
        }

        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    // --------------------------------------------------
    // HIT REACTION
    // --------------------------------------------------

    public void StartHitReaction()
    {
        isHit = true;
        isChasing = false;
        isAttacking = false;

        StopMoving();

        animator.SetFloat("Speed", 0f);
        animator.SetTrigger("Hit");
    }

    public void EndHitReaction()
    {
        isHit = false;
    }

    // --------------------------------------------------
    // ATTACK
    // --------------------------------------------------

    private void TryAttack()
    {
        if (attackTimer > 0f)
        {
            return;
        }

        // Enter attack state.
        isAttacking = true;
        isChasing = false;

        // Completely stop the NavMeshAgent.
        StopMoving();

        // Face the player before attacking.
        Vector3 directionToPlayer =
            player.position - transform.position;

        directionToPlayer.y = 0f;

        if (directionToPlayer.sqrMagnitude > 0.0001f)
        {
            transform.rotation =
                Quaternion.LookRotation(directionToPlayer);
        }

        // Start cooldown.
        attackTimer = attackCooldown;

        

        // Trigger attack animation.
        animator.SetTrigger("Attack");
    }

    // --------------------------------------------------
    // ATTACK END
    // --------------------------------------------------

    // Called by SkeletonAttackState when the
    // EnemyAttack Animator state finishes.
    public void EndAttack()
    {
        if (!isAttacking)
        {
            return;
        }

        isAttacking = false;

        // IMPORTANT:
        // Do NOT immediately resume the NavMeshAgent.
        //
        // The next Update() will look at the player's
        // current position and decide whether we should
        // chase or remain stopped.
        StopMoving();

        
    }

    // --------------------------------------------------
    // MOVEMENT
    // --------------------------------------------------

    private void StopMoving()
    {
        if (agent != null)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.ResetPath();
        }

        if (animator != null)
        {
            animator.SetFloat(
                "Speed",
                0f,
                0.1f,
                Time.deltaTime
            );
        }
    }

    // --------------------------------------------------
    // DEBUG GIZMOS
    // --------------------------------------------------

    private void OnDrawGizmosSelected()
    {
        // Detection range
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            detectionRange
        );

        // Chase distance
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            chaseDistance
        );

        // Stop distance
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(
            transform.position,
            stopDistance
        );

        // Fast speed distance
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(
            transform.position,
            fastSpeedDistance
        );

        // Current AI state indicator.
        if (isHit)
        {
            Gizmos.color = Color.magenta;
        }
        else if (isAttacking)
        {
            Gizmos.color = Color.red;
        }
        else if (isChasing)
        {
            Gizmos.color = Color.blue;
        }
        else
        {
            Gizmos.color = Color.white;
        }

        Gizmos.DrawWireCube(
            transform.position + Vector3.up * 2.5f,
            Vector3.one * 0.3f
        );
    }
}*/