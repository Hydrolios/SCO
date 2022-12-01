using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoverManager : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    public RectTransform textWindow;

    public static Action<string, Vector2> onMouseHover;
    public static Action onMouseAway;


    private void OnEnable()
    {
        Debug.Log("0.25");
        onMouseHover += ShowText;
        onMouseAway += HideText;
    }

    private void OnDisable()
    {
        Debug.Log("0.5");
        onMouseHover -= ShowText;
        onMouseAway -= HideText;
    }
    // Start is called before the first frame update
    void Start()
    {
        HideText();
    }

    private void ShowText(string text, Vector2 mousePos)
    {
        Debug.Log("1");
        infoText.text = text;
        textWindow.sizeDelta = new Vector2(infoText.preferredWidth > 250 ? 250 : infoText.preferredWidth, infoText.preferredHeight); //adjustable window with fixed width

        textWindow.gameObject.SetActive(true);
        textWindow.transform.position = new Vector2(mousePos.x + textWindow.sizeDelta.x - 50, mousePos.y); // make sure text is not covering
    }

    private void HideText()
    {
        Debug.Log("2");
        infoText.text = default;
        textWindow.gameObject.SetActive(false);
    }


}
