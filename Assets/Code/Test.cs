using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private GameObject button;
    [SerializeField] private Transform where;

    public void CreateButton()
    {
        Instantiate(button, where);
    }
}
