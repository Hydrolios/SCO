using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AreaNameChange : MonoBehaviour
{
    public GameObject areaname;
    public Player player;

    private void Start()
    {
        TextMeshProUGUI areanametxt = areaname.GetComponent<TextMeshProUGUI>();
        float playerY = player.transform.position.y;
        if (playerY > 14.5)
        {
            Debug.Log("started in north side");
            areanametxt.text = "Hue's Settlement";
            
        }
        else
        {
            Debug.Log("started in south side");
            areanametxt.text = "Entrance to Hue's Settlement";
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            TextMeshProUGUI areanametxt = areaname.GetComponent<TextMeshProUGUI>();
            float playerY = player.transform.position.y;
            // Update the HUD text or perform any other desired actions
            if (playerY < 14.5)
            {
                Debug.Log("coming from north side");
                areanametxt.text = "Entrance to Hue's Settlement";
            }
            else if (playerY > 14.5)
            {
                Debug.Log("coming from south side");
                areanametxt.text = "Hue's Settlement";
            }
        }
    }
}
