using UnityEngine;

namespace Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(0, 99)]
        [SerializeField] private int startingLevel;
        [SerializeField] private CharacterClass characterClass = CharacterClass.Grunt;
        [SerializeField] private Progression progression;

        private void Update()
        {
            if (gameObject.tag.Equals("Player"))
            {
                print(GetLevel());
            }
        }

        public float GetStat(Stats stat) => progression.GetStat(stat, characterClass, GetLevel());

        public int GetLevel()
        {
            var experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;
            
            float currentXp = experience.GetPoints();
            int penultimateLevel = progression.GetLevels(Stats.ExperienceToLevelUp, characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float xpToLevelUp = progression.GetStat(Stats.ExperienceToLevelUp, characterClass, level);
                if (xpToLevelUp > currentXp) return level;
            }
            
            return penultimateLevel + 1;
        }
    }
}
