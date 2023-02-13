using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDiscard : MonoBehaviour
{
    public void Discard() // used to discard the item ( need to connect the button to this function and have it correcly remove the right one)
    {
        /*the problem I am seeing is, when I click, it doesn't know what i clicked, only the position for
         * the transformation of the menu
         * when i click, i need to somehow save the inventoryitem for reference
        */
        InventoryItem itemInSlot = GetComponentInChildren<InventoryItem>();
        Items item = itemInSlot.item;
        itemInSlot.count--;
        if (itemInSlot.count <= 0)
        {
            Destroy(itemInSlot.gameObject);
        }
        else
        {
            itemInSlot.RefreshCount();
        }

    }
}
