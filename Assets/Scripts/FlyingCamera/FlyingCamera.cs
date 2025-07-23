using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FlyingCamera : MonoBehaviour
{
    [SerializeField] private bool _active = true;

    [SerializeField] private bool _enableRotation = true;
    [SerializeField] private float _mouseSense = 1.8f;

    [SerializeField] private bool _enableTranslation = true;
    [SerializeField] private float _translationSpeed = 55f;

    [SerializeField] private bool _enableMovement = true;
    [SerializeField] private float _movementSpeed = 10f;
    [SerializeField] private float _boostedSpeed = 50f;

    [SerializeField] private float _rotSmoothSpeed = 10f;

    [SerializeField] private bool _enableSpeedAcceleration = true;
    [SerializeField] private float _speedAccelerationFactor = 1.5f;

    [SerializeField] private CameraBounds cameraBounds;

    [SerializeField] private float minPitch = -85f;
    [SerializeField] private float maxPitch = 85f;

    [SerializeField] private CursorModeController cursorModeController;

    // Input events
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private float _scrollInput;
    private bool _boostInput;
    private bool _moveUpInput;
    private bool _moveDownInput;

    // Rotation state
    private float yaw;
    private float pitch;
    private Vector2 _smoothedLook;

    // Acceleration helper
    private float _currentIncrease = 1f;
    private float _currentIncreaseMem = 0f;

    private void Start()
    {
        var e = transform.eulerAngles;
        yaw = e.y;
        pitch = e.x;

        // Subscribe to centralized input events
        var ic = InputController.Instance;
        ic.OnMove += v => _moveInput = v;
        ic.OnLook += v => _lookInput = v;
        ic.OnScroll += v => _scrollInput = v;
        ic.OnBoost += b => _boostInput = b;
        ic.OnMoveUp += b => _moveUpInput = b;
        ic.OnMoveDown += b => _moveDownInput = b;
        ic.OnToggleUI += OnToggleUIMode;
    }

    private void OnDestroy()
    {
        // Unsubscribe
        if (InputController.Instance != null)
        {
            var ic = InputController.Instance;
            ic.OnMove -= v => _moveInput = v;
            ic.OnLook -= v => _lookInput = v;
            ic.OnScroll -= v => _scrollInput = v;
            ic.OnBoost -= b => _boostInput = b;
            ic.OnMoveUp -= b => _moveUpInput = b;
            ic.OnMoveDown -= b => _moveDownInput = b;
            ic.OnToggleUI -= OnToggleUIMode;
        }
    }

    private void OnToggleUIMode()
    {
        // Reset one‑frame UI-driven inputs
        _moveInput = Vector2.zero;
        _lookInput = Vector2.zero;
        _scrollInput = 0f;
        _boostInput = false;
        _moveUpInput = false;
        _moveDownInput = false;
    }

    private void CalculateCurrentIncrease(bool moving)
    {
        _currentIncrease = Time.deltaTime;
        if (!_enableSpeedAcceleration || !moving)
        {
            _currentIncreaseMem = 0f;
            _currentIncrease = Time.deltaTime;
            return;
        }
        _currentIncreaseMem += Time.deltaTime * (_speedAccelerationFactor - 1f);
        _currentIncrease = Time.deltaTime + Mathf.Pow(_currentIncreaseMem, 3f) * Time.deltaTime;
    }

    private void Update()
    {
        if (!_active) return;
        if (cursorModeController != null && cursorModeController.IsUIMode)
            return;

        // Zoom
        if (_enableTranslation && !Mathf.Approximately(_scrollInput, 0f))
        {
            transform.Translate(
                Vector3.forward * _scrollInput
                * Time.deltaTime * _translationSpeed
            );
        }

        // Movement
        if (_enableMovement)
        {
            Vector3 delta = transform.forward * _moveInput.y
                          + transform.right * _moveInput.x;

            delta += _moveUpInput ? Vector3.up : Vector3.zero;
            delta -= _moveDownInput ? Vector3.up : Vector3.zero;
            //if (_moveUpInput) delta += Vector3.up;
            //if (_moveDownInput) delta -= Vector3.up;

            bool moving = delta.sqrMagnitude > 0f;
            CalculateCurrentIncrease(moving);

            if (moving)
            {
                float speed = _boostInput ? _boostedSpeed : _movementSpeed;
                transform.position += delta * speed * _currentIncrease;
            }
        }
    }

    private void LateUpdate()
    {
        if ((cursorModeController == null || !cursorModeController.IsUIMode) && _enableRotation)
        {
            _smoothedLook.x = Mathf.Lerp(
                _smoothedLook.x, _lookInput.x,
                _rotSmoothSpeed * Time.deltaTime
            );
            _smoothedLook.y = Mathf.Lerp(
                _smoothedLook.y, _lookInput.y,
                _rotSmoothSpeed * Time.deltaTime
            );

            yaw += _smoothedLook.x * _mouseSense * Time.deltaTime;
            pitch -= _smoothedLook.y * _mouseSense * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }

        if (cameraBounds != null)
            transform.position = cameraBounds.ClampToBounds(transform.position);
    }
}
