using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject player;
    public GameObject InventoryMenuUI;
    public GameObject useMenu;
    public GameObject equipMenu;
    public GameObject itemInfoUI;
    public GameObject equipmentinfomenu;
    public GameObject consumableinfomenu;
    public GameObject armorinfomenu;
    public Player playerRef; //player reference


    private void Update() // this script is mainly for managing the opening/closing of the Inventory UI
    {
        
        playerRef = player.GetComponent<Player>();
        if (Input.GetKeyDown(KeyCode.I) && (playerRef.openedDialog == false) && (playerRef.openedUIPause == false) && (playerRef.openedUIGO == false) && (playerRef.openedUIShop == false) && (playerRef.openedUIStats == false))
        {
            Text curcash = InventoryMenuUI.transform.Find("Cash").GetComponent<Text>();
            curcash.text = "$" + PlayerPrefs.GetInt("currentcash");
            if (GameIsPaused && (playerRef.openedUIInven == true))
            {

                playerRef.openedUIInven = false;
                Resume();
                //Debug.Log("Inventory Close");
            }
            else if (playerRef.openedUIInven == false)
            {
                playerRef.openedUIInven = true;
                Pause();
                //Debug.Log("Inventory Open");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && (playerRef.openedUIInven == true) && (playerRef.openedUIPause == false))
        {

            playerRef.openedUIInven = false;
            Resume();
            //Debug.Log("Inventory Close");
           
        }
    }
    public void Resume()
    {
        PlayerPrefs.SetFloat("interact_range", 0.6f);
        playerRef.openedUIInven = false;
        InventoryMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void CloseInfo()
    {
        itemInfoUI.SetActive(false);
    }

    public void OpenInfo()
    {
        InventoryItem inventoryItem = FindObjectOfType<InventoryItem>();
        inventoryItem.equipMenu.transform.position = new Vector2(1500, 0);
        inventoryItem.useMenu.transform.position = new Vector2(1500, 0);
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        itemInfoUI.SetActive(true);
        Transform infoItemPic = transform.Find("Inventory/ItemInfo/FG/ItemPic/Item");
        if (inventoryManager.selectedItem.item.type == ItemType.Consumable)
        {
            Debug.Log("consumable");
            equipmentinfomenu.SetActive(false);
            armorinfomenu.SetActive(false);
            consumableinfomenu.SetActive(true);
            Transform infoItemName = transform.Find("Inventory/ItemInfo/FG/Consumable/StatValues/Name");
            Transform infoItemHP = transform.Find("Inventory/ItemInfo/FG/Consumable/StatValues/HP");
            Transform infoItemMP = transform.Find("Inventory/ItemInfo/FG/Consumable/StatValues/MP");
            Transform infoItemDesc = transform.Find("Inventory/ItemInfo/FG/Consumable/StatValues/Desc");
            Text itemname = infoItemName.GetComponent<Text>();
            Text itemHP = infoItemHP.GetComponent<Text>();
            Text itemMP = infoItemMP.GetComponent<Text>();
            Text itemDesc = infoItemDesc.GetComponent<Text>();
            itemname.text = inventoryManager.selectedItem.item.itemName;
            itemHP.text = "HP: " + inventoryManager.selectedItem.item.hp.ToString();
            itemMP.text = "MP: " + inventoryManager.selectedItem.item.mp.ToString();
            itemDesc.text = inventoryManager.selectedItem.item.desc;
        }
        
        
        Image itemimage = infoItemPic.GetComponent<Image>();
        itemimage.sprite = inventoryManager.selectedItem.image.sprite;
    }

    void Pause()
    {
        PlayerPrefs.SetFloat("interact_range", 0f);
        InventoryMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

  
}
