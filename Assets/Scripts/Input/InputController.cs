using System;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-100)]
public class InputController : MonoBehaviour
{
    public static InputController Instance { get; private set; }

    public event Action<Vector2> OnMove;
    public event Action<Vector2> OnLook;
    public event Action<float> OnScroll;
    public event Action<bool> OnBoost;
    public event Action<bool> OnMoveUp;
    public event Action<bool> OnMoveDown;
    public event Action OnToggleUI;
    public event Action          OnToggleHint;

    private TDInputActions _actions;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        _actions = new TDInputActions();

        // Move
        _actions.CameraControls.Move.performed += ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());
        _actions.CameraControls.Move.canceled += ctx => OnMove?.Invoke(Vector2.zero);

        // Look
        _actions.CameraControls.Look.performed += ctx => OnLook?.Invoke(ctx.ReadValue<Vector2>());
        _actions.CameraControls.Look.canceled += ctx => OnLook?.Invoke(Vector2.zero);

        // Scroll
        _actions.CameraControls.Scroll.performed += ctx => OnScroll?.Invoke(ctx.ReadValue<float>());
        _actions.CameraControls.Scroll.canceled += ctx => OnScroll?.Invoke(0f);

        // Boost
        _actions.CameraControls.Boost.performed += _ => OnBoost?.Invoke(true);
        _actions.CameraControls.Boost.canceled += _ => OnBoost?.Invoke(false);

        // MoveUp/MoveDown
        _actions.CameraControls.MoveUp.performed += _ => OnMoveUp?.Invoke(true);
        _actions.CameraControls.MoveUp.canceled += _ => OnMoveUp?.Invoke(false);

        _actions.CameraControls.MoveDown.performed += _ => OnMoveDown?.Invoke(true);
        _actions.CameraControls.MoveDown.canceled += _ => OnMoveDown?.Invoke(false);

        // ToggleUI
        _actions.CameraControls.ToggleUI.performed += _ => OnToggleUI?.Invoke();
        
        // Hint
        _actions.CameraControls.Hint.performed += _ => OnToggleHint?.Invoke();
    }

    private void OnEnable() => _actions.Enable();
    private void OnDisable() => _actions.Disable();
}
