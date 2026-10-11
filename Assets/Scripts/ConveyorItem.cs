using UnityEngine;

public class ConveyorItem : MonoBehaviour
{
    public float speed = 2f;
    private bool isBlocked = false;

    private void Update()
    {
        if (!isBlocked)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Item") || collision.gameObject.CompareTag("Blocker"))
        {
            isBlocked = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isBlocked = false; 
    }
}