using UnityEngine;

namespace Player
{
    public class SpecialPositionMarker : MonoBehaviour
    {
        [SerializeField] private float radius;
        [SerializeField] private Color color;
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, radius);
        }
    }
}