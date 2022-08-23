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
            health = GetComponent<BaseStats>().GetStat(Stats.Stats.Health);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            health = Mathf.Max(health - damage, 0);
            if (health <= 0f)
            {
                Die();

                AwardExperience(instigator);
            }
        }

        public float GetPercentage()
        {
            var maxHealth = GetComponent<BaseStats>().GetStat(Stats.Stats.Health);
            return 100 * (health / maxHealth);
        }

        private void Die()
        {
            if (IsDead) return;
            
            GetComponent<Animator>().SetTrigger(DieAnimationName);
            IsDead = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            var experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stats.Stats.ExperienceReward));
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health = (float) state;
            if (health <= 0f)
            {
                Die();
            }
        }
    }
}
