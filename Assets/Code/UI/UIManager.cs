using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    //[SerializeField] private UnityEvent onSmth;

    [SerializeField] private List<GameObject> toEnable = new List<GameObject>();
    [SerializeField] private List<GameObject> toDisable = new List<GameObject>();

    private void Start()
    {
        Application.targetFrameRate = 120;

        foreach (var screen in toEnable)
        {
            screen.SetActive(true);
        }
        foreach (var screen in toDisable)
        {
            screen.SetActive(false);
        }
    }
}
