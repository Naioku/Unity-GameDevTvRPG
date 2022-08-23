using System;
using UnityEngine;

namespace Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] progressionClasses;

        public float GetStat(Stats stat, CharacterClass characterClass, int level)
        {
            foreach (var progressionClass in progressionClasses)
            {
                if (progressionClass.CharacterClass != characterClass) continue;

                foreach (var progressionStat in progressionClass.ProgressionStats)
                {
                    if (progressionStat.Stat          != stat) continue;
                    if (progressionStat.Levels.Length < level) continue;
                    
                    return progressionStat.Levels[level - 1];
                }
            }
            return 0;
        }

        [Serializable]
        private class ProgressionCharacterClass
        {
            [SerializeField] private CharacterClass characterClass;
            [SerializeField] private ProgressionStat[] progressionStats;

            public CharacterClass CharacterClass => characterClass;
            public ProgressionStat[] ProgressionStats => progressionStats;
        }
        
        [Serializable]
        private class ProgressionStat
        {
            [SerializeField] private Stats stat;
            [SerializeField] private float[] levels;

            public Stats Stat => stat;
            public float[] Levels => levels;
        }
    }
}