using UnityEngine;

/// <summary>
/// Responsible for converting player movement input
/// into CharacterController movement and rotation.
///
/// This class does not handle animation or combat.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public sealed class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerManager playerManager;

    [Header("Movement")]
    [SerializeField, Min(0f)] private float walkSpeed = 2f;
    [SerializeField, Min(0f)] private float runSpeed = 5f;

    [Header("Rotation")]
    [SerializeField, Min(0f)] private float rotationSpeed = 10f;

    [Header("Input")]
    [SerializeField, Min(0f)] private float movementThreshold = 0.001f;

    private CharacterController characterController;

    /// <summary>
    /// Current movement speed.
    ///
    /// 0 = idle
    /// 2 = walking
    /// 5 = running
    ///
    /// This value is consumed by PlayerAnimator.
    /// </summary>
    public float CurrentSpeed { get; private set; }

    /// <summary>
    /// Current normalized movement direction.
    /// </summary>
    public Vector3 MovementDirection { get; private set; }

    /// <summary>
    /// Whether the character is currently moving.
    /// </summary>
    public bool IsMoving => CurrentSpeed > movementThreshold;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }

        if (playerManager == null)
        {
            playerManager = GetComponent<PlayerManager>();
        }

        ValidateReferences();
    }

    private void Update()
    {
        if (!CanMove())
        {
            StopMovement();
            return;
        }

        Move();
    }

    private void Move()
    {
        Vector2 input = playerInput.MoveInput;

        Vector3 movement = new Vector3(
            input.x,
            0f,
            input.y
        );

        float inputMagnitude = Mathf.Clamp01(
            movement.magnitude
        );

        if (inputMagnitude <= movementThreshold)
        {
            StopMovement();
            return;
        }

        MovementDirection = movement.normalized;

        CurrentSpeed = playerInput.IsRunning
            ? runSpeed
            : walkSpeed;

        Vector3 movementDelta =
            MovementDirection *
            CurrentSpeed *
            inputMagnitude *
            Time.deltaTime;

        characterController.Move(movementDelta);

        RotateTowards(MovementDirection);
    }

    public void StopMovement()
    {
        CurrentSpeed = 0f;
        MovementDirection = Vector3.zero;
    }

    private void RotateTowards(Vector3 direction)
    {
        if (direction.sqrMagnitude <= movementThreshold)
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

    private bool CanMove()
    {
        return playerManager == null ||
               playerManager.CanMove;
    }

    private void ValidateReferences()
    {
        if (playerInput == null)
        {
            Debug.LogError(
                $"{nameof(PlayerMovement)} requires a {nameof(PlayerInput)} reference.",
                this
            );
        }

        if (playerManager == null)
        {
            Debug.LogError(
                $"{nameof(PlayerMovement)} requires a {nameof(PlayerManager)} reference.",
                this
            );
        }
    }
}