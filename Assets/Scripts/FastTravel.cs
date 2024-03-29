using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FastTravel : MonoBehaviour
{
    public GameObject SelectionMenu;
    public GameObject TitleUI;
    public Button[] locationButtons; // Array to hold location buttons
    public GameObject fadeOut;
    public static bool GameIsPaused = false;
    public GameObject player;
    public Player playerRef; //player reference
    public Vector2[] playerPosition;
    public VectorValue playerStorage;
    private string[] locations = { "TownSaleria", "HuesSettlement", "Sunstead", "Location4", "Location5", "Location6" };


    public void Selection() // turn on the UI
    {
        SelectionMenu.SetActive(true);
        TitleUI.SetActive(true);
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
        TitleUI.SetActive(true);
        PlayerPrefs.SetFloat("interact_range", 0f);
        switch (SceneManager.GetActiveScene().name) // remove the current area from the list of fast travel options
        {
            case "Town_SaleriaV2":
                locationButtons[0].gameObject.SetActive(false);
                Debug.Log("removed saleria");
                break;
            case "Hues_Settlement_AftTint":
                locationButtons[1].gameObject.SetActive(false);
                Debug.Log("removed settlement");
                break;
            case "Sunstead":
                locationButtons[2].gameObject.SetActive(false);
                Debug.Log("removed sunstead");
                break;
        }

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


    public void FTSaleriaOnClick()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        PlayerPrefs.DeleteKey("ftNPC");
        TitleUI.SetActive(false);
        //SelectionMenu.SetActive(false);
        PlayerPrefs.SetFloat("interact_range", 0.6f);
        StartCoroutine(FTSaleria());
        
    }

    public void FTSettlementOnClick()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        PlayerPrefs.DeleteKey("ftNPC");
        TitleUI.SetActive(false);
        //SelectionMenu.SetActive(false);
        PlayerPrefs.SetFloat("interact_range", 0.6f);
        StartCoroutine(FTSettlement());
    }

    public void FTSunsteadOnClick()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        PlayerPrefs.DeleteKey("ftNPC");
        TitleUI.SetActive(false);
        //SelectionMenu.SetActive(false);
        PlayerPrefs.SetFloat("interact_range", 0.6f);
        StartCoroutine(FTSunstead());
    }

    IEnumerator FTSaleria()
    {
        Vector2 moveWindow = SelectionMenu.transform.position;
        moveWindow.x = 2000f;
        SelectionMenu.transform.position = moveWindow;
        fadeOut.SetActive(true);
        playerRef.StopSpeed();
        yield return new WaitForSeconds(1f);
        string sceneToLoad = "Town_SaleriaV2";
        playerStorage.initialValue = playerPosition[0];
        SceneManager.LoadScene(sceneToLoad);
    }
    IEnumerator FTSettlement()
    {
        Vector2 moveWindow = SelectionMenu.transform.position;
        moveWindow.x = 2000f;
        SelectionMenu.transform.position = moveWindow;
        fadeOut.SetActive(true);
        playerRef.StopSpeed();
        yield return new WaitForSeconds(1f);
        string sceneToLoad = "Hues_Settlement_AftTint";
        playerStorage.initialValue = playerPosition[1];
        SceneManager.LoadScene(sceneToLoad);
    }

    IEnumerator FTSunstead()
    {
        Vector2 moveWindow = SelectionMenu.transform.position;
        moveWindow.x = 2000f;
        SelectionMenu.transform.position = moveWindow;
        fadeOut.SetActive(true);
        playerRef.StopSpeed();
        yield return new WaitForSeconds(1f);
        string sceneToLoad = "Sunstead";
        playerStorage.initialValue = playerPosition[2];
        SceneManager.LoadScene(sceneToLoad);
    }



    public void Resume()
    {
        PlayerPrefs.SetFloat("interact_range", 0.6f);
        SelectionMenu.SetActive(false);
        TitleUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        PlayerPrefs.DeleteKey("ftNPC");
    }


}
