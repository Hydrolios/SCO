using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatItem : MonoBehaviour
{
    public ItemList itemList;
    public GameObject itemButton;
    public GameObject buttonGO;
    public BattleManager battleManager;
    public GameObject UseUI;

    public int id;
    public int count;
    public void Start()
    {
        LoadConsumables();
        
    }
    public void LoadConsumables() // checks all the consumable items, and instantiates it into the scene
    {
        for (int i = 0; i < 15; i++) // loads in the saved inventory with the playerprefs
        {
            int id = PlayerPrefs.GetInt("InventorySlotScene" + i + "ID", -1);
            int count = PlayerPrefs.GetInt("InventorySlotScene" + i + "Count", 0);
            Items item = null;

            for (int j = 0; j < itemList.items.Length; j++)
            {
                if (itemList.items[j].id == id)
                {
                    
                    item = itemList.items[j];
                    //Debug.Log(item.itemName + " " + count);
                    Debug.Log("testt");
                }
                if (item != null)
                {
                    break;
                }
            }
            if (item != null && item.type == ItemType.Consumable) //check if its a consumable
            {
                //if its a consumable, instantiate the prefab "Item" as a child
                GameObject buttonGO = Instantiate(itemButton, transform);
                Text buttonText = buttonGO.GetComponentInChildren<Text>();
                buttonText.text = item.name + " x" + count;
                buttonGO.GetComponent<Button>().onClick.AddListener(() => UseItem(buttonGO, count, item, id)); // need to add this second part to another method, this should open an UI
                

            }


        }
    }

    public void UseItem(GameObject buttonGO, int count, Items item, int id) //consumption of the item
    {
        Debug.Log("UseItem ran");
        StartCoroutine(UpdateButtonCount(buttonGO, count - 1, item));
        if(item.consumeType == ConsumableType.HP) // checks what type of consumable it is, hp, mp, buff, etc
        {
            PlayerPrefs.SetString("combatItemName", item.itemName);
            PlayerPrefs.SetInt("combatItemEffect", item.hp);
            battleManager.UseItem(); // will need one for UseItemHP
        }
        else if (item.consumeType == ConsumableType.MP)
        {
            PlayerPrefs.SetString("combatItemName", item.itemName);
            PlayerPrefs.SetInt("combatItemEffect", item.mp);
            battleManager.UseItem();// will need one for UseItemMP
        }
        
        
        
    }

    public IEnumerator UpdateButtonCount(GameObject buttonGO, int newCount, Items item) //updates the item count and remove button if theres no more
    {
        Debug.Log("Update button ran");
        count = newCount;
        Text buttonText = buttonGO.GetComponentInChildren<Text>();
        buttonText.text = item.itemName + " x" + count;

        Button button = buttonGO.GetComponent<Button>(); // this section of code ensures only one onClick listener is active and removes old ones
        button.onClick.RemoveAllListeners();
        button.GetComponent<Button>().onClick.AddListener(() => UseItem(buttonGO, count, item, id));
        if (count <= 0)
        {
            Destroy(buttonGO);
        }
        yield return new WaitForSeconds(0.5f);
        
    }
    /*
    public void ItemUseUI(int amount, Items item) //maybe do this later, I want to implement a confirmation like UI for "Use"
    {
        UseUI.SetActive(true);
        UseUI.transform.position = new Vector2(Input.mousePosition.x + 60, Input.mousePosition.y);
    }
    */


}
