
using UnityEngine;
using System.Collections.Generic;

public class PalletManager : MonoBehaviour
{
    public Animator ConveyorAnim;
    public Transform Conveyor;
    public GameObject palletPrefab;

    public List<PalletBehavior> queueLine = new List<PalletBehavior>();

    public float moveThreshold = 0.05f;

    void Update()
    {
        if (!GameManager.instance.isGameActive)
        {
            ConveyorAnim.SetBool("IsMoving_R", false);
            return;
        }

       
        if (queueLine.Count > 0 && queueLine[0].state == 2)
        {
            queueLine.RemoveAt(0);
        }

        if (queueLine.Count < 3)
        {
            GameObject newPallet = Instantiate(
                palletPrefab,
                new Vector3(-12f, transform.position.y, 0),
                Quaternion.identity
            );

            queueLine.Add(newPallet.GetComponent<PalletBehavior>());
        }

        bool isMoving = false;

        
        for (int i = 0; i < queueLine.Count; i++)
        {
            if (queueLine[i] == null)
                continue;

           
            
            if (i == 2) queueLine[i].targetX = -10f;
            if (i == 1) queueLine[i].targetX = -6f;
            if (i == 0) queueLine[i].targetX = -2f;

            
            if (Mathf.Abs(
                queueLine[i].transform.position.x -
                queueLine[i].targetX
            ) > moveThreshold)
            {
                isMoving = true;
            }
        }

       
        ConveyorAnim.SetBool("IsMoving_R", isMoving);
    }
}
