using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { START, WIN, LOSE, PLAYERTURN, ENEMYTURN }
public class BattleManager : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject AttackListUI;
    public GameObject ItemUI;
    public GameObject playerRageAnimation;
    public GameObject playerSlashAnimation;
    public GameObject playerEleRadAnimation;
    public GameObject playerEleRadCasting;
    public GameObject playerClone;
    public GameObject enemyClone;
    public GameObject fadeIN;
    public EventCheck eventChecker;
    public LevelSys levelSys;
    public PlayerSkills skills;

    UnitStats playerUnit;
    UnitStats enemyUnit;

    public ItemList itemList;
    public int item_id;

    public Text dialogueText;
    public string sceneToLoad;
    public bool tutorial;
    public int turncounter;
    public int buffcounter;
    public int itemcount;

    public BattleINFO playerHUD;
    public BattleINFO enemyHUD;

    public GameObject player;
    public GameObject rage;
    public Vector2 playerPosition;
    public VectorValue playerStorage;
    public NPCManager npcManager;
    public Encounter npcbattling;

    public BattleState state;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    //Spawns the units onto the scene and sets the values for the HUD
    IEnumerator SetupBattle()
    {
        turncounter = 0;
        buffcounter = 0;
        playerClone = Instantiate(playerPrefab);
        playerUnit = playerClone.GetComponent<UnitStats>();

        if (playerUnit.isPlayer)
        {
            playerUnit.unitName = "Kuller";
            playerUnit.unitLevel = PlayerPrefs.GetInt("playerlevel");
            playerUnit.attack = PlayerPrefs.GetInt("playerattack");
            playerUnit.maxHP = PlayerPrefs.GetInt("playerHPMax");
            playerUnit.currentHP = PlayerPrefs.GetInt("playerHPnow");
            playerUnit.maxMP = PlayerPrefs.GetInt("playerMPMax");
            playerUnit.currentMP = PlayerPrefs.GetInt("playerMPnow");
        }
      
        enemyClone = Instantiate(enemyPrefab);
        enemyUnit = enemyClone.GetComponent<UnitStats>();

        dialogueText.text = enemyUnit.unitName + " stops you from advancing";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);
        fadeIN.SetActive(false);
        if (PlayerPrefs.GetInt("learnedrage") != 0)
        {
            rage.SetActive(true);
        }
        else
        {
            rage.SetActive(false);
        }
        state = BattleState.PLAYERTURN;
        PlayerTurn();
        
    }

    void PlayerTurn()
    {
        dialogueText.text = playerUnit.unitName + "'s turn";
    }


    public void OnAttacksButton() //selecting "attacks" open up a list UI 
    {
        AttackListUI.SetActive(true);
    }

    public void AttacksListReturn() // selecting back arrows closes list
    {
        AttackListUI.SetActive(false);
    }

    public void OnItemButton() //selecting "items" open up a list UI
    {
        ItemUI.SetActive(true);
    }

    public void ItemListReturn() // selecting back arrow closes list
    {
        ItemUI.SetActive(false);
    }


    public void AttackSkill() // basic attack [slash]
    {
        if (state != BattleState.PLAYERTURN)
            return;
        AttackListUI.SetActive(false);
        StartCoroutine(PlayerAttack());
    }
    public void AttackEleRad() // elemental radiance attack [slash]
    {
        if (state != BattleState.PLAYERTURN)
            return;
        AttackListUI.SetActive(false);
        StartCoroutine(PlayerAttackEleRad());
    }
    public void AttackRage() // buff attack [rage]
    {
        if (state != BattleState.PLAYERTURN)
            return;
        AttackListUI.SetActive(false);
        StartCoroutine(PlayerBuffing());
    }
    public bool FirstVowel(char c)
    {
        return "aeiouAEIOU".IndexOf(c) != -1;
    }
    // future attack implementation February

    public void UseItem(int ident) // uses the item selected
    {
        if (state != BattleState.PLAYERTURN)
            return;
        ItemUI.SetActive(false);
        if(PlayerPrefs.HasKey("TempInventorySlotScene" + ident + "Count"))
        {
            PlayerPrefs.SetInt("TempInventorySlotScene" + ident + "Count", PlayerPrefs.GetInt("TempInventorySlotScene" + ident + "Count") - 1);
        }
        else
        {
            PlayerPrefs.SetInt("TempInventorySlotScene" + ident + "ID", PlayerPrefs.GetInt("InventorySlotScene" + ident + "ID", -1));
            PlayerPrefs.SetInt("TempInventorySlotScene" + ident + "Count", PlayerPrefs.GetInt("InventorySlotScene" + ident + "Count") - 1);
        }

        StartCoroutine(ItemUsed());

    }

    IEnumerator ItemUsed()
    {
        turncounter++;
        //Debug.Log("itembeingused!!");
        state = BattleState.ENEMYTURN;
        if(PlayerPrefs.GetString("HPMP") == "HP")
        {
            playerUnit.HealHP(PlayerPrefs.GetInt("combatItemEffect"));
            playerHUD.SetHP(playerUnit.currentHP);
            
        }
        else if(PlayerPrefs.GetString("HPMP") == "MP")
        {
            playerUnit.HealMP(PlayerPrefs.GetInt("combatItemEffect"));
            playerHUD.SetHP(playerUnit.currentMP);

        }
        dialogueText.text = playerUnit.unitName + " used " + PlayerPrefs.GetString("combatItemName") + "!";
        yield return new WaitForSeconds(2f);
        PlayerPrefs.DeleteKey("combatItemEffect");
        PlayerPrefs.DeleteKey("combatItemName");
        StartCoroutine(EnemyTurn());
    }

    //player attack sequence
    IEnumerator PlayerAttack() //basic attack
    {
        turncounter++;
        //Damage enemy
        bool isDead = enemyUnit.DealDamage(playerUnit.attack, playerUnit.raged);

        playerSlashAnimation.SetActive(true); // shows slash animation

        state = BattleState.ENEMYTURN;
        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = playerUnit.unitName + " uses " + PlayerPrefs.GetString("skillname") + " on " + enemyUnit.unitName + " and deals " + enemyUnit.damagedealt + " damage";
        yield return new WaitForSeconds(0.8f);
        playerSlashAnimation.SetActive(false);
        yield return new WaitForSeconds(1.2f);

        //Check if dead
        if (isDead) //Ends Combat if true
        {

            state = BattleState.WIN;
            StartCoroutine(ExitCombat());


        }
        else //Enemy Turn
        {

            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerAttackEleRad()
    {
        turncounter++;
        //Damage enemy
        bool isDead = enemyUnit.DealDamage(playerUnit.attack * 3, playerUnit.raged);

        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = playerUnit.unitName + " attacks " + enemyUnit.unitName + " and deals " + enemyUnit.damagedealt + " damage"; // 3x dmg multiplier
        playerClone.SetActive(false);// hides player 
        playerEleRadAnimation.SetActive(true); // shows elemental radiance animation
        playerEleRadCasting.SetActive(true); //shows player casting animation
        state = BattleState.ENEMYTURN;
        yield return new WaitForSeconds(1.5f);
        playerEleRadAnimation.SetActive(false); // shows elemental radiance animation
        yield return new WaitForSeconds(0.5f);
        playerClone.SetActive(true); // shows player 
        playerEleRadCasting.SetActive(false); //shows player casting animation
        //Check if dead
        if (isDead) //Ends Combat if true
        {

            state = BattleState.WIN;
            StartCoroutine(ExitCombat());
            

        }
        else //Enemy Turn
        {

            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerBuffing() // runs the animation and stat changes for the buff skill (currently rage)
    {
        turncounter++;
        dialogueText.text = playerUnit.unitName + " is enraged for 3 turns!";
        playerUnit.useMP(5);
        playerHUD.SetMP(playerUnit.currentMP);
        playerClone.SetActive(false);// hides player UI 
        playerRageAnimation.SetActive(true); // shows rage animation
        state = BattleState.ENEMYTURN;
        yield return new WaitForSeconds(1.55f);
        playerRageAnimation.SetActive(false);
        playerClone.SetActive(true);
        yield return new WaitForSeconds(0.45f);
        playerUnit.raged = true;
        buffcounter = turncounter + 3; 
        StartCoroutine(EnemyTurn());
    }

    //dialogue and decision making for Enemy
    IEnumerator EnemyTurn()
    {
        PlayerPrefs.DeleteKey("skillele");
        PlayerPrefs.DeleteKey("skillpower");
        PlayerPrefs.DeleteKey("skillname");
        bool isDead = playerUnit.DealDamage(enemyUnit.attack, enemyUnit.raged);
        
        playerHUD.SetHP(playerUnit.currentHP);
        PlayerPrefs.SetInt("playerHPnow", playerUnit.currentHP);
        PlayerPrefs.SetInt("playerMPnow", playerUnit.currentMP);
        //Debug.Log("player current HP: " + playerUnit.currentHP);
        dialogueText.text = enemyUnit.unitName + " attacks " + playerUnit.unitName + " and deals " + playerUnit.damagedealt + " damage";
        if(turncounter >= buffcounter && buffcounter != 0)
        {
            playerUnit.raged = false;
            buffcounter = 0;
        }
        yield return new WaitForSeconds(2f);

        if(isDead)
        {
            state = BattleState.LOSE;
            StartCoroutine(ExitCombat());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }

    }
    IEnumerator ExitCombat()
    {
        if (state == BattleState.WIN && tutorial)
        {
            dialogueText.text = "Pyramid God: You defeated me...";
            yield return new WaitForSeconds(1.5f);
            dialogueText.text = "But it only gets harder for you here on out!";
            yield return new WaitForSeconds(2f);
            playerStorage.initialValue = playerPosition;
            PlayerPrefs.SetInt("playerHPMax", 10);
            PlayerPrefs.SetInt("playerHPnow", 10);
            PlayerPrefs.SetInt("playerMPMax", 10);
            PlayerPrefs.SetInt("playerMPnow", 10);
            SceneManager.LoadScene(sceneToLoad);
        }

        else if (state == BattleState.WIN)
        {
            for(int i = 0; i <= 18; i++) // re updates the count of all inventory items after the battle is over
            {
                if(PlayerPrefs.HasKey("TempInventorySlotScene" + i + "ID"))
                {
                    PlayerPrefs.SetInt("InventorySlotScene" + i + "ID", PlayerPrefs.GetInt("TempInventorySlotScene" + i + "ID", -1));
                    PlayerPrefs.SetInt("InventorySlotScene" + i + "Count", PlayerPrefs.GetInt("TempInventorySlotScene" + i + "Count", 0));
                    PlayerPrefs.DeleteKey("TempInventorySlotScene" + i + "ID");
                    PlayerPrefs.DeleteKey("TempInventorySlotScene" + i + "Count");
                }    

            }
            
            if (enemyUnit.unitName == "Shade")
            {
                eventChecker.shadeKilled = true;
                PlayerPrefs.SetInt("shadeKilled", (eventChecker.shadeKilled ? 1 : 0));
                
            }
           if (enemyUnit.movetutorrage) // set the variable of rage to true so user can use it and it will show
            {
                dialogueText.text = "YOU DEFEATED ME!!!!!! I WILL TEACH YOU 'RAGE'";
                yield return new WaitForSeconds(2.5f);
                dialogueText.text = "USE IT TO DOUBLE THE ATTACK OF YOUR NEXT 3 MOVES!";
                yield return new WaitForSeconds(2.5f);
                bool learnedrage = true;
                PlayerPrefs.SetInt("learnedrage", (learnedrage ? 1 : 0));
            }
            dialogueText.text = "You've defeated " + enemyUnit.unitName + "!";
            yield return new WaitForSeconds(1.5f);
            dialogueText.text = "You've gained " + enemyUnit.exp + " EXP";
            yield return new WaitForSeconds(1.5f);
            Encounter.defeated = true;
            if (PlayerPrefs.HasKey("BattleReward")) // text to display reward item
            {
                string itemName = itemList.items[PlayerPrefs.GetInt("BattleReward")].itemName;
                string article = FirstVowel(itemName[0]) ? "an" : "a";
                dialogueText.text = "You received " + article + " " + itemName + "!";
                yield return new WaitForSeconds(1.5f);
            }
            
            if (PlayerPrefs.GetInt("exp") + enemyUnit.exp >= PlayerPrefs.GetInt("exptolevel")) // text to display level up event
            {
                dialogueText.text = "You leveled to " + (PlayerPrefs.GetInt("playerlevel") + 1) + "!";
                yield return new WaitForSeconds(1.5f);
            }
            levelSys.AddExp(enemyUnit.exp);
            if (PlayerPrefs.GetInt("encounter") !=0)
            {
                playerStorage.initialValue.x = PlayerPrefs.GetFloat("PBx");
                playerStorage.initialValue.y = PlayerPrefs.GetFloat("PBy");
                SceneManager.LoadScene(PlayerPrefs.GetInt("PreBattleScene"));
                bool battleover = false;
                PlayerPrefs.SetInt("encounter", (battleover ? 1 : 0)); 
               
            }
            else 
            {
               
                playerStorage.initialValue = playerPosition;
                SceneManager.LoadScene(sceneToLoad);
            }
            
            
        }
        else if (state == BattleState.LOSE)
        {
            dialogueText.text = "You've been defeated!";
            PlayerPrefs.DeleteKey("BattleReward");
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene("Menu");
            //loads main menu
        }
    }
  


}
