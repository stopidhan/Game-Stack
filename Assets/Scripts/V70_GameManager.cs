// filepath: /D:/Project Mobile/Unity/V70_MemoryMatch/Assets/Scripts/V70_GameManager.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// using System.Linq;

public class GameManager070 : MonoBehaviour
{
    public GameObject cardPrefab;
    public Sprite[] cardImages;
    private int numberOfPairs = 4; // Default number of pairs
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bonusText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI levelText;
    public GameObject pauseMenu;
    public GameObject endGameMenu;

    private V70_Card firstCard;
    private V70_Card secondCard;
    private bool canFlip = true;
    private int score = 0;
    private int totalScore = 0;
    private int consecutiveMatches = 0;
    private bool hintUsed = false;
    private int highScore = 0;
    private bool isPaused = false;
    private int level = 1;
    private const int maxLevel = 3;

    private void Start()
    {
        CreateCards();
        UpdateScoreText();
        UpdateLevelText();
        bonusText.text = "";
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreText();

        // hintButton.onClick.AddListener(UseHint);
        // pauseButton.onClick.AddListener(TogglePause);
    }

    public void CreateCards()
    {
        List<Sprite> selectedSprites = new List<Sprite>();
        for (int i = 0; i < numberOfPairs; i++)
        {
            selectedSprites.Add(cardImages[i]);
        }

        List<Sprite> cardSprites = new List<Sprite>(selectedSprites);
        cardSprites.AddRange(selectedSprites); // Duplicate the list to have pairs

        // Shuffle the list
        for (int i = 0; i < cardSprites.Count; i++)
        {
            Sprite temp = cardSprites[i];
            int randomIndex = Random.Range(i, cardSprites.Count);
            cardSprites[i] = cardSprites[randomIndex];
            cardSprites[randomIndex] = temp;
        }

        // Instantiate cards
        for (int i = 0; i < cardSprites.Count; i++)
        {
            GameObject card = Instantiate(cardPrefab, transform);
            V70_Card cardScript = card.GetComponent<V70_Card>();
            cardScript.frontImage.sprite = cardSprites[i];
        }
    }

    public bool CanFlipCard()
    {
        return canFlip && !isPaused;
    }

    public void CardFlipped(V70_Card card)
    {
        if (firstCard == null)
        {
            firstCard = card;
        }
        else if (secondCard == null)
        {
            secondCard = card;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        canFlip = false;

        yield return new WaitForSeconds(1f);

        if (firstCard.frontImage.sprite == secondCard.frontImage.sprite)
        {
            // Match found
            consecutiveMatches++;
            score += 50;
            if (consecutiveMatches > 1)
            {
                score += 25; // Bonus for consecutive matches
                StartCoroutine(ShowBonusText("+25 Bonus!"));
            }
            firstCard.IsMatched = true;
            secondCard.IsMatched = true;
            firstCard = null;
            secondCard = null;

            // Check if all pairs are matched
            if (AllPairsMatched())
            {
                if (level < maxLevel)
                {
                    NextLevel();
                }
                else
                {
                    EndGame();
                }
            }
        }
        else
        {
            // No match, flip back
            consecutiveMatches = 0;
            firstCard.HideFront();
            secondCard.HideFront();
            firstCard = null;
            secondCard = null;
        }

        UpdateScoreText();
        CheckHighScore();
        canFlip = true;
    }

    private bool AllPairsMatched()
    {
        V70_Card[] cards = FindObjectsOfType<V70_Card>();
        foreach (V70_Card card in cards)
        {
            if (!card.IsMatched)
            {
                return false;
            }
        }
        return true;
    }

    private void NextLevel()
    {
        totalScore += score; // Add current level score to total score
        score = 0; // Reset current level score
        level++;
        numberOfPairs += 2; // Increase the number of pairs for the next level
        UpdateLevelText();
        RestartGame();
    }

    private void EndGame()
    {
        totalScore += score; // Add final level score to total score
        finalScoreText.text = "Score: " + totalScore;
        endGameMenu.SetActive(true);
        Time.timeScale = 0;
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    private void UpdateLevelText()
    {
        levelText.text = "Level: " + level;
    }

    private void UpdateHighScoreText()
    {
        highScoreText.text = "High Score: " + highScore;
    }

    private void CheckHighScore()
    {
        if (totalScore > highScore)
        {
            highScore = totalScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            UpdateHighScoreText();
        }
    }

    private IEnumerator ShowBonusText(string text)
    {
        bonusText.text = text;
        bonusText.alpha = 1;

        yield return new WaitForSeconds(1f);

        bonusText.alpha = 0;
        bonusText.text = "";
    }

    public void UseHint()
    {
        if (!hintUsed)
        {
            hintUsed = true;
            StartCoroutine(ShowHint());
        }
    }

    private IEnumerator ShowHint()
    {
        V70_Card[] cards = FindObjectsOfType<V70_Card>();
        List<V70_Card> unmatchedCards = new List<V70_Card>();

        foreach (V70_Card card in cards)
        {
            if (!card.IsMatched && !card.IsFlipped())
            {
                unmatchedCards.Add(card);
            }
        }

        if (unmatchedCards.Count >= 2)
        {
            unmatchedCards[0].ShowFront();
            unmatchedCards[1].ShowFront();

            yield return new WaitForSeconds(1f);

            unmatchedCards[0].HideFront();
            unmatchedCards[1].HideFront();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
    }

    public void RestartGame()
    {
        // Reset game state
        score = 0;
        consecutiveMatches = 0;
        hintUsed = false;
        firstCard = null;
        secondCard = null;
        canFlip = true;
        isPaused = false;
        Time.timeScale = 1;

        // Destroy all existing cards
        V70_Card[] cards = FindObjectsOfType<V70_Card>();
        foreach (V70_Card card in cards)
        {
            Destroy(card.gameObject);
        }

        // Recreate cards
        CreateCards();
        UpdateScoreText();
        UpdateLevelText();
        pauseMenu.SetActive(false);
        endGameMenu.SetActive(false);
    }

    public void RestartGameFromBeginning()
    {
        // Reset game state
        score = 0;
        totalScore = 0; // Reset total score
        consecutiveMatches = 0;
        hintUsed = false;
        firstCard = null;
        secondCard = null;
        canFlip = true;
        isPaused = false;
        Time.timeScale = 1;

        // Reset level and number of pairs
        level = 1;
        numberOfPairs = 4;

        // Destroy all existing cards
        V70_Card[] cards = FindObjectsOfType<V70_Card>();
        foreach (V70_Card card in cards)
        {
            Destroy(card.gameObject);
        }

        // Recreate cards
        CreateCards();
        UpdateScoreText();
        UpdateLevelText();
        pauseMenu.SetActive(false);
        endGameMenu.SetActive(false);
    }
    // Exit
    public void ExitToMainMenu()
    {
        PlayerPrefs.DeleteAll();
        Time.timeScale = 1;
        SceneManager.LoadScene("V70_MainMenu");
    }
}