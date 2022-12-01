using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialogue dialogue;
    [SerializeField] Dialogue dialogue2;
    [SerializeField] Dialogue dialogue3;

    public HealthBar hpbar;
    public ManaBar mpbar;
    public Player playerRef;

    //bool conditions to seperate different instances for different uses
    public string sceneToLoad;
    public bool shopKeeper;
    public bool expgiver;
    public bool rbdreport;
    public bool elder;
    public bool soldier;
    public bool sleep;
    public bool sceneChangeReq; // check if a scene change is required 
    public Vector2 playerPosition;
    public VectorValue playerStorage;
    public void Interact()
    {
        playerRef.StopSpeed();
        if(elder)
        {     
            if (PlayerPrefs.GetInt("rbdreport") != 0) //dialogue for reporting back to elder after shade is killed
            {
                StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue3, sceneToLoad, sceneChangeReq, shopKeeper, expgiver));
            }
            else if (PlayerPrefs.GetInt("shadeKilled") != 0) //dialogue for coming back to elder
            {
                StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue2, sceneToLoad, sceneChangeReq, shopKeeper, expgiver));
                rbdreport = true;
                PlayerPrefs.SetInt("rbdreport", rbdreport ? 1 : 0);
            } 
            else
            {
                StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue, sceneToLoad, sceneChangeReq, shopKeeper, expgiver));
            }
        }
        else if (soldier)
        {
            if (rbdreport)
            {
                playerStorage.initialValue = playerPosition;
                StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue, sceneToLoad, sceneChangeReq, shopKeeper, expgiver));
                
                Debug.Log("rbdreported");
            }
            else
            {
                StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue, sceneToLoad, sceneChangeReq, shopKeeper, expgiver));
            }

        }
        else
        {
            StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue, sceneToLoad, sceneChangeReq, shopKeeper, expgiver));
            if(sleep)
            {
                PlayerPrefs.SetInt("playerHPnow", PlayerPrefs.GetInt("playerHPMax"));
                PlayerPrefs.SetInt("playerMPnow", PlayerPrefs.GetInt("playerMPMax"));
                hpbar.SetHealth();
                mpbar.SetHealth();
            }

        }
    }

}
