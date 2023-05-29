using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalsSceneManager : MonoBehaviour
{
    public void MoveToLevel(int buildIndex)
    {
        if (buildIndex < 2 || UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - 1 < buildIndex)
            return;

        CustomSceneManager.ChangeScene(buildIndex);
    }
}
