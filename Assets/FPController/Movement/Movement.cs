using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour {
    [Header("Camera Reference")]
    [SerializeField] private Transform cameraHolder;
    
    [Header("Move settings")]
    [SerializeField] private float moveSpeed = 8.0f;
    [SerializeField] private float gravity = -9.8f;
    
    private CharacterController controller;
    private PlayerInputActions _input;
    private Vector2 moveInput = Vector2.zero; 
    private Vector3 moveDirection = Vector3.zero; 
    private Vector3 velocity = Vector3.zero; 

    private void Awake() {
        _input = new PlayerInputActions();
        _input.Player.Move.performed += OnMovePerformed;
        _input.Player.Move.canceled += OnMoveCanceled;
    }

    private void Start() {
        controller = GetComponent<CharacterController>();

        if (controller == null) {
            enabled = false;
        }
        if (cameraHolder == null) {
            enabled = false;
        }
    }

    private void OnEnable() => _input.Enable();
    private void OnDisable() => _input.Disable();

    void Update() {
        if (controller.isGrounded && velocity.y < 0) {
            velocity.y = -2.0f;
        } else {
            velocity.y += gravity * Time.deltaTime;
        }

        Vector3 cameraForward = cameraHolder.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 cameraRight = cameraHolder.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();


        moveDirection = (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;
        
        Vector3 horizontalMove = moveDirection * moveSpeed * Time.deltaTime;
        
        Vector3 finalMove = new Vector3(horizontalMove.x, velocity.y * Time.deltaTime, horizontalMove.z);
        
        controller.Move(finalMove);
    }

    private void OnDestroy()
    {
        _input.Player.Move.performed -= OnMovePerformed;
        _input.Player.Move.canceled -= OnMoveCanceled;
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx) {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx) {
        moveInput = Vector2.zero;
    }
}