using System;
using System.Collections.Generic;
using Environment.Rooms;
using PoolObjects;
using Signals;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Managers_Controllers
{
    public class RoomPoolManager : MonoBehaviour
    {
        private DiContainer _diContainer;
        private SignalBus _signalBus;

        [SerializeField] private int poolCapacity;
        [SerializeField] private bool poolExpand;
        [SerializeField] private List<Room> startRoomPrefab;
        [SerializeField] private List<Room> commonRoomPrefab;
        [SerializeField] private List<Room> bossRoomPrefab;

        private PoolBase<Room> _startRoom;
        private PoolBase<Room> _room1;
        private PoolBase<Room> _room2;
        private PoolBase<Room> _room3;
        private PoolBase<Room> _room4;
        private PoolBase<Room> _room5;
        private PoolBase<Room> _room6;
        private PoolBase<Room> _room7;
        private PoolBase<Room> _room8;
        private PoolBase<Room> _room9;
        private PoolBase<Room> _room10;
        private PoolBase<Room> _room11;
        private PoolBase<Room> _boss1;
        private PoolBase<Room> _boss2;
        private PoolBase<Room> _boss3;
        private PoolBase<Room> _boss4;

        private List<PoolBase<Room>> _commonRooms;
        private List<PoolBase<Room>> _bossRooms;


        [Inject]
        private void Construct(DiContainer diContainer, SignalBus signalBus)
        {
            _diContainer = diContainer;
            _signalBus = signalBus;
        }

        private void Start()
        {
            _signalBus.Subscribe<DisableAllPoolObjectsSignal>(DisableAllPoolObjects);
            _commonRooms = new List<PoolBase<Room>>();
            _bossRooms = new List<PoolBase<Room>>();
            
            _startRoom = new PoolBase<Room>(startRoomPrefab[0], poolCapacity, poolExpand, transform, _diContainer);
            
            _commonRooms.Add(_room1 = new PoolBase<Room>(commonRoomPrefab[0], poolCapacity, poolExpand, transform, _diContainer));
            _commonRooms.Add(_room2 = new PoolBase<Room>(commonRoomPrefab[1], poolCapacity, poolExpand, transform, _diContainer));
            _commonRooms.Add(_room3 = new PoolBase<Room>(commonRoomPrefab[2], poolCapacity, poolExpand, transform, _diContainer));
            _commonRooms.Add(_room4 = new PoolBase<Room>(commonRoomPrefab[3], poolCapacity, poolExpand, transform, _diContainer));
            _commonRooms.Add(_room5 = new PoolBase<Room>(commonRoomPrefab[4], poolCapacity, poolExpand, transform, _diContainer));
            _commonRooms.Add(_room6 = new PoolBase<Room>(commonRoomPrefab[5], poolCapacity, poolExpand, transform, _diContainer));
            _commonRooms.Add(_room7 = new PoolBase<Room>(commonRoomPrefab[6], poolCapacity, poolExpand, transform, _diContainer));
            _commonRooms.Add(_room8 = new PoolBase<Room>(commonRoomPrefab[7], poolCapacity, poolExpand, transform, _diContainer));
            _commonRooms.Add(_room9 = new PoolBase<Room>(commonRoomPrefab[8], poolCapacity, poolExpand, transform, _diContainer));
            _commonRooms.Add(_room10 = new PoolBase<Room>(commonRoomPrefab[9], poolCapacity, poolExpand, transform, _diContainer));
            _commonRooms.Add(_room11 = new PoolBase<Room>(commonRoomPrefab[10], poolCapacity, poolExpand, transform, _diContainer));
            
            _bossRooms.Add(_boss1 = new PoolBase<Room>(bossRoomPrefab[0], poolCapacity, poolExpand, transform, _diContainer));
            _bossRooms.Add(_boss2 = new PoolBase<Room>(bossRoomPrefab[1], poolCapacity, poolExpand, transform, _diContainer));
            _bossRooms.Add(_boss3 = new PoolBase<Room>(bossRoomPrefab[2], poolCapacity, poolExpand, transform, _diContainer));
            _bossRooms.Add(_boss4 = new PoolBase<Room>(bossRoomPrefab[3], poolCapacity, poolExpand, transform, _diContainer));
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<DisableAllPoolObjectsSignal>(DisableAllPoolObjects);
        }

        public Room GetStartRoom()
        {
            var room = _startRoom.GetPoolElement();
            return room;
        }
        
        public Room GetCommonRoom()
        {
            var rnd = Random.Range(0, _commonRooms.Count);
            var room = _commonRooms[rnd].GetPoolElement();
            return room;
        }
        
        public Room GetBossRoom()
        {
            var rnd = Random.Range(0, _bossRooms.Count);
            var room = _bossRooms[rnd].GetPoolElement();
            return room;
        }

        private void DisableAllPoolObjects()
        {
            _startRoom.DisablePoolElements();
            
            foreach (var pool in _commonRooms)
            {
                pool.DisablePoolElements();
            }

            foreach (var pool in _bossRooms)
            {
                pool.DisablePoolElements();
            }
        }
    }
}