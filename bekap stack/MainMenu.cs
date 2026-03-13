using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void Button070()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void Button072()
    {
        SceneManager.LoadSceneAsync(2);
    }
    public void Button077()
    {
        SceneManager.LoadSceneAsync(3);
    }
    public void Button079()
    {
        SceneManager.LoadSceneAsync(4);
    }
    public void Button085()
    {
        SceneManager.LoadSceneAsync(5);
    }
    public void QuitButton()
    {
        Application.Quit();
    }
}
