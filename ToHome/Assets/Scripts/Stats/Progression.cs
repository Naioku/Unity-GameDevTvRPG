using System;
using UnityEngine;

namespace Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] characterClasses;

        [Serializable]
        private class ProgressionCharacterClass
        {
            [SerializeField] private CharacterClass characterClass;
            [SerializeField] private float[] health;
        }
    }
}