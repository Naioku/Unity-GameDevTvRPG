using Movement;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(Mover))]
    public class Fighter : MonoBehaviour
    {
        [SerializeField] private float weaponRange = 2f;
        
        private Transform _target;

        private void Update()
        {
            if (_target == null) return;
            
            var mover = GetComponent<Mover>();
            bool isInRange = Vector3.Distance(transform.position, _target.position) <= weaponRange;
            if (!isInRange)
            {
                mover.MoveTo(_target.position);
            }
            else
            {
                mover.Stop();
            }
        }

        public void Attack(CombatTarget target)
        {
            _target = target.transform;
            print("Take that, klus!");
        }

        public void Cancel()
        {
            _target = null;
        }
    }
}
