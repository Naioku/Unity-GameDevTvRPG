using System;
using UnityEngine;

namespace Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(0, 99)]
        [SerializeField] private int startingLevel;
        [SerializeField] private CharacterClass characterClass = CharacterClass.Grunt;
        [SerializeField] private Progression progression;

        private int _currentLevel;

        private void Start()
        {
            _currentLevel = CalculateLevel();
            var experience = GetComponent<Experience>();
            if (experience != null)
            {
                experience.OnExperienceGained += UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > _currentLevel)
            {
                _currentLevel = newLevel;
                print("Levelled up!");
            }
        }

        public float GetStat(Stats stat) => progression.GetStat(stat, characterClass, CalculateLevel());

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
    }
}
