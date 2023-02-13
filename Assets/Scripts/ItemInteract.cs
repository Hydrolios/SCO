using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteract : MonoBehaviour
{
    public GameObject itemMenu;
    private void ShowText(Vector2 mousePos)
    {
        itemMenu.SetActive(true);
        itemMenu.transform.position = new Vector2(mousePos.x + 25, mousePos.y); //show display at mouse
    }
}
