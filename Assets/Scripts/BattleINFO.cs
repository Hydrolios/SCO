using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleINFO : MonoBehaviour
{
    //script responsible for handling HP/MP HUD in combat
    public Text nameText;
    public Text levelText;
    public Text hpText;
    public Text mpText;
    public int maxHP;
    public int maxMP;
    public Slider hpSlider;
    public Slider mpSlider;

    public void SetHUD(UnitStats unit)
    {
        maxHP = unit.maxHP;
        maxMP = unit.maxMP;
        hpText.text = unit.currentHP + " / " + unit.maxHP;
        if(unit.maxMP > 0)
        {
            mpText.text = unit.currentMP + " / " + unit.maxMP;
        }
        
        nameText.text = unit.unitName;
        levelText.text = "Lvl " + unit.unitLevel;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
        mpSlider.maxValue = unit.maxMP;
        mpSlider.value = unit.currentMP;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
        if(hp < 0)
        {
            hp = 0;
        }
        hpText.text = hp + " / " + maxHP;
 
    }

    public void SetMP(int mp)
    {
        mpSlider.value = mp;
        mpText.text = mp + " / " + maxMP;

    }

}
