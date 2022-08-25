using System;
using System.Collections.Generic;
using Attributes;
using Core;
using GameDevTV.Utils;
using Movement;
using Saving;
using Stats;
using UnityEngine;

namespace Combat
{
    public class Fighter : MonoBehaviour, IAction, ISavable, IModifierProvider
    {
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private Transform rightHandTransform;
        [SerializeField] private Transform leftHandTransform;
        [SerializeField] private Weapon defaultWeapon;
        
        private Health _target;
        private float _timeSinceLastAttack = Mathf.Infinity;
        private LazyValue<Weapon> _currentWeapon;
        
        private static readonly int Attack1 = Animator.StringToHash("attack");
        private static readonly int StopAttack = Animator.StringToHash("stopAttack");

        private void Awake()
        {
            _currentWeapon = new LazyValue<Weapon>(SetUpDefaultWeapon);
        }

        private void Start()
        {
            _currentWeapon.ForceInit();
            EquipWeapon(_currentWeapon.Value);
        }

        private Weapon SetUpDefaultWeapon()
        {
            AttachWeapon(defaultWeapon);
            return defaultWeapon;
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            
            if (_target == null ||
                _target.IsDead) return;
            
            bool isInRange = Vector3.Distance(transform.position, _target.transform.position) <= _currentWeapon.Value.WeaponRange;
            if (!isInRange)
            {
                GetComponent<Mover>().MoveTo(_target.transform.position);
            }
            else
            {
                GetComponent<Mover>().CancelAction();
                AttackBehaviour();
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            if (weapon == null) return;
            AttachWeapon(weapon);
            _currentWeapon.Value = weapon;
        }

        private void AttachWeapon(Weapon weapon)
        {
            weapon.Spawn(rightHandTransform, leftHandTransform, GetComponent<Animator>());
        }

        public bool CanAttack(GameObject target)
        {
            if (target == null) return false;
            var testedTarget = target.GetComponent<Health>();
            return testedTarget != null && !testedTarget.IsDead;
        }

        public void Attack(GameObject target)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _target = target.GetComponent<Health>();
        }

        public void CancelAction()
        {
            TriggerStopAttackAnimation();
            _target = null;
        }

        public object CaptureState()
        {
            return _currentWeapon.Value.name;
        }

        public void RestoreState(object state)
        {
            var weapon = Resources.Load<Weapon>((string) state);
            EquipWeapon(weapon);
        }

        public Health GetTarget() => _target;

        public IEnumerable<float> GetAdditiveModifiers(Stats.Stats stat)
        {
            if (stat == Stats.Stats.Damage)
            {
                yield return _currentWeapon.Value.WeaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stats.Stats stat)
        {
            if (stat == Stats.Stats.Damage)
            {
                yield return _currentWeapon.Value.PercentageBonus;
            }
        }

        private void TriggerStopAttackAnimation()
        {
            var animator = GetComponent<Animator>();
            animator.ResetTrigger(Attack1);
            animator.SetTrigger(StopAttack);
        }

        private void AttackBehaviour()
        {
            transform.LookAt(_target.transform);
            if (_timeSinceLastAttack < timeBetweenAttacks) return;
            
            TriggerAttackAnimation();
            _timeSinceLastAttack = 0f;
        }

        private void TriggerAttackAnimation()
        {
            var animator = GetComponent<Animator>();
            animator.ResetTrigger(StopAttack);
            animator.SetTrigger(Attack1); // This trigger the Hit() event.
        }
        
        /// <summary>
        /// Triggered by Animation Event.
        /// </summary>
        private void Hit()
        {
            if (_target == null) return;

            var damage = GetComponent<BaseStats>().GetStat(Stats.Stats.Damage);
            // var levelDamage = 0f;
            if (_currentWeapon.Value.HasProjectile())
            {
                _currentWeapon.Value.LunchProjectile(gameObject, rightHandTransform, leftHandTransform, _target, damage);
            }
            else
            {
                _target.TakeDamage(gameObject, damage);
            }
        }
        
        /// <summary>
        /// Triggered by Animation Event.
        /// Leaves the decision to weapon rather than to the animation event, weather it should be shot or not.
        /// </summary>
        private void Shoot()
        {
            Hit();
        }
    }
}
