using Enemies;
using Markers;
using PoolObjects;
using UnityEngine;
using Zenject;

namespace Managers_Controllers
{
    public class PoolManager : MonoBehaviour
    {
        private DiContainer _diContainer;
        
        [Header("Arrow")]
        [SerializeField] private int arrowPoolCapacity;
        [SerializeField] private bool arrowPoolExpand;
        [SerializeField] private Arrow arrowPrefab;
        
        [Header("Spell")]
        [SerializeField] private int spellPoolCapacity;
        [SerializeField] private bool spellPoolExpand;
        [SerializeField] private Spell spellPrefab;
        
        [Header("Rune")]
        [SerializeField] private int runePoolCapacity;
        [SerializeField] private bool runePoolExpand;
        [SerializeField] private Rune runePrefab;
        
        [Header("Turret")]
        [SerializeField] private int turretPoolCapacity;
        [SerializeField] private bool turretPoolExpand;
        [SerializeField] private Doppelganger turretPrefab;
        
        [Header("EnemyArrow")]
        [SerializeField] private int enArPoolCapacity;
        [SerializeField] private bool enArPoolExpand;
        [SerializeField] private EnemyArrow enArPrefab;
        
        [Header("EnemySpell")]
        [SerializeField] private int enSpellPoolCapacity;
        [SerializeField] private bool enSpellPoolExpand;
        [SerializeField] private EnemySpell enSpellPrefab;
        
        [Header("BossSpell")]
        [SerializeField] private int bossSpellPoolCapacity;
        [SerializeField] private bool bossSpellPoolExpand;
        [SerializeField] private BossSpell bossSpellPrefab;
        
        [Header("BossRune")]
        [SerializeField] private int bossRunePoolCapacity;
        [SerializeField] private bool bossRunePoolExpand;
        [SerializeField] private BossRune bossRunePrefab;
        
        [Header("SwordsmanSkeleton")]
        [SerializeField] private int swordSkelPoolCapacity;
        [SerializeField] private bool swordSkelPoolExpand;
        [SerializeField] private SwordsmanSkeleton swordSkelPrefab;
        
        [Header("MarksmanSkeleton")]
        [SerializeField] private int bowSkelPoolCapacity;
        [SerializeField] private bool bowSkelPoolExpand;
        [SerializeField] private MarksmanSkeleton bowSkelPrefab;
        
        [Header("CasterSkeleton")]
        [SerializeField] private int staffSkelPoolCapacity;
        [SerializeField] private bool staffSkelPoolExpand;
        [SerializeField] private CasterSkeleton staffSkelPrefab;

        private PoolBase<Arrow> _arrowPool;
        private PoolBase<Spell> _spellPool;
        private PoolBase<Rune> _runePool;
        private PoolBase<Doppelganger> _turretPool;
        
        private PoolBase<EnemyArrow> _enArrowPool;
        private PoolBase<EnemySpell> _enSpellPool;
        private PoolBase<BossSpell> _bossSpellPool;
        private PoolBase<BossRune> _bossRunePool;

        private PoolBase<SwordsmanSkeleton> _swordSkelPool;
        private PoolBase<MarksmanSkeleton> _bowSkelPool;
        private PoolBase<CasterSkeleton> _staffSkelPool;

        [Inject]
        private void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }
        
        private void Start()
        {
            _arrowPool = new PoolBase<Arrow>(arrowPrefab, arrowPoolCapacity, arrowPoolExpand, transform, _diContainer);
            _spellPool = new PoolBase<Spell>(spellPrefab, spellPoolCapacity,spellPoolExpand, transform, _diContainer);
            _runePool = new PoolBase<Rune>(runePrefab, runePoolCapacity,runePoolExpand, transform, _diContainer);
            _turretPool = new PoolBase<Doppelganger>(turretPrefab, turretPoolCapacity,turretPoolExpand, transform, _diContainer);
            
            _enArrowPool = new PoolBase<EnemyArrow>(enArPrefab, enArPoolCapacity, enArPoolExpand, transform, _diContainer);
            _enSpellPool = new PoolBase<EnemySpell>(enSpellPrefab, enSpellPoolCapacity, enSpellPoolExpand, transform, _diContainer);
            _bossSpellPool = new PoolBase<BossSpell>(bossSpellPrefab, bossSpellPoolCapacity, bossSpellPoolExpand, transform, _diContainer);
            _bossRunePool = new PoolBase<BossRune>(bossRunePrefab, bossRunePoolCapacity, bossRunePoolExpand, transform, _diContainer);

            _swordSkelPool = new PoolBase<SwordsmanSkeleton>(swordSkelPrefab, swordSkelPoolCapacity, swordSkelPoolExpand, transform, _diContainer);
            _bowSkelPool = new PoolBase<MarksmanSkeleton>(bowSkelPrefab, bowSkelPoolCapacity, bowSkelPoolExpand, transform, _diContainer);
            _staffSkelPool = new PoolBase<CasterSkeleton>(staffSkelPrefab, staffSkelPoolCapacity, staffSkelPoolExpand, transform, _diContainer);
        }

        public Arrow GetArrow()
        {
            var arrow = _arrowPool.GetPoolElement();
            return arrow;
        }

        public Spell GetSpell()
        {
            var spell = _spellPool.GetPoolElement();
            return spell;
        }

        public Rune GetRune()
        {
            var rune = _runePool.GetPoolElement();
            return rune;
        }
        
        public Doppelganger GetTurret()
        {
            var turret = _turretPool.GetPoolElement();
            return turret;
        }

        public EnemyArrow GetEnemyArrow()
        {
            var arrow = _enArrowPool.GetPoolElement();
            return arrow;
        }
        
        public EnemySpell GetEnemySpell()
        {
            var spell = _enSpellPool.GetPoolElement();
            return spell;
        }

        public BossSpell GetBossSpell()
        {
            var spell = _bossSpellPool.GetPoolElement();
            return spell;
        }
        
        public BossRune GetBossRune()
        {
            var rune = _bossRunePool.GetPoolElement();
            return rune;
        }

        public SwordsmanSkeleton GetSwordSkel()
        {
            var enemy = _swordSkelPool.GetPoolElement();
            return enemy;
        }
        
        public MarksmanSkeleton GetBowSkel()
        {
            var enemy = _bowSkelPool.GetPoolElement();
            return enemy;
        }
        
        public CasterSkeleton GetStaffSkel()
        {
            var enemy = _staffSkelPool.GetPoolElement();
            return enemy;
        }
    }
}