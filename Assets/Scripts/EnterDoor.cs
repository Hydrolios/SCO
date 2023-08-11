using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDoor : MonoBehaviour
{

    //My scene transition script file
    public GameObject player;
    public Player playerRef;
    public GameObject fadeOut;
    public string sceneToLoad;
    public Vector2 playerPosition;
    public VectorValue playerStorage;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            Debug.Log("door triggered");
            InventoryManager inventoryManager = FindObjectOfType<InventoryManager>(); // gets the inventoryManager in the scene
            inventoryManager.SaveInventoryScene();

            fadeOut.SetActive(true);
            StartCoroutine(FadeOutWait());

            if(sceneToLoad == "Sunstead")
            {
                PlayerPrefs.SetInt("Sunstead_unlocked", 1);
            }

            

        }
    }

    IEnumerator FadeOutWait()
    {
        playerRef.StartCoroutine(playerRef.FadeIN());
        playerRef.StopSpeed();
        yield return new WaitForSeconds(1f);
        playerStorage.initialValue = playerPosition;
        SceneManager.LoadScene(sceneToLoad);
        
    }
}
