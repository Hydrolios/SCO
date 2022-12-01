using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public GameObject shopUI;
    public GameObject ListingUI;
    public Player playerRef;

    /* when a listing is clicked, the function attached to specified button displays the UI containing the components
     * i guess it can be done with variables and only 1 UI while the listing provides the data to the UI so it is an all-in-one
    */
    public void ListingClicked()
    {
        ListingUI.SetActive(true);  // show the clicked item  
    }    
    public void CloseShopUI()
    {
        PlayerPrefs.SetFloat("interact_range", 0.6f);
        playerRef.openedUIShop = false;
        shopUI.SetActive(false);
        Time.timeScale = 1f;
    }
}
