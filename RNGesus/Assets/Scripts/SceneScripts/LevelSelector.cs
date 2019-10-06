using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public Button[] levelButtons;

    void Start()
    {
        /*for (int i = 1; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = false;
        }*/
    }

    public void Select(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
