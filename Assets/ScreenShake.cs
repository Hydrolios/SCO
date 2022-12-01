using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    // Transform of the GameObject you want to shake
    private Transform transforming;

    // Desired duration of the shake effect
    private float shakeDuration = 1.5f;

    // A measure of magnitude for the shake. Tweak based on your preference
    private float shakeMagnitude = 0.5f;

    // A measure of how quickly the shake effect should evaporate
    private float dampingSpeed = 1.0f;

    // The initial position of the GameObject
    Vector3 initialPosition;

    public bool triggered;

    void Awake()
    {
        if (transforming == null)
        {
            transforming = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        initialPosition = transforming.localPosition;
    }

    public void Update()
    {
        triggered = PlayerPrefs.GetInt("caveshake") != 0;
        if (shakeDuration > 0)
        {
            transforming.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
            Debug.Log("Shake triggering");
        }
        else
        {
            shakeDuration = 0f;
            transforming.localPosition = initialPosition;
            Debug.Log("Shake over");
        }

    }

    public void TriggerShake()
    {
        if (!triggered)
        {
            shakeDuration = 2.0f;
            triggered = true;
            PlayerPrefs.SetInt("caveshake", (triggered ? 1 : 0));
            Debug.Log("shake triggered");
            
        }
    }


}
