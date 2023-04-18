using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDoorRBD : MonoBehaviour
{

    //My scene transition script file
    public GameObject player;
    public Player playerRef;
    public GameObject fadeOut;
    public string sceneToLoad;
    public string sceneToLoad2;
    public Vector2 playerPosition;
    public VectorValue playerStorage;
    public EventCheck eventCheck;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            InventoryManager inventoryManager = FindObjectOfType<InventoryManager>(); // gets the inventoryManager in the scene
            inventoryManager.SaveInventoryScene();

            fadeOut.SetActive(true);
            StartCoroutine(FadeOutWait());

            

        }
    }
    IEnumerator FadeOutWait()
    {
        playerRef.StartCoroutine(playerRef.FadeIN());
        playerRef.StopSpeed();
        yield return new WaitForSeconds(1f);

        if (eventCheck.shadeKilled)
        {
            playerStorage.initialValue = playerPosition;
            SceneManager.LoadScene(sceneToLoad2);
        }
        else
        {
            playerStorage.initialValue = playerPosition;
            SceneManager.LoadScene(sceneToLoad);
        }
    }


}
