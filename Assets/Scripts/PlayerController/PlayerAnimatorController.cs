using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;
    private PlayerInputActions input;
    private AttackHitbox attackHitbox;

    private bool isSlashing;
    private bool canCombo;
    private bool slash2Triggered;

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
            Debug.LogError("PlayerAnimatorController could not find an AttackHitbox.");
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
        if (!isSlashing)
        {
            Move();
        }
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

        Quaternion targetRotation = Quaternion.LookRotation(movement);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void OnSlash(
        UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!isSlashing)
        {
            StartSlash();
            return;
        }

        if (canCombo)
        {
            StartSlash2();
        }
    }

    private void StartSlash()
    {
        isSlashing = true;
        slash2Triggered = false;
        canCombo = false;

        animator.SetTrigger("Slash1");
    }

    private void StartSlash2()
    {
        slash2Triggered = true;
        canCombo = false;

        animator.SetTrigger("Slash2");
    }

    // --------------------------------------------------
    // COMBO
    // --------------------------------------------------

    public void OpenComboWindow()
    {
        canCombo = true;
        Debug.Log("Combo Window OPEN");
    }

    public void CloseComboWindow()
    {
        canCombo = false;
        Debug.Log("Combo Window CLOSED");
    }

    // Called when Slash1 actually finishes.
    // If Slash2 was triggered, keep the player locked.
    // Otherwise, the attack sequence is finished.
    public void CompleteSlash1()
    {
        if (!slash2Triggered)
        {
            EndAttack();
        }
    }

    // --------------------------------------------------
    // HITBOX
    // --------------------------------------------------

    public void EnableHitbox()
    {
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

    // --------------------------------------------------
    // FINAL ATTACK
    // --------------------------------------------------

    public void EndSlash()
    {
        EndAttack();

        Debug.Log("Slash End");
    }

    private void EndAttack()
    {
        isSlashing = false;
        canCombo = false;
        slash2Triggered = false;

        DisableHitbox();
    }
}