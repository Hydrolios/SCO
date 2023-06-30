using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastTravel : MonoBehaviour
{
    public GameObject SelectionMenu;
    public Button[] locationButtons; // Array to hold location buttons

    public static bool GameIsPaused = false;
    public GameObject player;
    public Player playerRef; //player reference

    private string[] locations = { "TownSaleria", "HuesSettlement", "Sunstead", "Location4", "Location5", "Location6" };

    public void Selection() // turn on the UI
    {
        SelectionMenu.SetActive(true);
        TravelButton();

    }


    public void TravelButton()
    {
        foreach (Button button in locationButtons)
        {
            button.gameObject.SetActive(false); // Disable all buttons initially
        }

        for (int i = 0; i < locations.Length; i++)
        {
            string locationName = locations[i];
            int locationVisited = PlayerPrefs.GetInt(locationName + "_unlocked"); // Check if location has been visited

            if (locationVisited == 1)
            {
                locationButtons[i].gameObject.SetActive(true); // Set the button for visited location to active
            }
        }
        Time.timeScale = 0f;
        SelectionMenu.SetActive(true);


    }

    public void Update()
    {
        playerRef = player.GetComponent<Player>();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0f)
            {
                Resume();
            }

        }
    }

    public void Resume()
    {
        PlayerPrefs.SetFloat("interact_range", 0.6f);
        SelectionMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        PlayerPrefs.DeleteKey("ftNPC");
    }


}
