using Interaction.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interaction
{
    public class Interactor : MonoBehaviour
    {
        [Header("Camera Reference")] 
        [SerializeField] private Transform cameraHolder;
        
        [Header("Raycast setting")]
        [SerializeField] private LayerMask raycastLayerMask;
        [SerializeField] private float raycastDistance;

        private IInteractable _interactable;
        private Collider _hitCollider;
        private PlayerInputActions _input;
        private Vector3 _raycastDirection;
        private bool _isHold;
        
        private void Awake() 
        {
            _input = new PlayerInputActions();
            _input.Player.Interact.performed += OnInteractPerformed;
            _input.Player.Interact.canceled += OnInteractCanceled;
        }

        private void Update()
        {
            if (!_isHold) return;
            
            _raycastDirection = cameraHolder.forward;
            bool collided = Physics.Raycast(cameraHolder.position, _raycastDirection, out var hit, raycastDistance, raycastLayerMask);
            bool isSameObject = collided && hit.collider == _hitCollider;
            
            if (!isSameObject)
            {
                _interactable?.CancelInteraction(); 
                _interactable = null;
                _hitCollider = null; 
            }
            
            _interactable?.UpdateInteraction(Time.deltaTime);
        }

        private void OnEnable() => _input.Enable();
        private void OnDisable() => _input.Disable();

        private void OnDestroy()
        {
            _input.Player.Interact.performed -= OnInteractPerformed;
            _input.Player.Interact.canceled -= OnInteractCanceled;
        }

        private void OnInteractPerformed(InputAction.CallbackContext ctx)
        {
            _isHold = true;
            _raycastDirection = cameraHolder.forward;
            if (Physics.Raycast(cameraHolder.position, _raycastDirection, out var hit, raycastDistance, raycastLayerMask))
            {
                Debug.DrawRay(cameraHolder.position, _raycastDirection * hit.distance, Color.red);
                _hitCollider = hit.collider;
                _interactable = _hitCollider.GetComponent<IInteractable>();
                if (_interactable == null) return;
                _interactable.StartInteraction(this);
            }
            
        }

        private void OnInteractCanceled(InputAction.CallbackContext ctx)
        { 
            _isHold = false;
            _interactable?.StopInteraction();
            _interactable = null;
        }
    }
}