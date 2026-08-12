using UnityEngine;
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

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 3.5f;
    [SerializeField] private float acceleration = 12f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 8f;

    private NavMeshAgent agent;
    private Animator animator;

    private bool isHit;
    private bool isChasing;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.speed = movementSpeed;
        agent.acceleration = acceleration;
        agent.stoppingDistance = stopDistance;
        agent.autoBraking = true;

        // Rotation is handled manually.
        agent.updateRotation = false;
    }

    private void Update()
    {
        if (player == null || isHit)
        {
            StopMoving();
            return;
        }

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        // Outside detection range.
        if (distance > detectionRange)
        {
            isChasing = false;
            StopMoving();
            return;
        }

        // Start chasing once the player moves far enough away.
        if (!isChasing && distance > chaseDistance)
        {
            isChasing = true;
        }

        // Stop once close enough.
        if (isChasing && distance <= stopDistance)
        {
            isChasing = false;
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            StopMoving();
        }
    }

    private void ChasePlayer()
    {
        agent.isStopped = false;
        agent.stoppingDistance = stopDistance;

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
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    public void StartHitReaction()
    {
        isHit = true;
        isChasing = false;

        StopMoving();

        animator.SetFloat("Speed", 0f);
        animator.SetTrigger("Hit");
    }

    public void EndHitReaction()
    {
        isHit = false;
    }

    private void StopMoving()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.ResetPath();

        animator.SetFloat(
            "Speed",
            0f,
            0.1f,
            Time.deltaTime
        );
    }

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
    }
}