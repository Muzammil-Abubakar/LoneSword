using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public sealed class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyManager enemyManager;

    [Header("Movement")]
    [SerializeField, Min(0f)] private float closeSpeed = 1.5f;
    [SerializeField, Min(0f)] private float farSpeed = 4f;

    [Tooltip("Distance at which the enemy switches from close speed to far speed.")]
    [SerializeField, Min(0f)] private float fastSpeedDistance = 4f;

    [SerializeField, Min(0f)] private float acceleration = 12f;

    [Header("Rotation")]
    [SerializeField, Min(0f)] private float rotationSpeed = 8f;

    private NavMeshAgent agent;

    public float CurrentSpeed { get; private set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (enemyManager == null)
        {
            enemyManager = GetComponent<EnemyManager>();
        }

        ConfigureAgent();
        ValidateReferences();
    }

    private void Update()
    {
        if (!CanMove())
        {
            StopMovement();
            return;
        }

        Transform target = enemyManager.Target;

        if (target == null)
        {
            StopMovement();
            return;
        }

        MoveTowardsTarget(target);
    }

    private void MoveTowardsTarget(Transform target)
    {
        float distance = Vector3.Distance(
            transform.position,
            target.position
        );

        agent.speed = distance >= fastSpeedDistance
            ? farSpeed
            : closeSpeed;

        agent.isStopped = false;
        agent.SetDestination(target.position);

        RotateTowardsMovement();

        CurrentSpeed = agent.velocity.magnitude;
    }

    private void RotateTowardsMovement()
    {
        Vector3 direction = agent.desiredVelocity;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.0001f)
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

    public void StopMovement()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.ResetPath();

        CurrentSpeed = 0f;
    }

    public void FaceTarget(Transform target)
{
    if (target == null)
    {
        return;
    }

    Vector3 direction = target.position - transform.position;
    direction.y = 0f;

    if (direction.sqrMagnitude <= 0.0001f)
    {
        return;
    }

    transform.rotation =
        Quaternion.LookRotation(direction);
}

    private bool CanMove()
    {
        return enemyManager != null &&
               enemyManager.CanMove;
    }

    private void ConfigureAgent()
    {
        agent.acceleration = acceleration;
        agent.autoBraking = true;

        // Rotation is handled by this component.
        agent.updateRotation = false;
    }

    private void ValidateReferences()
    {
        if (enemyManager == null)
        {
            Debug.LogError(
                $"{nameof(EnemyMovement)} requires an {nameof(EnemyManager)} reference.",
                this
            );
        }
    }
}