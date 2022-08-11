using Combat;
using Core;
using Movement;
using UnityEngine;

namespace Control
{
    [RequireComponent(
        typeof(Mover),
        typeof(Fighter),
        typeof(Health))]
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;

        private Fighter _fighter;
        private Health _health;
        private GameObject _player;

        private void Start()
        {
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _player = GameObject.FindWithTag("Player");
        }

        void Update()
        {
            if (_health.IsDead) return;
            
            if (IsInAttackRange(_player) && _fighter.CanAttack(_player))
            {
                _fighter.Attack(_player);
            }
            else
            {
                _fighter.CancelAction();
            }
        }

        private bool IsInAttackRange(GameObject target)
        {
            return Vector3.Distance(transform.position, target.transform.position) <= chaseDistance;
        }
    }
}
