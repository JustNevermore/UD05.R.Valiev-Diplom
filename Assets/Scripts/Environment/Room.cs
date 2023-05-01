using System;
using UnityEngine;
using Zenject;

namespace Environment
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private RoomMaster roomMaster;
        
        private Room[,] _allRooms;
        private int _capacity;
        
        public GameObject doorU;
        public GameObject doorD;
        public GameObject doorR;
        public GameObject doorL;

        public void Init(int cap, Room[,] rooms)
        {
            _capacity = cap;
            _allRooms = new Room[_capacity, _capacity];
            _allRooms = rooms;

            if (roomMaster) roomMaster.gameObject.SetActive(true);
        }

        public void DeactivateOther()
        {
            foreach (var room in _allRooms)
            {
                if (room)
                {
                    if (room != this)
                    {
                        room.gameObject.SetActive(false);
                    }
                }
            }
        }

        public void CloseDoors()
        {
            if (doorU) doorU.SetActive(true);
            if (doorD) doorD.SetActive(true);
            if (doorR) doorR.SetActive(true);
            if (doorL) doorL.SetActive(true);
        }
    }
}