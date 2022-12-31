using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheckButtonClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.Instance.CanGameStart = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StartCoroutine(WaitForStart());
    }

    IEnumerator WaitForStart()
    {
        yield return new WaitForEndOfFrame();
        GameManager.Instance.CanGameStart = true;
    }
}