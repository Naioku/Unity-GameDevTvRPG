using System;
using System.Linq;
using UnityEngine;

namespace Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(0, 99)]
        [SerializeField] private int startingLevel;
        [SerializeField] private CharacterClass characterClass = CharacterClass.Grunt;
        [SerializeField] private Progression progression;
        [SerializeField] private GameObject levelUpParticleEffect;
        [SerializeField] private bool shouldUseModifiers;

        public event Action<float> OnLevelUp;

        private int _currentLevel;
        private Experience _experience;

        private void Awake()
        {
            _experience = GetComponent<Experience>();
        }

        private void Start()
        {
            _currentLevel = CalculateLevel();
        }

        private void OnEnable()
        {
            if (_experience != null)
            {
                _experience.OnExperienceGained += UpdateLevel;
            }
        }
        
        private void OnDisable()
        {
            if (_experience != null)
            {
                _experience.OnExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > _currentLevel)
            {
                int oldLevel = _currentLevel;
                _currentLevel = newLevel;
                PlayLevelUpEffect();
                OnLevelUp?.Invoke(progression.GetStat(Stats.Health, characterClass, oldLevel));
            }
        }

        private void PlayLevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform);
        }

        public float GetStat(Stats stat) => shouldUseModifiers ?
            (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat)/100) :
            GetBaseStat(stat);

        private float GetBaseStat(Stats stat)
        {
            return progression.GetStat(stat, characterClass, CalculateLevel());
        }

        public int GetLevel()
        {
            return _currentLevel;
        }

        private int CalculateLevel()
        {
            var experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;
            
            float currentXp = experience.GetPoints();
            int maxLevel = progression.GetLevels(Stats.ExperienceToLevelUp, characterClass);
            for (int level = 0; level <= maxLevel - 1; level++)
            {
                float xpToLevelUp = progression.GetStat(Stats.ExperienceToLevelUp, characterClass, level);
                if (xpToLevelUp > currentXp) return level;
            }
            
            return maxLevel;
        }

        private float GetAdditiveModifier(Stats stat)
        {
            IModifierProvider[] modifierProviders = GetComponents<IModifierProvider>();

            return modifierProviders.SelectMany(modifierProvider => modifierProvider.GetAdditiveModifiers(stat))
                                    .Sum();
        }

        private float GetPercentageModifier(Stats stat)
        {
            IModifierProvider[] modifierProviders = GetComponents<IModifierProvider>();

            return modifierProviders.SelectMany(modifierProvider => modifierProvider.GetPercentageModifiers(stat))
                                    .Sum();
        }
    }
}
