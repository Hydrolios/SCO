using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject player;
    public GameObject pauseMenuUI;
    public Player playerRef; // player reference
    public Vector2 playerposition;
    private void Update()
    {
        playerRef = player.GetComponent<Player>();

        if ((Input.GetKeyDown(KeyCode.Escape)) && (playerRef.openedUIInven == false) && (playerRef.openedUIGO == false) && (playerRef.openedUIShop == false) && (playerRef.openedUIStats == false))
        {
            if (GameIsPaused && (playerRef.openedUIPause == true))             
            {
                Debug.Log("Stats UI is: " + playerRef.openedUIStats);
                playerRef.openedUIPause = false;
                Resume();
                Debug.Log("Pause Close");
            }
            else if (playerRef.openedUIPause == false)
            {
                Debug.Log("Stats UI is: " + playerRef.openedUIStats);
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
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        PlayerPrefs.SetFloat("interact_range", 0f);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void SaveGame()
    {
        //When we save we want to take the position and we will load it as the starting position as well as scene
        playerposition = playerRef.transform.position;
        int activeScene = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("ActiveScene", activeScene);
        PlayerPrefs.SetFloat("x", playerposition.x);
        PlayerPrefs.SetFloat("y", playerposition.y);
        PlayerPrefs.SetInt("savedHP", PlayerPrefs.GetInt("playerHPnow"));
        PlayerPrefs.SetInt("savedMP", PlayerPrefs.GetInt("playerMPnow"));

        Debug.Log("Position x: " + PlayerPrefs.GetFloat("x") + " Position y: " + PlayerPrefs.GetFloat("y"));
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
            StartCoroutine(LoadNewScene(PlayerPrefs.GetInt("ActiveScene")));
            playerRef.loadGame = true;
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
        playerRef.openedUIPause = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("Menu");
        

    }
    
}
