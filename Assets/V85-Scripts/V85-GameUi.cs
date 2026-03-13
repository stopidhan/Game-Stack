using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUi : MonoBehaviour
{
    [SerializeField]
    private GameObject restartButton;

    [Header("Knife Count Display")]
    [SerializeField]
    private GameObject panelKnives;
    [SerializeField]
    private GameObject[] knifePrefabs; // Array of different knife prefabs for each level
    [SerializeField]
    private Color useKnifeIconColor;

    [SerializeField]
    private TextMeshProUGUI levelText;

    private int knifeIconIndexToChange = 0;
    private int currentLevel = 1;

    [Header("Level Transition")]
    [SerializeField]
    private GameObject levelTransitionImage; // Reference ke gambar transisi
    [SerializeField]
    private float transitionDuration = 2f; // Durasi tampilan transisi

    [Header("Win Game UI")]
    [SerializeField]
    private GameObject winGameImage;    // Reference ke gambar kemenangan
    [SerializeField]
    private GameObject winRestartButton; // Reference ke tombol restart khusus menang

    public void ShowRestartButton()
    {
        restartButton.SetActive(true);
    }

    public void UpdateLevelAndKnives(int level, int knifeCount)
    {
        // Update level terlebih dahulu
        currentLevel = level;
        Debug.Log($"[UpdateLevelAndKnives] Setting to level {level} with {knifeCount} knives");
        
        // Update text level
        if (levelText != null)
        {
            levelText.text = "Level " + level.ToString();
        }

        // Hapus icon pisau lama
        ClearKnifeIcons();

        // Buat icon pisau baru dengan prefab yang sesuai level
        GameObject currentKnifePrefab = GetKnifePrefabForLevel();
        if (currentKnifePrefab != null)
        {
            for (int i = 0; i < knifeCount; i++)
            {
                GameObject newKnife = Instantiate(currentKnifePrefab, panelKnives.transform);
                Debug.Log($"Created knife icon {i+1} for level {currentLevel} using {currentKnifePrefab.name}");
            }
        }
    }

    private GameObject GetKnifePrefabForLevel()
    {
        if (knifePrefabs == null || knifePrefabs.Length == 0)
        {
            Debug.LogError("No knife prefabs assigned!");
            return null;
        }

        // Ubah currentLevel sebelum menggunakan
        int prefabIndex = (currentLevel - 1) % knifePrefabs.Length;
        return knifePrefabs[prefabIndex];
    }

    public void DecrementDisplayedKnifeCount()
    {
        if (knifeIconIndexToChange < panelKnives.transform.childCount)
        {
            panelKnives.transform.GetChild(knifeIconIndexToChange++)
                .GetComponent<Image>().color = useKnifeIconColor;
        }
        else
        {
            Debug.LogWarning("No more knives to decrement.");
        }
    }

    public void ClearKnifeIcons()
    {
        foreach (Transform child in panelKnives.transform)
        {
            Destroy(child.gameObject);
        }
        knifeIconIndexToChange = 0;
    }

    public IEnumerator ShowLevelTransition(int newLevel)
    {
        if (levelTransitionImage != null)
        {
            // Tampilkan gambar transisi
            levelTransitionImage.SetActive(true);

            // Tunggu sesuai durasi yang ditentukan
            yield return new WaitForSeconds(transitionDuration);

            // Sembunyikan gambar transisi
            levelTransitionImage.SetActive(false);
        }
    }

    // Method untuk menampilkan UI kemenangan
    public void ShowWinGameUI()
    {
        if (winGameImage != null)
        {
            winGameImage.SetActive(true);
        }
        if (winRestartButton != null)
        {
            winRestartButton.SetActive(true);
        }
        if (levelText != null)
        {
            levelText.gameObject.SetActive(false);
        }
    }
}
