using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCredit : MonoBehaviour
{
    public string sceneToLoad;
    void Start()
    {
        Invoke("Transition", 84.05f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Transition();
        }
    }

    public void Transition()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
