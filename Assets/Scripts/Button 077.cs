using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerClickHandler
{
    public void mainMenuButton()
    {
        Debug.Log("Main Menu Button Clicked"); // Debug tambahan
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainMenu");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Pointer Clicked"); // Debug tambahan
        mainMenuButton();
    }


}
