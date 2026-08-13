using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;
    private PlayerInputActions input;
    private AttackHitbox attackHitbox;

    private bool isSlashing;
    private bool isHit;

    // Movement input is sampled every frame.
    // While attacking, it is deliberately not applied.
    private Vector2 currentMoveInput;

    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        attackHitbox = GetComponentInChildren<AttackHitbox>(true);

        input = new PlayerInputActions();

        if (attackHitbox == null)
        {
            Debug.LogError(
                "PlayerAnimatorController could not find an AttackHitbox."
            );
        }
    }

    private void OnEnable()
    {
        input.Player.Enable();
        input.Player.Slash.performed += OnSlash;
    }

    private void OnDisable()
    {
        input.Player.Slash.performed -= OnSlash;
        input.Player.Disable();
    }

    private void Update()
    {
        // Always read the latest input.
        // We do NOT apply it while attacking.
        currentMoveInput = input.Player.Move.ReadValue<Vector2>();

        if (isSlashing || isHit)
        {
            return;
        }

        Move(currentMoveInput);
    }

    private void Move(Vector2 inputVector)
    {
        Vector3 movement = new Vector3(
            inputVector.x,
            0f,
            inputVector.y
        );

        bool isMoving = movement.sqrMagnitude > 0.001f;
        bool isRunning = input.Player.Run.IsPressed();

        float speed = isRunning ? runSpeed : walkSpeed;

        characterController.Move(
            movement * speed * Time.deltaTime
        );

        animator.SetFloat(
            "Speed",
            movement.magnitude * speed,
            0.15f,
            Time.deltaTime
        );

        if (!isMoving)
        {
            return;
        }

        Quaternion targetRotation =
            Quaternion.LookRotation(movement);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    // --------------------------------------------------
    // SLASH
    // --------------------------------------------------

    private void OnSlash(
        UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (isSlashing)
        {
            return;
        }

        StartSlash();
    }

    private void StartSlash()
    {
        isSlashing = true;

        // Make sure the locomotion animation is not
        // carrying a stale movement value into the attack.

        animator.SetTrigger("Slash1");
    }

    public void EndSlash()
    {
        EndAttack();
    }

    private void EndAttack()
    {
        
        isSlashing = false;

        // Reset the animation's movement value immediately.

        DisableHitbox();
    }
    
    // --------------------------------------------------
// HIT REACTION
// --------------------------------------------------

    public void TakeHit()
    {
        isSlashing = false;
        isHit = true;

        DisableHitbox();

        animator.SetFloat("Speed", 0f);
        animator.SetTrigger("Hit");

        Debug.Log("PLAYER HIT REACTION!");
    }
    
    public void EndHitReaction()
    {
        isHit = false;
        Debug.Log("PLAYER HIT REACTION ENDED!");
    }

    // --------------------------------------------------
    // HITBOX
    // --------------------------------------------------

    public void EnableHitbox()
    {
        
        animator.SetFloat("Speed", 0f);
        if (attackHitbox == null)
        {
            return;
        }

        attackHitbox.EnableHitbox();
    }

    public void DisableHitbox()
    {
        if (attackHitbox == null)
        {
            return;
        }

        attackHitbox.DisableHitbox();
    }
}