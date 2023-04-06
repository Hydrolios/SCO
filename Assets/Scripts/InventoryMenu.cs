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
            if(equipmentinfomenu.activeSelf || armorinfomenu.activeSelf || consumableinfomenu.activeSelf)
            {
                equipmentinfomenu.SetActive(false);
                armorinfomenu.SetActive(false);
                consumableinfomenu.SetActive(false);
                itemInfoUI.SetActive(false);
            }
            else
            {
                playerRef.openedUIInven = false;
                Resume();
                //Debug.Log("Inventory Close");
            }


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
        else if (inventoryManager.selectedItem.item.type == ItemType.Equipment && inventoryManager.selectedItem.item.equipType == EquipmentType.Weapon)
        {
            Debug.Log("Weapon");
            equipmentinfomenu.SetActive(true);
            armorinfomenu.SetActive(false);
            consumableinfomenu.SetActive(false);
            Transform infoItemName = transform.Find("Inventory/ItemInfo/FG/Weapon/StatValues/Name");
            Transform infoItemHP = transform.Find("Inventory/ItemInfo/FG/Weapon/StatValues/HP");
            Transform infoItemMP = transform.Find("Inventory/ItemInfo/FG/Weapon/StatValues/MP");
            Transform infoItemDesc = transform.Find("Inventory/ItemInfo/FG/Weapon/StatValues/Desc");
            Transform infoItemATT = transform.Find("Inventory/ItemInfo/FG/Weapon/StatValues/ATT");
            Transform infoItemDEF = transform.Find("Inventory/ItemInfo/FG/Weapon/StatValues/DEF");
            Transform infoItemENX = transform.Find("Inventory/ItemInfo/FG/Weapon/StatValues/ENX");
            Transform infoItemCHR = transform.Find("Inventory/ItemInfo/FG/Weapon/StatValues/CHR");
            Transform infoItemRAD = transform.Find("Inventory/ItemInfo/FG/Weapon/StatValues/RAD");
            Transform infoItemSOL = transform.Find("Inventory/ItemInfo/FG/Weapon/StatValues/SOL");
            Text itemname = infoItemName.GetComponent<Text>();
            Text itemHP = infoItemHP.GetComponent<Text>();
            Text itemMP = infoItemMP.GetComponent<Text>();
            Text itemDesc = infoItemDesc.GetComponent<Text>();
            Text itemATT = infoItemATT.GetComponent<Text>();
            Text itemDEF = infoItemDEF.GetComponent<Text>();
            Text itemENX = infoItemENX.GetComponent<Text>();
            Text itemCHR = infoItemCHR.GetComponent<Text>();
            Text itemRAD = infoItemRAD.GetComponent<Text>();
            Text itemSOL = infoItemSOL.GetComponent<Text>();
            itemname.text = inventoryManager.selectedItem.item.itemName;
            itemHP.text = "HP: " + inventoryManager.selectedItem.item.hp.ToString();
            itemMP.text = "MP: " + inventoryManager.selectedItem.item.mp.ToString();
            itemDesc.text = inventoryManager.selectedItem.item.desc;
            itemATT.text = "ATT: " + inventoryManager.selectedItem.item.att.ToString();
            itemDEF.text = "DEF: " + inventoryManager.selectedItem.item.def.ToString();
            itemENX.text = "ENX: " + inventoryManager.selectedItem.item.enx.ToString();
            itemCHR.text = "CHR: " + inventoryManager.selectedItem.item.chr.ToString();
            itemRAD.text = "RAD: " + inventoryManager.selectedItem.item.rad.ToString();
            itemSOL.text = "SOL: " + inventoryManager.selectedItem.item.sol.ToString();
        }
        else if (inventoryManager.selectedItem.item.type == ItemType.Equipment && inventoryManager.selectedItem.item.equipType != EquipmentType.Weapon)
        {
            Debug.Log("Equipment");
            equipmentinfomenu.SetActive(false);
            armorinfomenu.SetActive(true);
            consumableinfomenu.SetActive(false);
            Transform infoItemName = transform.Find("Inventory/ItemInfo/FG/Armor/StatValues/Name");
            Transform infoItemHP = transform.Find("Inventory/ItemInfo/FG/Armor/StatValues/HP");
            Transform infoItemMP = transform.Find("Inventory/ItemInfo/FG/Armor/StatValues/MP");
            Transform infoItemDesc = transform.Find("Inventory/ItemInfo/FG/Armor/StatValues/Desc");
            Transform infoItemATT = transform.Find("Inventory/ItemInfo/FG/Armor/StatValues/ATT");
            Transform infoItemDEF = transform.Find("Inventory/ItemInfo/FG/Armor/StatValues/DEF");
            Transform infoItemENX = transform.Find("Inventory/ItemInfo/FG/Armor/StatValues/ENX");
            Transform infoItemCHR = transform.Find("Inventory/ItemInfo/FG/Armor/StatValues/CHR");
            Transform infoItemRAD = transform.Find("Inventory/ItemInfo/FG/Armor/StatValues/RAD");
            Transform infoItemSOL = transform.Find("Inventory/ItemInfo/FG/Armor/StatValues/SOL");
            Text itemname = infoItemName.GetComponent<Text>();
            Text itemHP = infoItemHP.GetComponent<Text>();
            Text itemMP = infoItemMP.GetComponent<Text>();
            Text itemDesc = infoItemDesc.GetComponent<Text>();
            Text itemATT = infoItemATT.GetComponent<Text>();
            Text itemDEF = infoItemDEF.GetComponent<Text>();
            Text itemENX = infoItemENX.GetComponent<Text>();
            Text itemCHR = infoItemCHR.GetComponent<Text>();
            Text itemRAD = infoItemRAD.GetComponent<Text>();
            Text itemSOL = infoItemSOL.GetComponent<Text>();
            itemname.text = inventoryManager.selectedItem.item.itemName;
            itemHP.text = "HP: " + inventoryManager.selectedItem.item.hp.ToString();
            itemMP.text = "MP: " + inventoryManager.selectedItem.item.mp.ToString();
            itemDesc.text = inventoryManager.selectedItem.item.desc;
            itemATT.text = "ATT: " + inventoryManager.selectedItem.item.att.ToString();
            itemDEF.text = "DEF: " + inventoryManager.selectedItem.item.def.ToString();
            itemENX.text = "ENX: " + inventoryManager.selectedItem.item.enx.ToString();
            itemCHR.text = "CHR: " + inventoryManager.selectedItem.item.chr.ToString();
            itemRAD.text = "RAD: " + inventoryManager.selectedItem.item.rad.ToString();
            itemSOL.text = "SOL: " + inventoryManager.selectedItem.item.sol.ToString();
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
