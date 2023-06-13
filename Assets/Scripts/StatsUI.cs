using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    // script for handling user stats with consideration of equipment 
    public static bool GameIsPaused = false;
    public GameObject player;
    public GameObject statUI;
    public GameObject hoverUI;
    public int currentWeaponID;
    public int attvalue;
    public int solvalue;
    public int radvalue;
    public int chrvalue;
    public int enxvalue;
    public Player playerRef; //player reference

    private void Start()
    {
        currentWeaponID = PlayerPrefs.GetInt("CurrentWeaponID", -1);
    }
    private void Update()
    {
        playerRef = player.GetComponent<Player>();
        if (Input.GetKeyDown(KeyCode.X) && (playerRef.fadeINrestriction == true) && (playerRef.openedDialog == false) && (playerRef.openedUIPause == false) && (playerRef.openedUIGO == false) && (playerRef.openedUIShop == false) && (playerRef.openedUIInven == false))
        {
            
            if (GameIsPaused && (playerRef.openedUIStats == true))
            {

                playerRef.openedUIStats = false;
                Resume();
                //Debug.Log("Stat window Close");
            }
            else if (playerRef.openedUIStats == false)
            {
                
                playerRef.openedUIStats = true;
                Pause();
                //Debug.Log("Stat window Open");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && (playerRef.openedUIStats == true) && (playerRef.openedUIPause == false))
        {

            playerRef.openedUIStats = false;
            Resume();
            //Debug.Log("Stat window Close");

        }
    }


    public void Resume()
    {
        PlayerPrefs.SetFloat("interact_range", 0.6f);
        playerRef.openedUIStats = false;
        statUI.SetActive(false);
        hoverUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        PlayerPrefs.SetFloat("interact_range", 0f);
        statUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}
