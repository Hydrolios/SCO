using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventCheck : MonoBehaviour
{
    // this script is for managing event updates during scenes to allow for different dialogues or new routes
    public bool shadeKilled;
    public bool rbdreport;
    public bool fastTravelunlock;
    public bool huespt1;
    private void OnEnable() //for handling scene changes
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() //for handling scene changes
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        fastTravelunlock = PlayerPrefs.GetInt("fastTravelUnlocked") != 0;
        rbdreport = PlayerPrefs.GetInt("rbdreport") != 0;
        shadeKilled = PlayerPrefs.GetInt("shadeKilled") != 0;
        huespt1 = PlayerPrefs.GetInt("Sunstead_Pt1") != 0;
    }

    void Start()
    {
        // Call OnSceneLoaded manually to update state when scene is reloaded or refreshed
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

}
