using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    // this script is mainly for dealing damage in combat
    // most of the methods are for enemies as this script mainly handles the players HP/MP
    // this also includes hp/mp related features for the player such as consumables or skill uses

    public bool isPlayer;

    public string unitName;
    public string[] enemyNames = { "Shade", "Captain Tint" }; // list of enemys with special AI
    public int unitLevel;

    public int attack;
    public int damagedealt;
    public double minDmg;
    public double maxDmg;

    public int maxHP;
    public int currentHP;
    public int maxMP;
    public int currentMP;
    public int def;
    public int exp;
    public int cash;

    public int rad;
    public int enx;
    public int sol;
    public int chr;

    public bool movetutorrage;

    //public bool essence; 
    //public bool 
    //inherit traits (essence, some others) will give bonuses to certain stats, e.g. essence = higher att multiplier
    //player will be able to change trait based on gear but NPCS will have inherit and fixed traits
    public bool raged;
    public bool blocking;

    public void Start()
    {
        if (isPlayer)
        {
            unitName = "Spectrum";
            unitLevel = PlayerPrefs.GetInt("playerlevel");
            attack = PlayerPrefs.GetInt("playerattack");
            maxHP = PlayerPrefs.GetInt("playerHPMax");
            currentHP = PlayerPrefs.GetInt("playerHPnow");
            maxMP = PlayerPrefs.GetInt("playerMPMax");
            currentMP = PlayerPrefs.GetInt("playerMPnow");
            sol = PlayerPrefs.GetInt("playersol");
            enx = PlayerPrefs.GetInt("playerenx");
            rad = PlayerPrefs.GetInt("playerrad");
            chr = PlayerPrefs.GetInt("playerchr");
        }
    }



    // check to see if damage from the player has killed an enemy 
    public bool DealDamage(int dmg, bool buffed) //will need to adjust this or create a new method to take in elements
    {
        

        if (buffed)
        {
            damagedealt = Mathf.RoundToInt(AttackRoll(dmg) * 1.5f);
            currentHP -= damagedealt;
            Debug.Log(damagedealt);
        }
        else
        {
            damagedealt = AttackRoll(dmg);
            currentHP -= damagedealt;
            Debug.Log(damagedealt);

        }


        return currentHP <= 0;
    }

    public bool EnemyDealDamage(int dmg, bool block, string name, int turncounter, int enemyhp, int enemymaxhp)
    {
        //Resets the value of all checks for special attacks
        PlayerPrefs.SetInt("HeavyAttack", 0);
        
        for (int i = 0; i< enemyNames.Length; i++) 
        {

            if(name == enemyNames[i]) // execute special enemy AI depending on the enemy
            {
                Debug.Log(name + " found at inquiry " + i);
                string methodName = enemyNames[i].Replace(" ", "") + "DealDamage";
                MethodInfo method = GetType().GetMethod(methodName);
                method.Invoke(this, new object[] { dmg, block, turncounter, enemyhp, enemymaxhp });
                return currentHP <= 0;

            }
        }
        Debug.Log("name not found");
        // if name is not found, default attack
        DealDamageDefault(dmg, block);
        // returns if dead or not after damage dealt
        return currentHP <= 0;


    }
    public void ShadeDealDamage(int dmg, bool block, int turncounter, int currhp, int maxhp)
    {
        Debug.Log("turncounter is " + turncounter);
        if(turncounter % 2 == 0) //turn is even, set damagedealt to be powered
        {
            Debug.Log("Powered attack");
            damagedealt = Mathf.RoundToInt(AttackRoll(dmg) * 2f);
            
        }
        if (block)
        {
            if (turncounter % 2 == 0) //turn is even
            {
                PlayerPrefs.SetInt("HeavyAttack", 1);
                //turncount is even so powered attack but block is active so dmg is still reduced
                damagedealt = Mathf.RoundToInt(damagedealt * 0.2f);
                currentHP -= damagedealt;
                Debug.Log(damagedealt);
            }
            else
            {
                PlayerPrefs.SetInt("HeavyAttack", 0);
                //no power attack but block is active so dmg is reduced
                damagedealt = Mathf.RoundToInt(AttackRoll(dmg) * 0.2f);
                currentHP -= damagedealt;
                Debug.Log(damagedealt);
            }
            
        }
        else // did not block and not an even turn, so do a normal attack
        {
            if (turncounter % 2 == 0)
            {
                PlayerPrefs.SetInt("HeavyAttack", 1);
                currentHP -= damagedealt;
                Debug.Log(damagedealt);
            }
            else
            {
                PlayerPrefs.SetInt("HeavyAttack", 0);
                damagedealt = AttackRoll(dmg);
                currentHP -= damagedealt;
                Debug.Log(damagedealt);
            }
        }
        
    }

    public void CaptainTintDealDamage(int dmg, bool block, int turncounter, int currhp, int maxhp)
    {

        if(currhp < Mathf.RoundToInt(maxhp * 0.3f)) // while Tint is less than 30% hp, there is a chance he can heal
        {
            Debug.Log("HP is less than 30%, chance to heal is possible");
            int chanceroll = Random.Range(0, 2);
            if(chanceroll == 1)
            {
                
                int healamt = Mathf.RoundToInt(Random.Range(maxhp * 0.1f, maxhp * 0.2f));
                Debug.Log(healamt + " amount healed");
                //currentHP += healamt;
                Debug.Log("Captain Tint casts heal and healed");
                PlayerPrefs.SetInt("enemyhealed", 1);
                PlayerPrefs.SetInt("enemyhealamt", healamt);
            }
            else
            {
                if (block)
                {
                    if (turncounter % 2 == 0)
                    {
                        Debug.Log("Heavy attack but blocked");
                        PlayerPrefs.SetInt("HeavyAttack", 1);
                        damagedealt = Mathf.RoundToInt((AttackRoll(dmg) * 2f) * 0.2f);
                        currentHP -= damagedealt;
                        Debug.Log(damagedealt);
                    }
                    else
                    {
                        Debug.Log("light attack but blocked");
                        PlayerPrefs.SetInt("HeavyAttack", 0);
                        damagedealt = Mathf.RoundToInt(AttackRoll(dmg) * 0.2f);
                        currentHP -= damagedealt;
                        Debug.Log(damagedealt);
                    }
                }
                else
                {
                    if (turncounter % 2 == 0)
                    {
                        Debug.Log("Heavy attack");
                        PlayerPrefs.SetInt("HeavyAttack", 1);
                        damagedealt = Mathf.RoundToInt(AttackRoll(dmg) * 2f);
                        currentHP -= damagedealt;
                        Debug.Log(damagedealt);
                    }
                    else
                    {
                        PlayerPrefs.SetInt("HeavyAttack", 0);
                        damagedealt = AttackRoll(dmg);
                        currentHP -= damagedealt;
                        Debug.Log(damagedealt);
                    }
                }

            }
        }
        else
        {
            if (block)
            {
                if (turncounter % 2 == 0)
                {
                    Debug.Log("Heavy attack but blocked");
                    PlayerPrefs.SetInt("HeavyAttack", 1);
                    damagedealt = Mathf.RoundToInt((AttackRoll(dmg) * 2f) * 0.2f);
                    currentHP -= damagedealt;
                    Debug.Log(damagedealt);
                }
                else
                {
                    Debug.Log("light attack but blocked");
                    PlayerPrefs.SetInt("HeavyAttack", 0);
                    damagedealt = Mathf.RoundToInt(AttackRoll(dmg) * 0.2f);
                    currentHP -= damagedealt;
                    Debug.Log(damagedealt);
                }
            }
            else
            {
                if (turncounter % 2 == 0)
                {
                    Debug.Log("Heavy attack");
                    PlayerPrefs.SetInt("HeavyAttack", 1);
                    damagedealt = Mathf.RoundToInt(AttackRoll(dmg) * 2f);
                    currentHP -= damagedealt;
                    Debug.Log(damagedealt);
                }
                else
                {
                    PlayerPrefs.SetInt("HeavyAttack", 0);
                    damagedealt = AttackRoll(dmg);
                    currentHP -= damagedealt;
                    Debug.Log(damagedealt);
                }
            }
        }
        
    }

    public bool DealDamageDefault(int dmg, bool block)
    {
        if (block)
        {
            damagedealt = Mathf.RoundToInt(AttackRoll(dmg) * 0.2f);
            currentHP -= damagedealt;
            Debug.Log(damagedealt);
        }
        else
        {
            damagedealt = AttackRoll(dmg);
            currentHP -= damagedealt;
            Debug.Log(damagedealt);

        }

        // returns if dead or not after damage dealt
        return currentHP <= 0;
    }

    public bool useMP(int mp) // method for consuming mp to use skills
    {
        if(mp > currentMP)
        {
            return false;
        }
        else
        {
            currentMP -= mp;
            return true;
        }
    }
    public bool useHP(int hp) // method for consuming hp to use skills
    {
        if (hp > currentHP)
        {
            return false;
        }
        else
        {
            currentHP -= hp;
            return true;
        }
    }
    public void HealHP(int health) // method for healing from an item
    {
        if(currentHP + health <= maxHP)
        {
            currentHP += health;
        }
        else
        {
            currentHP = maxHP;
        }
        
    }
    public void HealMP(int mana) // method for healing from an item
    {
        if (currentMP + mana <= maxMP)
        {
            currentMP += mana;
        }
        else
        {
            currentMP = maxMP;
        }

    }


    //rolls from a random range for damage given character stats
    public int AttackRoll(int dmg) // future: takes in power of attack (like in pokemon) * base attack by power in percent * stat that influences
    {

        if (PlayerPrefs.GetString("skillele") == "Physical")
        {
            //Debug.Log("skill was physical");
            minDmg = 0.85 * (PlayerPrefs.GetFloat("skillpower") * dmg) * (1 + (PlayerPrefs.GetInt("playersol") * 0.01));
            maxDmg = 1.15 * (PlayerPrefs.GetFloat("skillpower") * dmg) * (1 + (PlayerPrefs.GetInt("playersol") * 0.01));
            //Debug.Log("min dmg is " + minDmg);
            //Debug.Log("max dmg is " + maxDmg);

        }
        else if (PlayerPrefs.GetString("skillele") == "Solstice")
        {
            //Debug.Log("skill was solstice");
            minDmg = 0.85 * (PlayerPrefs.GetFloat("skillpower") * dmg) * (1 + (PlayerPrefs.GetInt("playersol") * 0.015));
            maxDmg = 1.15 * (PlayerPrefs.GetFloat("skillpower") * dmg) * (1 + (PlayerPrefs.GetInt("playersol") * 0.015));
            //Debug.Log("min dmg is " + minDmg);
            //Debug.Log("max dmg is " + maxDmg);
        }

        else
        {
            minDmg = 0.85 * dmg;
            maxDmg = 1.15 * dmg;
            //Debug.Log("attack was non elemental");
        }

        return Mathf.CeilToInt(Random.Range((float)minDmg, (float)maxDmg));
    }

}
