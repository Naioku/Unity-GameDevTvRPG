using System;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] progressionClasses;

        private Dictionary<CharacterClass, Dictionary<Stats, float[]>> _lookupTable;

        public float GetStat(Stats stat, CharacterClass characterClass, int level)
        {
            BuildLookup();
            float[] levels = _lookupTable[characterClass][stat];
            
            return levels.Length < level ? 0f : levels[level - 1];
        }

        private void BuildLookup()
        {
            if (_lookupTable != null) return;
            
            _lookupTable = new Dictionary<CharacterClass, Dictionary<Stats, float[]>>();
            foreach (var progressionClass in progressionClasses)
            {
                var progressionClassDict = new Dictionary<Stats, float[]>();
                foreach (var progressionStat in progressionClass.ProgressionStats)
                {
                    progressionClassDict.Add(progressionStat.Stat, progressionStat.Levels);
                }
                
                _lookupTable.Add(progressionClass.CharacterClass, progressionClassDict);
            }
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