using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCManager : MonoBehaviour
{
	public GameObject battling = null;
	public bool loaded;

	private static NPCManager _Instance;


    private void Awake()
    {
        if(_Instance == null)
        {
			_Instance = this;
			DontDestroyOnLoad(gameObject);
        }
		else if (_Instance != this)
        {
			Destroy(gameObject);
        }
    }

    private void Start()
    {
        battling = null;
    }

    public void Encountered(GameObject npc)
    {

		battling = npc;
       
    }

}