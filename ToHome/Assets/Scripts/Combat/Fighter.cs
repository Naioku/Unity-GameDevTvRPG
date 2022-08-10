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
        
        private Transform _target;
        private float _timeSinceLastAttack;
        
        private static readonly int Attack1 = Animator.StringToHash("attack");

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            
            if (_target == null) return;
            
            bool isInRange = Vector3.Distance(transform.position, _target.position) <= weaponRange;
            if (!isInRange)
            {
                GetComponent<Mover>().MoveTo(_target.position);
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
            _target.GetComponent<Health>().TakeDamage(weaponDamage);
        }

        public void Attack(CombatTarget target)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _target = target.transform;
        }

        public void CancelAction()
        {
            _target = null;
        }
    }
}
