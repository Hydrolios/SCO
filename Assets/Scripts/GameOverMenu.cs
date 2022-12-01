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
            int activeScene = PlayerPrefs.GetInt("ActiveScene");

            float x = PlayerPrefs.GetFloat("x");
            float y = PlayerPrefs.GetFloat("y");
            playerRef.sceneTransitioned = false;
            playerRef.openedUIPause = false;
            GameIsPaused = false;
            Debug.Log(playerRef.sceneTransitioned);
            Vector2 playerposition = new Vector2(x, y);
            StartCoroutine(LoadNewScene(activeScene));
            playerRef.transform.position = playerposition;
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
