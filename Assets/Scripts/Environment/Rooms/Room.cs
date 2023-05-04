using System.Collections.Generic;
using UnityEngine;

namespace Environment.Rooms
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private RoomType type;
        [SerializeField] private RoomMaster roomMaster;
        
        private Room[,] _allRooms;

        public GameObject doorU;
        public GameObject doorD;
        public GameObject doorR;
        public GameObject doorL;

        private List<GameObject> _interactableDoors;

        public RoomType Type => type;

        public void Init(Room[,] rooms)
        {
            _allRooms = rooms;

            _interactableDoors = new List<GameObject>();

            if (doorU && !doorU.activeInHierarchy) _interactableDoors.Add(doorU);
            if (doorD && !doorD.activeInHierarchy) _interactableDoors.Add(doorD);
            if (doorR && !doorR.activeInHierarchy) _interactableDoors.Add(doorR);
            if (doorL && !doorL.activeInHierarchy) _interactableDoors.Add(doorL);

                if (roomMaster) roomMaster.gameObject.SetActive(true);
        }

        public void SetActiveOtherRooms(bool flag)
        {
            foreach (var room in _allRooms)
            {
                if (room)
                {
                    if (room != this)
                    {
                        room.gameObject.SetActive(flag);
                    }
                }
            }
        }

        public void CloseDoors(bool flag)
        {
            foreach (var door in _interactableDoors)
            {
                door.SetActive(flag);
            }
        }
    }
}