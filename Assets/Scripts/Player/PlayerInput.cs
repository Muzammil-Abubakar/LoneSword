using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Responsible only for reading player input.
///
/// This class does not:
/// - Move the player
/// - Rotate the player
/// - Play animations
/// - Perform attacks
/// - Handle combat
/// </summary>
public sealed class PlayerInput : MonoBehaviour
{
    private PlayerInputActions inputActions;

    /// <summary>
    /// Current movement input.
    ///
    /// X = horizontal
    /// Y = forward/backward
    /// </summary>
    public Vector2 MoveInput { get; private set; }

    /// <summary>
    /// True while the Run action is being held.
    /// </summary>
    public bool IsRunning { get; private set; }

    /// <summary>
    /// Fired when the player requests an attack.
    ///
    /// Combat will subscribe to this later.
    /// </summary>
    public event Action AttackRequested;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Slash.performed += OnSlashPerformed;
    }

    private void OnDisable()
    {
        inputActions.Player.Slash.performed -= OnSlashPerformed;

        inputActions.Player.Disable();
    }

    private void OnDestroy()
    {
        inputActions.Dispose();
    }

    private void Update()
    {
        ReadMovement();
        ReadRun();
    }

    private void ReadMovement()
    {
        MoveInput = inputActions.Player.Move.ReadValue<Vector2>();
    }

    private void ReadRun()
    {
        IsRunning = inputActions.Player.Run.IsPressed();
    }

    private void OnSlashPerformed(
        InputAction.CallbackContext context)
    {
        AttackRequested?.Invoke();
    }
}