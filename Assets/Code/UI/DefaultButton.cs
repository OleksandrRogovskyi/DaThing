using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DefaultButton : UIButtonEvents
{
    public UnityEngine.Events.UnityEvent onClick;
    public UnityEngine.Events.UnityEvent onPressAndHold;
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private Ease ease;

    private Image image;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Transform content;

    [SerializeField] private float holdDuration = 1f;
    private Coroutine holdCoroutine;

    private bool greyedOut = false;

    private void Awake()
    {
        image = content.GetComponent<Image>();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        holdCoroutine = StartCoroutine(HoldRoutine());

        if (greyedOut) return;
        image.DOFade(0.8f, duration).SetEase(ease);
        content.DOScale(0.95f, duration).SetEase(ease);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            holdCoroutine = null;

            if (greyedOut) return;
            ResetButton();
            onClick?.Invoke();
        }
    }

    private void FadeToTransparent()
    {
        image.DOFade(0f, duration).SetEase(ease);
        text.DOFade(0f, duration).SetEase(ease);
    }

    private void FadeToGreyOut()
    {
        image.DOFade(0.4f, duration).SetEase(ease);
    }

    private void FadeToOpaque()
    {
        image.DOFade(1f, duration).SetEase(ease);
        text.DOFade(1f, duration).SetEase(ease);
    }

    private void ResetButton()
    {
        FadeToOpaque();
        content.DOScale(1f, duration).SetEase(ease);
    }

    public override void OnDrag(PointerEventData eventData)
    {

    }

    private IEnumerator HoldRoutine()
    {
        yield return new WaitForSeconds(holdDuration);

        OnHold();
        holdCoroutine = null;
    }

    private void OnHold()
    {
        onPressAndHold?.Invoke();
        ResetButton();
    }

    public void SetGreyedOut(bool greyOut)
    {
        if (greyedOut == greyOut) return;
        greyedOut = greyOut;
        if (greyedOut)
        {
            FadeToGreyOut();
        }
        else
        {
            FadeToOpaque();

        }
    }
}
