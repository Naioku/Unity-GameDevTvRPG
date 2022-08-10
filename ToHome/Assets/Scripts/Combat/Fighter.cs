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
            var mover = GetComponent<Mover>();
            
            if (_target != null)
            {
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
    }
}
