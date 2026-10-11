using UnityEngine;

public class OverflowSensor : MonoBehaviour
{
    private float timeInZone = 0f;
    public float jamTimeLimit = 1f; // The box must be stuck here for 1 second to cause a Game Over

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            timeInZone += Time.deltaTime;
            
            if (timeInZone >= jamTimeLimit && GameManager.instance.isGameActive)
            {
                // We call a new function to pass a custom game over message
                GameManager.instance.TriggerGameOver("Conveyor Jammed!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            timeInZone = 0f; // Reset the timer if the box falls through or gets grabbed
        }
    }
}