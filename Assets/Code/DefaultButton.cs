using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DefaultButton : UIButtonEvents
{
    public UnityEngine.Events.UnityEvent onClick;
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private float slidingDuration = 1f;
    [SerializeField] private Ease ease = Ease.OutBack;
    [SerializeField] private float swipeThreshold = 100f;
    [SerializeField] private float discardDistance = 300f;

    private Vector2 dragStartPos;
    private bool isDragging;
    private bool isPressed;
    float deltaX;

    private Image image;
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        image = GetComponent<Image>();
        Application.targetFrameRate = 120;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        image.DOFade(0.8f, duration).SetEase(ease);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (isPressed && !isDragging)
        {
            onClick?.Invoke();
        }
        image.DOFade(1f, duration).SetEase(ease);


        if (Mathf.Abs(deltaX) >= swipeThreshold)
        {
            if (deltaX > 0)
            {
                OnSwipeRight();
            }

            isDragging = false;
        }
        else
        {
            ResetButton();
            isDragging = false;
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (!isDragging)
        {
            dragStartPos = eventData.pressPosition;
            isDragging = true;
        }

        Vector2 currentPos = eventData.position;
        deltaX = currentPos.x - dragStartPos.x;

        if (deltaX > 0)
        {
            transform.localPosition = new Vector3(deltaX, transform.localPosition.y, transform.localPosition.z);

            if (Mathf.Abs(deltaX) >= swipeThreshold)
            {
                transform.DOScale(0.9f, 0.1f).SetEase(ease);
            }
            else
            {
                transform.DOScale(1f, 0.1f).SetEase(ease);
            }
        }
    }
    private void OnSwipeRight()
    {
        transform.DOLocalMoveX(transform.localPosition.x + discardDistance, slidingDuration).SetEase(ease).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
        FadeToTransparent();
    }

    private void FadeToTransparent()
    {
        image.DOFade(0f, slidingDuration).SetEase(ease);
        text.DOFade(0f, slidingDuration).SetEase(ease);
    }

    private void FadeToOpaque()
    {
        image.DOFade(1f, slidingDuration).SetEase(ease);
        text.DOFade(1f, slidingDuration).SetEase(ease);
    }

    private void ResetButton()
    {
        transform.DOLocalMoveX(0f, duration).SetEase(ease);
        FadeToOpaque();
    }

}
