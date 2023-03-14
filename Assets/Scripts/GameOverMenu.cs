using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject player;
    public GameObject goMenuUI;
    public Player playerRef;
    private void Update()
    {
        playerRef = player.GetComponent<Player>();
        if (playerRef.currentHealth <= 0)
        {
            playerRef.openedUIGO = true;
            Pause();
        }

    }

    void Pause()
    {
        goMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadGame()
    {
        //When we load we want to set the starting position of the Player
        if (PlayerPrefs.HasKey("x"))
        {
            playerRef.sceneTransitioned = false;
            playerRef.openedUIPause = false;
            GameIsPaused = false;
            StartCoroutine(LoadNewScene(PlayerPrefs.GetInt("ActiveScene")));
            playerRef.loadGame = true;
            InventoryManager inventoryManager = FindObjectOfType<InventoryManager>(); // gets the inventoryManager in the scene
            inventoryManager.LoadInventory();
            Debug.Log("LoadedGame");
            PlayerPrefs.SetInt("playerattack", PlayerPrefs.GetInt("Savedplayerattack"));
            PlayerPrefs.SetInt("playersol", PlayerPrefs.GetInt("Savedplayersol"));
            PlayerPrefs.SetInt("playerrad", PlayerPrefs.GetInt("Savedplayerrad"));
            PlayerPrefs.SetInt("playerenx", PlayerPrefs.GetInt("Savedplayerenx"));
            PlayerPrefs.SetInt("playerchr", PlayerPrefs.GetInt("Savedplayerchr"));
            PlayerPrefs.SetInt("load", (playerRef.loadGame ? 1 : 0));
            Debug.Log("player reference of boolean load game: " + playerRef.loadGame);
            Time.timeScale = 1f;
        }
        else
        {
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
    public void ExitToMenu()
    {
        playerRef.openedUIGO = false;
        goMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("Menu");


    }
}
