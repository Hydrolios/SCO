using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    public bool isPlayer;

    public string unitName;
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



    // check to see if damage has killed an enemy
    public bool DealDamage(int dmg, bool buffed)
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


        if (currentHP <= 0)
            return true;
        else
            return false;
    }
    //rolls from a random range for damage given character stats
    public int AttackRoll(int dmg) // future: takes in power of attack (like in pokemon) * base attack by power in percent * stat that influences
    {

        if (PlayerPrefs.GetString("skillele") == "Physical" || PlayerPrefs.GetString("skillele") == "Solstice")
        {
            minDmg = (0.85 + (PlayerPrefs.GetInt("playersol") * 0.01)) * (PlayerPrefs.GetFloat("skillpower") * dmg);
            maxDmg = (1.15 + (PlayerPrefs.GetInt("playersol") * 0.01)) * (PlayerPrefs.GetFloat("skillpower") * dmg);

        }
        
        else
        {
            minDmg = 0.85 * dmg;
            maxDmg = 1.15 * dmg;
            Debug.Log("attack was non elemental");
        }

        return Mathf.RoundToInt(Random.Range((float)minDmg, (float)maxDmg));
    }

}
