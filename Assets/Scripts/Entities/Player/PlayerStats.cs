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
        
        private float _levelExperience;
        private int _level;
        
        public float LevelExperience
        {
            get => _levelExperience;
            private set
            {
                _levelExperience = value;
                OnExperienceChanged?.Invoke(_levelExperience);
            }
        }

        public float LevelFactor => (float)Math.Pow(EXPERIENCE_TO_LEVEL_FACTOR, Level - 1);
        public float ExperienceToNextLevel => FIRST_LEVEL_EXPERIENCE * LevelFactor;
        public float TotalExperience { get; private set; }
        
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
            TotalExperience += amount;
            LevelExperience += amount;
            while (LevelExperience >= FIRST_LEVEL_EXPERIENCE * LevelFactor)
            {
                LevelExperience -= FIRST_LEVEL_EXPERIENCE * LevelFactor;
                Level++;
            }
        }
        
        
        public void ResetStats()
        {
            LevelExperience = 0;
            Level = 1;
        }
        
        
        private void Awake()
        {
            LevelExperience = 0;
            Level = 1;
        }
    }
}