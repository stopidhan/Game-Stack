// Namespace yang dibutuhkan dari Unity
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Memastikan komponen GameUi terpasang pada GameObject yang sama
[RequireComponent(typeof(GameUi))]
public class GameController : MonoBehaviour
{
    // Implementasi Singleton untuk akses global
    public static GameController Instance { get; private set; }

    // Jumlah pisau yang dapat dilempar pemain
    private int knifeCount;

    [Header("Knife Swapping")]
    // Posisi tempat pisau baru akan muncul
    [SerializeField]
    private Vector2 knifeSpawnPosition;

    // Tambahkan reference ke Log/Target
    [SerializeField]
    private Transform logTransform;

    // Referensi ke pengontrol UI
    public GameUi GameUi { get; private set; }

    // Variabel untuk level saat ini
    [SerializeField]
    private int currentLevel = 1;

    // Jumlah pisau awal di level 1
    [SerializeField]
    private int initialKnifeCount;

    // Pisau tambahan per level
    [SerializeField]
    private int knifeIncrementPerLevel = 2;

    [Header("Knife Variants")]
    [SerializeField]
    private GameObject[] knifePrefabs; // Array untuk menyimpan berbagai jenis pisau

    [Header("Sound Effects")]
    [SerializeField]
    private AudioClip levelUpSound;
    [SerializeField]
    private AudioClip winSound;    // Tambahkan ini
    [SerializeField]
    private AudioClip loseSound;   // Tambahkan ini
    private AudioSource audioSource;

    // Inisialisasi pola Singleton
    private void Awake()
    {
        // Memastikan hanya ada satu instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        GameUi = GetComponent<GameUi>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    // Pengaturan kondisi awal permainan
    private void Start()
    {
        knifeCount = initialKnifeCount + (knifeIncrementPerLevel * (currentLevel - 1));
        
        // Update level dan UI sekaligus
        GameUi.UpdateLevelAndKnives(currentLevel, knifeCount);

        if (knifeCount > 0)
        {
            SpawnKnife();
        }
    }

    // Dipanggil ketika pisau berhasil mengenai target
    public void OnSuccessfulKnifeHit()
    {
        // Jika masih ada pisau, spawn pisau berikutnya
        if (knifeCount > 0)
        {
            SpawnKnife();
        }
        // Jika tidak ada pisau tersisa, periksa apakah masih ada level berikutnya
        else
        {
            if (currentLevel < 3) 
            {
                StartNextLevelSequence();
            }
            else
            {
                StartGameOverSequence(true); // Menang total jika semua level selesai
            }
        }
    }

    // Membuat pisau baru di posisi spawn
    private void SpawnKnife()
    {
        knifeCount--;
        // Pilih pisau berdasarkan level saat ini
        int knifeIndex = (currentLevel - 1) % knifePrefabs.Length;
        Instantiate(knifePrefabs[knifeIndex], knifeSpawnPosition, Quaternion.identity);
    }

    // Memulai urutan game over
    public void StartGameOverSequence(bool win)
    {
        StartCoroutine(GameOverSequenceCoroutine(win));
    }

    // Menangani apa yang terjadi saat permainan berakhir
    private IEnumerator GameOverSequenceCoroutine(bool win)
    {
        if (win)
        {
            if (winSound != null)
            {
                audioSource.PlayOneShot(winSound);
            }
            yield return new WaitForSeconds(0.5f);
            GameUi.ShowWinGameUI();
        }
        else
        {
            if (loseSound != null)
            {
                audioSource.PlayOneShot(loseSound);
            }
            GameUi.ShowRestartButton();
        }
    }

    // Memuat ulang scene untuk memulai permainan kembali
    public void RestartGame()
    {
        Time.timeScale = 1f; // Reset timeScale ke normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Logika untuk memulai level berikutnya
    private void StartNextLevelSequence()
    {
        StartCoroutine(NextLevelTransitionRoutine());
    }

    private IEnumerator NextLevelTransitionRoutine()
    {
        currentLevel++;
        knifeCount = initialKnifeCount + (knifeIncrementPerLevel * (currentLevel - 1));
        
        // Play level up sound
        if (levelUpSound != null)
        {
            audioSource.PlayOneShot(levelUpSound);
        }

        // Hapus pisau yang tertancap
        ClearStuckKnives();

        // Tampilkan transisi level
        yield return StartCoroutine(GameUi.ShowLevelTransition(currentLevel));
        
        // Update UI setelah transisi
        GameUi.UpdateLevelAndKnives(currentLevel, knifeCount);

        // Spawn pisau baru
        SpawnKnife();
    }

    private void ClearStuckKnives()
    {
        // Hapus semua pisau yang tertancap di log
        foreach (Transform child in logTransform)
        {
            if (child.GetComponent<KnifeScript>() != null)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
