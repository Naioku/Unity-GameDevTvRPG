using System;
using System.Linq;
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
                    from progressionCharacterClass 
                    in characterClasses
                    where progressionCharacterClass.CharacterClass == characterClass 
                    select progressionCharacterClass.GetHealth(level)).FirstOrDefault();
        }

        [Serializable]
        private class ProgressionCharacterClass
        {
            [SerializeField] private CharacterClass characterClass;
            [SerializeField] private float[] health;

            public CharacterClass CharacterClass => characterClass;

            public float GetHealth(int level)
            {
                return health[level - 1];
            }
        }
    }
}