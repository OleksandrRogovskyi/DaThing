using DG.Tweening;
using TMPro;
using UnityEngine;

public class Anim_ShowDiscardButton : MonoBehaviour
{
    [SerializeField] private GameObject button;
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private Ease ease;

    [SerializeField] private TMP_InputField titleInputField;

    private void OnEnable()
    {
        button.transform.localPosition = button.transform.localPosition - new Vector3(0, 300, 0);
        button.transform.localScale = Vector3.zero;
        button.transform.DOScale(1f, duration).SetEase(ease);
        button.transform.DOLocalMoveY(-1021, duration).SetEase(ease);
    }

    public void CancelTaskScreation()
    {
        titleInputField.text = "";
        button.SetActive(false);
    }
}
