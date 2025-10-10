using UnityEngine;

public class Test : MonoBehaviour
{
    public void TestSmth()
    {
        VibrationManager.Instance.Vibrate(0.1f, 100);
    }
}
