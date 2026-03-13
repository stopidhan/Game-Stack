using UnityEngine;

public class PowerUp_79 : MonoBehaviour 
{
    public enum PowerUpType 
{
    SpeedBoost,
    ExtraLife,
    ExtraTime
}
    [SerializeField] private PowerUpType powerUpType;
    [SerializeField] private float speedBoostDuration = 5f;
    [SerializeField] private float speedMultiplier = 1.5f;
    
    private GameManagerEndless_79 gameManagerEndless;
    private GameManagerTimer_79 gameManagerTimer;
    AudioManager_79 audioManager;

    void Awake()
    {
       audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager_79>();
    }
    void Start() 
    {
        gameManagerEndless = FindObjectOfType<GameManagerEndless_79>();
        gameManagerTimer = FindObjectOfType<GameManagerTimer_79>();
    }

    void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.CompareTag("Basket")) 
        {
            if (gameManagerEndless != null)
            {
                switch (powerUpType) 
                {
                    case PowerUpType.SpeedBoost:
                        gameManagerEndless.ActivateSpeedBoost(speedBoostDuration, speedMultiplier);
                        break;
                    case PowerUpType.ExtraLife:
                        gameManagerEndless.AddLife();
                        break;
                }
            }
            if (gameManagerTimer != null)
            {
                switch (powerUpType) 
                {
                    case PowerUpType.SpeedBoost:
                        gameManagerTimer.ActivateSpeedBoost(speedBoostDuration, speedMultiplier);
                        break;
                    case PowerUpType.ExtraTime:
                        gameManagerTimer.AddTime(10);
                        break;
                }
            }
            audioManager.PlaySFX(audioManager.powerUp);
            Destroy(gameObject);
        }
    }
}