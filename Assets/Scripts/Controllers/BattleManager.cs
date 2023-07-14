using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { START, WIN, LOSE, PLAYERTURN, ENEMYTURN }
public class BattleManager : MonoBehaviour
{
    // handles most of the interactions within a battle scene
    /* involvemenet includes changing HUD, calling upon other scripts such as BattleINFO, PlayerSkills, UnitStats
     * handles all animations and turn states
     */

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject AttackListUI;
    public GameObject ItemUI;
    public GameObject heavyAttackAnimation;
    public GameObject enemyAttackAnimation;
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
    public int blockingcounter;
    public int turncounter;
    public int buffcounter;
    public int itemcount;
    //public int partysize;

    public BattleINFO playerHUD;
    public BattleINFO enemyHUD;

    public GameObject player;
    public GameObject rageButton;
    public GameObject blockButton;
    public Vector2 playerPosition;
    public VectorValue playerStorage;
    public NPCManager npcManager;
    public Encounter npcbattling;

    public bool aftBattle_Diag;
    public string[] aftBattle_lines;
    public bool textAftBattle;

    public BattleState state;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        PlayerPrefs.DeleteKey("skillele");
        PlayerPrefs.DeleteKey("skillpower");
        PlayerPrefs.DeleteKey("skillname");
        StartCoroutine(SetupBattle());

    }

    //Spawns the units onto the scene and sets the values for the HUD
    IEnumerator SetupBattle()
    {
        turncounter = 0;
        buffcounter = 0;
        blockingcounter = 0;
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

        
        
        if (PlayerPrefs.GetInt("learnedblock") != 0) //shows block
        {
            blockButton.SetActive(true);
        }
        else
        {
            blockButton.SetActive(false);
        }
        if (PlayerPrefs.GetInt("learnedrage") != 0) //shows rage
        {
            rageButton.SetActive(true);
        }
        else
        {
            rageButton.SetActive(false);
        }
        yield return new WaitForSeconds(2f);
        fadeIN.SetActive(false);
        state = BattleState.PLAYERTURN;
        PlayerTurn();
        
    }

    void PlayerTurn()
    {
        Debug.Log("turn counter is: " + turncounter);
        dialogueText.text = playerUnit.unitName + "'s turn";
    }

    public void OnBlockButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerBlock());

    }

    public void OnAttacksButton() //selecting "attacks" open up a list UI 
    {
        if (state != BattleState.PLAYERTURN)
            return;
        AttackListUI.SetActive(true);
    }

    public void AttacksListReturn() // selecting back arrows closes list
    {
        AttackListUI.SetActive(false);
    }

    public void OnItemButton() //selecting "items" open up a list UI
    {
        if (state != BattleState.PLAYERTURN)
            return;
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
    
    public void WeaponSkill() //need this to be flexible and used for all equipped weapon and all buttons
    {
        AttackListUI.SetActive(false);
        StartCoroutine(WeaponSkillAttack());

    }

    IEnumerator WeaponSkillAttack()
    {
        if(PlayerPrefs.GetInt("hpcost", 0) > 0 && PlayerPrefs.GetInt("mpcost", 0) > 0) // if there is an required HP and MP usage
        {
            Debug.Log("hp & mp cost method");
            bool usable = playerUnit.useHP(PlayerPrefs.GetInt("hpcost", 0)) && playerUnit.useMP(PlayerPrefs.GetInt("mpcost", 0));
            if (!usable)
            {
                dialogueText.text = "You do not have enough HP or MP to use this!";
                yield return new WaitForSeconds(1.5f);
                PlayerTurn();
            }
            else
            {
                playerHUD.SetHP(playerUnit.currentHP);
                playerHUD.SetMP(playerUnit.currentMP);
                PlayerPrefs.DeleteKey("mpcost");
                PlayerPrefs.DeleteKey("hpcost");
                bool isDead = enemyUnit.DealDamage(playerUnit.attack, playerUnit.raged);
                enemyHUD.SetHP(enemyUnit.currentHP);
                dialogueText.text = playerUnit.unitName + " uses " + PlayerPrefs.GetString("skillname") + " on " + enemyUnit.unitName + " and deals " + enemyUnit.damagedealt + " damage";
                turncounter++;
                state = BattleState.ENEMYTURN;

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
        }

        else if (PlayerPrefs.GetInt("hpcost", 0) > 0) // if there is a required hp usage attached to skill
        {
            Debug.Log("hp cost method");
            bool usable = playerUnit.useHP(PlayerPrefs.GetInt("hpcost", 0));
            // not enough health to use the skill
            if (!usable)
            {
                dialogueText.text = "You do not have enough HP to use this!";
                yield return new WaitForSeconds(1.5f);
                PlayerTurn();
            }
            else
            {
                playerHUD.SetHP(playerUnit.currentHP);
                PlayerPrefs.DeleteKey("hpcost");
                bool isDead = enemyUnit.DealDamage(playerUnit.attack, playerUnit.raged);
                enemyHUD.SetHP(enemyUnit.currentHP);
                dialogueText.text = playerUnit.unitName + " uses " + PlayerPrefs.GetString("skillname") + " on " + enemyUnit.unitName + " and deals " + enemyUnit.damagedealt + " damage";
                turncounter++;
                state = BattleState.ENEMYTURN;

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

        }
        else if (PlayerPrefs.GetInt("mpcost", 0) > 0) // if there is a required mp usage attached to skill
        {
            Debug.Log("mp cost method");
            bool usable = playerUnit.useMP(PlayerPrefs.GetInt("mpcost", 0));
            // not enough mana to use the skill
            if (!usable)
            {
                dialogueText.text = "You do not have enough MP to use this!";
                yield return new WaitForSeconds(1.5f);
                PlayerTurn();
            }
            else
            {
                playerHUD.SetMP(playerUnit.currentMP);
                PlayerPrefs.DeleteKey("mpcost");
                bool isDead = enemyUnit.DealDamage(playerUnit.attack, playerUnit.raged);
                enemyHUD.SetHP(enemyUnit.currentHP);
                dialogueText.text = playerUnit.unitName + " uses " + PlayerPrefs.GetString("skillname") + " on " + enemyUnit.unitName + " and deals " + enemyUnit.damagedealt + " damage";
                turncounter++;
                state = BattleState.ENEMYTURN;

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

        }

    }

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
            playerHUD.SetMP(playerUnit.currentMP);

        }
        dialogueText.text = playerUnit.unitName + " used " + PlayerPrefs.GetString("combatItemName") + "!";
        yield return new WaitForSeconds(2f);
        PlayerPrefs.DeleteKey("combatItemEffect");
        PlayerPrefs.DeleteKey("combatItemName");
        StartCoroutine(EnemyTurn());
    }

    //player attack sequence
    IEnumerator PlayerBlock()
    {
        turncounter++;
        state = BattleState.ENEMYTURN;
        dialogueText.text = playerUnit.unitName + " prepares to block!";
        //playerBlockAnimation.SetActive(true);
        playerUnit.blocking = true;
        blockingcounter = turncounter;
        yield return new WaitForSeconds(2f);
        StartCoroutine(EnemyTurn());

    }
    IEnumerator PlayerAttack() //basic attack for the player "slash"
    {
        turncounter++;
        //checks if the enemy is dead from the attack
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
        bool usable = playerUnit.useMP(5);
        // not enough mana to use the buff
        if (!usable)
        {
            dialogueText.text = "You do not have enough MP to use this!";
            yield return new WaitForSeconds(1.5f);
            PlayerTurn();
        }
        //enough mana to use the buff
        else
        {
            turncounter++;
            dialogueText.text = playerUnit.unitName + " is enraged for 3 turns!";

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
        
    }

    //dialogue and decision making for Enemy
    IEnumerator EnemyTurn()
    {
        PlayerPrefs.DeleteKey("skillele");
        PlayerPrefs.DeleteKey("skillpower");
        PlayerPrefs.DeleteKey("skillname");
        PlayerPrefs.DeleteKey("enemyhealed");
        PlayerPrefs.DeleteKey("enemyhealamt");
        //need to make consideration for enemy turns being able to heal themselves
        // as it stands, enemy turn actions can only affect the player
        bool isDead = playerUnit.EnemyDealDamage(enemyUnit.attack, playerUnit.blocking, enemyUnit.unitName, turncounter, enemyUnit.currentHP, enemyUnit.maxHP);
        //checks what animation to play
        if(PlayerPrefs.GetInt("enemyhealed", 0) == 1)
        {
            enemyUnit.HealHP(PlayerPrefs.GetInt("enemyhealamt", 0));
        }
        else if (PlayerPrefs.GetInt("HeavyAttack", 0) == 1)
        {
            heavyAttackAnimation.SetActive(true);
        }
        else
        {
            enemyAttackAnimation.SetActive(true);
        }
        
        playerHUD.SetHP(playerUnit.currentHP);
        enemyHUD.SetHP(enemyUnit.currentHP); // for consideration of enemies being able to heal
        //Debug.Log("player current HP: " + playerUnit.currentHP);
        if (PlayerPrefs.GetInt("enemyhealed", 0) == 1)
        {
            dialogueText.text = enemyUnit.unitName + " heals for " + PlayerPrefs.GetInt("enemyhealamt", 0);
        }
        else
        {
            dialogueText.text = enemyUnit.unitName + " attacks " + playerUnit.unitName + " and deals " + playerUnit.damagedealt + " damage";
        }
            
        //checks if the buff duration is over
        if (turncounter >= buffcounter && buffcounter != 0) 
        {
            playerUnit.raged = false;
            buffcounter = 0;
        }
        // checks if the block duration is over
        Debug.Log("Turn: " + turncounter + " Blockcounter: " + blockingcounter);
        if (turncounter >= blockingcounter && blockingcounter != 0) 
        {
            playerUnit.blocking = false;
            blockingcounter = 0;
        }
        yield return new WaitForSeconds(0.8f);
        //turn off all animations
        heavyAttackAnimation.SetActive(false);
        enemyAttackAnimation.SetActive(false);
        yield return new WaitForSeconds(0.7f);
        //checks if the unit is dead
        if (isDead)
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
        if (state == BattleState.WIN && aftBattle_Diag)
        {
            for (int i = 0; i < aftBattle_lines.Length; i++)
            {
                dialogueText.text = aftBattle_lines[i];
                yield return new WaitForSeconds(1.5f);
            }

        }


        if (state == BattleState.WIN)
        {
            PlayerPrefs.SetInt("playerHPnow", playerUnit.currentHP);
            PlayerPrefs.SetInt("playerMPnow", playerUnit.currentMP);
            for (int i = 0; i <= 18; i++) // re updates the count of all inventory items after the battle is over
            {
                if(PlayerPrefs.HasKey("TempInventorySlotScene" + i + "ID"))
                {
                    PlayerPrefs.SetInt("InventorySlotScene" + i + "ID", PlayerPrefs.GetInt("TempInventorySlotScene" + i + "ID", -1));
                    PlayerPrefs.SetInt("InventorySlotScene" + i + "Count", PlayerPrefs.GetInt("TempInventorySlotScene" + i + "Count", 0));
                    PlayerPrefs.DeleteKey("TempInventorySlotScene" + i + "ID");
                    PlayerPrefs.DeleteKey("TempInventorySlotScene" + i + "Count");
                }    

            }
            if(tutorial)
            {
                PlayerPrefs.SetInt("playerHPMax", 10);
                PlayerPrefs.SetInt("playerHPnow", 10);
                PlayerPrefs.SetInt("playerMPMax", 10);
                PlayerPrefs.SetInt("playerMPnow", 10);
            }
            
            if (enemyUnit.unitName == "Shade")
            {
                Debug.Log("Shade Defeated!");
                eventChecker.shadeKilled = true;
                PlayerPrefs.SetInt("shadeKilled", (eventChecker.shadeKilled ? 1 : 0));
                
            }
            if (enemyUnit.unitName == "Captain Tint")
            {
                Debug.Log("Captain Tint Defeated!");
                eventChecker.fastTravelunlock = true;
                PlayerPrefs.SetInt("fastTravelUnlocked", (eventChecker.fastTravelunlock ? 1 : 0));
                PlayerPrefs.SetInt("HuesSettlement_unlocked", 1);
            }
            if (enemyUnit.movetutorrage) // set the variable of rage to true so user can use it and it will show
            {
                dialogueText.text = "YOU DEFEATED ME!!!!!! I WILL TEACH YOU 'RAGE'";
                yield return new WaitForSeconds(2.5f);
                dialogueText.text = "USE IT TO POWER UP YOUR ATTACKS FOR YOUR NEXT 3 TURNS!";
                yield return new WaitForSeconds(2.5f);
                bool learnedrage = true;
                PlayerPrefs.SetInt("learnedrage", (learnedrage ? 1 : 0));
            }
            dialogueText.text = "You've defeated " + enemyUnit.unitName + "!";
            yield return new WaitForSeconds(1.25f);
            if(enemyUnit.exp > 0)
            {
                dialogueText.text = "You've gained " + enemyUnit.exp + " EXP";
                yield return new WaitForSeconds(1.25f);
            }
            if(enemyUnit.cash > 0)
            {
                dialogueText.text = "You've received $" + enemyUnit.cash + "!";
                PlayerPrefs.SetInt("currentcash", PlayerPrefs.GetInt("currentcash") + enemyUnit.cash);
                yield return new WaitForSeconds(1.25f);
            }

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
            if (textAftBattle)
            {
                PlayerPrefs.SetInt("spawnText", textAftBattle ? 1 : 0);
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
