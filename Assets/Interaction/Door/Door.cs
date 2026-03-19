using UnityEngine;

namespace Interaction.Door
{
    public class Door : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float _openTime = 0.5f;
        [SerializeField] private float _maxAngle = 90.0f;
        private DoorLogic _logic;

        public DoorLogic Logic 
        { 
            get 
            {
                if (_logic == null) 
                {
                    _logic = new DoorLogic(_openTime, _maxAngle);
                }
                return _logic;
            }
        }
    }
}