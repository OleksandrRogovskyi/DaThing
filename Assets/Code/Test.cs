using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private GameObject test;

    public void TestSmth()
    {
        test.GetComponent<Notification_Manager>().ScheduleNotification("Test Title", "Test Message", System.DateTime.Now.AddSeconds(10));
    }
}
