using DG.Tweening;
using UnityEngine;

public class ShowDiscardButton : MonoBehaviour
{
    [SerializeField] private GameObject button;
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private Ease ease;

    [SerializeField] private UITaskCreationScreen taskCreationScreen;

    private void OnEnable()
    {
        button.transform.localPosition = button.transform.localPosition - new Vector3(0, 300, 0);
        button.transform.localScale = Vector3.zero;
        button.transform.DOScale(1f, duration).SetEase(ease);
        button.transform.DOLocalMoveY(-1021, duration).SetEase(ease);
    }

    public void CancelTaskScreation()
    {
        taskCreationScreen.ResetAfterCreation();
        button.SetActive(false);
    }
}
