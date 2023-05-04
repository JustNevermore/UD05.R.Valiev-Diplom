using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Managers_Controllers;
using Player;
using Signals;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Environment.Rooms
{
    public class RoomMaster : MonoBehaviour
    {
        private SignalBus _signalBus;
        private EnemySpawnManager _spawnManager;
        private PoolManager _poolManager;
        private SphereCollider _collider;
        
        private int _maxSpawnDist;
        private Room _myRoom;
        private RoomType _type;
        private RoomDifficulty _difficulty;
        private RoomSpawnerConfig _config;

        private int _enemyAlive;
        private const float _roomClearDelay = 3f;

        private Collider[] _colliders = new Collider[50];

        [SerializeField] private Light light;
        private Color32 _greenLight = new Color32(206, 255, 208, 255);


        [Inject]
        private void Construct(SignalBus signalBus, EnemySpawnManager spawnManager, PoolManager pool)
        {
            _signalBus = signalBus;
            _spawnManager = spawnManager;
            _poolManager = pool;
        }
        
        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
            _myRoom = GetComponentInParent<Room>();
            _type = _myRoom.Type;
            _maxSpawnDist = _spawnManager.MaxSpawnDist;
        }

        private void Start()
        {
            SetDifficulty();
            SetupRoom();
            _signalBus.Subscribe<OnEnemyDeathSignal>(DeathRegister);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<OnEnemyDeathSignal>(DeathRegister);
        }

        private void SetDifficulty()
        {
            var distance= (transform.position - Vector3.zero).magnitude;

            if (distance < _spawnManager.MedDistLimit)
            {
                _difficulty = RoomDifficulty.Easy;
            }
            else if (distance > _spawnManager.MedDistLimit && distance < _spawnManager.HardDistLimit)
            {
                _difficulty = RoomDifficulty.Medium;
            }
            else
            {
                _difficulty = RoomDifficulty.Hard;
            }
        }
        
        private void SetupRoom()
        {
            _config = _spawnManager.GetSpawnerConfig(_difficulty);
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.GetComponent<PlayerController>())
            {
                _collider.enabled = false;
                BattleMode();
            }
        }

        private void BattleMode()
        {
            _myRoom.SetActiveOtherRooms(false);
            _myRoom.CloseDoors(true);

            if (_type == RoomType.Common)
            {
                SpawnEnemies();
            }
        }

        private async void SpawnEnemies()
        {
            _enemyAlive = 0;
            
            List<Vector2Int> coords = new List<Vector2Int>();

            var half = Convert.ToInt32(_maxSpawnDist * 0.5f);
            
            for (int y = -half; y <= half; y++)
            {
                for (int x = -half; x <= half; x++)
                {
                    coords.Add(new Vector2Int(x, y));
                }
            }

            if (_config.SwordSkelSpawn)
            {
                var rnd = Random.Range(_config.SwordSkelMin, _config.SwordSkelMax + 1);

                _enemyAlive += rnd;

                for (int i = 0; i < rnd; i++)
                {
                    var tr = transform.position;
                    var enemy = _poolManager.GetSwordSkel();
                    var select = Random.Range(0, coords.Count);
                    var pos = coords[select];
                    enemy.transform.position = new Vector3(tr.x + pos.x, tr.y,tr.z + pos.y);
                    coords.RemoveAt(select);
                    
                    await UniTask.Delay(TimeSpan.FromSeconds(_spawnManager.SpawnDelay));
                }
            }
            
            if (_config.BowSkelSpawn)
            {
                var rnd = Random.Range(_config.BowSkelMin, _config.BowSkelMax + 1);

                _enemyAlive += rnd;

                for (int i = 0; i < rnd; i++)
                {
                    var tr = transform.position;
                    var enemy = _poolManager.GetBowSkel();
                    var select = Random.Range(0, coords.Count);
                    var pos = coords[select];
                    enemy.transform.position = new Vector3(tr.x + pos.x, tr.y,tr.z + pos.y);
                    coords.RemoveAt(select);
                    
                    await UniTask.Delay(TimeSpan.FromSeconds(_spawnManager.SpawnDelay));
                }
            }
            
            if (_config.StaffSkelSpawn)
            {
                var rnd = Random.Range(_config.StaffSkelMin, _config.StaffSkelMax + 1);

                _enemyAlive += rnd;

                for (int i = 0; i < rnd; i++)
                {
                    var tr = transform.position;
                    var enemy = _poolManager.GetStaffSkel();
                    var select = Random.Range(0, coords.Count);
                    var pos = coords[select];
                    enemy.transform.position = new Vector3(tr.x + pos.x, tr.y,tr.z + pos.y);
                    coords.RemoveAt(select);
                    
                    await UniTask.Delay(TimeSpan.FromSeconds(_spawnManager.SpawnDelay));
                }
            }
        }

        private void DeathRegister()
        {
            _enemyAlive--;

            if (_enemyAlive == 0)
            {
                RoomClear();
            }
        }
        
        private async void RoomClear()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_roomClearDelay));
            
            _myRoom.SetActiveOtherRooms(true);
            _myRoom.CloseDoors(false);
            light.color = _greenLight;
            gameObject.SetActive(false);
        }
    }
}