using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Environment.Rooms;
using Managers_Controllers;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Environment
{
    public class DungeonGenerator : MonoBehaviour
    {
        private RoomPoolManager _pool;
        
        [SerializeField] private int roomSpawnCount;
        [SerializeField] private int bossRoomSpawnMinCount;

        private readonly int _matrixSize = 21;
        private int _centerCoord;
        private readonly float _prefabSideValue = 60f;
        private int _commonRoomLimit;
        private readonly int _connectRoomLimit = 500;
        private readonly int _connectBossLimit = 100;
        private int _bossLimit;
        private int _bossCount;
        private bool _switcher;

        private Room[,] _spawnedRooms;

        [Inject]
        private void Construct(RoomPoolManager poolManager)
        {
            _pool = poolManager;
        }

        public void GenerateDungeon()
        {
            _centerCoord = Convert.ToInt32((_matrixSize - 1) * 0.5f);

            _switcher = false;
            
            do
            {
                TryGenerateDungeon();
            } while (!_switcher);
            
            
            foreach (var room in _spawnedRooms)
            {
                if (room)
                {
                    room.Init(_spawnedRooms);
                }
            }
        }

        private void TryGenerateDungeon()
        {
            _bossLimit = _connectBossLimit;
            _commonRoomLimit = roomSpawnCount - bossRoomSpawnMinCount;
            _bossCount = bossRoomSpawnMinCount;
            
            _spawnedRooms = new Room[_matrixSize, _matrixSize];
            _spawnedRooms[_centerCoord, _centerCoord] = _pool.GetStartRoom();
            _spawnedRooms[_centerCoord, _centerCoord].transform.position = Vector3.zero;

            for (int i = 0; i < roomSpawnCount; i++)
            {
                PlaceRoom(i);
            }

            while (_bossCount > 0)
            {
                for (int i = _commonRoomLimit; i < roomSpawnCount; i++)
                {
                    PlaceRoom(i);
                }
            
                _bossLimit--;
                if (_bossLimit == 0)
                {
                    break;
                }
            }
            
            if (_bossCount == bossRoomSpawnMinCount)
            {
                DestroyDungeon();
                _switcher = false;
            }
            
            _switcher = true;
        }

        private void PlaceRoom(int selector)
        {
            HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();

            for (int x = 0; x < _spawnedRooms.GetLength(0); x++)
            {
                for (int y = 0; y < _spawnedRooms.GetLength(1); y++)
                {
                    if (_spawnedRooms[x,y] == null)
                        continue;

                    var maxX = _spawnedRooms.GetLength(0) - 1;
                    var maxY = _spawnedRooms.GetLength(1) - 1;

                    if (x > 0 && _spawnedRooms[x - 1, y] == null)
                        vacantPlaces.Add(new Vector2Int(x - 1, y));

                    if (y > 0 && _spawnedRooms[x, y - 1] == null)
                        vacantPlaces.Add(new Vector2Int(x, y - 1));

                    if (x < maxX && _spawnedRooms[x + 1, y] == null)
                        vacantPlaces.Add(new Vector2Int(x + 1, y));

                    if (y < maxY && _spawnedRooms[x, y + 1] == null)
                        vacantPlaces.Add(new Vector2Int(x, y + 1));
                }
            }

            Room newRoom = SelectRoomPrefab(selector);

            var limit = _connectRoomLimit;
            
            while (limit-- > 0)
            {
                Vector2Int position = vacantPlaces.ElementAt(Random.Range(0, vacantPlaces.Count));

                if (ConnectRoom(newRoom, position))
                {
                    newRoom.transform.position = new Vector3(position.x - _centerCoord, 0, position.y - _centerCoord) * _prefabSideValue;
                    _spawnedRooms[position.x, position.y] = newRoom;
                    if (selector >= _commonRoomLimit)
                    {
                        _bossCount--;
                    }
                    return;
                }
            }
            
            newRoom.gameObject.SetActive(false);
        }

        private Room SelectRoomPrefab(int selector)
        {
            if (selector < _commonRoomLimit)
            {
                Room newRoom = _pool.GetCommonRoom();
                return newRoom;
            }
            else
            {
                Room newRoom = _pool.GetBossRoom();
                return newRoom;
            }
        }

        private bool ConnectRoom(Room room, Vector2Int pos)
        {
            var maxX = _spawnedRooms.GetLength(0) - 1;
            var maxY = _spawnedRooms.GetLength(1) - 1;

            List<Vector2Int> neighbours = new List<Vector2Int>();

            if (room.doorU != null && pos.y < maxY && _spawnedRooms[pos.x, pos.y + 1]?.doorD != null)
                neighbours.Add(Vector2Int.up);
            
            if (room.doorD != null && pos.y > 0 && _spawnedRooms[pos.x, pos.y - 1]?.doorU != null)
                neighbours.Add(Vector2Int.down);
            
            if (room.doorR != null && pos.x < maxX && _spawnedRooms[pos.x + 1, pos.y]?.doorL != null)
                neighbours.Add(Vector2Int.right);
            
            if (room.doorL != null && pos.x > 0 && _spawnedRooms[pos.x - 1, pos.y]?.doorR != null)
                neighbours.Add(Vector2Int.left);

            if (neighbours.Count == 0) return false;

                Vector2Int selectedDir = neighbours[Random.Range(0, neighbours.Count)];

            Room selectedRoom = _spawnedRooms[pos.x + selectedDir.x, pos.y + selectedDir.y];

            if (selectedDir == Vector2Int.up)
            {
                room.doorU.SetActive(false);
                selectedRoom.doorD.SetActive(false);
            }
            else if (selectedDir == Vector2Int.down)
            {
                room.doorD.SetActive(false);
                selectedRoom.doorU.SetActive(false);
            }
            else if (selectedDir == Vector2Int.right)
            {
                room.doorR.SetActive(false);
                selectedRoom.doorL.SetActive(false);
            }
            else if (selectedDir == Vector2Int.left)
            {
                room.doorL.SetActive(false);
                selectedRoom.doorR.SetActive(false);
            }
            
            return true;
        }
        
        private void DestroyDungeon()
        {
            foreach (var room in _spawnedRooms)
            {
                if (room != null)
                {
                    room.gameObject.SetActive(false);
                }
            }
        }
    }
}