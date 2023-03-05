using UnityEngine;

namespace Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected float attackDamage = 10f;
        
        protected const string SwordAnim = "sword";
        protected const string StaffAnim = "staff";
        protected const string BowAnim = "bow";
        protected string _currentAnim;

        public string CurrentAnim => _currentAnim;

        protected abstract void SetAnimSelector();
        
        public abstract void DoAttack();
    }
}