using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UIButtonEvents : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public abstract void OnDrag(PointerEventData eventData);

    public abstract void OnPointerDown(PointerEventData eventData);

    public abstract void OnPointerUp(PointerEventData eventData);
}
