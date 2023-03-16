using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public GameObject shopUI;
    public Text poor;
    public ListingUI listing;
    public Items buyItem;
    public Player playerRef;
    public Button buyButton;
    public Text cash;


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
       if(buyItem.buycost > PlayerPrefs.GetInt("currentcash"))
        {
            Debug.Log("Player is too poor");
            StartCoroutine(PoorPopup());

        }
       else if (buyItem.buycost <= PlayerPrefs.GetInt("currentcash"))
        {
            InventoryManager inventoryManager = FindObjectOfType<InventoryManager>(); // gets the inventoryManager in the scene
            inventoryManager.AddItem(buyItem);

            //subtract price from players cur cash then update
            PlayerPrefs.SetInt("currentcash", PlayerPrefs.GetInt("currentcash") - buyItem.buycost);
            cash.text = "$" + PlayerPrefs.GetInt("currentcash");


        }

    }

    IEnumerator PoorPopup()
    {
        float startTime = Time.realtimeSinceStartup;
        float waitTime = 1f;
        poor.transform.position = new Vector2(400, 300);
        while (Time.realtimeSinceStartup < startTime + waitTime) // need to use realtime as time scale is 0 while menuing 
        {
            yield return null;
        }

        poor.transform.position = new Vector2(1000, 0);
    }

}
