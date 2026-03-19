using UnityEngine;
using UnityEngine.Serialization;

namespace Interaction.Valve
{
    public class ValveAnimation: MonoBehaviour
    {
        [Header("Valve Reference")]
        [SerializeField] private Valve valve;
        private ValveLogic _logic;

        private void Start()
        {
            _logic = valve.Logic;
            _logic.OnProgressChanged += change;
        }

        private void change(float progress)
        {
            transform.rotation = Quaternion.Euler(-1 * progress * 360, 0, 0);
        }
    }
}