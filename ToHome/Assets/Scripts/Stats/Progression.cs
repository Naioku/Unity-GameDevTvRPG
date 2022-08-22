using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] characterClasses;

        public float GetHealth(CharacterClass characterClass, int level)
        {
            return (
                    from progressionCharacterClass in characterClasses 
                    where progressionCharacterClass.CharacterClass == characterClass 
                    select progressionCharacterClass.GetHealth(level))
               .FirstOrDefault();
        }

        [Serializable]
        private class ProgressionCharacterClass
        {
            [SerializeField] private CharacterClass characterClass;
            [SerializeField] private ProgressionStat[] progressionStats;

            public CharacterClass CharacterClass => characterClass;

            public float GetHealth(int level)
            {
                return (
                        from progressionStat in progressionStats
                        where progressionStat.Stat == Stats.Health
                        select progressionStat.GetLevel(level)
                    ).FirstOrDefault();
            }
        }
        
        [Serializable]
        private class ProgressionStat
        {
            [SerializeField] private Stats stat;
            [SerializeField] private float[] levels;

            public Stats Stat => stat;
            
            public float GetLevel(int level)
            {
                return levels[level - 1];
            }
        }
    }
}