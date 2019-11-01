using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public string newGameScene;

    public Button[] levelButtons;

    void Start()
    {
        /*for (int i = 1; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = false;
        }*/
    }

    public void Select()
    {
        List<string> playerStrings = new List<string>();
        List<string> enemyStrings = new List<string>();
        playerStrings.Add("Assets/Prefabs/Characters/Jesus.prefab");
        playerStrings.Add("Assets/Prefabs/Characters/Mary.prefab");

        enemyStrings.Add("Assets/Prefabs/Enemies/Pontius Pilates.prefab");
        enemyStrings.Add("Assets/Prefabs/Enemies/BusinessGoon1.prefab");
        enemyStrings.Add("Assets/Prefabs/Enemies/BusinessGoon2.prefab");

        CharacterManager.Load(newGameScene, playerStrings, enemyStrings);
        //SceneManager.LoadScene(levelName);
    }
}
