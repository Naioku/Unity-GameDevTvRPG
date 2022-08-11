using Combat;
using Core;
using Movement;
using UnityEngine;

namespace Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 3f;

        private Fighter _fighter;
        private Health _health;
        private Mover _mover;
        private ActionScheduler _actionScheduler;
        private GameObject _player;
        
        private Vector3 _guardPosition;
        private float _timeSinceLastSawPlayer = Mathf.Infinity;

        private void Start()
        {
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _player = GameObject.FindWithTag("Player");

            _guardPosition = transform.position;
        }

        void Update()
        {
            if (_health.IsDead) return;
            
            if (IsInAttackRangeWith(_player) && _fighter.CanAttack(_player))
            {
                _fighter.Attack(_player);
                _timeSinceLastSawPlayer = 0f;

            }
            else if (_timeSinceLastSawPlayer <= suspicionTime)
            {
                _actionScheduler.CancelCurrentAction();
            }
            else
            {
                _mover.StartMoveAction(_guardPosition);
            }

            _timeSinceLastSawPlayer += Time.deltaTime;
        }

        private bool IsInAttackRangeWith(GameObject target)
        {
            return Vector3.Distance(transform.position, target.transform.position) <= chaseDistance;
        }

        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
