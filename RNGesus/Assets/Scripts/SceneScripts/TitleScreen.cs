using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public string newGameScene;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {
        List<string> playerStrings = new List<string>();
        List<string> enemyStrings = new List<string>();
        playerStrings.Add("Assets/Prefabs/Characters/Jesus.prefab");
        playerStrings.Add("Assets/Prefabs/Characters/Mary.prefab");

        enemyStrings.Add("Assets/Prefabs/Enemies/BusinessGoon1.prefab");
        enemyStrings.Add("Assets/Prefabs/Enemies/BusinessGoon2.prefab");

        CharacterManager.Load(newGameScene, playerStrings, enemyStrings);

        //SceneManager.LoadScene(newGameScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
