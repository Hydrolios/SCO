using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDoorRBD : MonoBehaviour
{

    //My scene transition script file
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
}
