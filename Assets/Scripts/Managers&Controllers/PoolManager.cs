using Markers;
using UnityEngine;

namespace Managers_Controllers
{
    public class PoolManager : MonoBehaviour
    {
        [SerializeField] private int arrowPoolCapacity;
        [SerializeField] private bool arrowPoolExpand;
        [SerializeField] private Arrow arrowPrefab;

        private PoolBase<Arrow> _arrowPool;

        private void Start()
        {
            _arrowPool = new PoolBase<Arrow>(arrowPrefab, arrowPoolCapacity, arrowPoolExpand, transform);
        }

        public Arrow GetArrow()
        {
            var arrow = _arrowPool.GetPoolElement();
            return arrow;
        }
    }
}