using UnityEngine;
using System.Collections; 

public class DeliveryZone : MonoBehaviour
{
    [Header("Indicators")]
    public GameObject checkPrefab; 
    public GameObject xPrefab;     

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            
            bool isCorrectSequence = GameManager.instance.ValidateItem(collision.gameObject); 

            if (isCorrectSequence)
            {
                GameManager.instance.AddScore(1); // Add to score
                ShowIndicator(checkPrefab, collision.transform.position);
            }
            else
            {
                ShowIndicator(xPrefab, collision.transform.position);
            }

            StartCoroutine(LingerAndDestroy(collision.gameObject));
        }
    }

    private void ShowIndicator(GameObject indicatorPrefab, Vector3 spawnPos)
    {
        Vector3 indicatorPosition = spawnPos + new Vector3(0, 1f, 0);
        GameObject indicator = Instantiate(indicatorPrefab, indicatorPosition, Quaternion.identity);
        
        Destroy(indicator, 1f); 
    }

    private IEnumerator LingerAndDestroy(GameObject item)
    {
        item.GetComponent<Rigidbody2D>().simulated = false; 

        yield return new WaitForSeconds(2f);

        Destroy(item);
    }
}