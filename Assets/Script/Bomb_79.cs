using UnityEngine;

public class Bomb079 : MonoBehaviour
{
    public enum BombType
    {
        Normal,     // -1 life
        Heavy,      // -3 lives
        Deadly      // instant death
    }
    public BombType bombType;
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
        if (collision.tag == "Basket")
        {
            if (gameManagerEndless != null)
            {
                switch (bombType)
                {
                    case BombType.Normal:
                        gameManagerEndless.LoseLife(1);
                        break;
                    case BombType.Heavy:
                        gameManagerEndless.LoseLife(3);
                        break;
                    case BombType.Deadly:
                        gameManagerEndless.InstantDeath();
                        break;
                }
            }

            if (gameManagerTimer != null)
            {
                gameManagerTimer.InstantDeath();
            }

            audioManager.PlaySFX(audioManager.damaged);
            Destroy(gameObject);
        }
        else if (collision.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }
}