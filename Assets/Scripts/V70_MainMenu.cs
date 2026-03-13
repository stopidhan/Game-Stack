// filepath: /D:/Project Mobile/Unity/V70_MemoryMatch/Assets/Scripts/V70_MainMenu.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class V70_MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("V70_GameScene");
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("MainMenu");

    }
}