using UnityEngine;
using System.Collections.Generic;

public class PalletManager : MonoBehaviour
{
    public GameObject palletPrefab;
    public List<PalletBehavior> queueLine = new List<PalletBehavior>();

    void Update()
    {
        if (!GameManager.instance.isGameActive) return;

        //Remove the front pallet from the list the exact moment it finishes and drives away
        if (queueLine.Count > 0 && queueLine[0].state == 2)
        {
            queueLine.RemoveAt(0);
        }

        //Keep exactly 3 pallets spawned at all times
        if (queueLine.Count < 3)
        {
            GameObject newPallet = Instantiate(palletPrefab, new Vector3(-12f, transform.position.y, 0), Quaternion.identity);
            queueLine.Add(newPallet.GetComponent<PalletBehavior>());
        }

        //Tell every pallet in line where it is supposed to park
        for (int i = 0; i < queueLine.Count; i++)
        {
            if (i == 0) queueLine[i].targetX = -2f; //first row
            if (i == 1) queueLine[i].targetX = -6f; //second row
            if (i == 2) queueLine[i].targetX = -10f; //third/last row
        }
    }
}