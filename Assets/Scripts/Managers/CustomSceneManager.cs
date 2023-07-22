using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneID {MainMenu, LevelSelect, Level01 , Level02 , Level03 }

public static class CustomSceneManager
{
    public static SceneID currentSceneID;
    public static void ChangeScene(int num)
    {
        SceneManager.LoadScene(num);
        currentSceneID = (SceneID)num;
    }
    public static void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
        currentSceneID = SceneID.MainMenu;
    }
    public static void QuitGame()
    {
        Application.Quit();
    }
}
