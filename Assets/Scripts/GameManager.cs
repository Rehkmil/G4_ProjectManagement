using UnityEngine;
using System.Collections.Generic;
using TMPro; 
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool isChallengeMode = false; 

    [Header("Level Settings")]
    public bool isFIFOLevel = true; 
    public float levelDuration = 60f; 
    private float timeRemaining;
    public bool isGameActive = true;

    [Header("Gameplay UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText; 
    public int score = 0;

    [Header("End Game UI")]
    public GameObject endPanel; 
    public TextMeshProUGUI finalScoreText; 
    public GameObject challengeButton; 

    public List<GameObject> activeItems = new List<GameObject>();
    public List<GameObject> correctlyGrabbedItems = new List<GameObject>();

    [Header("Prep Timer")]
    public float prepDuration = 3f; 
    private float prepTimer;
    public TextMeshProUGUI centerCountdownText;

    public float GetTimePercentage() 
    {
        return timeRemaining / levelDuration;
    }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        timeRemaining = levelDuration;
        UpdateTimerUI();

        isGameActive = false; 
        prepTimer = prepDuration;
        
        if (centerCountdownText != null) 
        {
            centerCountdownText.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        // 1. Run the Prep Timer first
        if (prepTimer > 0)
        {
            prepTimer -= Time.deltaTime;
            
            if (centerCountdownText != null)
            {
                centerCountdownText.text = Mathf.CeilToInt(prepTimer).ToString();
            }

            if (prepTimer <= 0)
            {
                isGameActive = true;
                if (centerCountdownText != null)
                {
                    centerCountdownText.text = "GO!";
                    Destroy(centerCountdownText.gameObject, 1f); 
                }
            }
            return; 
        }

        if (isGameActive && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerUI();

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                TriggerGameOver("Time's Up!");
            }
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
        }
    }

    public void RegisterItem(GameObject newItem)
    {
        if (isGameActive) activeItems.Add(newItem);
    }

    public void ProcessGrab(GameObject grabbedItem)
    {
        if (activeItems.Count == 0) return;

        bool isCorrect = false;

        if (isFIFOLevel)
        {
            if (activeItems[0] == grabbedItem) isCorrect = true;
        }
        else
        {
            if (activeItems[activeItems.Count - 1] == grabbedItem) isCorrect = true;
        }

        if (isCorrect)
        {
            correctlyGrabbedItems.Add(grabbedItem);
        }

        activeItems.Remove(grabbedItem);
    }

    public bool ValidateItem(GameObject droppedItem)
    {
        if (!isGameActive) return false;

        if (correctlyGrabbedItems.Contains(droppedItem))
        {
            correctlyGrabbedItems.Remove(droppedItem);
            return true;
        }

        return false;
    }

    public void AddScore(int amount)
    {
        if (!isGameActive) return;
        score += amount;
        if (scoreText != null) scoreText.text = "Score: " + score;
    }

    public void TriggerGameOver(string reason)
    {
        isGameActive = false;
        Time.timeScale = 0f; 

        if (endPanel != null)
        {
            endPanel.SetActive(true);

            finalScoreText.text = reason + "\nFinal Score: " + score;
            
            if (isChallengeMode && challengeButton != null)
            {
                challengeButton.SetActive(false);
            }
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadChallengeMode()
    {
        isChallengeMode = true; 
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        isChallengeMode = false; 
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu"); 
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        isChallengeMode = false;
    }
}