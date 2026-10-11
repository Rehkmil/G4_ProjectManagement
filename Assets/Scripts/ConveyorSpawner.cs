using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ConveyorSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public List<GameObject> itemPrefabs;
    
    [Header("Normal Mode")]
    public float normalSpawnInterval = 3f;

    [Header("Challenge Mode Limits")]
    public float challengeStartInterval = 3f; 
    public float challengeMinInterval = 1f;   

    private float currentInterval;
    private float timer;
    private int itemNumber = 1;

    private void Start()
    {
        currentInterval = GameManager.isChallengeMode ? challengeStartInterval : normalSpawnInterval;
    }

    private void Update()
    {
        if (!GameManager.instance.isGameActive) return;

        if (GameManager.isChallengeMode)
        {
            float timePercent = GameManager.instance.GetTimePercentage();
            currentInterval = Mathf.Lerp(challengeMinInterval, challengeStartInterval, timePercent);
        }

        timer += Time.deltaTime;

        if (timer >= currentInterval)
        {
            SpawnBox();
            timer = 0f;
        }
    }

    private void SpawnBox()
    {
        if (itemPrefabs.Count == 0) return;

        int randomIndex = Random.Range(0, itemPrefabs.Count);
        GameObject selectedPrefab = itemPrefabs[randomIndex];
        GameObject newBox = Instantiate(selectedPrefab, transform.position, Quaternion.identity);

        GameManager.instance.RegisterItem(newBox);

        TextMeshProUGUI boxLabel = newBox.GetComponentInChildren<TextMeshProUGUI>();
        if (boxLabel != null)
        {
            boxLabel.text = itemNumber.ToString();
        }
        
        itemNumber++; 
    }
}