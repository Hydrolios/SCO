using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{

    public float shakeAmount = 0.2f;
    public float shakeDuration = 0.5f;

    public Player playerref;
    private Vector3 originalPos;
    public CinemachineVirtualCamera virtualCamera;

    public bool hasShaken = false;


    public void ShakeEffect()
    {
        PlayerPrefs.SetInt("tunnel1Shake", 0);
        if (!hasShaken && PlayerPrefs.GetInt("tunnel1Shake", 0) == 0 ) // makes it shake only once, re-entering the scene does not cause shake
        {
            Debug.Log("beginning shake");
            StartCoroutine(ShakeScreen());
            hasShaken = true;
        }
    }

    IEnumerator ShakeScreen()
    {
        //PlayerPrefs.SetInt("tunnel1Shake", 1);
        yield return new WaitForSeconds(0.25f);
        Debug.Log("shaking");
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            
            float x = Random.Range(-1f, 1f) * shakeAmount;
            float y = Random.Range(-1f, 1f) * shakeAmount;
            virtualCamera.transform.localPosition = originalPos + new Vector3(x, y, 0f);
            elapsed += Time.deltaTime;
            playerref.StopSpeed();
            yield return null;
        }
        playerref.ResumeSpeed();
        virtualCamera.transform.localPosition = originalPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            //Debug.Log("Start of screen shake script");
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            originalPos = virtualCamera.transform.localPosition;
            Debug.Log("player encountered");
            ShakeEffect();
        }
    }
}