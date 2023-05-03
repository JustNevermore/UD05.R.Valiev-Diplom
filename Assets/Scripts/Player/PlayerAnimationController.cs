using ItemBehaviours;
using Signals;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private SignalBus _signalBus;

        private Animator _animator;

        [SerializeField] private RuntimeAnimatorController unarmedAnimator;
        [SerializeField] private RuntimeAnimatorController swordAnimator;
        [SerializeField] private RuntimeAnimatorController bowAnimator;
        [SerializeField] private RuntimeAnimatorController staffAnimator;
        
        private static readonly int MoveBool = Animator.StringToHash("Move");

        public Animator Anim => _animator;


        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            _signalBus.Subscribe<OnChangeWeaponTypeSignal>(UpdateAnimator);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<OnChangeWeaponTypeSignal>(UpdateAnimator);
        }

        private void UpdateAnimator(OnChangeWeaponTypeSignal signal)
        {
            switch (signal.Type)
            {
                case WeaponType.None:
                    _animator.runtimeAnimatorController = unarmedAnimator;
                    break;
                case WeaponType.Sword:
                    _animator.runtimeAnimatorController = swordAnimator;
                    break;
                case WeaponType.Bow:
                    _animator.runtimeAnimatorController = bowAnimator;
                    break;
                case WeaponType.Staff:
                    _animator.runtimeAnimatorController = staffAnimator;
                    break;
            }
        }

        public void Move(bool flag)
        {
            _animator.SetBool(MoveBool, flag);
        }
    }
}