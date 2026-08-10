using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimatorController : MonoBehaviour
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
        input.Player.Slash.performed += OnSlash;
        input.Player.Death.performed += OnDeath;

        input.Player.Enable();
    }

    private void OnDisable()
    {
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

        Vector3 movement = new Vector3(
            inputVector.x,
            0f,
            inputVector.y
        );

        bool isMoving = movement != Vector3.zero;
        bool isRunning = input.Player.Run.IsPressed();


        float speed = isRunning ? runSpeed : walkSpeed;


        characterController.Move(
            movement * speed * Time.deltaTime
        );


        // Controls Idle/Walk/Run Blend Tree
        float animationSpeed = movement.magnitude * speed;

        animator.SetFloat(
            "Speed",
            animationSpeed,
            0.15f,
            Time.deltaTime
        );


        if (!isMoving || isPlayingAction)
            return;


        Quaternion targetRotation = Quaternion.LookRotation(movement);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }


    private void OnSlash(InputAction.CallbackContext context)
    {
        if (isPlayingAction)
            return;

        animator.Play("Slash");

        isPlayingAction = true;

        Invoke(
            nameof(FinishAction),
            GetCurrentAnimationLength()
        );
    }


    private void OnDeath(InputAction.CallbackContext context)
    {
        if (isPlayingAction)
            return;

        animator.Play("Death");

        isPlayingAction = true;

        Invoke(
            nameof(FinishAction),
            GetCurrentAnimationLength()
        );
    }


    private void FinishAction()
    {
        isPlayingAction = false;
    }


    private float GetCurrentAnimationLength()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        return state.length;
    }
}