using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    // this script is mainly for dealing damage in combat

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
            unitName = "Kuller";
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
            damagedealt = AttackRoll(dmg) * 2;
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

    public bool EnemyDealDamage(int dmg, bool block, string name, int turncounter)
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
                method.Invoke(this, new object[] { dmg, block, turncounter });
                return currentHP <= 0;

            }
        }
        // if name is not found, default attack
        DealDamageDefault(dmg, block);
        // returns if dead or not after damage dealt
        return currentHP <= 0;


    }
    public void ShadeDealDamage(int dmg, bool block, int turncounter)
    {
        Debug.Log("turncounter is " + turncounter);
        if(turncounter % 2 == 0) //turn is even, sett damagedealt to be powered
        {
            Debug.Log("Powered attack");
            damagedealt = Mathf.RoundToInt(AttackRoll(dmg) * 2f);
            
        }
        if (block)
        {
            if (turncounter % 2 == 0) //turn is even
            {
                PlayerPrefs.SetInt("HeavyAttack", 1);
                //turn is even so powered attack but block is active so dmg is still reduced
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

    public bool useMP(int mp)
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

        if (PlayerPrefs.GetString("skillele") == "Physical" || PlayerPrefs.GetString("skillele") == "Solstice")
        {
            //Debug.Log("skill was physical");
            minDmg = 0.85 + (PlayerPrefs.GetFloat("skillpower") * dmg) * (1 + (PlayerPrefs.GetInt("playersol") * 0.01));
            maxDmg = 1.15 + (PlayerPrefs.GetFloat("skillpower") * dmg) * (1 + (PlayerPrefs.GetInt("playersol") * 0.01));
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
