using Attributes;
using Combat;
using Core;
using GameDevTV.Utils;
using Movement;
using UnityEngine;

namespace Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 3f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float waypointTolerance = 1f;
        [SerializeField] private float dwellingTime = 2f;

        private Fighter _fighter;
        private Health _health;
        private Mover _mover;
        private ActionScheduler _actionScheduler;
        private GameObject _player;
        
        private LazyValue<Vector3> _guardPosition;
        private int _currentWaypointIndex;
        private float _timeSinceLastSawPlayer = Mathf.Infinity;
        private float _timeSinceArrivedAtWaypoint = Mathf.Infinity;

        private void Awake()
        {
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _player = GameObject.FindWithTag("Player");

            _guardPosition = new LazyValue<Vector3>(GetInitialGuardPosition);
        }

        private void Start()
        {
            _guardPosition.ForceInit();
        }

        private Vector3 GetInitialGuardPosition()
        {
            return transform.position;
        }

        private void Update()
        {
            if (_health.IsDead) return;
            
            if (IsInAttackRangeWith(_player) && _fighter.CanAttack(_player))
            {
                AttackBehaviour();
            }
            else if (_timeSinceLastSawPlayer <= suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        private bool IsInAttackRangeWith(GameObject target)
        {
            return Vector3.Distance(transform.position, target.transform.position) <= chaseDistance;
        }

        private void AttackBehaviour()
        {
            _fighter.Attack(_player);
            _timeSinceLastSawPlayer = 0f;
        }

        private void SuspicionBehaviour()
        {
            _actionScheduler.CancelCurrentAction();
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = _guardPosition.Value;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    _timeSinceArrivedAtWaypoint = 0f;
                    ReloadWaypoint();
                }

                nextPosition = GetCurrentWaypointPosition();
            }

            if (_timeSinceArrivedAtWaypoint > dwellingTime)
            {
                _mover.StartMoveAction(nextPosition);
            }
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypointPosition());
            return distanceToWaypoint <= waypointTolerance;
        }

        private void ReloadWaypoint()
        {
            _currentWaypointIndex = patrolPath.GetNextIndex(_currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypointPosition()
        {
            return patrolPath.GetWaypointPosition(_currentWaypointIndex);
        }

        private void UpdateTimers()
        {
            _timeSinceLastSawPlayer += Time.deltaTime;
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
