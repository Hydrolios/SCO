using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemListing : MonoBehaviour
{
    public Items[] items;
    public GameObject itemListing;
    public GameObject listing;

    void Start()
    {
        foreach (Items item in items)
        {
            GameObject newItem = Instantiate(itemListing, listing.transform);
            newItem.name = item.name;

            
            Transform nameTextTf = newItem.transform.Find("ItemName");
            if (nameTextTf != null)
            {
                //Debug.Log("itemname found");
                TextMeshProUGUI nameText = nameTextTf.GetComponent<TextMeshProUGUI>();
                if(nameText != null)
                {
                    //Debug.Log("text component found");
                    nameText.text = item.name;
                }
                
            }
            ListingButton listingButton = newItem.GetComponent<ListingButton>(); // trying to pass the item values to button
            if (listingButton != null)
            {
                listingButton.item = item;
            }
        }
    }
}


