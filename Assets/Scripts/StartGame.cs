using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public GameObject player;
    public GameObject helpUI;
    private Player playerRef;
    public void PlayGame()
    {
        // start of game, delete all player prefs and set the player pref data required for a fresh start
        playerRef = player.GetComponent<Player>();
        playerRef.newGameCheck = true;
        playerRef.textReq = true;
        Time.timeScale = 1f;
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString("versionText", "Alpha V.0.12.023");
        PlayerPrefs.SetInt("newgame", playerRef.newGameCheck ? 1 : 0);
        PlayerPrefs.SetInt("spawnText", playerRef.textReq ? 1 : 0);
        SceneManager.LoadScene("DreamCombat");
        PlayerPrefs.SetInt("playerlevel", 1);
        PlayerPrefs.SetInt("exptolevel", 25);
        PlayerPrefs.SetInt("exp", 0);
        PlayerPrefs.SetInt("currentcash", 0);
        PlayerPrefs.SetInt("playerHPMax", 10);
        PlayerPrefs.SetInt("playerHPnow", 10);
        PlayerPrefs.SetInt("playerMPMax", 10);
        PlayerPrefs.SetInt("playerMPnow", 10);
        PlayerPrefs.SetInt("playerattack", 6);
        PlayerPrefs.SetFloat("interact_range", 0.6f);
        PlayerPrefs.SetInt("playersol", 5);
        PlayerPrefs.SetInt("playerenx", 5);
        PlayerPrefs.SetInt("playerrad", 5);
        PlayerPrefs.SetInt("playerchr", 5);

    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("x"))
        {
            // checks if a saved position exist, by checking if an X position is saved
            
            StartCoroutine(LoadNewScene(PlayerPrefs.GetInt("ActiveScene")));
            playerRef = player.GetComponent<Player>();
            playerRef.loadGame = true;
            PlayerPrefs.SetFloat("interact_range", 0.6f);
            PlayerPrefs.SetInt("load", (playerRef.loadGame ? 1 : 0));
            Time.timeScale = 1f;

        }
        else
        {
            // returns nothing if there is no saved data
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
    public void QuitGame()
    {
        
        Application.Quit(); // Closes the application
    }

    public void Help()
    {
        helpUI.SetActive(true);
    }

    public void HelpClose()
    {
        helpUI.SetActive(false);
    }
}
