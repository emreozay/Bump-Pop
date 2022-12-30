using UnityEngine;
using UnityEngine.EventSystems;

public class CheckButtonClick : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        DragAndShoot.isStopNow = true;
    }
}
