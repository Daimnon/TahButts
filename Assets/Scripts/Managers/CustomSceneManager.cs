using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CustomSceneManager
{
    public static void ChangeScene(int num)
    {
        SceneManager.LoadScene(num);
    }
    public static void QuitGame()
    {
        Application.Quit();
    }
}
