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

        //need to remember to include the individual saving of the equipment slots, hatEquip...wepEquip
        InventoryItem hatItem = hatEquip.GetComponentInChildren<InventoryItem>();
        if (hatItem != null)
        {
            //Debug.Log("A");
            PlayerPrefs.SetInt("InventorySlotA", hatItem.item.id);
        }

        InventoryItem shirtItem = shirtEquip.GetComponentInChildren<InventoryItem>();
        if (shirtItem != null)
        {
            //Debug.Log("B");
            PlayerPrefs.SetInt("InventorySlotB", shirtItem.item.id);
        }

        InventoryItem pantsItem = pantsEquip.GetComponentInChildren<InventoryItem>();
        if (pantsItem != null)
        {
            //Debug.Log("C");
            PlayerPrefs.SetInt("InventorySlotC", pantsItem.item.id);
        }

        InventoryItem weaponItem = wepEquip.GetComponentInChildren<InventoryItem>();
        if (weaponItem != null)
        {
            //Debug.Log("D");
            PlayerPrefs.SetInt("InventorySlotD", weaponItem.item.id);
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

        
        InventoryItem hatItem = hatEquip.GetComponentInChildren<InventoryItem>();
        if (hatItem != null)
        {
            //Debug.Log("A");
            PlayerPrefs.SetInt("InventorySlotA", hatItem.item.id);
        }

        InventoryItem shirtItem = shirtEquip.GetComponentInChildren<InventoryItem>();
        if (shirtItem != null)
        {
            //Debug.Log("B");
            PlayerPrefs.SetInt("InventorySlotB", shirtItem.item.id);
        }

        InventoryItem pantsItem = pantsEquip.GetComponentInChildren<InventoryItem>();
        if (pantsItem != null)
        {
            //Debug.Log("C");
            PlayerPrefs.SetInt("InventorySlotC", pantsItem.item.id);
        }

        InventoryItem weaponItem = wepEquip.GetComponentInChildren<InventoryItem>();
        if (weaponItem != null)
        {
            //Debug.Log("D");
            PlayerPrefs.SetInt("InventorySlotD", weaponItem.item.id);
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

    public void DiscardItem() // Discards one item
    {
        selectedItem.count--;
        if(selectedItem.count <=0)
        {
            Destroy(selectedItem.gameObject);
        }
        else
        {
            selectedItem.RefreshCount();
        }
        selectedItem.ItemUsed();

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
                PlayerPrefs.SetInt("playerattack", PlayerPrefs.GetInt("playerattack") - itemInSlot.item.att);
                PlayerPrefs.SetInt("playersol", PlayerPrefs.GetInt("playersol") - itemInSlot.item.sol);
                PlayerPrefs.SetInt("playerrad", PlayerPrefs.GetInt("playerrad") - itemInSlot.item.rad);
                PlayerPrefs.SetInt("playerenx", PlayerPrefs.GetInt("playerenx") - itemInSlot.item.enx);
                PlayerPrefs.SetInt("playerchr", PlayerPrefs.GetInt("playerchr") - itemInSlot.item.chr);
                Destroy(selectedItem.gameObject);
                SpawnNewItem(selectedItem.item, slot);
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
                PlayerPrefs.SetInt("playerattack", PlayerPrefs.GetInt("playerattack") - itemInSlot.item.att);
                PlayerPrefs.SetInt("playersol", PlayerPrefs.GetInt("playersol") - itemInSlot.item.sol);
                PlayerPrefs.SetInt("playerrad", PlayerPrefs.GetInt("playerrad") - itemInSlot.item.rad);
                PlayerPrefs.SetInt("playerenx", PlayerPrefs.GetInt("playerenx") - itemInSlot.item.enx);
                PlayerPrefs.SetInt("playerchr", PlayerPrefs.GetInt("playerchr") - itemInSlot.item.chr);
                Destroy(selectedItem.gameObject);
                SpawnNewItem(selectedItem.item, slot);
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
                PlayerPrefs.SetInt("playerattack", PlayerPrefs.GetInt("playerattack") - itemInSlot.item.att);
                PlayerPrefs.SetInt("playersol", PlayerPrefs.GetInt("playersol") - itemInSlot.item.sol);
                PlayerPrefs.SetInt("playerrad", PlayerPrefs.GetInt("playerrad") - itemInSlot.item.rad);
                PlayerPrefs.SetInt("playerenx", PlayerPrefs.GetInt("playerenx") - itemInSlot.item.enx);
                PlayerPrefs.SetInt("playerchr", PlayerPrefs.GetInt("playerchr") - itemInSlot.item.chr);
                Destroy(selectedItem.gameObject);
                SpawnNewItem(selectedItem.item, slot);
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
            if (itemInSlot == null)
            {
                SpawnNewItem(selectedItem.item, slot);
                

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
                PlayerPrefs.SetInt("playerattack", PlayerPrefs.GetInt("playerattack") - itemInSlot.item.att);
                PlayerPrefs.SetInt("playersol", PlayerPrefs.GetInt("playersol") - itemInSlot.item.sol);
                PlayerPrefs.SetInt("playerrad", PlayerPrefs.GetInt("playerrad") - itemInSlot.item.rad);
                PlayerPrefs.SetInt("playerenx", PlayerPrefs.GetInt("playerenx") - itemInSlot.item.enx);
                PlayerPrefs.SetInt("playerchr", PlayerPrefs.GetInt("playerchr") - itemInSlot.item.chr);
                Destroy(selectedItem.gameObject);
                SpawnNewItem(selectedItem.item, slot);
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
        PlayerPrefs.SetInt("playerattack", PlayerPrefs.GetInt("playerattack") - selectedItem.item.att);
        PlayerPrefs.SetInt("playersol", PlayerPrefs.GetInt("playersol") - selectedItem.item.sol);
        PlayerPrefs.SetInt("playerrad", PlayerPrefs.GetInt("playerrad") - selectedItem.item.rad);
        PlayerPrefs.SetInt("playerenx", PlayerPrefs.GetInt("playerenx") - selectedItem.item.enx);
        PlayerPrefs.SetInt("playerchr", PlayerPrefs.GetInt("playerchr") - selectedItem.item.chr);
        Destroy(selectedItem.gameObject);
        selectedItem.ItemUsed();

    }

}
