using UnityEngine;

public class ConveyorCleanup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            // First, remove it from the GameManager so the system stops tracking it
            GameManager.instance.activeItems.Remove(collision.gameObject);
            
            // Then, safely destroy the object
            Destroy(collision.gameObject);
        }
    }
}