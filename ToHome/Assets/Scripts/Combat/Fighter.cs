using System;
using Core;
using Movement;
using UnityEngine;

namespace Combat
{
    [RequireComponent(
        typeof(Mover), 
        typeof(Animator),
        typeof(ActionScheduler))]
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private float weaponDamage = 5f;
        
        private Health _target;
        private float _timeSinceLastAttack = Mathf.Infinity;
        
        private static readonly int Attack1 = Animator.StringToHash("attack");
        private static readonly int StopAttack = Animator.StringToHash("stopAttack");

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            
            if (_target == null ||
                _target.IsDead) return;
            
            bool isInRange = Vector3.Distance(transform.position, _target.transform.position) <= weaponRange;
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

        // Animation Event
        private void Hit()
        {
            if (_target == null) return;
            
            _target.TakeDamage(weaponDamage);
        }
    }
}
