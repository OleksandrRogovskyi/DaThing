using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleButton : UIButtonEvents
{
    public UnityEngine.Events.UnityEvent onValueChanged;

    public bool value = false;
    [SerializeField] private Transform tick;

    [SerializeField] private float duration = 0.2f;
    [SerializeField] private Ease ease;

    private Image image;
    [SerializeField] private Transform content;

    private void Awake()
    {
        image = content.GetComponent<Image>();
        ResetButton();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        image.DOFade(0.8f, duration).SetEase(ease);
        content.DOScale(0.95f, duration).SetEase(ease);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        value = !value;
        ResetButton();
        onValueChanged?.Invoke();
    }

    public void ResetButton()
    {
        image.DOFade(1f, duration).SetEase(ease);
        content.DOScale(1f, duration).SetEase(ease);

        if (value)
        {
            tick.DOScale(1f, duration).SetEase(ease);
        }
        else
        {
            tick.DOScale(0f, duration).SetEase(ease);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {

    }
}
