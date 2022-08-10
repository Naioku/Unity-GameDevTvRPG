using Core;
using Movement;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(Mover))]
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float weaponRange = 2f;
        
        private Transform _target;

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
    }
}
