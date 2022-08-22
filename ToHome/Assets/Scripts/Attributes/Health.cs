using Core;
using Saving;
using Stats;
using UnityEngine;

namespace Attributes
{
    public class Health : MonoBehaviour, ISavable
    {
        [SerializeField] private float health = 100f;

        private static readonly int DieAnimationName = Animator.StringToHash("die");
        
        public bool IsDead { get; private set; }

        private void Start()
        {
            health = GetComponent<BaseStats>().GetHealth();
        }

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            RefreshDeathState();
        }

        private void RefreshDeathState()
        {
            if (health <= 0f)
            {
                Die();
            }
        }

        private void Die()
        {
            if (IsDead) return;
            
            GetComponent<Animator>().SetTrigger(DieAnimationName);
            IsDead = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health = (float) state;
            RefreshDeathState();
            
        }
    }
}
