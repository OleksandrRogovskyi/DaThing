using System.Collections;
using Unity.Notifications.Android;
using UnityEngine;

public class Notification_Manager : MonoBehaviour
{
    const string CHANNEL_ID = "tasks_channel";

    void Start()
    {
        // Register the channel always (for Android 8+)
        var channel = new AndroidNotificationChannel()
        {
            Id = CHANNEL_ID,
            Name = "Tasks",
            Importance = Importance.Default,
            Description = "Task reminders",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        // If on Android 13+, request permission
        StartCoroutine(RequestNotificationPermissionIfNeeded());
    }

    IEnumerator RequestNotificationPermissionIfNeeded()
    {
        var status = AndroidNotificationCenter.UserPermissionToPost;
        if (status == PermissionStatus.NotRequested)
        {
            var request = new PermissionRequest();
            // Wait until user responds
            while (request.Status == PermissionStatus.RequestPending)
            {
                yield return null;
            }

            if (request.Status == PermissionStatus.Allowed)
            {
                Debug.Log("Notification permission granted");
            }
            else
            {
                Debug.LogWarning("Notification permission denied");
            }
        }
    }

    public int ScheduleNotification(string title, string message, System.DateTime fireTime)
    {
        var notification = new AndroidNotification()
        {
            Title = title,
            Text = message,
            FireTime = fireTime
        };

        return AndroidNotificationCenter.SendNotification(notification, CHANNEL_ID);
    }
}
