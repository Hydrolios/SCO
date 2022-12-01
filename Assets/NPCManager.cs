using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public GameObject npcprefab;
    public GameObject clonenpc;
    public static GameObject battling;
    public GameObject playerprefab;
    public GameObject cloneplayer;
    public Vector2 npcpos;

	private static NPCManager _Instance;
	public static NPCManager Instance
	{
		get
		{
			if (!_Instance)
			{
				// NOTE: read docs to see directory requirements for Resources.Load!
				var prefab = Resources.Load<GameObject>("Assets/Prefabs/Assets/RBDGuard_L.prefab");
				// create the prefab in your scene
				var inScene = Instantiate<GameObject>(prefab);
				// try find the instance inside the prefab
				_Instance = inScene.GetComponentInChildren<NPCManager>();
				// guess there isn't one, add one
				if (!_Instance) _Instance = inScene.AddComponent<NPCManager>();
				// mark root as DontDestroyOnLoad();
				DontDestroyOnLoad(_Instance.transform.root.gameObject);
			}
			return _Instance;
		}
	}
	private void Start()
    {
        clonenpc = Instantiate(npcprefab);
        clonenpc.transform.position = npcpos;
        battling = clonenpc;
		//cloneplayer = Instantiate(playerprefab);
		//NPC should call a method in NPC manager giving gameobject THIS 

	}

}
