using UnityEngine;
using System.Collections.Generic;
using TMPro; 

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Level Settings")]
    public bool isFIFOLevel = true; 
    public float timeRemaining = 60f; // Set your level time here in seconds
    public bool isGameActive = true;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText; // Drag your Timer text object here
    public int score = 0;

    public List<GameObject> activeItems = new List<GameObject>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        // This runs the timer down every frame as long as the game is active
        if (isGameActive && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerUI();

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                EndGame();
            }
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            // Formats the math into standard Minutes:Seconds (e.g., 01:45)
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
        }
    }

    public void RegisterItem(GameObject newItem)
    {
        // Only allow new items if the game is still running
        if (isGameActive) activeItems.Add(newItem);
    }

    public bool ValidateItem(GameObject droppedItem)
    {
        // If time is up, prevent them from validating/scoring
        if (!isGameActive || activeItems.Count == 0) return false;

        bool isCorrect = false;

        if (isFIFOLevel)
        {
            if (activeItems[0] == droppedItem) isCorrect = true;
        }
        else
        {
            if (activeItems[activeItems.Count - 1] == droppedItem) isCorrect = true;
        }

        activeItems.Remove(droppedItem);
        return isCorrect;
    }

    public void AddScore(int amount)
    {
        if (!isGameActive) return;
        
        score += amount;
        if (scoreText != null) scoreText.text = "Score: " + score;
    }

    private void EndGame()
    {
        isGameActive = false;
        Debug.Log("Time is up! Final Score: " + score);
        // We can trigger a UI panel here later to show "Level Complete"
    }
}