using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;

        // public delegate void ExperienceGainedDelegele();
        // public event ExperienceGainedDelegele onExpereinceGained;
        public event Action onExpereinceGained;

        public void GainExprerience(float experience)
        {
            experiencePoints += experience;
            onExpereinceGained();
        }

        public float GetPoints(){
            return experiencePoints;
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}