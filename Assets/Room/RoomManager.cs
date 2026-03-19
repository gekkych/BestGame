using System.Collections.Generic;
using UnityEngine;

namespace Room
{
    public enum ManagerStates
    {
        Default, Search, Chase
    }
    // This is a very important class
    // Its purpose to manage all infinity room system
    public class RoomManager : MonoBehaviour
    {
        [SerializeField] private RoomController[] _roomPrefabs;
        [SerializeField] private int _poolSizePerPrefab = 3;
        private static RoomManager instance;
        private ManagerStates state = ManagerStates.Default;
        private List<RoomController> _pool = new List<RoomController>();
        private RoomController _lastRoom;
        private RoomController _currentRoom; 

        
        public void Start()
        {
            InitializePool();
            _currentRoom = GetRandomFreeRoom();
            _currentRoom.gameObject.SetActive(true);
            _currentRoom.transform.position = Vector3.zero;
            SpawnNext();
        }
        
        private void InitializePool()
        {
            int idCounter = 0;
            foreach (var prefab in _roomPrefabs)
            {
                for (int i = 0; i < _poolSizePerPrefab; i++)
                {
                    RoomController room = Instantiate(prefab);
                    room.Id = idCounter++;
                    room.onEnter += OnPlayerEnteredRoom;
                    room.ReturnToDepo(); 
                    _pool.Add(room);
                }
            }
        }

        private void OnPlayerEnteredRoom(RoomController room)
        {
            if (room == _currentRoom) return;
            if (_lastRoom != null && _lastRoom != room)
            {
                _lastRoom.ReturnToDepo();
            }
            _lastRoom = _currentRoom;
            _currentRoom = room;
            SpawnNext();
        }
        
        private void SpawnNext()
        {
            RoomController next = GetRandomFreeRoom();
            next.gameObject.SetActive(true);
            next.Teleport(_currentRoom.Exit);
        }
        
        private RoomController GetRandomFreeRoom()
        {
            var freeRooms = _pool.FindAll(r => !r.IsEntered && r != _currentRoom);
            return freeRooms[Random.Range(0, freeRooms.Count)];
        }
    }
}