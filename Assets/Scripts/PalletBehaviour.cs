using UnityEngine;
using System.Collections.Generic;

public class PalletBehavior : MonoBehaviour
{
    public float speed = 5f;
    public float targetX; // The Manager updates this automatically
    public List<GameObject> boxes; 
    
    public int state = 0; 
    private bool isRegistered = false;

    void Update()
    {
        if (!GameManager.instance.isGameActive) return;

        if (state == 0) 
        {
            // Slide towards whatever target position the Manager assigns
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetX, transform.position.y, 0), speed * Time.deltaTime);

            // If we reach the unloading zone (-2f) and haven't registered yet
            if (transform.position.x == -2f && !isRegistered)
            {
                RegisterStack();
                isRegistered = true;
                state = 1; 
            }
        }
        else if (state == 1) 
        {
            // Wait until all boxes are grabbed by the player
            if (GameManager.instance.activeItems.Count == 0)
            {
                state = 2; 
            }
        }
        else if (state == 2) 
        {
            // Drive away to the right side of the screen
            transform.position += Vector3.right * speed * Time.deltaTime;
            if (transform.position.x > 10f) 
            {
                Destroy(gameObject);
            }
        }
    }

    private void RegisterStack()
    {
        GameManager.instance.activeItems.Clear();
        foreach (GameObject box in boxes)
        {
            if (box != null) GameManager.instance.activeItems.Add(box);
        }
    }
}