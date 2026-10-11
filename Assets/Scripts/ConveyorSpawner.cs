using UnityEngine;
using System.Collections.Generic; // Required for Lists
using TMPro;

public class ConveyorSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public List<GameObject> itemPrefabs; // Add as many different prefabs as you want in the Inspector
    public float spawnInterval = 3f; 

    private float timer;
    private int itemNumber = 1; 

    private void Update()
    {
        if (!GameManager.instance.isGameActive) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnBox();
            timer = 0f; 
        }
    }

    private void SpawnBox()
    {
        // Safety check: Don't try to spawn if the list is empty
        if (itemPrefabs.Count == 0) return;

        // 1. Pick a random prefab from the list
        int randomIndex = Random.Range(0, itemPrefabs.Count);
        GameObject selectedPrefab = itemPrefabs[randomIndex];

        // 2. Spawn the chosen prefab
        GameObject newBox = Instantiate(selectedPrefab, transform.position, Quaternion.identity);

        // 3. Register it with the GameManager
        GameManager.instance.RegisterItem(newBox);

        // 4. Update the text label
        TextMeshProUGUI boxLabel = newBox.GetComponentInChildren<TextMeshProUGUI>();
        if (boxLabel != null)
        {
            boxLabel.text = itemNumber.ToString();
        }
        
        itemNumber++; 
    }
}