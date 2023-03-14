using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListingUI : MonoBehaviour
{
    public GameObject itemStatUI;

    public void OnButtonClick()
    {
        itemStatUI.SetActive(true);

    }

    public void CloseUI()
    {
        itemStatUI.SetActive(false);
    }
    
}
