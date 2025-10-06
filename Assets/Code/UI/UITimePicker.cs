using UnityEngine;
using UnityEngine.EventSystems;

public class UITimePicker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public UnityEngine.Events.UnityEvent onValueChanged;

    [SerializeField] int count;
    [SerializeField] GameObject timeText;
    [SerializeField] private Transform container;

    private bool isDragging = false;
    private Vector2 dragStartPos;
    private Vector3 containerStartPos;

    private void Start()
    {
        for (int i = 0; i < count; i++)
        {
            var timeItem = Instantiate(timeText, container);
            timeItem.GetComponent<TMPro.TextMeshProUGUI>().text = i.ToString("D2");
        }

        var timeItemHeight = timeText.GetComponent<RectTransform>().rect.height;
        container.localPosition = new Vector3(0, -count * timeItemHeight / 2 + timeItemHeight / 2, 0);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        dragStartPos = eventData.position;
        containerStartPos = container.localPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector2 currentPos = eventData.position;
        float deltaY = currentPos.y - dragStartPos.y;

        container.localPosition = containerStartPos + new Vector3(0, deltaY, 0);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }
}
