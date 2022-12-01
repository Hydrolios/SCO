using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCheck : MonoBehaviour
{
    // this script is for managing event updates during scenes to allow for different dialogues or new routes
    public bool shadeKilled;
    public bool rbdreport;
    public void Update()
    {
        rbdreport = PlayerPrefs.GetInt("rbdreport") != 0;
        shadeKilled = PlayerPrefs.GetInt("shadeKilled") != 0;
    }

}
