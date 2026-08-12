using UnityEngine;

public class SkeletonAI : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private float stopDistance = 1.5f;

    private CharacterController characterController;
    private Animator animator;
    private bool isHit;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player == null)
            return;

        if (isHit)
            return;

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distance <= detectionRange)
        {
            if (distance > stopDistance)
            {
                ChasePlayer();
            }
            else
            {
                StopMoving();
            }
        }
        else
        {
            StopMoving();
        }
    }

    private void ChasePlayer()
    {
        Vector3 direction = player.position - transform.position;

        direction.y = 0f;

        if (direction == Vector3.zero)
            return;

        direction.Normalize();

        characterController.Move(
            direction * moveSpeed * Time.deltaTime
        );

        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        animator.SetFloat(
            "Speed",
            moveSpeed,
            0.15f,
            Time.deltaTime
        );
    }
    
    public void StartHitReaction()
    {
        isHit = true;

        animator.SetFloat("Speed", 0f);
        animator.SetTrigger("Hit");
    }
    
    public void EndHitReaction()
    {
        isHit = false;
    }
    
    private void StopMoving()
    {
        animator.SetFloat(
            "Speed",
            0f,
            0.15f,
            Time.deltaTime
        );
    }


    private void OnDrawGizmosSelected()
    {
        // Draw detection range in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position,
            detectionRange
        );
    }
}