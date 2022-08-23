using Saving;
using UnityEngine;

namespace Attributes
{
    public class Experience : MonoBehaviour, ISavable
    {
        [SerializeField] private float experience;

        public void GainExperience(float experiencePoints)
        {
            experience += experiencePoints;
        }

        public object CaptureState()
        {
            return experience;
        }

        public void RestoreState(object state)
        {
            experience = (float) state;
        }

        public float GetExperiencePoints()
        {
            return experience;
        }
    }
}
