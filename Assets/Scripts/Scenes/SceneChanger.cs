using JSAM;
using UI;
using UnityEngine;

namespace Scenes
{
    public static class SceneChanger
    {
        private const int CINEMATIC_SCENE = 0;
        private const int MAIN_MENU_SCENE = 1;
        private const int GAMEPLAY_SCENE = 2;
        
        
        public static void GoToCinematicScene()
        {
            ChangeSceneFaded(CINEMATIC_SCENE);
        }


        public static void GoToMainMenuScene()
        {
            ChangeSceneFaded(MAIN_MENU_SCENE);
        }


        public static void GoToGameplayScene()
        {
            ChangeSceneFaded(GAMEPLAY_SCENE);
        }


        public static void ChangeSceneFaded(int sceneIndexToLoad)
        {
            AudioManager.StopAllMusic(false);
            Object.FindObjectOfType<ScreenFader>().EndScene(sceneIndexToLoad);
        }


        public static void ChangeSceneInstant(int sceneIndexToLoad)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndexToLoad);
        }
    }
}