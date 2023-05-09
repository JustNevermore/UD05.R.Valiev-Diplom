using PoolObjects;
using PoolObjects.Vfx;
using UnityEngine;
using Zenject;

namespace Managers_Controllers
{
    public class FxPoolManager : MonoBehaviour
    {
        private DiContainer _diContainer;
        
        [Header("BaseEffects")]
        [SerializeField] private int basePoolCapacity;
        [SerializeField] private bool basePoolExpand;
        [Space]
        [SerializeField] private HitEffect hitPrefab;
        [SerializeField] private DotEffect dotPrefab;

        [Header("RareEffects")]
        [SerializeField] private int rarePoolCapacity;
        [SerializeField] private bool rarePoolExpand;
        [Space]
        [SerializeField] private SlashEffect slashPrefab;
        [SerializeField] private ShockwaveEffect shockwavePrefab;
        [SerializeField] private ArrowHailEffect arrowHailPrefab;
        [SerializeField] private SwordGroundEffect swordGroundPrefab;

        private PoolBase<HitEffect> _hitPool;
        private PoolBase<DotEffect> _dotPool;
        
        private PoolBase<SlashEffect> _slashPool;
        private PoolBase<ShockwaveEffect> _shockwavePool;
        private PoolBase<ArrowHailEffect> _arrowHailPool;
        private PoolBase<SwordGroundEffect> _swordGroundPool;


        [Inject]
        private void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        private void Start()
        {
            _hitPool = new PoolBase<HitEffect>(hitPrefab, basePoolCapacity, basePoolExpand, transform, _diContainer);
            _dotPool = new PoolBase<DotEffect>(dotPrefab, basePoolCapacity, basePoolExpand, transform, _diContainer);
            
            _slashPool = new PoolBase<SlashEffect>(slashPrefab, rarePoolCapacity, rarePoolExpand, transform, _diContainer);
            _shockwavePool = new PoolBase<ShockwaveEffect>(shockwavePrefab, rarePoolCapacity, rarePoolExpand, transform, _diContainer);
            _arrowHailPool = new PoolBase<ArrowHailEffect>(arrowHailPrefab, rarePoolCapacity, rarePoolExpand, transform, _diContainer);
            _swordGroundPool = new PoolBase<SwordGroundEffect>(swordGroundPrefab, rarePoolCapacity, rarePoolExpand, transform, _diContainer);
        }

        public void PlayHitEffect(Vector3 pos)
        {
            var effect = _hitPool.GetPoolElement();
            effect.transform.position = pos;
        }

        public void PlayDotEffect(Vector3 pos)
        {
            var effect = _dotPool.GetPoolElement();
            effect.transform.position = pos;
        }

        public void PlaySlashEffect(Vector3 pos, Quaternion rot)
        {
            var effect = _slashPool.GetPoolElement();
            effect.transform.position = pos;
            effect.transform.rotation = rot;
        }

        public void PlayShocwaveEffect(Vector3 pos)
        {
            var effect = _shockwavePool.GetPoolElement();
            effect.transform.position = pos;
        }

        public void PlayArrowHailEffect(Vector3 pos)
        {
            var effect = _arrowHailPool.GetPoolElement();
            effect.transform.position = pos;
        }
        
        public void PlaySwordGroundEffect(Vector3 pos)
        {
            var effect = _swordGroundPool.GetPoolElement();
            effect.transform.position = pos;
        }
    }
}