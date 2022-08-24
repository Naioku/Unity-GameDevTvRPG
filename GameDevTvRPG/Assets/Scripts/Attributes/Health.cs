using Core;
using Saving;
using Stats;
using UnityEngine;

namespace Attributes
{
    public class Health : MonoBehaviour, ISavable
    {
        private float _healthPoints = -1f;

        private static readonly int DieAnimationName = Animator.StringToHash("die");
        
        public bool IsDead { get; private set; }

        private void Start()
        {
            var baseStats = GetComponent<BaseStats>();
            if (_healthPoints < 0f)
            {
                _healthPoints = baseStats.GetStat(Stats.Stats.Health);
            }

            baseStats.OnLevelUp += RestoreHealth;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print($"{gameObject.name} took damage: {damage}.");
            
            _healthPoints = Mathf.Max(_healthPoints - damage, 0);
            if (_healthPoints <= 0f)
            {
                Die();

                AwardExperience(instigator);
            }
        }

        public float GetPercentage()
        {
            var maxHealth = GetComponent<BaseStats>().GetStat(Stats.Stats.Health);
            return 100 * (_healthPoints / maxHealth);
        }

        public float GetHealthPoints()
        {
            return _healthPoints;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stats.Stats.Health);
        }

        private void RestoreHealth(float oldMaxHealth)
        {
            var damageTaken = oldMaxHealth - _healthPoints;
            var baseStats = GetComponent<BaseStats>();
            var newMaxHealth = baseStats.GetStat(Stats.Stats.Health);

            _healthPoints = newMaxHealth - damageTaken;
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
            return _healthPoints;
        }

        public void RestoreState(object state)
        {
            _healthPoints = (float) state;
            if (_healthPoints <= 0f)
            {
                Die();
            }
        }
    }
}
