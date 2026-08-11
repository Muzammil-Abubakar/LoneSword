using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;
    private PlayerInputActions input;
    private bool slash2Triggered;
    private bool isSlashing;
    private bool canCombo;

    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        input = new PlayerInputActions();
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

        // Controls Idle / Walk / Run Blend Tree
        float animationSpeed = movement.magnitude * speed;

        animator.SetFloat(
            "Speed",
            animationSpeed,
            0.15f,
            Time.deltaTime
        );

        if (!isMoving)
            return;

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
            isSlashing = true;
            slash2Triggered = false;

            animator.SetTrigger("Slash1");
        }
        else if (canCombo)
        {
            slash2Triggered = true;
            animator.SetTrigger("Slash2");
        }
    }
    
    public void OpenComboWindow()
    {
        canCombo = true;
        Debug.unityLogger.Log("Open Combo Window");  
    }
    
    public void CloseComboWindow()
    {
        canCombo = false;

        if (!slash2Triggered)
        {
            isSlashing = false;
        }
    }
    
    public void EndSlash1()
    {
        if (!canCombo)
        {
            isSlashing = false;
        }
    }
    
    public void EndSlash()
    {
        isSlashing = false;
        canCombo = false;
        Debug.unityLogger.Log("Slash End");  
    }
    
    public void Testing()
    {
        
        Debug.unityLogger.Log("TestPass");  
    }
}