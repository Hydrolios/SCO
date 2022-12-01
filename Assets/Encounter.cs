using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Encounter : MonoBehaviour
{
    [SerializeField] Dialogue dialogue;

    public static GameObject npcBattling;
    public GameObject player;
    public GameObject triggered;
    public string sceneToLoad;
    /*
    private void Awake() // for making this object a singleton
    {
        if (instance == null)
        {
            instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject); 
    }
    */
    public void Start()
    {
        npcBattling = this.gameObject;
        triggered.SetActive(false); //notification above NPC to display if they have been triggered by player entering line of sight
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            triggered.SetActive(true);
            StartCoroutine(DialogueManager.Instance.ShowDialogueV3(dialogue, sceneToLoad, true));
            
        }
    }
}
