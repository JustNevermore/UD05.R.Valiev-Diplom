using Markers;
using PoolObjects;
using UnityEngine;

namespace Managers_Controllers
{
    public class PoolManager : MonoBehaviour
    {
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

        private PoolBase<Arrow> _arrowPool;
        private PoolBase<Spell> _spellPool;
        private PoolBase<Rune> _runePool;
        private PoolBase<Doppelganger> _turretPool;

        private void Start()
        {
            _arrowPool = new PoolBase<Arrow>(arrowPrefab, arrowPoolCapacity, arrowPoolExpand, transform);
            _spellPool = new PoolBase<Spell>(spellPrefab, spellPoolCapacity,spellPoolExpand, transform);
            _runePool = new PoolBase<Rune>(runePrefab, runePoolCapacity,runePoolExpand, transform);
            _turretPool = new PoolBase<Doppelganger>(turretPrefab, turretPoolCapacity,turretPoolExpand, transform);
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
    }
}