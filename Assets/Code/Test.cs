using UnityEngine;

public class Test : MonoBehaviour
{
    public void Test1()
    {
        VibrationManager.Instance.Vibrate(1f, 100);
    }

    public void Test2()
    {
        VibrationManager.Instance.Vibrate(0.5f, 100);
    }

    public void Test3()
    {
        VibrationManager.Instance.Vibrate(0.001f, 100);
    }
}
