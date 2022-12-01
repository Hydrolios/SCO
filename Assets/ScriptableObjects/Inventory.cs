using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public int inventorySpace = 15;
    public List<Items> items = new List<Items>();
    
    public bool Add(Items item)
    {
        if (!item.isDefaultItem)
        {
            if (items.Count >= inventorySpace)
            {
                Debug.Log("Not enough inventory space");
                return false;
            }
            items.Add(item);
        }
        return true;
    }

    public void Remove(Items item)
    {
        items.Remove(item);
    }

}
