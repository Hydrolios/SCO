using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    /*made this to try to manage all chest states so the state can persist between scenes without relying on playerprefs
     * since playerprefs is ran on applicationquit, I dont want chest data to persist without players explicitly saving
     * so this would prevent item lost in cases where the players open a chest, but doesn't save and quit
    */
    private static ChestManager instance;

    public static ChestManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ChestManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("ChestManager");
                    instance = go.AddComponent<ChestManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }

    private Dictionary<int, NPCController> chestDict = new Dictionary<int, NPCController>();

    private void Awake()
    {
        instance = this;
    }

    public void RegisterChest(NPCController chest)
    {
        if (!chestDict.ContainsKey(chest.chest_id))
        {
            chestDict.Add(chest.chest_id, chest);
        }
    }

    public void SaveChestStates()
    {
        foreach (NPCController chest in chestDict.Values)
        {
            //chest.SaveState();
        }
    }

    public void LoadChestStates()
    {
        foreach (NPCController chest in chestDict.Values)
        {
            //chest.LoadState();
        }
    }
}
