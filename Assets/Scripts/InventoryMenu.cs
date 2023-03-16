using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject player;
    public GameObject InventoryMenuUI;
    public GameObject useMenu;
    public GameObject equipMenu;
    public Player playerRef; //player reference


    private void Update() // this script is mainly for managing the opening/closing of the Inventory UI
    {
        
        playerRef = player.GetComponent<Player>();
        if (Input.GetKeyDown(KeyCode.I) && (playerRef.openedDialog == false) && (playerRef.openedUIPause == false) && (playerRef.openedUIGO == false) && (playerRef.openedUIShop == false) && (playerRef.openedUIStats == false))
        {
            Text curcash = InventoryMenuUI.transform.Find("Cash").GetComponent<Text>();
            curcash.text = "$" + PlayerPrefs.GetInt("currentcash");
            if (GameIsPaused && (playerRef.openedUIInven == true))
            {

                playerRef.openedUIInven = false;
                Resume();
                //Debug.Log("Inventory Close");
            }
            else if (playerRef.openedUIInven == false)
            {
                playerRef.openedUIInven = true;
                Pause();
                //Debug.Log("Inventory Open");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && (playerRef.openedUIInven == true) && (playerRef.openedUIPause == false))
        {

            playerRef.openedUIInven = false;
            Resume();
            //Debug.Log("Inventory Close");
           
        }
    }
    public void Resume()
    {
        PlayerPrefs.SetFloat("interact_range", 0.6f);
        playerRef.openedUIInven = false;
        InventoryMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        PlayerPrefs.SetFloat("interact_range", 0f);
        InventoryMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

  
}
