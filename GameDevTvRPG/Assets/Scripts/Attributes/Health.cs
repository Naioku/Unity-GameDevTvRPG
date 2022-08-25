using System;
using Core;
using GameDevTV.Utils;
using Saving;
using Stats;
using UnityEngine;

namespace Attributes
{
    public class Health : MonoBehaviour, ISavable
    {
        private BaseStats _baseStats;
        private LazyValue<float> _healthPoints;

        private static readonly int DieAnimationName = Animator.StringToHash("die");

        public bool IsDead { get; private set; }

        private void Awake()
        {
            _healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stats.Stats.Health);
        }

        private void Start()
        {
            _healthPoints.ForceInit();
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().OnLevelUp += RestoreHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().OnLevelUp -= RestoreHealth;

        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            _healthPoints.Value = Mathf.Max(_healthPoints.Value - damage, 0);
            if (_healthPoints.Value <= 0f)
            {
                Die();

                AwardExperience(instigator);
            }
        }

        public float GetPercentage()
        {
            var maxHealth = GetComponent<BaseStats>().GetStat(Stats.Stats.Health);
            return 100 * (_healthPoints.Value / maxHealth);
        }

        public float GetHealthPoints()
        {
            return _healthPoints.Value;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stats.Stats.Health);
        }

        private void RestoreHealth(float oldMaxHealth)
        {
            var damageTaken = oldMaxHealth - _healthPoints.Value;
            var baseStats = GetComponent<BaseStats>();
            var newMaxHealth = baseStats.GetStat(Stats.Stats.Health);

            _healthPoints.Value = newMaxHealth - damageTaken;
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
            _healthPoints.Value = (float) state;
            if (_healthPoints.Value <= 0f)
            {
                Die();
            }
        }
    }
}
