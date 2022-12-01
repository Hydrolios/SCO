using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsText : MonoBehaviour
{
    public GameObject player;
    public Player playerRef;
    public UnitStats statRef;
    public Text stat;
    public bool rad;
    public bool sol;
    public bool enx;
    public bool chr;
    public bool hp;
    public bool mp;
    public bool exp;
    public bool lvl;
    public bool skillpts;
    // Start is called before the first frame update
    void Update()
    {
        playerRef = player.GetComponent<Player>();
        if (chr)
        {
            stat.text = PlayerPrefs.GetInt("playerchr").ToString();
        }
        else if (rad)
        {
            stat.text = PlayerPrefs.GetInt("playerrad").ToString();
        }
        else if (sol)
        {
            stat.text = PlayerPrefs.GetInt("playersol").ToString();
        }
        else if (enx)
        {
            stat.text = PlayerPrefs.GetInt("playerenx").ToString();
        }
        else if (hp)
        {
            stat.text = PlayerPrefs.GetInt("playerHPnow").ToString() + "/" + PlayerPrefs.GetInt("playerHPMax").ToString();
        }
        else if (mp)
        {
            stat.text = PlayerPrefs.GetInt("playerMPnow").ToString() + "/" + PlayerPrefs.GetInt("playerMPMax").ToString();
        }
        else if (exp)
        {
            stat.text = PlayerPrefs.GetInt("exp").ToString() + "/" + PlayerPrefs.GetInt("exptolevel");
        }
        else if (lvl)
        {
            stat.text = PlayerPrefs.GetInt("playerlevel").ToString();
        }
        else if (skillpts)
        {
            stat.text = PlayerPrefs.GetInt("skillpts").ToString();
        }
        else
        {
            Debug.Log("invalid stats");
        }
        
    }

}
