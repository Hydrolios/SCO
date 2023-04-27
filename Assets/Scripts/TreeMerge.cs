using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeMerge : MonoBehaviour
{

    public GameObject[] treePrefabs;
    void Start()
    {
        foreach (GameObject treePrefab in treePrefabs)
        {
            GameObject treeInstance = Instantiate(treePrefab, transform);
        }
    }
}
