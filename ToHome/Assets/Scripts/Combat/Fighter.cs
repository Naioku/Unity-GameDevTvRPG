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
        private float _timeSinceLastAttack;
        
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

        private void AttackBehaviour()
        {
            if (_timeSinceLastAttack < timeBetweenAttacks) return;
            
            // This trigger the Hit() event.
            GetComponent<Animator>().SetTrigger(Attack1);
            _timeSinceLastAttack = 0f;
        }
        
        // Animation Event
        private void Hit()
        {
            if (_target == null) return;
            
            _target.TakeDamage(weaponDamage);
        }

        public void Attack(CombatTarget target)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _target = target.GetComponent<Health>();
        }

        public void CancelAction()
        {
            GetComponent<Animator>().SetTrigger(StopAttack);
            _target = null;
        }
    }
}
