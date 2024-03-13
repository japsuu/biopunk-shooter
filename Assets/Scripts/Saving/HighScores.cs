﻿using UnityEngine;

namespace Saving
{
    public static class HighScores
    {
        private const string HIGHSCORE_KEY = "HighScore";
        
        
        public static void SaveHighScore(int score)
        {
            if (score > GetHighScore())
                PlayerPrefs.SetInt(HIGHSCORE_KEY, score);
        }
        
        
        public static int GetHighScore()
        {
            return PlayerPrefs.GetInt(HIGHSCORE_KEY, 0);
        }


        public static void ResetHighScore()
        {
            PlayerPrefs.SetInt(HIGHSCORE_KEY, 0);
        }
        
        
        public static bool IsNewHighScore(int score)
        {
            return score > GetHighScore();
        }
    }
}