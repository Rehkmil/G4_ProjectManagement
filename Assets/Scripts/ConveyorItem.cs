using UnityEngine;

public class ConveyorItem : MonoBehaviour
{
    public float speed = 2f; 

    private void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
}