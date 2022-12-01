using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Battle, Dialogue}

public class GameController : MonoBehaviour
{
    [SerializeField] Player playercontroller;
    public GameObject ragetutor;



    GameState state;

    private void Start()
    {
        if (PlayerPrefs.GetInt("learnedrage") != 0)
        {
            ragetutor.SetActive(false);
        }
        else
        {
            ragetutor.SetActive(true);
        }
        DialogueManager.Instance.OnShowDialogue += () =>
        {
            state = GameState.Dialogue;
        };

        DialogueManager.Instance.OnCloseDialogue += () =>
        {
            if (state == GameState.Dialogue)
            {
                state = GameState.FreeRoam;
            }
        };
    }
    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playercontroller.HandleUpdate();
        }

        else if (state == GameState.Dialogue)
        {
            DialogueManager.Instance.HandleUpdate();
        }
    }

}
