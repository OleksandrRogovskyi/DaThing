using System;
using System.Collections;
using Unity.Notifications.Android;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    private const string CHANNEL_ID = "tasks_channel";
    private const int DAYS_AHEAD_TO_SCHEDULE = 14;

    private TaskManager taskManager;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        taskManager = FindAnyObjectByType<TaskManager>(FindObjectsInactive.Include);
        InitializeChannel();
        StartCoroutine(RequestNotificationPermissionIfNeeded());

        //Invoke("RefreshNotifications", 0.5f);
    }

    private void InitializeChannel()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = CHANNEL_ID,
            Name = "Task Reminders",
            Importance = Importance.Default,
            Description = "Daily task reminders",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    private IEnumerator RequestNotificationPermissionIfNeeded()
    {
        var status = AndroidNotificationCenter.UserPermissionToPost;
        if (status == PermissionStatus.NotRequested)
        {
            var request = new PermissionRequest();
            while (request.Status == PermissionStatus.RequestPending)
                yield return null;
        }
    }

    /// <summary>
    /// Clears all notifications and re-schedules them for the upcoming days.
    /// </summary>
    public void RefreshNotifications()
    {
        AndroidNotificationCenter.CancelAllNotifications();

        if (taskManager == null || taskManager.taskList == null)
            return;

        foreach (var task in taskManager.taskList.tasks)
        {
            if (task.repeat)
            {
                ScheduleRepeatingTask(task);
            }
            else
            {
                // single occurrence task
                //ScheduleNotification(task.title, task.description, task.randomTime);
            }
        }

        Debug.Log("Notifications refreshed!");
    }

    /// <summary>
    /// Schedule notifications for the next N days according to repeatDays.
    /// </summary>
    private void ScheduleRepeatingTask(DailyTaskData task)
    {
        DateTime today = DateTime.Now.Date;

        for (int i = 0; i < DAYS_AHEAD_TO_SCHEDULE; i++)
        {
            DateTime targetDay = today.AddDays(i);
            int dayOfWeek = (int)targetDay.DayOfWeek;

            if (task.repeatDays.value[dayOfWeek])
            {
                DateTime randomTime = GetRandomTime(task.startTime, task.endTime, targetDay);
                ScheduleNotification(task.title, task.description, randomTime);
                Debug.Log($"Scheduled '{task.title}' on {randomTime}");
            }
        }
    }

    /// <summary>
    /// Generates a random time between the task's start and end times on a specific date.
    /// </summary>
    private DateTime GetRandomTime(DateTime start, DateTime end, DateTime targetDate)
    {
        // ignore date component of start/end
        TimeSpan range = end - start;
        if (range.TotalSeconds <= 0)
            range = TimeSpan.FromHours(1);

        double randomSeconds = UnityEngine.Random.Range(0f, (float)range.TotalSeconds);
        DateTime result = new DateTime(targetDate.Year, targetDate.Month, targetDate.Day, start.Hour, start.Minute, 0)
            .AddSeconds(randomSeconds);

        return result;
    }

    /// <summary>
    /// Actually sends the local notification.
    /// </summary>
    public int ScheduleNotification(string title, string message, DateTime fireTime)
    {
        if (fireTime <= DateTime.Now)
            return -1;

        var notification = new AndroidNotification()
        {
            Title = title,
            Text = message,
            FireTime = fireTime,
        };

        int id = AndroidNotificationCenter.SendNotification(notification, CHANNEL_ID);
        return id;
    }
}
