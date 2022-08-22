using UnityEngine;

namespace Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass = CharacterClass.Grunt;
        [SerializeField] private Progression progression;

        public float GetHealth() => progression.GetHealth(characterClass, startingLevel);
        public float GetExperienceReward() => 10f;
    }
}
