using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDoorTav : MonoBehaviour
{

    //My scene transition script file for tavern hysteria
    public GameObject player;
    public string sceneToLoad;
    public string sceneToLoad2;
    public Vector2 playerPosition;
    public VectorValue playerStorage;
    public EventCheck eventCheck;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            if (eventCheck.rbdreport)
            {
                Debug.Log("rbdreported");
                playerStorage.initialValue = playerPosition;
                SceneManager.LoadScene(sceneToLoad2);
            }
            else
            {
                Debug.Log("not rbdreported");
                playerStorage.initialValue = playerPosition;
                SceneManager.LoadScene(sceneToLoad);
            }

        }
    }
}
