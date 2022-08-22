using Attributes;
using Core;
using UnityEngine;

namespace Combat
{
    public class Projectile : MonoBehaviour
    { 
        [SerializeField] private float speed = 1f;
        [SerializeField] private bool isHoming;
        [SerializeField] private GameObject hitEffect;
        [SerializeField] private float maxLiveTime = 10f;
        [SerializeField] private GameObject[] destroyOnHit;
        [SerializeField] private float lifeAfterImpact = 2f;
        
        private Health _target;
        private float _damage;

        void Update()
        {
            if (_target == null) return;

            if (isHoming && !_target.IsDead)
            {
                transform.LookAt(GetAimLocation());
            }
            
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage)
        {
            _target = target;
            _damage += damage;
            transform.LookAt(GetAimLocation());
            
            Destroy(gameObject, maxLiveTime);
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
            if (_target.IsDead) return;

            speed = 0f;
            
            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), Quaternion.identity);
            }
            _target.TakeDamage(_damage);

            foreach (var obj in destroyOnHit)
            {
                Destroy(obj);
            }
            
            Destroy(gameObject, lifeAfterImpact);
        }
    }
}
