using System;
using Core;
using UnityEngine;

namespace Combat
{
    public class Projectile : MonoBehaviour
    { 
        [SerializeField] private float speed = 1f;
        
        private Health _target;
        private float _damage = 0f;

        void Update()
        {
            if (_target == null) return;
            
            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage)
        {
            _target = target;
            _damage += damage;
        }

        private Vector3 GetAimLocation()
        {
            var targetCollider = _target.GetComponent<CapsuleCollider>();
            if (targetCollider == null)
            {
                return _target.transform.position;
            }
            return _target.transform.position + Vector3.up * targetCollider.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != _target) return;
            
            _target.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
