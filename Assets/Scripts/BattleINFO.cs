using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleINFO : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    public Slider hpSlider;
    //public Slider mpSlider;

    public void SetHUD(UnitStats unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "Lvl " + unit.unitLevel;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }

}
