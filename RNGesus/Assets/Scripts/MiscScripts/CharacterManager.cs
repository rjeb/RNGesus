using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CharacterManager {
 
    private static List<string> characterPaths;
    private static List<string> enemyPaths;
 
    public static void Load(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public static void Load(string sceneName,  List<string> characterPaths, List<string> enemyPaths) {
        CharacterManager.characterPaths = characterPaths;
        CharacterManager.enemyPaths = enemyPaths;
        SceneManager.LoadScene(sceneName);
    }
 
    public static List<string> getCharacters() {
        return characterPaths;
    }

    public static List<string> getEnemys() {
        return enemyPaths;
    }
 
    public static void setCharacters(List<string> input) {
        CharacterManager.characterPaths = input;
    }

    public static void setEnemies(List<string> input) {
        CharacterManager.enemyPaths = input;
    }
 
}
 