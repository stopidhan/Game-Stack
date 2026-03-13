using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit079 : MonoBehaviour
{
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
            Destroy(gameObject);
            audioManager.PlaySFX(audioManager.point);
            if (gameManagerEndless != null)
            {
                gameManagerEndless.AddScore(1);
            }
            if (gameManagerTimer != null)
            {
                gameManagerTimer.AddScore(1);
            }
        }
        else if (collision.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }
}