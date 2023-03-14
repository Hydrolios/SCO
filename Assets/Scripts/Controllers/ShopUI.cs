using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public GameObject shopUI;
    public ListingUI listing;
    public Items buyItem;
    public Player playerRef;
    public Button buyButton;


    public void CloseShopUI()
    {
        PlayerPrefs.SetFloat("interact_range", 0.6f);
        playerRef.openedUIShop = false;
        shopUI.SetActive(false);
        listing.CloseUI();
        Time.timeScale = 1f;
    }

    public void SetItem(Items item)
    {
        buyItem = item;
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyItem);
    }

    public void BuyItem()
    {
       
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>(); // gets the inventoryManager in the scene
        inventoryManager.AddItem(buyItem);
        //subtract price from players cur cash then update
    }

}
