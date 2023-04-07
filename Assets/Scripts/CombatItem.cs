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
    public GameObject emptyUI;

    public int itemcount;
    public int id;
    public int count;
    public void Start()
    {
        itemcount = 0;
        LoadConsumables();
        
    }
    public void LoadConsumables() // checks all the consumable items, and instantiates it into the scene
    {
        for (int i = 0; i <= 18; i++) // loads in the saved inventory with the playerprefs
        {
            int id = PlayerPrefs.GetInt("InventorySlotScene" + i + "ID", -1);
            int count = PlayerPrefs.GetInt("InventorySlotScene" + i + "Count", 0);
            Items item = null;
            if (itemcount > 0) // remove the Empty text in the item list if there are items
            {
                emptyUI.SetActive(false);
            }

            for (int j = 0; j < itemList.items.Length; j++)
            {
                if (itemList.items[j].id == id)
                {
                    
                    item = itemList.items[j];
                    //Debug.Log(item.itemName + " " + count);
                }
                if (item != null)
                {
                    break;
                }
            }
            if (item != null && item.type == ItemType.Consumable) //check if its a consumable
            {
                itemcount++;
                Debug.Log("itemslot id in button creation loop is " + i);
                //if its a consumable, instantiate the prefab "Item" as a child
                GameObject buttonGO = Instantiate(itemButton, transform);
                Text buttonText = buttonGO.GetComponentInChildren<Text>();
                int temp = i;
                buttonText.text = item.name + " x" + count;
                buttonGO.GetComponent<Button>().onClick.AddListener(() => UseItem(buttonGO, count, item, temp)); // need to add this second part to another method, this should open an UI
                

            }


        }
    }

    public void UseItem(GameObject buttonGO, int count, Items item, int ident) //consumption of the item
    {
        if(battleManager.state == BattleState.PLAYERTURN )
        {
            Debug.Log("supposedly the item slot id "+ ident);
            StartCoroutine(UpdateButtonCount(buttonGO, count - 1, item));
            if (item.consumeType == ConsumableType.HP) // checks what type of consumable it is, hp, mp, buff, etc
            {
                PlayerPrefs.SetString("HPMP", "HP");
                PlayerPrefs.SetString("combatItemName", item.itemName);
                PlayerPrefs.SetInt("combatItemEffect", item.hp);
                battleManager.UseItem(ident); // will need one for UseItemHP
            }
            else if (item.consumeType == ConsumableType.MP)
            {
                PlayerPrefs.SetString("HPMP", "MP");
                PlayerPrefs.SetString("combatItemName", item.itemName);
                PlayerPrefs.SetInt("combatItemEffect", item.mp);
                battleManager.UseItem(ident);// will need one for UseItemMP
            }

        }

    }

    public IEnumerator UpdateButtonCount(GameObject buttonGO, int newCount, Items item) //updates the item count and remove button if theres no more
    {
        //Debug.Log("Update button ran");
        count = newCount;
        Text buttonText = buttonGO.GetComponentInChildren<Text>();
        buttonText.text = item.itemName + " x" + count;

        Button button = buttonGO.GetComponent<Button>(); // this section of code ensures only one onClick listener is active and removes old ones
        button.onClick.RemoveAllListeners();
        button.GetComponent<Button>().onClick.AddListener(() => UseItem(buttonGO, count, item, id));
        if (count <= 0)
        {
            Destroy(buttonGO);
            itemcount--;
            if(itemcount == 0)
            {
                emptyUI.SetActive(true);
            }    
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
