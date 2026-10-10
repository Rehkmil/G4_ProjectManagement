using UnityEngine;

public class ClawController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public Transform holdAnchor; 

    [Header("Boundary Limits (Deadzone)")]
    public float minX = -5f;
    public float maxX = 5f;
    public float minY = -3f;
    public float maxY = 5f;

    private GameObject itemInZone = null;
    private GameObject heldItem = null;

    private void Update()
    {
        // 1. Calculate intended movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        
        Vector3 newPosition = transform.position + new Vector3(moveX, moveY, 0) * moveSpeed * Time.deltaTime;

        // 2. Clamp the position to strictly enforce your limits
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        // 3. Apply the final locked position
        transform.position = newPosition;

        // Handle Grab and Drop
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (heldItem == null && itemInZone != null)
            {
                GrabItem();
            }
            else if (heldItem != null)
            {
                DropItem();
            }
        }
    }

    private void GrabItem()
    {
        heldItem = itemInZone;

        ConveyorItem conveyorMovement = heldItem.GetComponent<ConveyorItem>();
        if (conveyorMovement != null)
        {
            conveyorMovement.enabled = false;
        }

        heldItem.transform.position = holdAnchor.position;
        heldItem.transform.SetParent(holdAnchor);
    }

    private void DropItem()
    {
        heldItem.transform.SetParent(null);
        heldItem = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item") && heldItem == null)
        {
            itemInZone = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == itemInZone && heldItem == null)
        {
            itemInZone = null;
        }
    }
}