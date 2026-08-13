using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerManager playerManager;

    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    private void Awake()
    {
        characterController =
            GetComponent<CharacterController>();

        playerManager =
            GetComponent<PlayerManager>();

        if (characterController == null)
        {
            Debug.LogError(
                "PlayerMovement could not find CharacterController."
            );
        }

        if (playerManager == null)
        {
            Debug.LogError(
                "PlayerMovement could not find PlayerManager."
            );
        }
    }

    private void Update()
    {
        if (!playerManager.CanMove())
        {
            return;
        }

        Move();
    }

    private void Move()
    {
        Vector2 input =
            playerManager.GetMoveInput();

        Vector3 movement = new Vector3(
            input.x,
            0f,
            input.y
        );

        bool isMoving =
            movement.sqrMagnitude > 0.001f;

        bool isRunning =
            playerManager.IsRunning();

        float speed =
            isRunning ? runSpeed : walkSpeed;

        characterController.Move(
            movement * speed * Time.deltaTime
        );

        playerManager.SetMovementAnimation(
            movement.magnitude * speed
        );

        if (!isMoving)
        {
            return;
        }

        Quaternion targetRotation =
            Quaternion.LookRotation(movement);

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
    }

    public void StopMovement()
    {
        playerManager.SetMovementAnimation(0f);
    }
}