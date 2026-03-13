using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager_79 : MonoBehaviour
{
    public void PlayEndlessGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("EndlessMode");
    }
    public void PlayTimerGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("TimerMode");
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}