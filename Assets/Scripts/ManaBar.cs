using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxMana(int mana)
    {
        slider.maxValue = mana;
        slider.value = PlayerPrefs.GetInt("playerMPnow");
    }
    public void SetHealth()
    {
        slider.value = PlayerPrefs.GetInt("playerMPnow");
    }
}
