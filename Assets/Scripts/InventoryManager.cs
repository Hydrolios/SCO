using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int maxStack = 99;
    public InventorySlot[] inventorySlots;
    public InventorySlot hatEquip;
    public InventorySlot shirtEquip;
    public InventorySlot pantsEquip;
    public InventorySlot wepEquip;
    public InventoryMenu discardui;
    public GameObject inventoryItemPrefab;
    public ItemList itemList;
    public InventorySlot selectedSlot;
    public InventoryItem selectedItem;
    public Player player;

    public int currentWeaponID;
    public int attvalue;
    public int solvalue;
    public int radvalue;
    public int chrvalue;
    public int enxvalue;


    public void Reward()
    {
        //Debug.Log("item rewarded from battle");

        int reward = PlayerPrefs.GetInt("BattleReward");
        Debug.Log(itemList.items[reward]);
        AddItem(itemList.items[reward]);
        PlayerPrefs.DeleteKey("BattleReward");
    }

    public void ClickedSlot(GameObject slot) // Gets the selected inventory slot that was clicked
    {
        selectedSlot = slot.GetComponent<InventorySlot>();
        selectedItem = selectedSlot.GetComponentInChildren<InventoryItem>();
        Debug.Log(selectedItem.item);


    }
    public bool AddItem(Items item) // Adds an item to the inventory but checks for a place to insert the item
    {
        //Debug.Log("in process of adding the item");
        for (int i = 0; i < inventorySlots.Length; i++) //finds the slot with same item to stack
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>(); //checks if equip slot is occupied
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStack && itemInSlot.item.stackable == true)
            {
                //Debug.Log("Slot found of same item");
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        for (int i = 0; i < inventorySlots.Length; i++) //finds an empty slot to add the item to
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>(); //checks if equip slot is occupied
            if (itemInSlot == null)
            {
                //Debug.Log("Empty slot found for the item");
                SpawnNewItem(item, slot);
                return true;
            }
        }
        
        return false;
        
    }

    void SpawnNewItem(Items item, InventorySlot slot) //instantiates the scriptable object item
    {
        GameObject newItemGO = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }

     public void SaveInventory() // saves inventory info by cycling through the inventory and saving item id + count
    {
        PlayerPrefs.DeleteKey("InventorySlotA");
        PlayerPrefs.DeleteKey("InventorySlotB");
        PlayerPrefs.DeleteKey("InventorySlotC");
        PlayerPrefs.DeleteKey("InventorySlotD");
        for (int i = 0; i < inventorySlots.Length; i++) //wipes out the previously saved first as different save may use different amounts of inv space
        {
            PlayerPrefs.DeleteKey("InventorySlot" + i + "ID");
            PlayerPrefs.DeleteKey("InventorySlot" + i + "Count");
        }
        for (int i = 0; i < inventorySlots.Length; i++) // then saves the new iteration of the inventory
        {
            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                //Debug.Log("one");
                PlayerPrefs.SetInt("InventorySlot" + i + "ID", itemInSlot.item.id); 
                PlayerPrefs.SetInt("InventorySlot" + i + "Count", itemInSlot.count);
                
            }

        }

    }

    public void DeleteInv() // for testing functionality of cleaning out the inventory
    {
        for (int i = 0; i < inventorySlots.Length; i++) // gets rid of any of the existing inventory to prevent duplication
        {
            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                Destroy(itemInSlot.gameObject);

            }
        }
    }
   

    
    public void LoadInventory() // loads inventory info for save/load
    {

        for (int i = 0; i < inventorySlots.Length; i++) // gets rid of any of the existing inventory to prevent duplication
        {
            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                for (int a = 0; a < itemInSlot.count; a++)
                {
                    Destroy(itemInSlot.gameObject);
                }
                
            }
        }
        
        for (int i = 0; i < inventorySlots.Length; i++) // loads in the saved inventory with the playerprefs
        {
            int id = PlayerPrefs.GetInt("InventorySlot" + i + "ID", -1);
            int count = PlayerPrefs.GetInt("InventorySlot" + i + "Count", 0);
            Items item = null;

            for (int j = 0; j < itemList.items.Length; j++)
            {
                if (itemList.items[j].id == id)
                {
                    item = itemList.items[j];
                }
                if (item != null)
                {
                    break;
                }
            }
            if (item != null && count > 0)
            {
                SpawnNewItem(item, inventorySlots[i]);
                InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();
                itemInSlot.count = count;
                itemInSlot.RefreshCount();
            }


        }


    }

    public void LoadEquipped(int id, InventorySlot slot) // loads the data of equipped 
    {
        int itemid = id;
        Items item = null;
        for (int j = 0; j < itemList.items.Length; j++)
        {
            if (itemList.items[j].id == itemid)
            {
                item = itemList.items[j];
                Debug.Log(itemid);
            }
            if (item != null)
            {
                break;
            }
        }
        if (item != null)
        {
            SpawnNewItem(item, slot);
        }
    }
    
    public void SaveInventoryScene() // saves inventory info by cycling through the inventory and saving item id + count
    {
        PlayerPrefs.DeleteKey("InventorySlotA");
        PlayerPrefs.DeleteKey("InventorySlotB");
        PlayerPrefs.DeleteKey("InventorySlotC");
        PlayerPrefs.DeleteKey("InventorySlotD");
        for (int i = 0; i < inventorySlots.Length; i++) //wipes out the previously saved first
        {
            PlayerPrefs.DeleteKey("InventorySlotScene" + i + "ID");
            PlayerPrefs.DeleteKey("InventorySlotScene" + i + "Count");

        }
        for (int i = 0; i < inventorySlots.Length; i++) // then saves the new iteration of the inventory
        {
            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                PlayerPrefs.SetInt("InventorySlotScene" + i + "ID", itemInSlot.item.id);
                PlayerPrefs.SetInt("InventorySlotScene" + i + "Count", itemInSlot.count);
            }

        }

        
    }

    public void LoadInventoryScene() // loads inventory info for scene change
    {

        for (int i = 0; i < inventorySlots.Length; i++) // gets rid of any of the existing inventory to prevent duplication
        {
            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                for (int a = 0; a < itemInSlot.count; a++)
                {
                    Destroy(itemInSlot.gameObject);
                }
            }
        }
        
        for (int i = 0; i < inventorySlots.Length; i++) // loads in the saved inventory with the playerprefs
        {
            int id = PlayerPrefs.GetInt("InventorySlotScene" + i + "ID", -1);
            int count = PlayerPrefs.GetInt("InventorySlotScene" + i + "Count", 0);
            Items item = null;
            for (int j = 0; j < itemList.items.Length; j++) 
            {
                if (itemList.items[j].id == id)
                {
                    item = itemList.items[j];
                }
                if (item != null)
                {
                    break;
                }
            }
            if (item != null && count > 0)
            {
                SpawnNewItem(item, inventorySlots[i]);
                InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();
                itemInSlot.count = count;
                itemInSlot.RefreshCount();
            }
        }

    }

    public void DiscardItemPopup() // when clicking discard, opens up the discard confirmation window
    {
        selectedItem.DiscardItemUI();

    }
    public void DiscardItem() // Discards one item
    {
        selectedItem.count--;
        if (selectedItem.count <= 0)
        {
            Destroy(selectedItem.gameObject);
        }
        else
        {
            selectedItem.RefreshCount();
        }
        selectedItem.ItemUsed();
        selectedItem.HideDiscardItemUI();

    }

    public void DontDiscardItem() // closes discard window
    {
        selectedItem.HideDiscardItemUI();

    }
    public void ConsumeItem() // uses one item
    {
        selectedItem.count--;
        selectedItem.Consume(player);
        if (selectedItem.count <= 0)
        {
            Destroy(selectedItem.gameObject);
        }
        else
        {
            selectedItem.RefreshCount();
        }
        selectedItem.ItemUsed();

    }

    public void ItemInfo() // opens up UI to display item info, similar to shop
    {

    }
    
    public void EquipItem() // equipping item and making sure to swap with already equipped item WIP
    {
        selectedItem.count--;
        selectedItem.RefreshCount();
        selectedItem.ItemUsed();
        if(selectedItem.item.equipType == EquipmentType.Hat)
        {
            InventorySlot slot = hatEquip;
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>(); //checks if equip slot is occupied
            if (itemInSlot == null)
            {
                SpawnNewItem(selectedItem.item, slot);
                //PlayerPrefs.SetInt("playerDEFnow", PlayerPrefs.GetInt("playerDEFnow") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerHPnow", PlayerPrefs.GetInt("playerHPnow") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerHPMax", PlayerPrefs.GetInt("playerHPMax") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerMPnow", PlayerPrefs.GetInt("playerMPnow") + selectedItem.item.mp);
                PlayerPrefs.SetInt("playerMPMax", PlayerPrefs.GetInt("playerMPMax") + selectedItem.item.mp);
                PlayerPrefs.SetInt("playerattack", PlayerPrefs.GetInt("playerattack") + selectedItem.item.att);
                PlayerPrefs.SetInt("playersol", PlayerPrefs.GetInt("playersol") + selectedItem.item.sol);
                PlayerPrefs.SetInt("playerrad", PlayerPrefs.GetInt("playerrad") + selectedItem.item.rad);
                PlayerPrefs.SetInt("playerenx", PlayerPrefs.GetInt("playerenx") + selectedItem.item.enx);
                PlayerPrefs.SetInt("playerchr", PlayerPrefs.GetInt("playerchr") + selectedItem.item.chr);
                Destroy(selectedItem.gameObject);
            }
            else // else is for swapping out equip
            {
                AddItem(itemInSlot.item);
                //PlayerPrefs.SetInt("playerDEFnow", PlayerPrefs.GetInt("playerDEFnow") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerHPnow", PlayerPrefs.GetInt("playerHPnow") - itemInSlot.item.hp);
                PlayerPrefs.SetInt("playerHPMax", PlayerPrefs.GetInt("playerHPMax") - itemInSlot.item.hp);
                PlayerPrefs.SetInt("playerMPnow", PlayerPrefs.GetInt("playerMPnow") - itemInSlot.item.mp);
                PlayerPrefs.SetInt("playerMPMax", PlayerPrefs.GetInt("playerMPMax") - itemInSlot.item.mp);
                PlayerPrefs.SetInt("playerattack", PlayerPrefs.GetInt("playerattack") - itemInSlot.item.att);
                PlayerPrefs.SetInt("playersol", PlayerPrefs.GetInt("playersol") - itemInSlot.item.sol);
                PlayerPrefs.SetInt("playerrad", PlayerPrefs.GetInt("playerrad") - itemInSlot.item.rad);
                PlayerPrefs.SetInt("playerenx", PlayerPrefs.GetInt("playerenx") - itemInSlot.item.enx);
                PlayerPrefs.SetInt("playerchr", PlayerPrefs.GetInt("playerchr") - itemInSlot.item.chr);
                Destroy(selectedItem.gameObject); // deletes the new item from inventory
                Destroy(itemInSlot.gameObject); // deletes the item in the weapon slot
                SpawnNewItem(selectedItem.item, slot); //adds the new item into the weapon slot
                //PlayerPrefs.SetInt("playerDEFnow", PlayerPrefs.GetInt("playerDEFnow") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerHPnow", PlayerPrefs.GetInt("playerHPnow") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerHPMax", PlayerPrefs.GetInt("playerHPMax") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerMPnow", PlayerPrefs.GetInt("playerMPnow") + selectedItem.item.mp);
                PlayerPrefs.SetInt("playerMPMax", PlayerPrefs.GetInt("playerMPMax") + selectedItem.item.mp);
                PlayerPrefs.SetInt("playerattack", PlayerPrefs.GetInt("playerattack") + selectedItem.item.att);
                PlayerPrefs.SetInt("playersol", PlayerPrefs.GetInt("playersol") + selectedItem.item.sol);
                PlayerPrefs.SetInt("playerrad", PlayerPrefs.GetInt("playerrad") + selectedItem.item.rad);
                PlayerPrefs.SetInt("playerenx", PlayerPrefs.GetInt("playerenx") + selectedItem.item.enx);
                PlayerPrefs.SetInt("playerchr", PlayerPrefs.GetInt("playerchr") + selectedItem.item.chr);
            }
        }

        else if (selectedItem.item.equipType == EquipmentType.Shirt)
        {
            InventorySlot slot = shirtEquip;
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>(); //checks if equip slot is occupied
            if (itemInSlot == null)
            {
                SpawnNewItem(selectedItem.item, slot);
                //PlayerPrefs.SetInt("playerDEFnow", PlayerPrefs.GetInt("playerDEFnow") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerHPnow", PlayerPrefs.GetInt("playerHPnow") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerHPMax", PlayerPrefs.GetInt("playerHPMax") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerMPnow", PlayerPrefs.GetInt("playerMPnow") + selectedItem.item.mp);
                PlayerPrefs.SetInt("playerMPMax", PlayerPrefs.GetInt("playerMPMax") + selectedItem.item.mp);
                PlayerPrefs.SetInt("playerattack", PlayerPrefs.GetInt("playerattack") + selectedItem.item.att);
                PlayerPrefs.SetInt("playersol", PlayerPrefs.GetInt("playersol") + selectedItem.item.sol);
                PlayerPrefs.SetInt("playerrad", PlayerPrefs.GetInt("playerrad") + selectedItem.item.rad);
                PlayerPrefs.SetInt("playerenx", PlayerPrefs.GetInt("playerenx") + selectedItem.item.enx);
                PlayerPrefs.SetInt("playerchr", PlayerPrefs.GetInt("playerchr") + selectedItem.item.chr);
                Destroy(selectedItem.gameObject);
            }
            else
            {
                AddItem(itemInSlot.item);
                //PlayerPrefs.SetInt("playerDEFnow", PlayerPrefs.GetInt("playerDEFnow") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerHPnow", PlayerPrefs.GetInt("playerHPnow") - itemInSlot.item.hp);
                PlayerPrefs.SetInt("playerHPMax", PlayerPrefs.GetInt("playerHPMax") - itemInSlot.item.hp);
                PlayerPrefs.SetInt("playerMPnow", PlayerPrefs.GetInt("playerMPnow") - itemInSlot.item.mp);
                PlayerPrefs.SetInt("playerMPMax", PlayerPrefs.GetInt("playerMPMax") - itemInSlot.item.mp);
                PlayerPrefs.SetInt("playerattack", PlayerPrefs.GetInt("playerattack") - itemInSlot.item.att);
                PlayerPrefs.SetInt("playersol", PlayerPrefs.GetInt("playersol") - itemInSlot.item.sol);
                PlayerPrefs.SetInt("playerrad", PlayerPrefs.GetInt("playerrad") - itemInSlot.item.rad);
                PlayerPrefs.SetInt("playerenx", PlayerPrefs.GetInt("playerenx") - itemInSlot.item.enx);
                PlayerPrefs.SetInt("playerchr", PlayerPrefs.GetInt("playerchr") - itemInSlot.item.chr);
                Destroy(selectedItem.gameObject); // deletes the new item from inventory
                Destroy(itemInSlot.gameObject); // deletes the item in the weapon slot
                SpawnNewItem(selectedItem.item, slot); //adds the new item into the weapon slot
                //PlayerPrefs.SetInt("playerDEFnow", PlayerPrefs.GetInt("playerDEFnow") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerHPnow", PlayerPrefs.GetInt("playerHPnow") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerHPMax", PlayerPrefs.GetInt("playerHPMax") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerMPnow", PlayerPrefs.GetInt("playerMPnow") + selectedItem.item.mp);
                PlayerPrefs.SetInt("playerMPMax", PlayerPrefs.GetInt("playerMPMax") + selectedItem.item.mp);
                PlayerPrefs.SetInt("playerattack", PlayerPrefs.GetInt("playerattack") + selectedItem.item.att);
                PlayerPrefs.SetInt("playersol", PlayerPrefs.GetInt("playersol") + selectedItem.item.sol);
                PlayerPrefs.SetInt("playerrad", PlayerPrefs.GetInt("playerrad") + selectedItem.item.rad);
                PlayerPrefs.SetInt("playerenx", PlayerPrefs.GetInt("playerenx") + selectedItem.item.enx);
                PlayerPrefs.SetInt("playerchr", PlayerPrefs.GetInt("playerchr") + selectedItem.item.chr);
            }
        }

        else if (selectedItem.item.equipType == EquipmentType.Pants)
        {
            InventorySlot slot = pantsEquip;
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>(); //checks if equip slot is occupied
            if(itemInSlot == null)
            {
                SpawnNewItem(selectedItem.item, slot);
                //PlayerPrefs.SetInt("playerDEFnow", PlayerPrefs.GetInt("playerDEFnow") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerHPnow", PlayerPrefs.GetInt("playerHPnow") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerHPMax", PlayerPrefs.GetInt("playerHPMax") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerMPnow", PlayerPrefs.GetInt("playerMPnow") + selectedItem.item.mp);
                PlayerPrefs.SetInt("playerMPMax", PlayerPrefs.GetInt("playerMPMax") + selectedItem.item.mp);
                PlayerPrefs.SetInt("playerattack", PlayerPrefs.GetInt("playerattack") + selectedItem.item.att);
                PlayerPrefs.SetInt("playersol", PlayerPrefs.GetInt("playersol") + selectedItem.item.sol);
                PlayerPrefs.SetInt("playerrad", PlayerPrefs.GetInt("playerrad") + selectedItem.item.rad);
                PlayerPrefs.SetInt("playerenx", PlayerPrefs.GetInt("playerenx") + selectedItem.item.enx);
                PlayerPrefs.SetInt("playerchr", PlayerPrefs.GetInt("playerchr") + selectedItem.item.chr);
                Destroy(selectedItem.gameObject);
            }
            else
            {
                AddItem(itemInSlot.item);
                //PlayerPrefs.SetInt("playerDEFnow", PlayerPrefs.GetInt("playerDEFnow") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerHPnow", PlayerPrefs.GetInt("playerHPnow") - itemInSlot.item.hp);
                PlayerPrefs.SetInt("playerHPMax", PlayerPrefs.GetInt("playerHPMax") - itemInSlot.item.hp);
                PlayerPrefs.SetInt("playerMPnow", PlayerPrefs.GetInt("playerMPnow") - itemInSlot.item.mp);
                PlayerPrefs.SetInt("playerMPMax", PlayerPrefs.GetInt("playerMPMax") - itemInSlot.item.mp);
                PlayerPrefs.SetInt("playerattack", PlayerPrefs.GetInt("playerattack") - itemInSlot.item.att);
                PlayerPrefs.SetInt("playersol", PlayerPrefs.GetInt("playersol") - itemInSlot.item.sol);
                PlayerPrefs.SetInt("playerrad", PlayerPrefs.GetInt("playerrad") - itemInSlot.item.rad);
                PlayerPrefs.SetInt("playerenx", PlayerPrefs.GetInt("playerenx") - itemInSlot.item.enx);
                PlayerPrefs.SetInt("playerchr", PlayerPrefs.GetInt("playerchr") - itemInSlot.item.chr);
                Destroy(selectedItem.gameObject); // deletes the new item from inventory
                Destroy(itemInSlot.gameObject); // deletes the item in the weapon slot
                SpawnNewItem(selectedItem.item, slot); //adds the new item into the weapon slot
                //PlayerPrefs.SetInt("playerDEFnow", PlayerPrefs.GetInt("playerDEFnow") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerHPnow", PlayerPrefs.GetInt("playerHPnow") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerHPMax", PlayerPrefs.GetInt("playerHPMax") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerMPnow", PlayerPrefs.GetInt("playerMPnow") + selectedItem.item.mp);
                PlayerPrefs.SetInt("playerMPMax", PlayerPrefs.GetInt("playerMPMax") + selectedItem.item.mp);
                PlayerPrefs.SetInt("playerattack", PlayerPrefs.GetInt("playerattack") + selectedItem.item.att);
                PlayerPrefs.SetInt("playersol", PlayerPrefs.GetInt("playersol") + selectedItem.item.sol);
                PlayerPrefs.SetInt("playerrad", PlayerPrefs.GetInt("playerrad") + selectedItem.item.rad);
                PlayerPrefs.SetInt("playerenx", PlayerPrefs.GetInt("playerenx") + selectedItem.item.enx);
                PlayerPrefs.SetInt("playerchr", PlayerPrefs.GetInt("playerchr") + selectedItem.item.chr);
            }

        }

        else if (selectedItem.item.equipType == EquipmentType.Weapon)
        {
            InventorySlot slot = wepEquip;
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>(); //checks if equip slot is occupied
            Debug.Log(itemInSlot);
            if (itemInSlot == null)
            {
                Debug.Log("No equipped weapon, adding weapon");
                SpawnNewItem(selectedItem.item, slot);

                //PlayerPrefs.SetInt("playerDEFnow", PlayerPrefs.GetInt("playerDEFnow") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerHPnow", PlayerPrefs.GetInt("playerHPnow") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerHPMax", PlayerPrefs.GetInt("playerHPMax") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerMPnow", PlayerPrefs.GetInt("playerMPnow") + selectedItem.item.mp);
                PlayerPrefs.SetInt("playerMPMax", PlayerPrefs.GetInt("playerMPMax") + selectedItem.item.mp);
                PlayerPrefs.SetInt("playerattack", PlayerPrefs.GetInt("playerattack") + selectedItem.item.att);
                PlayerPrefs.SetInt("playersol", PlayerPrefs.GetInt("playersol") + selectedItem.item.sol);
                PlayerPrefs.SetInt("playerrad", PlayerPrefs.GetInt("playerrad") + selectedItem.item.rad);
                PlayerPrefs.SetInt("playerenx", PlayerPrefs.GetInt("playerenx") + selectedItem.item.enx);
                PlayerPrefs.SetInt("playerchr", PlayerPrefs.GetInt("playerchr") + selectedItem.item.chr);
                Destroy(selectedItem.gameObject);
            }
            else
            {
                Debug.Log("Has equipped weapon, taking off weapon");
                AddItem(itemInSlot.item); // adds the equipped item into inventory
                //PlayerPrefs.SetInt("playerDEFnow", PlayerPrefs.GetInt("playerDEFnow") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerHPnow", PlayerPrefs.GetInt("playerHPnow") - itemInSlot.item.hp);
                PlayerPrefs.SetInt("playerHPMax", PlayerPrefs.GetInt("playerHPMax") - itemInSlot.item.hp);
                PlayerPrefs.SetInt("playerMPnow", PlayerPrefs.GetInt("playerMPnow") - itemInSlot.item.mp);
                PlayerPrefs.SetInt("playerMPMax", PlayerPrefs.GetInt("playerMPMax") - itemInSlot.item.mp);
                PlayerPrefs.SetInt("playerattack", PlayerPrefs.GetInt("playerattack") - itemInSlot.item.att);
                PlayerPrefs.SetInt("playersol", PlayerPrefs.GetInt("playersol") - itemInSlot.item.sol);
                PlayerPrefs.SetInt("playerrad", PlayerPrefs.GetInt("playerrad") - itemInSlot.item.rad);
                PlayerPrefs.SetInt("playerenx", PlayerPrefs.GetInt("playerenx") - itemInSlot.item.enx);
                PlayerPrefs.SetInt("playerchr", PlayerPrefs.GetInt("playerchr") - itemInSlot.item.chr);
                Destroy(selectedItem.gameObject); // deletes the new item from inventory
                Destroy(itemInSlot.gameObject); // deletes the item in the weapon slot
                SpawnNewItem(selectedItem.item, slot); //adds the new item into the weapon slot
                //PlayerPrefs.SetInt("playerDEFnow", PlayerPrefs.GetInt("playerDEFnow") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerHPnow", PlayerPrefs.GetInt("playerHPnow") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerHPMax", PlayerPrefs.GetInt("playerHPMax") + selectedItem.item.hp);
                PlayerPrefs.SetInt("playerMPnow", PlayerPrefs.GetInt("playerMPnow") + selectedItem.item.mp);
                PlayerPrefs.SetInt("playerMPMax", PlayerPrefs.GetInt("playerMPMax") + selectedItem.item.mp);
                PlayerPrefs.SetInt("playerattack", PlayerPrefs.GetInt("playerattack") + selectedItem.item.att);
                PlayerPrefs.SetInt("playersol", PlayerPrefs.GetInt("playersol") + selectedItem.item.sol);
                PlayerPrefs.SetInt("playerrad", PlayerPrefs.GetInt("playerrad") + selectedItem.item.rad);
                PlayerPrefs.SetInt("playerenx", PlayerPrefs.GetInt("playerenx") + selectedItem.item.enx);
                PlayerPrefs.SetInt("playerchr", PlayerPrefs.GetInt("playerchr") + selectedItem.item.chr);
            }
        }
    }
    public void UnequipItem()
    {
        selectedItem.count--;
        selectedItem.RefreshCount();
        AddItem(selectedItem.item);
        //PlayerPrefs.SetInt("playerDEFnow", PlayerPrefs.GetInt("playerDEFnow") + selectedItem.item.hp);
        PlayerPrefs.SetInt("playerHPnow", PlayerPrefs.GetInt("playerHPnow") - selectedItem.item.hp);
        PlayerPrefs.SetInt("playerHPMax", PlayerPrefs.GetInt("playerHPMax") - selectedItem.item.hp);
        PlayerPrefs.SetInt("playerMPnow", PlayerPrefs.GetInt("playerMPnow") - selectedItem.item.mp);
        PlayerPrefs.SetInt("playerMPMax", PlayerPrefs.GetInt("playerMPMax") - selectedItem.item.mp);
        PlayerPrefs.SetInt("playerattack", PlayerPrefs.GetInt("playerattack") - selectedItem.item.att);
        PlayerPrefs.SetInt("playersol", PlayerPrefs.GetInt("playersol") - selectedItem.item.sol);
        PlayerPrefs.SetInt("playerrad", PlayerPrefs.GetInt("playerrad") - selectedItem.item.rad);
        PlayerPrefs.SetInt("playerenx", PlayerPrefs.GetInt("playerenx") - selectedItem.item.enx);
        PlayerPrefs.SetInt("playerchr", PlayerPrefs.GetInt("playerchr") - selectedItem.item.chr);
        Destroy(selectedItem.gameObject);
        selectedItem.ItemUsed();

    }

}
