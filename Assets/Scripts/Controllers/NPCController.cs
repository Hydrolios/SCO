using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialogue dialogue;
    [SerializeField] Dialogue dialogue2;
    [SerializeField] Dialogue dialogue3;

    public HealthBar hpbar;
    public ManaBar mpbar;
    public Player playerRef;

    //some stuff for the ability of giving item upon interaction (mainly for things like chest)
    public InventoryManager inventoryManager;
    public ItemList itemList;
    public int item_id; //item to be given or rewarded
    public int chest_id; // each chest has a unique id
    public int npc_id;

    //bool conditions to seperate different instances for different uses
    public string sceneToLoad;
    public bool repeatDiag;
    public bool diagV2;
    public bool shopKeeper;
    public bool expgiver;
    public bool rbdreport;
    public bool elder;
    public bool soldier;
    public bool sleep;
    public bool item;
    public bool item_inf;
    public bool item_battle;
    public bool battle;
    public bool sceneChangeReq; // check if a scene change is required 
    public Vector2 playerPosition;
    public VectorValue playerStorage;
    public void Interact()
    {
        playerRef.StopSpeed();
        if(item_battle)
        {
            InventoryManager inventoryManager = FindObjectOfType<InventoryManager>(); // gets the inventoryManager in the scene
            inventoryManager.SaveInventoryScene();
            PlayerPrefs.SetInt("BattleReward", item_id);

        }

        if(battle)
        {
            InventoryManager inventoryManager = FindObjectOfType<InventoryManager>(); // gets the inventoryManager in the scene
            inventoryManager.SaveInventoryScene();
        }


        if (elder)
        {
            if (PlayerPrefs.GetInt("rbdreport") != 0) //dialogue for reporting back to elder after shade is killed
            {
                StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue3, sceneToLoad, sceneChangeReq, shopKeeper, expgiver));
            }
            else if (PlayerPrefs.GetInt("shadeKilled") != 0) //dialogue for coming back to elder
            {
                StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue2, sceneToLoad, sceneChangeReq, shopKeeper, expgiver));
                rbdreport = true;
                PlayerPrefs.SetInt("rbdreport", rbdreport ? 1 : 0);
            }
            else
            {
                StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue, sceneToLoad, sceneChangeReq, shopKeeper, expgiver));
            }
        }
        else if (soldier) // special case for story progression, some soldiers require a check to unlock other dialogue options
        {
            if (rbdreport)
            {
                playerStorage.initialValue = playerPosition;
                StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue, sceneToLoad, sceneChangeReq, shopKeeper, expgiver));

                Debug.Log("rbdreported");
            }
            else
            {
                StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue, sceneToLoad, sceneChangeReq, shopKeeper, expgiver));
            }

        }
        else if (item && PlayerPrefs.HasKey("ChestOpenedID" + chest_id)) // if its an item and has key, it signifies it is opened
        {

            StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue2, sceneToLoad, sceneChangeReq, shopKeeper, expgiver));

        }
        else if (item) // for giving an item, it will set it so chest is opened as a prefab afterwards voiding any more rewards
        {
            PlayerPrefs.SetInt("ChestOpenedID" + chest_id, 1);
            ReceiveItem(item_id);
            StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue, sceneToLoad, sceneChangeReq, shopKeeper, expgiver));

        }
        else if (item_inf)
        {
            ReceiveItem(item_id);
            StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue, sceneToLoad, sceneChangeReq, shopKeeper, expgiver));

        }
        else
        {
            StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue, sceneToLoad, sceneChangeReq, shopKeeper, expgiver));
            if(sleep)
            {
                PlayerPrefs.SetInt("playerHPnow", PlayerPrefs.GetInt("playerHPMax"));
                PlayerPrefs.SetInt("playerMPnow", PlayerPrefs.GetInt("playerMPMax"));
                hpbar.SetHealth();
                mpbar.SetHealth();
            }

        }
    }

    public void ReceiveItem(int id) //item ID is in refernce to the itemstoreceive set up by me, 0 is the first item, 1 is the 2nd... etc
    {
        bool result = inventoryManager.AddItem(itemList.items[id]);
        if (result == true)
        {
            Debug.Log("item gave");
        }
        else
        {
            Debug.Log("item not added");
        }
    }

 

}
