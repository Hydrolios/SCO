using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject saveText;
    public GameObject noSaveText;
    public GameObject noloadText;
    public GameObject player;
    public GameObject pauseMenuUI;
    public GameObject versionText;
    public Player playerRef; // player reference
    public Vector2 playerposition;

    public void Start()
    {
        Text versionTxt = versionText.GetComponent<Text>();
        versionTxt.text = PlayerPrefs.GetString("versionText");
        Debug.Log(versionTxt.text);
    }
    private void Update()
    {
        playerRef = player.GetComponent<Player>();

        if (Input.GetKeyDown(KeyCode.Escape) && GameObject.Find("BattleManager") != null)
        {
            Debug.Log("Found battlemanager");
            if (GameIsPaused && (playerRef.openedUIPause == true))
            {
                playerRef.openedUIPause = false;
                Resume();
                Debug.Log("Pause Close");
            }
            else if (playerRef.openedUIPause == false && !PlayerPrefs.HasKey("ftNPC"))
            {
                playerRef.openedUIPause = true;
                Pause();
                Debug.Log("Pause Open");
            }

        }
        else if (Input.GetKeyDown(KeyCode.Escape) && (playerRef.fadeINrestriction == true) && (playerRef.openedDialog == false) && (playerRef.openedUIInven == false) && (playerRef.openedUIGO == false) && (playerRef.openedUIShop == false) && (playerRef.openedUIStats == false))
        {
            if (GameIsPaused && (playerRef.openedUIPause == true))
            {
                playerRef.openedUIPause = false;
                Resume();
                Debug.Log("Pause Close");
            }
            else if (playerRef.openedUIPause == false && !PlayerPrefs.HasKey("ftNPC"))
            {
                playerRef.openedUIPause = true;
                Pause();
                Debug.Log("Pause Open");
            }
        }

    }

    public void Resume()
    {
        PlayerPrefs.SetFloat("interact_range", 0.6f);
        playerRef.openedUIPause = false;
        versionText.SetActive(false);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        PlayerPrefs.SetFloat("interact_range", 0f);
        versionText.SetActive(true);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void SaveGame()
    {
        if (SceneManager.GetActiveScene().name == "RBD_TreasureRoom")
        {
            Debug.Log("Unable to save in this map");
            StartCoroutine(NoSaveText());
            return;
        }
        //When we save we want to take the position and we will load it as the starting position as well as scene
        playerposition = playerRef.transform.position;
        StartCoroutine(MoveText());
        int activeScene = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("ActiveScene", activeScene);
        PlayerPrefs.SetFloat("x", playerposition.x);
        PlayerPrefs.SetFloat("y", playerposition.y);
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>(); // gets the inventoryManager in the scene
        inventoryManager.SaveInventory();
        Debug.Log("Position x: " + PlayerPrefs.GetFloat("x") + " Position y: " + PlayerPrefs.GetFloat("y"));
        Debug.Log("Savedgame");
        //Player stats
        PlayerPrefs.SetInt("savedMaxHP", PlayerPrefs.GetInt("playerHPMax"));
        PlayerPrefs.SetInt("savedMaxMP", PlayerPrefs.GetInt("playerMPMax"));
        PlayerPrefs.SetInt("savedHP", PlayerPrefs.GetInt("playerHPnow"));
        PlayerPrefs.SetInt("savedMP", PlayerPrefs.GetInt("playerMPnow"));
        PlayerPrefs.SetInt("Savedplayerattack", PlayerPrefs.GetInt("playerattack"));
        PlayerPrefs.SetInt("Savedplayersol", PlayerPrefs.GetInt("playersol"));
        PlayerPrefs.SetInt("Savedplayerrad", PlayerPrefs.GetInt("playerrad"));
        PlayerPrefs.SetInt("Savedplayerenx", PlayerPrefs.GetInt("playerenx"));
        PlayerPrefs.SetInt("Savedplayerchr", PlayerPrefs.GetInt("playerchr"));
        PlayerPrefs.SetInt("cash", PlayerPrefs.GetInt("currentcash"));
        PlayerPrefs.SetInt("saveEXP", PlayerPrefs.GetInt("exp"));
        PlayerPrefs.SetInt("saveplevel", PlayerPrefs.GetInt("playerlevel"));
        PlayerPrefs.SetInt("saveEXPlevel", PlayerPrefs.GetInt("exptolevel"));

        //Players story progression
        for (int i = 0; i < 15; i++) // saving chest states
        {
            //Debug.Log("chest states saved");
            PlayerPrefs.SetInt("chestIDState" + i, PlayerPrefs.GetInt("ChestOpenedID" + i, 0));
        }
        PlayerPrefs.SetInt("saveBlock", PlayerPrefs.GetInt("learnedblock", 0));
        PlayerPrefs.SetInt("saveRage", PlayerPrefs.GetInt("learnedrage", 0));
        PlayerPrefs.SetInt("shadeSave", PlayerPrefs.GetInt("shadeKilled", 0));
        PlayerPrefs.Save();

    }
    public void LoadGame()
    {
        //When we load we want to set the starting position of the Player
        if (PlayerPrefs.HasKey("x"))
        {
            playerRef.sceneTransitioned = false;
            playerRef.openedUIPause = false;
            GameIsPaused = false;
            playerRef.loadGame = true;
            InventoryManager inventoryManager = FindObjectOfType<InventoryManager>(); // gets the inventoryManager in the scene
            inventoryManager.LoadInventory();
            Debug.Log("LoadedGame");
            PlayerPrefs.SetInt("playerattack", PlayerPrefs.GetInt("Savedplayerattack"));
            PlayerPrefs.SetInt("playersol", PlayerPrefs.GetInt("Savedplayersol"));
            PlayerPrefs.SetInt("playerrad", PlayerPrefs.GetInt("Savedplayerrad"));
            PlayerPrefs.SetInt("playerenx", PlayerPrefs.GetInt("Savedplayerenx"));
            PlayerPrefs.SetInt("playerchr", PlayerPrefs.GetInt("Savedplayerchr"));
            PlayerPrefs.SetInt("currentcash", PlayerPrefs.GetInt("cash"));
            PlayerPrefs.SetInt("load", (playerRef.loadGame ? 1 : 0));
            Debug.Log("player reference of boolean load game: " + playerRef.loadGame);
            Time.timeScale = 1f;
            StartCoroutine(LoadNewScene(PlayerPrefs.GetInt("ActiveScene")));
        }
        else
        {
            StartCoroutine(NoLoadTxt());
            Debug.Log("No Save Data");
        }
    }

    IEnumerator LoadNewScene(int sceneBuildIndex)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneBuildIndex);
        asyncOperation.allowSceneActivation = false;

        while (asyncOperation.progress < 0.9f)
        {
            yield return null;
        }

        asyncOperation.allowSceneActivation = true;
    }

    IEnumerator NoLoadTxt()
    {
        noloadText.GetComponent<RectTransform>().anchoredPosition = new Vector2(-625, -500);
        yield return new WaitForSecondsRealtime(1f);
        noloadText.GetComponent<RectTransform>().anchoredPosition = new Vector2(-800, 0);
    }
    IEnumerator MoveText() // move text on screen to tell user what is going on, saving game, no load found, etc
    {
        saveText.GetComponent<RectTransform>().anchoredPosition = new Vector2(-625, -500); //directly influences the X Y in inspector
        yield return new WaitForSecondsRealtime(1f);
        saveText.GetComponent<RectTransform>().anchoredPosition = new Vector2(-800, 0);
    }
    IEnumerator NoSaveText() // move text on screen to tell user what is going on, saving game, no load found, etc
    {
        noSaveText.GetComponent<RectTransform>().anchoredPosition = new Vector2(-625, -500); //directly influences the X Y in inspector
        yield return new WaitForSecondsRealtime(1f);
        noSaveText.GetComponent<RectTransform>().anchoredPosition = new Vector2(-800, 0);
    }
    public void ExitToMenu()
    {
        playerRef.openedUIPause = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("Menu");
        

    }
    
}
