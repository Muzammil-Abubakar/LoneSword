using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerInputActions input;

    public Vector2 MoveInput { get; private set; }

    public bool IsRunning =>
        input.Player.Run.IsPressed();

    public bool SlashPressed { get; private set; }

    private void Awake()
    {
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
        MoveInput =
            input.Player.Move.ReadValue<Vector2>();

        SlashPressed = false;
    }

    private void OnSlash(
        UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        SlashPressed = true;
    }
}