using UnityEngine;

namespace Interaction.Door
{
    public class DoorAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Door _door;
        [SerializeField] private Vector3 _rotationAxis = Vector3.up; 
        
        private DoorLogic _logic;
        private Quaternion _initialRotation;

        private void Awake()
        {
            _initialRotation = transform.localRotation;
        }

        private void Start()
        {
            _logic = _door.Logic;
            _logic.OnProgressChanged += UpdateRotation;
        }

        private void UpdateRotation(float currentAngle)
        {
            Quaternion deltaRotation = Quaternion.Euler(_rotationAxis * currentAngle);
            transform.localRotation = _initialRotation * deltaRotation;
        }

        private void OnDestroy()
        {
            if (_logic != null)
            {
                _logic.OnProgressChanged -= UpdateRotation;
            }
        }
    }
}