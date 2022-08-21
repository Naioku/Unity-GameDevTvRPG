using UnityEngine;

namespace Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] private int baseLevel = 1;
        [SerializeField] private CharacterClass characterClass = CharacterClass.Grunt;
    }
}
