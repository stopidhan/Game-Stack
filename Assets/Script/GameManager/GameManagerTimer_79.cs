using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerTimer_79 : MonoBehaviour
{
    //score
    public Text scoreText;
    private int score = 0;

    //game over
    public Text gameOverText;
    public GameObject restartButton;
    public GameObject pauseButton;
    public GameObject mainMenuButton;
    public Image overlayPanel;

    //pause
    public Text countdownText;

    //timer
    public Text timerText;
    public float gameTime = 60f;
    private float currentTime;

    //getar saat terdamage
    public Camera mainCamera;
    private Vector3 originalCameraPosition;
    private float shakeDuration = 0.5f;
    private float shakeMagnitude = 0.05f;
    private Coroutine shakeCoroutine;

    //kecepatan basket
    [SerializeField] private Text speedBoostTimerText;
    private float currentSpeedBoostTime = 0f;
    private bool isSpeedBoosted = false;
    public GameObject basket;
    private Coroutine speedBoostCoroutine;
    private BasketController basketController;

    //suara
    AudioManager_79 audioManager;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager_79>();
    }

    void Start()
    {
        currentTime = gameTime;
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
        overlayPanel.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(false);
        UpdateTimerText();

        if (basket == null)
        {
            Debug.LogError("Basket reference is missing!");
            return;
        }

        basketController = basket.GetComponent<BasketController>();
        if (basketController == null)
        {
            Debug.LogError("BasketController component not found on basket object!");
            return;
        }

        originalCameraPosition = mainCamera.transform.position;
    }

    void Update()
    {
        if (Time.timeScale > 0)
        {
            UpdateTimer();
        }
    }

    private void UpdateTimer()
    {
        currentTime -= Time.deltaTime;
        UpdateTimerText();

        if (currentTime <= 0)
        {
            GameOver();
        }
    }

    private void UpdateTimerText()
    {
        timerText.text = "Time: " + Mathf.CeilToInt(currentTime).ToString();
    }

    public void AddTime(float seconds)
    {
        currentTime += seconds;
        UpdateTimerText();
        audioManager.PlaySFX(audioManager.powerUp);
    }

    public void AddScore(int value)
    {
        score += value;
        scoreText.text = "Score : " + score;
    }

    public void GameOver()
    {
        audioManager.StopBackgroundMusic();
        audioManager.PlaySFX(audioManager.death);
        StartCoroutine(GameOverWithDelay());
    }

    private IEnumerator GameOverWithDelay()
    {
        yield return new WaitForSeconds(0.3f); // Delay 1 detik

        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            mainCamera.transform.position = originalCameraPosition;
        }

        Time.timeScale = 0; // Hentikan game
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);
        overlayPanel.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Lanjutkan game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Muat ulang scene saat ini
    }

    public void PauseGame()
    {
        Time.timeScale = 0; // Hentikan game

        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            mainCamera.transform.position = originalCameraPosition;
        }
    }

    public void ResumeGame()
    {
        StartCoroutine(ResumeGameWithCountdown());
        audioManager.PlaySFX(audioManager.countdown);
    }

    public void toMainMenu()
    {
        SceneManager.LoadScene("MainMenu 079");
    }

    private IEnumerator ResumeGameWithCountdown()
    {
        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1);
        }

        countdownText.gameObject.SetActive(false);
        Time.timeScale = 1; // Lanjutkan game
    }
    private IEnumerator ScreenShake()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            if (Time.timeScale == 0)
            {
                yield break; // Stop shaking if the game is paused
            }

            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            mainCamera.transform.position = new Vector3(originalCameraPosition.x + x, originalCameraPosition.y + y, originalCameraPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        mainCamera.transform.position = originalCameraPosition;
    }

    public void InstantDeath()
    {
        currentTime = 0;
        UpdateTimerText();
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(ScreenShake());
        GameOver();
    }

    public void ActivateSpeedBoost(float duration, float multiplier)
    {
        if (isSpeedBoosted)
        {
            // Add duration to existing timer
            currentSpeedBoostTime += duration;
        }
        else
        {
            // Start new speed boost
            if (speedBoostCoroutine != null)
                StopCoroutine(speedBoostCoroutine);

            speedBoostCoroutine = StartCoroutine(SpeedBoostRoutine(duration, multiplier));
        }
    }

    private IEnumerator SpeedBoostRoutine(float duration, float multiplier)
    {
        isSpeedBoosted = true;
        basketController.moveSpeed *= multiplier;
        currentSpeedBoostTime = duration;

        if (speedBoostTimerText == null)
        {
            Debug.LogError("Speed Boost Timer Text is missing!");
            yield break;
        }

        speedBoostTimerText.text = "Speed Boost!";
        float blinkInterval = 0.5f; // Adjust blink speed here

        while (currentSpeedBoostTime > 0)
        {
            if (Time.timeScale > 0)
            {
                Debug.Log("Time.deltaTime: " + Time.deltaTime);
                Debug.Log("Time.timeScale: " + Time.timeScale);
                currentSpeedBoostTime -= blinkInterval;
                Debug.Log("Sisa waktu boost: " + currentSpeedBoostTime);
                speedBoostTimerText.gameObject.SetActive(!speedBoostTimerText.gameObject.activeSelf);
                yield return new WaitForSecondsRealtime(blinkInterval);
            }
            else
            {
                yield return null;
            }
        }

        isSpeedBoosted = false;
        basketController.ResetSpeed();
        speedBoostTimerText.gameObject.SetActive(false);
    }
}