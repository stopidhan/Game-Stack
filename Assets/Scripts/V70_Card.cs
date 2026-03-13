// filepath: Assets/Scripts/V70_Card.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class V70_Card : MonoBehaviour, IPointerClickHandler
{
    public Image frontImage;
    public Image backImage;
    public AudioClip flipSound;
    private AudioSource audioSource;
    private bool isFlipped = false;
    private GameManager070 gameManager;

    public bool IsMatched { get; set; } = false;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager070>();
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsMatched || !gameManager.CanFlipCard())
            return;

        if (!isFlipped)
        {
            ShowFront();
            gameManager.CardFlipped(this);
        }
    }

    public void ShowFront()
    {
        frontImage.gameObject.SetActive(true);
        backImage.gameObject.SetActive(false);
        isFlipped = true;
        PlayFlipSound();
    }

    public void HideFront()
    {
        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(true);
        isFlipped = false;
        PlayFlipSound();
    }

    public void SetMatched()
    {
        IsMatched = true; // Menggunakan property IsMatched alih-alih field isMatched
    }

    public bool IsFlipped()
    {
        return isFlipped;
    }

    private void PlayFlipSound()
    {
        if (flipSound != null)
        {
            audioSource.PlayOneShot(flipSound);
        }
    }
}