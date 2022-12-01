using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    public string skillname;
    public string skillele;
    public float skillpower;
    public void AttackSlash()
    {
        skillname = "Slash";
        PlayerPrefs.SetString("skillname", skillname);
        skillele = "Physical";
        PlayerPrefs.SetString("skillele", skillele);
        skillpower = 0.8f;
        PlayerPrefs.SetFloat("skillpower", skillpower);
    }

}
