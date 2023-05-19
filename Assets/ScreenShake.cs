using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{

    public float shakeAmount = 0.2f;
    public float shakeDuration = 0.5f;

    private Vector3 originalPos;
    public CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        Debug.Log("Start of screen shake script");
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        originalPos = virtualCamera.transform.localPosition;
        StartCoroutine(ShakeScreen());
        /*if (PlayerPrefs.GetInt("tunnel1Shake", 0) == 0)
        {
            StartCoroutine(ShakeScreen());
        }*/
        
    }

    IEnumerator ShakeScreen()
    {
        PlayerPrefs.SetInt("tunnel1Shake", 1);
        yield return new WaitForSeconds(0.25f);
        Debug.Log("shaking");
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeAmount;
            float y = Random.Range(-1f, 1f) * shakeAmount;
            virtualCamera.transform.localPosition = originalPos + new Vector3(x, y, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        virtualCamera.transform.localPosition = originalPos;
    }

}