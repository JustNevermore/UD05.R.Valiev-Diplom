using System;
using Environment;
using UnityEngine;
using Zenject;

namespace Managers_Controllers
{
    public class GameManager : MonoBehaviour
    {
        private DungeonGenerator _dungeonGenerator;


        [Inject]
        private void Construct(DungeonGenerator dungeonGenerator)
        {
            _dungeonGenerator = dungeonGenerator;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                _dungeonGenerator.GenerateDungeon();
            }
            
            if (Input.GetKeyDown(KeyCode.M))
            {
                _dungeonGenerator.DestroyDungeon();
            }
        }
    }
}