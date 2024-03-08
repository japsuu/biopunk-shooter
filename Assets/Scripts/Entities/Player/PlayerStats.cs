using System;
using Singletons;

namespace Entities.Player
{
    public class PlayerStats : SingletonBehaviour<PlayerStats>
    {
        public static event Action<float> OnExperienceChanged;  //TODO: UI should listen to this event.
        public static event Action<int> OnLevelChanged;
        
        private const float EXPERIENCE_TO_LEVEL_FACTOR = 1.2f;
        private const float FIRST_LEVEL_EXPERIENCE = 100f;
        
        private float _experience;
        private int _level;
        
        public float Experience
        {
            get => _experience;
            private set
            {
                _experience = value;
                OnExperienceChanged?.Invoke(_experience);
            }
        }

        public float LevelFactor => (float)Math.Pow(EXPERIENCE_TO_LEVEL_FACTOR, Level - 1);
        public float ExperienceToNextLevel => FIRST_LEVEL_EXPERIENCE * LevelFactor;
        
        public int Level
        {
            get => _level;
            private set
            {
                _level = value;
                OnLevelChanged?.Invoke(_level);
            }
        }
        
        
        public void AddExperience(float amount)
        {
            Experience += amount;
            while (Experience >= FIRST_LEVEL_EXPERIENCE * LevelFactor)
            {
                Experience -= FIRST_LEVEL_EXPERIENCE * LevelFactor;
                Level++;
            }
        }
        
        
        public void ResetStats()
        {
            Experience = 0;
            Level = 1;
        }
        
        
        private void Awake()
        {
            Experience = 0;
            Level = 1;
        }
    }
}