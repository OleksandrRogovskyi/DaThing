using UnityEngine;
using UnityEngine.EventSystems;

public class UITimePicker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private bool isDragging = false;
    private Vector2 dragStartPos;
    private float deltaY;

    [SerializeField] private Transform hours;

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging)
        {
            dragStartPos = eventData.pressPosition;
            isDragging = true;
        }

        Vector2 currentPos = eventData.position;
        deltaY = currentPos.y - dragStartPos.y;

        hours.localPosition = new Vector3(hours.localPosition.x, deltaY, hours.localPosition.z);
    }
    public void OnPointerDown(PointerEventData eventData)
    {

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }
}
