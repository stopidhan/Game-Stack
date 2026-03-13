using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private KeyCode pauseKey = KeyCode.P;
    private static bool isInteractingWithUI = false;

    void Start()
    {
        if (pauseMenu == null)
        {
            Debug.LogError("Pause Menu is not assigned! Please assign it in the Unity Inspector.");
            return;
        }
        pauseMenu.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isInteractingWithUI = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StartCoroutine(ResetUIInteraction());
    }

    private IEnumerator ResetUIInteraction()
    {
        yield return new WaitForEndOfFrame();
        isInteractingWithUI = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (Time.timeScale == 0f)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        StartCoroutine(ResumeGameRoutine());
    }

    private IEnumerator ResumeGameRoutine()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        yield return new WaitForEndOfFrame();
    }

    public static bool IsInteractingWithUI()
    {
        return isInteractingWithUI || EventSystem.current.IsPointerOverGameObject();
    }
}
