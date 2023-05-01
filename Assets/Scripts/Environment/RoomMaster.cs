using System;
using Player;
using UnityEngine;

namespace Environment
{
    public class RoomMaster : MonoBehaviour
    {
        [SerializeField] private new Light light;
        private Color32 _greenLight = new Color32(206, 255, 208, 255);

        private Room _myRoom;
        
        private void Awake()
        {
            _myRoom = GetComponentInParent<Room>();
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.GetComponent<PlayerController>())
            {
                _myRoom.DeactivateOther();
                _myRoom.CloseDoors();
            }
        }
    }
}