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
        for (int i = 1; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = false;
        }
    }

    public void SelectLevel(LevelStateMachine LSM)
    {
        List<string> playerStrings = new List<string>();
        List<string> enemyStrings = new List<string>();
        playerStrings.Add("Assets/Prefabs/Characters/Jesus.prefab");
        playerStrings.Add("Assets/Prefabs/Characters/Mary.prefab");


        if (LSM.enemy1 != ""){
            enemyStrings.Add(LSM.enemy1);
        }
        if (LSM.enemy2 != ""){
            enemyStrings.Add(LSM.enemy2);
        }
        if (LSM.enemy3 != ""){
            enemyStrings.Add(LSM.enemy3);
        }
        
        if (LSM.enemy4 != ""){
            enemyStrings.Add(LSM.enemy4);
        }
        /*
        foreach (string e in enemies)
        {
            playerStrings.Add(e);
        }
        */
        //enemyStrings.Add("Assets/Prefabs/Enemies/Pontius Pilates.prefab");
        //enemyStrings.Add("Assets/Prefabs/Enemies/BusinessGoon1.prefab");
        //enemyStrings.Add("Assets/Prefabs/Enemies/BusinessGoon2.prefab");

        CharacterManager.Load(newGameScene, playerStrings, enemyStrings);
        //SceneManager.LoadScene(levelName);
    }
}
