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
        
        private Transform _target;
        
        private static readonly int Attack1 = Animator.StringToHash("attack");

        private void Update()
        {
            if (_target == null) return;
            
            bool isInRange = Vector3.Distance(transform.position, _target.position) <= weaponRange;
            if (!isInRange)
            {
                GetComponent<Mover>().MoveTo(_target.position);
            }
            else
            {
                GetComponent<Mover>().CancelAction();
                GetComponent<Animator>().SetTrigger(Attack1);
            }
        }

        public void Attack(CombatTarget target)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _target = target.transform;
            print("Take that, klus!");
        }

        public void CancelAction()
        {
            _target = null;
        }
        
        // Animation Event
        private void Hit()
        {
            
        }
    }
}
