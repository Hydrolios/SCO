using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscardAttach : MonoBehaviour
{
    public Button discardButton;
    public Button closeDiscardButton;
    public GameObject inventoryManager;
    // Start is called before the first frame update
    void Start()
    {
        //finds the inventory manager in scene
        inventoryManager = GameObject.Find("InventoryManager");

        //check if the button and inventory manager exists
        if (inventoryManager != null && discardButton != null && closeDiscardButton != null)
        {
            // Find the DiscardItem method in the InventoryManager script and attach it to the button click event.
            InventoryManager inventoryManagerScript = inventoryManager.GetComponent<InventoryManager>();
            if (inventoryManagerScript != null)
            {
                discardButton.onClick.AddListener(() => inventoryManagerScript.DiscardItem());
                closeDiscardButton.onClick.AddListener(() => inventoryManagerScript.DontDiscardItem());
            }
            else
            {
                Debug.LogError("InventoryManager script not found on the InventoryManager GameObject.");
            }
        }
    }

}
