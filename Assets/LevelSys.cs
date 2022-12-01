using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSys : MonoBehaviour
{
    public int level;
    public int exp;
    public int levelexp;
    public int attack;
    public int hp;
    public int skillpts;

    public void Update()
    {
        level = PlayerPrefs.GetInt("playerlevel");
        exp = PlayerPrefs.GetInt("exp");
        levelexp = PlayerPrefs.GetInt("exptolevel");
        attack = PlayerPrefs.GetInt("playerattack");
        hp = PlayerPrefs.GetInt("playerHPMax");
        skillpts = PlayerPrefs.GetInt("skillpts");
    }

    public void AddExp(int amount)
    {
        exp += amount;
        if (exp >= levelexp)
        {
            level++;
            exp -= levelexp;
            levelexp = Mathf.RoundToInt(levelexp + (15 * level));
            PlayerPrefs.SetInt("playerattack", PlayerPrefs.GetInt("playerattack") + 1); //want to make a complex formula for future
            PlayerPrefs.SetInt("playerHPMax", PlayerPrefs.GetInt("playerHPMax") + 3); //want to make a complex formula for future
            PlayerPrefs.SetInt("playerMPMax", PlayerPrefs.GetInt("playerMPMax") + 2);
            PlayerPrefs.SetInt("skillpts", PlayerPrefs.GetInt("skillpts") + 2);
            PlayerPrefs.SetInt("playerHPnow", PlayerPrefs.GetInt("playerHPnow") + 3);
            PlayerPrefs.SetInt("playerMPnow", PlayerPrefs.GetInt("playerMPnow") + 2);
            
        }
        PlayerPrefs.SetInt("playerlevel", level);
        PlayerPrefs.SetInt("exp", exp);
        PlayerPrefs.SetInt("exptolevel", levelexp);

    }

  
}
