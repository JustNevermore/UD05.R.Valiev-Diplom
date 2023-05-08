using System;
using System.Collections;
using Markers;
using Player;
using Signals;
using UnityEngine;
using Zenject;

namespace Environment
{
    public class PortalGate : MonoBehaviour
    {
        private PlayerController _playerController;
        private SignalBus _signalBus;
        
        [SerializeField] private PortalDestination destination;
        private SpawnPointMarker _spawnMarker;
        private DungeonSpawnPointMarker _dungeonSpawnMarker;

        private Coroutine _teleportCor;
        private readonly float _teleportDelay = 3f;
        

        [Inject]
        private void Construct(PlayerController playerController, SignalBus signalBus)
        {
            _playerController = playerController;
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _spawnMarker = FindObjectOfType<SpawnPointMarker>();
            _dungeonSpawnMarker = FindObjectOfType<DungeonSpawnPointMarker>();
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.GetComponent<PlayerController>())
            {
                _teleportCor = StartCoroutine(TeleportationCor());
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.GetComponent<PlayerController>())
            {
                StopCoroutine(_teleportCor);
            }
        }

        private IEnumerator TeleportationCor()
        {
            yield return new WaitForSeconds(_teleportDelay);

            if (destination == PortalDestination.Spawn)
            {
                _playerController.transform.position = _spawnMarker.transform.position;
                _signalBus.Fire<ReturnToSpawnSignal>();
            }
            else
            {
                _playerController.transform.position = _dungeonSpawnMarker.transform.position;
                _signalBus.Fire<GoToDungeonSignal>();
            }
        }
    }
}