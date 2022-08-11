using System;
using Combat;
using Movement;
using UnityEngine;

namespace Control
{
    [RequireComponent(
        typeof(Mover),
        typeof(Fighter))]
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;

        private Fighter _fighter;
        private GameObject _player;

        private void Start()
        {
            _fighter = GetComponent<Fighter>();
            _player = GameObject.FindWithTag("Player");
        }

        void Update()
        {
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
