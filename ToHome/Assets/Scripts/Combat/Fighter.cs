using Attributes;
using Core;
using Movement;
using Saving;
using UnityEngine;

namespace Combat
{
    [RequireComponent(
        typeof(Mover), 
        typeof(Animator),
        typeof(ActionScheduler))]
    public class Fighter : MonoBehaviour, IAction, ISavable
    {
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private Transform rightHandTransform;
        [SerializeField] private Transform leftHandTransform;
        [SerializeField] private Weapon defaultWeapon;
        
        private Health _target;
        private float _timeSinceLastAttack = Mathf.Infinity;
        private Weapon _currentWeapon;
        
        private static readonly int Attack1 = Animator.StringToHash("attack");
        private static readonly int StopAttack = Animator.StringToHash("stopAttack");

        private void Start()
        {
            if (_currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            
            if (_target == null ||
                _target.IsDead) return;
            
            bool isInRange = Vector3.Distance(transform.position, _target.transform.position) <= _currentWeapon.WeaponRange;
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
            weapon.Spawn(rightHandTransform, leftHandTransform, GetComponent<Animator>());
            _currentWeapon = weapon;
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
            return _currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            var weapon = Resources.Load<Weapon>((string) state);
            EquipWeapon(weapon);
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

            if (_currentWeapon.HasProjectile())
            {
                _currentWeapon.LunchProjectile(rightHandTransform, leftHandTransform, _target);
            }
            else
            {
                _target.TakeDamage(_currentWeapon.WeaponDamage);
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
