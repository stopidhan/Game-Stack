using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void Button070()
    {
        SceneManager.LoadSceneAsync("V70_MainMenu");
    }
    public void Button072()
    {
        SceneManager.LoadSceneAsync("Stack - 072");
    }
    public void Button077()
    {
        SceneManager.LoadSceneAsync("FruitNinja 077");
    }
    public void Button079()
    {
        SceneManager.LoadSceneAsync("MainMenu 079");
    }
    public void Button085()
    {
        SceneManager.LoadSceneAsync("V85-MainMenu");
    }
    public void QuitButton()
    {
        Application.Quit();
    }
}
