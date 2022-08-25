using System;
using Saving;
using UnityEngine;

namespace Stats
{
    public class Experience : MonoBehaviour, ISavable
    {
        [SerializeField] private float experience;
        public event Action OnExperienceGained;

        public void GainExperience(float experiencePoints)
        {
            experience += experiencePoints;
            OnExperienceGained?.Invoke();
        }

        public object CaptureState()
        {
            return experience;
        }

        public void RestoreState(object state)
        {
            experience = (float) state;
        }

        public float GetPoints()
        {
            return experience;
        }
    }
}
