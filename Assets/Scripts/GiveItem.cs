using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveItem : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Items[] itemsToReceive;

    public void ReceiveItem(int id)
    {
        inventoryManager.AddItem(itemsToReceive[id]);
        Debug.Log("item gave");
    }
}
