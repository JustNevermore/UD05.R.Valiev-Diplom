using UnityEngine;
using Weapons;

namespace Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private RuntimeAnimatorController swordAnimSet;
        [SerializeField] private RuntimeAnimatorController staffAnimSet;
        [SerializeField] private RuntimeAnimatorController bowAnimSet;
        
        private Animator _anim;
        
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int AltAttack = Animator.StringToHash("AltAttack");
        private static readonly int Special = Animator.StringToHash("Special");

        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }

        public void UpdateAnimator()
        {
            var currentWeapon = GetComponentInChildren<Weapon>().CurrentAnim;

            _anim.runtimeAnimatorController = currentWeapon switch
            {
                "sword" => swordAnimSet,
                "staff" => staffAnimSet,
                "bow" => bowAnimSet,
                _ => _anim.runtimeAnimatorController
            };
        }

        public void DoAttack()
        {
            _anim.SetTrigger(Attack);
        }

        public void DoAltAttack()
        {
            _anim.SetTrigger(AltAttack);
        }

        public void DoSpecial()
        {
            _anim.SetTrigger(Special);
        }
    }
}