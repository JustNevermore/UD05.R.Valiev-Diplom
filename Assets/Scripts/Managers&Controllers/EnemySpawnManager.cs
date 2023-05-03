﻿using System.Collections.Generic;
using Environment;
using UnityEngine;

namespace Managers_Controllers
{
    public class EnemySpawnManager : MonoBehaviour
    {
        [SerializeField] private int maxSpawnDist;
        [SerializeField] private float spawnDelay;
        [SerializeField] private float medDistLimit;
        [SerializeField] private float hardDistLimit;
        [Space]
        [SerializeField] private List<RoomSpawnerConfig> easySetups;
        [SerializeField] private List<RoomSpawnerConfig> medSetups;
        [SerializeField] private List<RoomSpawnerConfig> hardSetups;
        public float MedDistLimit => medDistLimit;
        public float HardDistLimit => hardDistLimit;

        public float SpawnDelay => spawnDelay;

        public int MaxSpawnDist => maxSpawnDist;

        public RoomSpawnerConfig GetSpawnerConfig(RoomDifficulty difficulty)
        {
            RoomSpawnerConfig config = ScriptableObject.CreateInstance<RoomSpawnerConfig>();
            
            switch (difficulty)
            {
                case RoomDifficulty.Easy:
                    config = easySetups[Random.Range(0, easySetups.Count)];
                    break;
                case RoomDifficulty.Medium:
                    config = medSetups[Random.Range(0, medSetups.Count)];
                    break;
                case RoomDifficulty.Hard:
                    config = hardSetups[Random.Range(0, hardSetups.Count)];
                    break;
            }

            return config;
        }
    }
}