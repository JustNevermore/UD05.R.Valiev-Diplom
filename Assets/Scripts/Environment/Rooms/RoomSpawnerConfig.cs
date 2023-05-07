using UnityEngine;

namespace Environment.Rooms
{
    [CreateAssetMenu(fileName = "SpawnerConfig", menuName = "SpawnerConfig/New Spawner Config", order = 0)]
    public class RoomSpawnerConfig : ScriptableObject
    {
        [Header("SwordsmanSkeleton")]
        [SerializeField] private bool swordSkelSpawn;
        [SerializeField] private int swordSkelMin;
        [SerializeField] private int swordSkelMax;
        
        [Header("MarksmanSkeleton")]
        [SerializeField] private bool bowSkelSpawn;
        [SerializeField] private int bowSkelMin;
        [SerializeField] private int bowSkelMax;
        
        [Header("CasterSkeleton")]
        [SerializeField] private bool staffSkelSpawn;
        [SerializeField] private int staffSkelMin;
        [SerializeField] private int staffSkelMax;
        [Space]
        [Header("Boss")]
        [Header("BossLich")]
        [SerializeField] private bool bossLichSpawn;

        public bool SwordSkelSpawn => swordSkelSpawn;
        public int SwordSkelMin => swordSkelMin;
        public int SwordSkelMax => swordSkelMax;
        public bool BowSkelSpawn => bowSkelSpawn;
        public int BowSkelMin => bowSkelMin;
        public int BowSkelMax => bowSkelMax;
        public bool StaffSkelSpawn => staffSkelSpawn;
        public int StaffSkelMin => staffSkelMin;
        public int StaffSkelMax => staffSkelMax;

        public bool BossLichSpawn => bossLichSpawn;
    }
}