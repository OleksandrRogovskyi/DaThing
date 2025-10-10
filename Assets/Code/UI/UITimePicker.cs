using DG.Tweening; // ✅ Make sure you have DOTween imported
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UITimePicker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public UnityEngine.Events.UnityEvent onValueChanged;

    [SerializeField] private int count;
    [SerializeField] private GameObject timeTextPrefab;
    [SerializeField] private RectTransform container;
    [SerializeField] private float duration = 0.25f;

    private bool isDragging = false;
    private Vector2 dragStartPos;
    private Vector3 containerStartPos;
    private float itemHeight;
    private int currentIndex = -1;
    private Tween snapTween;

    private List<GameObject> items = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < count; i++)
        {
            var timeItem = Instantiate(timeTextPrefab, container);
            timeItem.GetComponent<TextMeshProUGUI>().text = i.ToString("D2");
            items.Add(timeItem);
        }
        itemHeight = timeTextPrefab.GetComponent<RectTransform>().rect.height;
        container.localPosition = new Vector3(0, -count * itemHeight / 2 + itemHeight / 2, 0);

        UpdateCenteredIndex();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        snapTween?.Kill();
        dragStartPos = eventData.position;
        containerStartPos = container.localPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector2 currentPos = eventData.position;
        float deltaY = currentPos.y - dragStartPos.y;
        float newY = containerStartPos.y + deltaY;
        float maxY = (count - 1) * itemHeight / 2f + itemHeight / 2f;
        float minY = -maxY;
        newY = Mathf.Clamp(newY, minY, maxY);

        container.localPosition = new Vector3(0, newY, 0);

        UpdateCenteredIndex();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;

        int targetIndex = GetCenteredIndex();
        float targetY = GetSnapYPosition(targetIndex);

        snapTween?.Kill();
        snapTween = container.DOLocalMoveY(targetY, duration).SetEase(Ease.OutQuad);
    }

    private int GetCenteredIndex()
    {
        float containerY = container.localPosition.y;
        float halfHeight = (count - 1) * itemHeight / 2f;
        float relativeY = halfHeight + containerY;
        int index = Mathf.RoundToInt(relativeY / itemHeight);
        return Mathf.Clamp(index, 0, count - 1);
    }

    private float GetSnapYPosition(int index)
    {
        float halfHeight = (count - 1) * itemHeight / 2f;
        return -halfHeight + index * itemHeight;
    }

    private void UpdateCenteredIndex()
    {
        int newIndex = GetCenteredIndex();
        if (newIndex != currentIndex)
        {
            currentIndex = newIndex;
            OnTimeValueSelected(newIndex);
        }
    }

    private void OnTimeValueSelected(int index)
    {
        onValueChanged?.Invoke();
        VibrationManager.Instance?.Vibrate(0.1f, 100);

        foreach (var item in items)
        {
            item.transform.DOKill();
            item.transform.localScale = Vector3.one;
        }

        items[index].transform.DOScale(1.4f, duration / 2).OnComplete(() =>
        {
            items[index].transform.DOScale(1f, duration / 2);
        });
    }

    public int GetTimeValue()
    {
        return currentIndex;
    }
}
