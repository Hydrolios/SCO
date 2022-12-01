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
    public bool textReq; //for trigger text dialogue in spawning in a scene
    static int healthPool;
    public int currentHealth;
    static int manaPool;
    public int currentMana;
    public int level;
    public int exp;
    public HealthBar healthBar;
    public ManaBar manaBar;

    void Start()
    {
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
        level = PlayerPrefs.GetInt("playerlevel");
        loadGame = PlayerPrefs.GetInt("load") != 0;
        newGameCheck = PlayerPrefs.GetInt("newgame") != 0;
        Debug.Log("Load game boolean check: " + loadGame);
        if (newGameCheck) // New game check to put player in a fixed position at the first scene only for new game
        {
            Debug.Log("Position was from a new game");
            transform.position = new Vector2(-2.5f, -1.5f);
            newGameCheck = false;
            PlayerPrefs.SetInt("newgame", (newGameCheck ? 1 : 0));
            currentHealth = healthPool;
            currentMana = manaPool;
        }
        else if (loadGame == false)
        {
            transform.position = startingPosition.initialValue;
            Debug.Log("Position was not from a load");
        }   
        else
        {
            Debug.Log("Position was from a load");
            xPos = PlayerPrefs.GetFloat("x");
            yPos = PlayerPrefs.GetFloat("y");
            currentHealth = PlayerPrefs.GetInt("savedHP");
            currentMana = PlayerPrefs.GetInt("savedMP");
            PlayerPrefs.SetInt("playerHPnow", currentHealth);
            PlayerPrefs.SetInt("playerMPnow", currentMana);
            healthBar.SetHealth();
            Vector2 loadPos = new Vector2(xPos, yPos);
            transform.position = loadPos;
            loadGame = false;
            PlayerPrefs.SetInt("load", (loadGame ? 1 : 0));

        }
        if(textReq)
        {
            StartCoroutine(DialogueManager.Instance.ShowDialogueV2(dialogue, textReq));
            textReq = false;
            PlayerPrefs.SetInt("spawnText", (loadGame ? 1 : 0));
        }

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) //debugger key for console
        {
            Debug.Log("Debug Log:");
            Debug.Log("player hp now:" + PlayerPrefs.GetInt("playerHPnow"));
            Debug.Log("player hp max:" + PlayerPrefs.GetInt("playerHPMax"));
            Debug.Log("player level:" + PlayerPrefs.GetInt("playerlevel"));

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
