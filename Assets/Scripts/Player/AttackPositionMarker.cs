using UnityEngine;

namespace Player
{
    public class AttackPositionMarker : MonoBehaviour
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