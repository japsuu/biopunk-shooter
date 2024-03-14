using UnityEngine;

namespace Saving
{
    public static class Cinematics
    {
        private const string HAS_SEEN_CINEMATIC_KEY = "SeenCinematic";
        
        
        public static bool HasPlayerSeenCinematic()
        {
            return PlayerPrefs.GetInt(HAS_SEEN_CINEMATIC_KEY, 0) == 1;
        }
        
        
        public static void SetPlayerHasSeenCinematic(bool hasSeenCinematic)
        {
            PlayerPrefs.SetInt(HAS_SEEN_CINEMATIC_KEY, hasSeenCinematic ? 1 : 0);
        }
    }
}