using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;
    private PlayerInputActions input;

    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    private bool isPlayingAction;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        input.Player.Idle.performed += OnIdle;
        input.Player.Slash.performed += OnSlash;
        input.Player.Death.performed += OnDeath;

        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.Idle.performed -= OnIdle;
        input.Player.Slash.performed -= OnSlash;
        input.Player.Death.performed -= OnDeath;

        input.Player.Disable();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (isPlayingAction)
            return;

        Vector2 inputVector = input.Player.Move.ReadValue<Vector2>();

        Vector3 movement = new Vector3(inputVector.x, 0f, inputVector.y);

        bool isMoving = movement != Vector3.zero;
        bool isRunning = input.Player.Run.IsPressed();

        float speed = isRunning ? runSpeed : walkSpeed;

        characterController.Move(movement * speed * Time.deltaTime);

        if (!isMoving)
        {
            animator.Play("Idle");
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(movement);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        if (isRunning)
        {
            animator.Play("Run");
        }
        else
        {
            animator.Play("Walk");
        }
    }

    private void OnIdle(InputAction.CallbackContext context)
    {
        if (!isPlayingAction)
            animator.Play("Idle");
    }

    private void OnSlash(InputAction.CallbackContext context)
    {
        if (isPlayingAction)
            return;

        isPlayingAction = true;
        animator.Play("Slash");
        Invoke(nameof(FinishAction), GetAnimationLength("Slash"));
    }

    private void OnDeath(InputAction.CallbackContext context)
    {
        if (isPlayingAction)
            return;

        isPlayingAction = true;
        animator.Play("Death");
        Invoke(nameof(FinishAction), GetAnimationLength("Death"));
    }

    private void FinishAction()
    {
        isPlayingAction = false;
    }

    private float GetAnimationLength(string stateName)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.length;
    }
}