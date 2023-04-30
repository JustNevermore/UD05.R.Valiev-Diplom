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

        private PoolBase<Arrow> _arrowPool;
        private PoolBase<Spell> _spellPool;
        private PoolBase<Rune> _runePool;
        private PoolBase<Doppelganger> _turretPool;
        
        private PoolBase<EnemyArrow> _enArrowPool;
        private PoolBase<EnemySpell> _enSpellPool;

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
    }
}