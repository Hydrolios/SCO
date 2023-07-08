using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceBlock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("fastTravelUnlocked") != 1)
        {
            transform.position = new Vector2(1000, 1000);
        }
    }

}
