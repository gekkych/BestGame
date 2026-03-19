using UnityEngine;

namespace Interaction.Valve
{
    public class Valve : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float _interactionTime;
        private ValveLogic _logic;

        public ValveLogic Logic 
        { 
            get 
            {
                if (_logic == null) 
                {
                    _logic = new ValveLogic(_interactionTime);
                }
                return _logic;
            }
        }
    }
}