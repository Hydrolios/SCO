using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public Dialogue dialogue;
    private BoxCollider2D boxCollider;

    public Rigidbody2D rb_player;
    public float moveSpeed = 5f;
    public Animator animator;
    Vector2 movement;

    public LayerMask interactableLayer;

    public VectorValue startingPosition;
    public float xPos;
    public float yPos;
    public float interactRad;
    public bool newGameCheck;
    public bool loadGame;
    public bool sceneTransitioned = false;
    public bool openedUIInven = false;
    public bool openedUIPause = false;
    public bool openedUIGO = false;
    public bool openedUIShop = false;
    public bool openedUIStats = false;
    public bool openedDialog = false;
    public bool textReq; //for trigger text dialogue in spawning in a scene
    static int healthPool;
    public int currentHealth;
    static int manaPool;
    public int currentMana;
    public int level;
    public int money;
    public int exp;
    public HealthBar healthBar;
    public ManaBar manaBar;

    void Start()
    {
        Debug.Log("Player executes");
        ResumeSpeed();
        boxCollider = GetComponent<BoxCollider2D>();
        healthPool = PlayerPrefs.GetInt("playerHPMax");
        manaPool = PlayerPrefs.GetInt("playerMPMax");
        currentHealth = PlayerPrefs.GetInt("playerHPnow");
        currentMana = PlayerPrefs.GetInt("playerMPnow");
        healthBar.SetMaxHealth(healthPool);
        manaBar.SetMaxMana(manaPool);
        textReq = PlayerPrefs.GetInt("spawnText") != 0;
        exp = PlayerPrefs.GetInt("exp");
        money = PlayerPrefs.GetInt("currentcash");
        level = PlayerPrefs.GetInt("playerlevel");
        loadGame = PlayerPrefs.GetInt("load") != 0;
        newGameCheck = PlayerPrefs.GetInt("newgame") != 0;
        //Debug.Log("Load game boolean check: " + loadGame);
        if (newGameCheck) // New game check to put player in a fixed position at the first scene only for new game
        {
            //Debug.Log("Position was from a new game");
            transform.position = new Vector2(-2.5f, -1.5f);
            newGameCheck = false;
            PlayerPrefs.SetInt("newgame", (newGameCheck ? 1 : 0));
            currentHealth = healthPool;
            currentMana = manaPool;
        }
        else if (loadGame == false)
        {
            transform.position = startingPosition.initialValue;
            InventoryManager inventoryManager = FindObjectOfType<InventoryManager>(); // gets the inventoryManager in the scene
            inventoryManager.LoadInventoryScene();
            //Debug.Log("Position was not from a load");
        }   
        else // player is spawning from a save file
        {
            Debug.Log("Position was from a load");
            xPos = PlayerPrefs.GetFloat("x");
            yPos = PlayerPrefs.GetFloat("y");
            
            healthBar.SetHealth();
            Vector2 loadPos = new Vector2(xPos, yPos);
            transform.position = loadPos;
            loadGame = false;
            //Player stats
            PlayerPrefs.SetInt("playerHPMax", PlayerPrefs.GetInt("savedMaxHP"));
            PlayerPrefs.SetInt("playerMPMax", PlayerPrefs.GetInt("savedMaxMP"));
            currentHealth = PlayerPrefs.GetInt("savedHP");
            currentMana = PlayerPrefs.GetInt("savedMP");
            PlayerPrefs.SetInt("playerHPnow", currentHealth);
            PlayerPrefs.SetInt("playerMPnow", currentMana);
            PlayerPrefs.SetInt("playerattack", PlayerPrefs.GetInt("Savedplayerattack"));
            PlayerPrefs.SetInt("playersol", PlayerPrefs.GetInt("Savedplayersol"));
            PlayerPrefs.SetInt("playerrad", PlayerPrefs.GetInt("Savedplayerrad"));
            PlayerPrefs.SetInt("playerenx", PlayerPrefs.GetInt("Savedplayerenx"));
            PlayerPrefs.SetInt("playerchr", PlayerPrefs.GetInt("Savedplayerchr"));
            PlayerPrefs.SetInt("currentcash", PlayerPrefs.GetInt("cash"));
            PlayerPrefs.SetInt("exp", PlayerPrefs.GetInt("saveEXP"));
            PlayerPrefs.SetInt("playerlevel", PlayerPrefs.GetInt("saveplevel"));
            PlayerPrefs.SetInt("exptolevel", PlayerPrefs.GetInt("saveEXPlevel"));
            //Players story progression
            PlayerPrefs.SetInt("shadeKilled", PlayerPrefs.GetInt("shadeSave", 0));
            PlayerPrefs.SetInt("load", (loadGame ? 1 : 0));
            InventoryManager inventoryManager = FindObjectOfType<InventoryManager>(); // gets the inventoryManager in the scene
            inventoryManager.LoadInventory();
            Time.timeScale = 1f;

        }
        if(textReq) // text for loading into a scene
        {
            StartCoroutine(DialogueManager.Instance.ShowDialogueV2(dialogue, textReq));
            textReq = false;
            PlayerPrefs.SetInt("spawnText", (loadGame ? 1 : 0));
        }
        if(PlayerPrefs.HasKey("BattleReward"))
        {
            InventoryManager inventoryManager = FindObjectOfType<InventoryManager>(); // gets the inventoryManager in the scene
            inventoryManager.Reward();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) //debugger key for console
        {
            //ItemList itemList = FindObjectOfType<ItemList>();
            Debug.Log("Debug Log:");
            Debug.Log(PlayerPrefs.GetInt("shadeKilled"));
            PlayerPrefs.SetInt("shadeKilled", 0);
            /*for (int i = 0; i <= 18; i++) // loads in the saved inventory with the playerprefs
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
                    }
                    if (item != null)
                    {
                        break;
                    }
                }
                if (item != null && item.equipType == EquipmentType.Weapon) //check if its a consumable
                {
                    Debug.Log("Position of item is in " + i);


                }


            }*/
            //TakeDamage(2);
            //Debug.Log("player hp now:" + PlayerPrefs.GetInt("playerHPnow"));
            //Debug.Log("player hp max:" + PlayerPrefs.GetInt("playerHPMax"));
            //Debug.Log("player level:" + PlayerPrefs.GetInt("playerlevel"));

        }
        else if (Input.anyKey) //used to adjust size of interactable radius in case player opens a UI next to another interactable, this is to prevent overlap
        {
            interactRad = PlayerPrefs.GetFloat("interact_range");
        }
       
        

    }

    public void TakeDamage(int damage) // code to reduce health points when a damage trigger occurs
    {
        currentHealth -= damage;
        PlayerPrefs.SetInt("playerHPnow", currentHealth);
        healthBar.SetHealth();
    }

    public void HealDamage(int damage) // code to heal health points for times like using a consumable from inventory
    {
        
        currentHealth += damage;
        if (currentHealth > PlayerPrefs.GetInt("playerHPMax"))
        {
            int placeholder;
            placeholder = currentHealth - PlayerPrefs.GetInt("playerHPMax");
            currentHealth -= placeholder;
        }
        PlayerPrefs.SetInt("playerHPnow", currentHealth);
        healthBar.SetHealth();
    }

    public void HealMana (int mana)
    {
        currentMana += mana;
        if (currentMana > PlayerPrefs.GetInt("playerMPMax"))
        {
            int placeholder;
            placeholder = currentMana - PlayerPrefs.GetInt("playerMPMax");
            currentMana -= placeholder;
        }
        PlayerPrefs.SetInt("playerMPnow", currentMana);
        manaBar.SetHealth();

    }
    public void StopSpeed() // code to pause player speed
    {
        moveSpeed = 0f;
    }

    public void ResumeSpeed() // code to resume player speed after pausing it
    {
        moveSpeed = 5f;
    }



    public void HandleUpdate() // code to move the player in 1 of eight directions, and adjust the speed of the player
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }


    }
    void FixedUpdate() // code to make sure movement is consistent regardless of user PC specs
    {


        rb_player.MovePosition(rb_player.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);



    }

    void Interact() //Function to interact with things
    {
        var facingDir = new Vector3(animator.GetFloat("Horizontal"), animator.GetFloat("Vertical"));
        var interactPos = transform.position + facingDir;

        var collider = Physics2D.OverlapCircle(interactPos, interactRad, interactableLayer);
        if (collider != null) //checks if hitboxes are overlapping to see if player is in range to interact with said thing
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }
}
