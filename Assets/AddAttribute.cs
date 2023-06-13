using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAttribute : MonoBehaviour
{
    // script for assigning stat points in the stats UI
    public int points;
    public GameObject sol;
    public GameObject enx;
    public GameObject rad;
    public GameObject chr;
    public int solstat;
    public int enxstat;
    public int radstat;
    public int chrstat;
    // Update is called once per frame
    void Update()
    {
        solstat = PlayerPrefs.GetInt("playersol");
        enxstat = PlayerPrefs.GetInt("playerenx");
        radstat = PlayerPrefs.GetInt("playerrad");
        chrstat = PlayerPrefs.GetInt("playerchr");
        points = PlayerPrefs.GetInt("skillpts");
        if (PlayerPrefs.GetInt("skillpts") >= 1)
        {
            sol.SetActive(true);
            enx.SetActive(true);
            rad.SetActive(true);
            chr.SetActive(true);

        }
        else
        {
            sol.SetActive(false);
            enx.SetActive(false);
            rad.SetActive(false);
            chr.SetActive(false);
        }
    }

    public void AddSol()
    {
        solstat += 1;
        PlayerPrefs.SetInt("playersol", solstat);
        points -= 1;
        PlayerPrefs.SetInt("skillpts", points);
    }
    public void AddEnx()
    {
        enxstat += 1;
        PlayerPrefs.SetInt("playerenx", enxstat);
        points -= 1;
        PlayerPrefs.SetInt("skillpts", points);
    }
    public void AddRad()
    {
        radstat += 1;
        PlayerPrefs.SetInt("playerrad", radstat);
        points -= 1;
        PlayerPrefs.SetInt("skillpts", points);
    }
    public void AddChr()
    {
        chrstat += 1;
        PlayerPrefs.SetInt("playerchr", chrstat);
        points -= 1;
        PlayerPrefs.SetInt("skillpts", points);
    }
}
