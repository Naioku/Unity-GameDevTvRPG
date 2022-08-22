using UnityEngine;

namespace Attributes
{
    public class Experience : MonoBehaviour
    {
        [SerializeField] private float experience;

        public void GainExperience(float experiencePoints)
        {
            experience += experiencePoints;
        }
    }
}
