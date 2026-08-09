using System.Collections;
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
        Vector2 inputVector = input.Player.Move.ReadValue<Vector2>();

        Vector3 movement = new Vector3(inputVector.x, 0f, inputVector.y);

        bool isMoving = inputVector != Vector2.zero;
        bool isRunning = input.Player.Run.IsPressed();

        float speed = isRunning ? runSpeed : walkSpeed;

        characterController.Move(movement * speed * Time.deltaTime);

        float currentSpeed = movement.magnitude * speed;

        animator.SetFloat("Speed", currentSpeed, 0.15f, Time.deltaTime);

        if (!isMoving)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(movement);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void OnIdle(InputAction.CallbackContext context)
    {
    }

    private void OnSlash(InputAction.CallbackContext context)
    {
        if (isPlayingAction)
            return;

        StartCoroutine(PlayAction("Slash"));
    }

    private void OnDeath(InputAction.CallbackContext context)
    {
        if (isPlayingAction)
            return;

        StartCoroutine(PlayAction("Death"));
    }

    private IEnumerator PlayAction(string stateName)
    {
        isPlayingAction = true;

        animator.Play(stateName);

        // Wait one frame so Animator actually enters the new state.
        yield return null;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Wait for the animation to finish.
        yield return new WaitForSeconds(stateInfo.length);

        isPlayingAction = false;

        // Return to the Movement Blend Tree.
        animator.Play("Movement");
    }
}