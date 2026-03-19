using System;
using Interaction.Door;
using UnityEngine;

namespace Room
{
    public class RoomController : MonoBehaviour
    {
        public int Id;
        public bool IsEntered { get; set; }

        [SerializeField] private Sequencer _sequencer;
        [SerializeField] private Transform _enter;
        [SerializeField] private Transform _exit;
        [SerializeField] private DoorInteract[] _doorsInRoom;
        public Transform Enter => _enter;
        public Transform Exit => _exit;

        public event Action<RoomController> onEnter;

        public void OnTriggerEnter(Collider other)
        {
            if (IsEntered || !other.CompareTag("Player")) return;

            IsEntered = true;
            onEnter?.Invoke(this);
        }

        public void ReturnToDepo()
        {
            foreach (var door in _doorsInRoom)
            {
                if (door != null) door.ResetDoorInstant();
            }

            IsEntered = false;
            gameObject.SetActive(false);

            transform.position = new Vector3(Id * 100, -1000, 0);
        }
        public void Teleport(Transform to) //Teleport room to exit of current room
        {
            transform.rotation = to.rotation;
            Vector3 offset = transform.position - _enter.position;
            transform.position = to.position + offset;
        }
    }
}
