using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelSelectorManager
{

    private static int unlockedNodes;

    public static void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void Load(string sceneName, int input)
    {
        LevelSelectorManager.unlockedNodes = input;
        SceneManager.LoadScene(sceneName);
    }

    public static int getUnlockedNodes()
    {
        return unlockedNodes;
    }

    public static void setUnlockedNodes(int input)
    {
        unlockedNodes = input;
    }

    public static void resetNodes()
    {
        unlockedNodes = 1;
    }

    public static void incrementNodes()
    {
        unlockedNodes++;
    }
    
}
