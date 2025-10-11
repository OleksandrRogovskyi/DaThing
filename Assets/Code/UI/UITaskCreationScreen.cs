using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITaskCreationScreen : MonoBehaviour
{
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private Ease ease;


    private TaskManager taskManager;
    private NotificationManager notificationManager;

    [SerializeField] private TMP_InputField titleInputField;
    [SerializeField] private TMP_InputField descriptionInputField;
    [SerializeField] private ToggleButton repeat;
    [SerializeField] private GameObject discardButton;
    [SerializeField] private GameObject savebutton;

    [SerializeField] private GameObject repeatDaysSection;
    [SerializeField] private List<Transform> repeatDays = new List<Transform>();

    [SerializeField] private int scheduleDays = 14;

    [SerializeField] private GameObject timePicker1;
    [SerializeField] private GameObject timePicker2;

    [SerializeField] private UITimePicker startHours;
    [SerializeField] private UITimePicker startMinutes;
    [SerializeField] private UITimePicker endHours;
    [SerializeField] private UITimePicker endMinutes;

    private void Start()
    {
        taskManager = FindAnyObjectByType<TaskManager>(FindObjectsInactive.Include);
        notificationManager = FindAnyObjectByType<NotificationManager>(FindObjectsInactive.Include);
        InputValidationCheck();
        ToggleRepeat();
    }

    public void SaveNewTask()
    {
        DateTime startTime = new DateTime(1, 1, 1, startHours.GetTimeValue(), startMinutes.GetTimeValue(), 00);
        DateTime endTime = new DateTime(1, 1, 1, endHours.GetTimeValue(), endMinutes.GetTimeValue(), 00);

        taskManager.AddTaskToStorage(titleInputField.text, descriptionInputField.text, repeat.value, GetRepeatDays(), startTime, endTime, GenerateNotificationSchedule(GetRepeatDays(), startTime, endTime));

        ScheduleNotification();
        ResetAfterCreation();
    }

    public void ResetAfterCreation()
    {
        titleInputField.text = "";
        descriptionInputField.text = "";
        repeat.value = false;
        repeat.ResetButton();
        ToggleRepeat();
    }

    private void ScheduleNotification()
    {
        var task = taskManager.taskList.tasks[taskManager.taskList.tasks.Count - 1];

        foreach (var time in task.scheduledNotifications)
        {
            Debug.Log(time);
        }

        //notificationManager.ScheduleNotification(task.title, task.description, task.randomTime);
    }

    public void InputValidationCheck()
    {
        if (!string.IsNullOrEmpty(titleInputField.text) && repeat.value && AtleastOneDaySelected() || !string.IsNullOrEmpty(titleInputField.text) && !repeat.value)
            savebutton.GetComponent<DefaultButton>().SetGreyedOut(false);
        else
            savebutton.GetComponent<DefaultButton>().SetGreyedOut(true);
    }

    public void TimeValidation()
    {
        int startHour = startHours.GetTimeValue();
        int startMinute = startMinutes.GetTimeValue();
        int endHour = endHours.GetTimeValue();
        int endMinute = endMinutes.GetTimeValue();

        DateTime startTime = new DateTime(1, 1, 1, startHour, startMinute, 0);
        DateTime endTime = new DateTime(1, 1, 1, endHour, endMinute, 0);

        if (endTime <= startTime)
        {
            if (startMinute >= 59)
            {
                endMinute = 0;
                endHour = startHour + 1;

                if (endHour >= 23)
                    endHour = 0;
            }
            else
            {
                endMinute = startMinute + 1;
                endHour = startHour;
            }

            endHours.SetTime(endHour);
            endMinutes.SetTime(endMinute);
        }
    }

    private bool AtleastOneDaySelected()
    {
        foreach (var day in repeatDays)
        {
            var toggle = day.GetComponentInChildren<ToggleButton>();
            if (toggle != null && toggle.value)
            {
                return true;
            }
        }
        return false;
    }

    private TaskRepeatDays GetRepeatDays()
    {
        var taskRepeatDays = new TaskRepeatDays();

        if (repeat.value)
        {
            for (int i = 0; i < repeatDays.Count && i < taskRepeatDays.value.Length; i++)
            {
                var toggle = repeatDays[i].GetComponentInChildren<ToggleButton>();
                taskRepeatDays.value[i] = toggle != null && toggle.value;
            }
        }

        return taskRepeatDays;
    }

    public void ToggleRepeat()
    {
        if (repeat.value)
        {
            foreach (var day in repeatDays)
            {
                repeatDaysSection.SetActive(true);
                day.DOScale(1f, duration).SetEase(ease);
                day.gameObject.GetComponentInChildren<ToggleButton>().value = false;
                day.gameObject.GetComponentInChildren<ToggleButton>().ResetButton();
            }

            timePicker1.transform.DOScale(1f, duration).SetEase(ease);
            timePicker2.transform.DOScale(1f, duration).SetEase(ease);

            startHours.SetTime(DateTime.Now.Hour);
            startMinutes.SetTime(DateTime.Now.Minute);
            endHours.SetTime(DateTime.Now.Hour);
            endMinutes.SetTime(DateTime.Now.Minute + 1);
        }
        else
        {
            foreach (var day in repeatDays)
            {
                day.DOScale(0f, duration).SetEase(ease).OnComplete(() =>
                {
                    repeatDaysSection.SetActive(false);
                });
            }
            timePicker1.transform.DOScale(0f, duration).SetEase(ease);
            timePicker2.transform.DOScale(0f, duration).SetEase(ease);

            startHours.SetTime(0);
            startMinutes.SetTime(0);
            endHours.SetTime(0);
            endMinutes.SetTime(0);
        }
    }

    private DateTime GetRandomTime(DateTime start, DateTime end, DateTime targetDate)
    {
        TimeSpan range = end - start;
        if (range.TotalSeconds <= 0)
            range = TimeSpan.FromHours(1);

        double randomSeconds = UnityEngine.Random.Range(0f, (float)range.TotalSeconds);
        DateTime result = new DateTime(targetDate.Year, targetDate.Month, targetDate.Day, start.Hour, start.Minute, 0)
            .AddSeconds(randomSeconds);

        return result;
    }
    private List<DateTime> GenerateNotificationSchedule(TaskRepeatDays days, DateTime startTime, DateTime endTime)
    {
        List<DateTime> scheduledTimes = new List<DateTime>();

        DateTime today = DateTime.Now.Date;

        for (int i = 0; i < scheduleDays; i++)
        {
            DateTime targetDay = today.AddDays(i);
            int dayOfWeek = (int)targetDay.DayOfWeek;

            if (days.value[dayOfWeek])
            {
                DateTime randomTime = GetRandomTime(startTime, endTime, targetDay);

                scheduledTimes.Add(randomTime);

                /*notificationManager.ScheduleNotification(task.title, task.description, randomTime);
                Debug.Log($"Scheduled '{task.title}' on {randomTime}");*/
            }
        }

        return scheduledTimes;
    }
}
