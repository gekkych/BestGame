using UnityEngine;
using UnityEngine.InputSystem;

/// Controls camera via mouse for playable character
/// Expected for virtual camera pined to CameraHolder
public class CameraView : MonoBehaviour 
    {    
    [Header("Camera Reference")]
    [SerializeField] private Transform cameraHolder;

    [Header("Mouse settings")]
    [SerializeField] private float sensitivity = 2.0f; 
    [SerializeField] private float verticalAngleMax = 90.0f;
    [SerializeField] private bool invertY = false; 
    private PlayerInputActions _input;
    private Vector2 _deltaMouse;
    private float _verticalRotation = 0.0f; 
    private float _horizontalRotation = 0.0f;

    private void Awake() 
    {
        _input = new PlayerInputActions();
        _input.Player.Look.performed += OnLookPerformed;
        _input.Player.Look.canceled += OnLookCanceled;
    }

    void Start() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _horizontalRotation = transform.localEulerAngles.y;
        _verticalRotation = cameraHolder.localEulerAngles.x;
        
        if (_verticalRotation > 180f) _verticalRotation -= 360f;
    }

    private void OnEnable() => _input.Enable();

    private void OnDisable() => _input.Disable();

    void Update() 
    {
        if (_deltaMouse == Vector2.zero) return;

        float mouseX = _deltaMouse.x * sensitivity * Time.deltaTime;
        float mouseY = _deltaMouse.y * sensitivity * Time.deltaTime;

        _horizontalRotation += mouseX;
        
        if (invertY)
        {
            _verticalRotation += mouseY; 
        }
        else
        {
            _verticalRotation -= mouseY; 
        }

        _verticalRotation = Mathf.Clamp(_verticalRotation, -verticalAngleMax, verticalAngleMax);

        transform.localRotation = Quaternion.Euler(0f, _horizontalRotation, 0f);
        cameraHolder.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f); 

        _deltaMouse = Vector2.zero;
        
    }

    private void OnDestroy() 
    {
        _input.Player.Look.performed -= OnLookPerformed;
        _input.Player.Look.canceled -= OnLookCanceled;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;   
    }

    private void OnLookPerformed(InputAction.CallbackContext context) 
    {
        _deltaMouse = context.ReadValue<Vector2>();
    } 

    private void OnLookCanceled(InputAction.CallbackContext context) 
    {
        _deltaMouse = Vector2.zero;
    }
}
