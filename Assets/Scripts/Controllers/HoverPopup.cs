using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverPopup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string showText;
    public float deltaTime;
    public void OnPointerEnter(PointerEventData eventData) //for when UI is hovered
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
        //Debug.Log("hovered");
    }

    public void OnPointerExit(PointerEventData eventData) //for when UI is unhovered
    {
        StopAllCoroutines();
        HoverManager.onMouseAway();
        //Debug.Log("unhovered");
    }

    private void ShowMessages()
    {
        HoverManager.onMouseHover(showText, Input.mousePosition);
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(0f);
        ShowMessages();
    }
}
