using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListingButton : MonoBehaviour
{
    public ListingUI ShopUI;
    public Items item;
    public void OnButtonClick() // get the listing component from the parent
    {
        Debug.Log(item);
        GameObject shopUIObj = GameObject.Find("ShopUI"); // finds shop ui
        ShopUI = shopUIObj.GetComponent<ListingUI>(); // gets the listingUI component of it
        GameObject itemInfo = ShopUI.itemStatUI; // gets specifically the itemstatUI of the listingUI
        
        Text itemInfoATT = itemInfo.transform.Find("ATT").GetComponent<Text>();
        Text itemInfoDEF = itemInfo.transform.Find("DEF").GetComponent<Text>();
        Text itemInfoHP = itemInfo.transform.Find("HP").GetComponent<Text>();
        Text itemInfoMP = itemInfo.transform.Find("MP").GetComponent<Text>();
        Text itemInfoSOL = itemInfo.transform.Find("SOL").GetComponent<Text>();
        Text itemInfoENX = itemInfo.transform.Find("ENX").GetComponent<Text>();
        Text itemInfoRAD = itemInfo.transform.Find("RAD").GetComponent<Text>();
        Text itemInfoCHR = itemInfo.transform.Find("CHR").GetComponent<Text>();
        Text itemInfoDSC = itemInfo.transform.Find("Description").GetComponent<Text>();

        itemInfoDSC.text = item.desc;
        //main stat value
        itemInfoATT.text = "ATT : " + item.att.ToString();
        itemInfoHP.text = "HP : " + item.hp.ToString();
        itemInfoDEF.text = "DEF : " + item.def.ToString();
        itemInfoMP.text = "MP : " + item.mp.ToString();
        //elemental stat values
        itemInfoSOL.text = "SOL : " + item.sol.ToString();
        itemInfoENX.text = "ENX : " + item.enx.ToString();
        itemInfoRAD.text = "RAD : " + item.rad.ToString();
        itemInfoCHR.text = "CHR : " + item.chr.ToString();

        //itemInfoATT.text = item.att.ToString();

        itemInfo.SetActive(true);
    }


}
