using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlidingButton : UIButtonEvents
{
    [HideInInspector] public int id;

    private bool greyedOut = false;

    [SerializeField] private float duration = 0.2f;
    [SerializeField] private float slidingDuration = 1f;
    [SerializeField] private Ease ease;
    [SerializeField] private float swipeThreshold = 100f;
    [SerializeField] private float discardDistance = 300f;

    private Vector2 dragStartPos;
    private bool isDragging;
    private bool isPressed;
    private bool isScaledDown = false;
    float deltaX;

    private Image image;
    [SerializeField] public TextMeshProUGUI info;
    [SerializeField] public TextMeshProUGUI time;
    [SerializeField] private Transform content;

    private void Awake()
    {
        image = content.GetComponent<Image>();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (greyedOut) return;

        isPressed = true;
        image.DOFade(0.8f, duration).SetEase(ease);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (greyedOut) return;

        if (Mathf.Abs(deltaX) >= swipeThreshold)
        {
            if (deltaX > 0)
            {
                OnSwipeRight();
            }
        }
        else
        {
            ResetButton();
            FadeToOpaque();
        }

        isDragging = false;
        isPressed = false;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (greyedOut) return;

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

            if (Mathf.Abs(deltaX) > swipeThreshold && !isScaledDown)
            {
                isScaledDown = true;
                content.DOScale(0.9f, 0.1f).SetEase(ease);
                if (Application.isMobilePlatform)
                {
                    VibrationManager.Instance.Vibrate(0.4f, 100);
                }
            }
            else if (Mathf.Abs(deltaX) < swipeThreshold - 100 && isScaledDown)
            {
                isScaledDown = false;
                content.DOScale(1f, 0.1f).SetEase(ease);
            }
        }
    }

    private void OnSwipeRight()
    {
        transform.DOLocalMoveX(transform.localPosition.x + discardDistance, slidingDuration).SetEase(ease).OnComplete(() =>
        {
            DeleteButton();
        });
        FadeToTransparent();
    }

    private void FadeToTransparent()
    {
        image.DOFade(0f, slidingDuration).SetEase(ease);
        info.DOFade(0f, slidingDuration).SetEase(ease);
        time.DOFade(0f, slidingDuration).SetEase(ease);
    }

    public void FadeToGreyOut()
    {
        image.DOFade(0.4f, duration).SetEase(ease);
        info.DOFade(0.4f, duration).SetEase(ease);
        time.DOFade(0.4f, duration).SetEase(ease);
    }

    private void FadeToOpaque()
    {
        image.DOFade(1f, slidingDuration).SetEase(ease);
        info.DOFade(1f, slidingDuration).SetEase(ease);
        time.DOFade(1f, slidingDuration).SetEase(ease);
    }

    private void ResetButton()
    {
        content.DOScale(1f, duration).SetEase(ease);
        transform.DOLocalMoveX(0f, duration).SetEase(ease);
    }

    private void DeleteButton()
    {
        var taskManager = FindAnyObjectByType<TaskManager>(FindObjectsInactive.Include);
        taskManager.MarkTaskAsDone(id);

        if (taskManager.taskList.tasks[id].repeat)
        {
            SetGreyedOut(true);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetGreyedOut(bool greyOut)
    {
        if (greyedOut == greyOut) return;
        greyedOut = greyOut;
        if (greyedOut)
        {
            FadeToGreyOut();
            content.DOScale(1f, duration).SetEase(ease);
            transform.DOLocalMoveX(0f, slidingDuration).SetEase(ease);
        }
        else
        {
            FadeToOpaque();

        }
    }
}
