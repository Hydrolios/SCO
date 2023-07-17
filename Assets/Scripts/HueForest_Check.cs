using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HueForest_Check : MonoBehaviour
{
    // script for blocking the character from progressing if they haven't spoken to Hue yet
    // (purely for immersion/story)
    public GameObject player;
    public Player playerRef;
    public Vector2 playerPosition;
    public EventCheck EventManager;
    public Dialogue dialogue;


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            Debug.Log("story check - hue encounter pt 1 - triggered");
            if(!EventManager.huespt1)
            {
                player.transform.position = new Vector2(player.transform.position.x, player.transform.position.y-1);
                StartCoroutine(DialogueManager.Instance.ShowDialogueV2(dialogue));
            }
        }
    }
}
