using System;
using InventorySystem;
using ItemBehaviours;
using Signals;
using UnityEditor.Animations;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private SignalBus _signalBus;

        private Animator _animator;

        [SerializeField] private AnimatorController unarmedAnimator;
        [SerializeField] private AnimatorController swordAnimator;
        
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
            }
        }

        public void Move(bool flag)
        {
            _animator.SetBool(MoveBool, flag);
        }
    }
}