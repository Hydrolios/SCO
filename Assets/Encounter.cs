using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Encounter : MonoBehaviour
{
    [SerializeField] Dialogue dialogue;

    public GameObject triggered;
    public string sceneToLoad;
    public static bool defeated;
    
    public string objectID;
    public string designatedScene;
    public Vector2 designatedPos;
    private void Awake()
    {
        objectID = name + transform.position.ToString();
    }
    

    void Start()
    {
        Debug.Log("DPos: " + designatedPos);
        Debug.Log("DScene: " + designatedScene);
        Debug.Log("CScene: " + SceneManager.GetActiveScene().name);
        triggered.SetActive(false); //notification above NPC to display if they have been triggered by player entering line of sight
        for (int i = 0; i < Object.FindObjectsOfType<Encounter>().Length; i++)
        {
            if(Object.FindObjectsOfType<Encounter>()[i] != this)
            {
                if (Object.FindObjectsOfType<Encounter>()[i].objectID == objectID)
                {
                    Destroy(gameObject);
                }
            }
        }
    
        DontDestroyOnLoad(gameObject);
        
    }
    private void Update()
    {
        if (defeated && triggered.activeInHierarchy)
        {
            Defeated(new Vector2(999f, 999f));
        }
        if (SceneManager.GetActiveScene().name != designatedScene)
        {
            transform.position = new Vector2 (999f, 999f);
        }
        else if(SceneManager.GetActiveScene().name == designatedScene)
        {
            transform.position = designatedPos;
        }    
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            defeated = false;
            triggered.SetActive(true);
            StartCoroutine(DialogueManager.Instance.ShowDialogueV3(dialogue, sceneToLoad, true));
            
            
        }
    }

    public void Defeated(Vector2 moveaway)
    {
        transform.position = moveaway;
        designatedPos = moveaway;
        triggered.SetActive(false);
        defeated = false;
    }
}
