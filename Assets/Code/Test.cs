using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private GameObject test;

    public void Bruh()
    {
        if (test.activeSelf == false)
        {
            test.SetActive(true);
        }
        else
        {
            test.SetActive(false);
        }
    }
}
