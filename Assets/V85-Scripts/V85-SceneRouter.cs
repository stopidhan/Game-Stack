using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRouter : MonoBehaviour
{
    public static void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("V85-MainMenu");
    }

    public void LoadGameScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("V85-GameScene");
    }

    public void LoadSettingsScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("V85-SettingsScene");
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
