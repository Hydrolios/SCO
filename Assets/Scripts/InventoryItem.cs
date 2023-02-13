using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IPointerExitHandler
{
    public Items item;
    public Image image;
    public Text countText;
    public GameObject useMenu;
    public GameObject equipMenu;

    private Button button;

    public int count = 1;

    void Start()  // On start, find the menu required for item interactions
    {
        useMenu = GameObject.Find("ConsumableMenu");
        equipMenu = GameObject.Find("EquipMenu");
        button = GetComponent<Button>();
        button.onClick.AddListener(HandleClick);

        //code to detect mouse hovering and calls upon OnPointerExit when the user stops hovering
        EventTrigger eventTrigger = button.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener((data) => { OnPointerExit((PointerEventData)data); });
        eventTrigger.triggers.Add(entry);
    }

    void Update() // Checks to see if the gameObject is deleted by DiscardItem in InventoryManager.cs, if the gameObject is deleted, move the Menu off the screen
    {
        if (!gameObject)
        {
            useMenu.transform.position = new Vector2(1000, 0);
            equipMenu.transform.position = new Vector2(1000, 0);
        }
    }

    void HandleClick() // On click of an item in the inventory, decide whether it is a consumable or equipment
    {
        InventoryItem itemInSlot = GetComponentInChildren<InventoryItem>(); // gets the component thats a child of the gameObject clicked?
        Items item = itemInSlot.item; // this is the item in the designated inventory slot
        if (item.type == ItemType.Equipment)
        {
            equipMenu.SetActive(true);
            equipMenu.transform.position = new Vector2(Input.mousePosition.x + 60, Input.mousePosition.y); //show display at mouse
        }
        else if (item.type == ItemType.Consumable)
        {
            useMenu.SetActive(true);
            useMenu.transform.position = new Vector2(Input.mousePosition.x + 60, Input.mousePosition.y); //show display at mouse
        }

        // Sends the information about the selected slot to Inventory Manager
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>(); 
        InventorySlot inventorySlot = GetComponentInParent<InventorySlot>();
        inventoryManager.ClickedSlot(inventorySlot.gameObject);
    }

    public void OnPointerExit(PointerEventData eventData) // hide the menu when cursor is off the button
    {
        StartCoroutine(HideMenus());
    }

    IEnumerator HideMenus() // hide the menu after a fixed amount of time if the user hovers off it
    {
        float startTime = Time.realtimeSinceStartup;
        float waitTime = 1f;

        while (Time.realtimeSinceStartup < startTime + waitTime) // need to use realtime as time scale is 0 while menuing 
        {
            yield return null;
        }
        equipMenu.transform.position = new Vector2(1000,0);
        useMenu.transform.position = new Vector2(1000,0);

    }
    public void ItemUsed() // method to immediately hide menu after using/equiping/discarding an item
    {
        equipMenu.transform.position = new Vector2(1000, 0);
        useMenu.transform.position = new Vector2(1000, 0);

    }
    public void InitializeItem(Items newItem) 
    {
        image = GetComponentInChildren<Image>();
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
    }

    public void RefreshCount() // add up items if it is stackable
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void Consume(Player player)
    {

        InventoryItem itemInSlot = GetComponentInChildren<InventoryItem>(); // gets the component thats a child of the gameObject clicked?
        Items item = itemInSlot.item; // this is the item in the designated inventory slot
        player.HealDamage(item.hp);
    }


}
