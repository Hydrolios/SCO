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


    void Start()
    {
        Debug.Log("one");
        LoadInventoryScene();
    }
    /*
    private void OnDestroy()
    {
        Debug.Log("two");
        SaveInventoryScene();
    }
    */
    public void ClickedSlot(GameObject slot) // Gets the selected inventory slot that was clicked
    {
        selectedSlot = slot.GetComponent<InventorySlot>();
        selectedItem = selectedSlot.GetComponentInChildren<InventoryItem>();
        Debug.Log(selectedItem.item);


    }
    public bool AddItem(Items item) // Adds an item to the inventory but checks for a place to insert the item
    {
        for (int i = 0; i < inventorySlots.Length; i++) //finds the slot with same item to stack
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>(); //checks if equip slot is occupied
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStack && itemInSlot.item.stackable == true)
            {
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
        for (int i = 0; i < inventorySlots.Length; i++) //wipes out the previously saved first
        {
            PlayerPrefs.DeleteKey("InventorySlot" + i + "ID");
            PlayerPrefs.DeleteKey("InventorySlot" + i + "Count");
        }
        for (int i = 0; i < inventorySlots.Length; i++) // then saves the new iteration of the inventory
        {
            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                Debug.Log("item was saved");
                PlayerPrefs.SetInt("InventorySlot" + i + "ID", itemInSlot.item.id); 
                PlayerPrefs.SetInt("InventorySlot" + i + "Count", itemInSlot.count);
                
            }

        }
        //need to remember to include the individual saving of the equipment slots, hatEquip...wepEquip
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

            int id = PlayerPrefs.GetInt("InventorySlot" + i + "ID");
            int count = PlayerPrefs.GetInt("InventorySlot" + i + "Count");
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
    
    public void SaveInventoryScene() // saves inventory info by cycling through the inventory and saving item id + count
    {
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
            Debug.Log(i);
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
            }
            else
            {
                AddItem(itemInSlot.item);
                Destroy(itemInSlot.item);
                SpawnNewItem(selectedItem.item, slot);
            }
        }

        else if (selectedItem.item.equipType == EquipmentType.Shirt)
        {
            InventorySlot slot = shirtEquip;
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>(); //checks if equip slot is occupied
            if (itemInSlot == null)
            {
                SpawnNewItem(selectedItem.item, slot);
            }
            else
            {
                AddItem(itemInSlot.item);
                Destroy(itemInSlot.item);
                SpawnNewItem(selectedItem.item, slot);
            }
        }

        else if (selectedItem.item.equipType == EquipmentType.Pants)
        {
            InventorySlot slot = pantsEquip;
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>(); //checks if equip slot is occupied
            if(itemInSlot == null)
            {
                SpawnNewItem(selectedItem.item, slot);
            }
            else
            {
                AddItem(itemInSlot.item);
                Destroy(itemInSlot.item);
                SpawnNewItem(selectedItem.item, slot);
            }

        }

        else if (selectedItem.item.equipType == EquipmentType.Weapon)
        {
            InventorySlot slot = wepEquip;
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>(); //checks if equip slot is occupied
            if (itemInSlot == null)
            {
                SpawnNewItem(selectedItem.item, slot);
            }
            else
            {
                AddItem(itemInSlot.item);
                Destroy(itemInSlot.item);
                SpawnNewItem(selectedItem.item, slot);
            }
        }



    }

}
