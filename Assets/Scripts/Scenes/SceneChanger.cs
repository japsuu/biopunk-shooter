using UI;
using UnityEngine;

namespace Scenes
{
    public static class SceneChanger
    {
        private const int MAIN_MENU_SCENE = 0;
        private const int GAMEPLAY_SCENE = 1;


        public static void GoToMainMenuScene()
        {
            Object.FindObjectOfType<ScreenFader>().EndScene(MAIN_MENU_SCENE);
        }


        public static void GoToGameplayScene()
        {
            Object.FindObjectOfType<ScreenFader>().EndScene(GAMEPLAY_SCENE);
        }
    }
}