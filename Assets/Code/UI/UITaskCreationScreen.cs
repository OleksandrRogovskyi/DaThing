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

    [SerializeField] private GameObject timePickerSection;

    [SerializeField] private UITimePicker startHours;
    [SerializeField] private UITimePicker startMinutes;
    [SerializeField] private UITimePicker endHours;
    [SerializeField] private UITimePicker endMinutes;

    private void Start()
    {
        taskManager = FindAnyObjectByType<TaskManager>(FindObjectsInactive.Include);
        notificationManager = FindAnyObjectByType<NotificationManager>(FindObjectsInactive.Include);
        InputValidationCheck();
        ToggleRepeatDays();
    }

    public void SaveNewTask()
    {
        DateTime startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, startHours.GetTimeValue(), startMinutes.GetTimeValue(), 00);
        DateTime endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, endHours.GetTimeValue(), endMinutes.GetTimeValue(), 00);

        taskManager.AddTaskToStorage(titleInputField.text, descriptionInputField.text, repeat.value, GetRepeatDays(), startTime, endTime, GetRandomTimePeriod(startTime, endTime));

        ScheduleNotification();
        ResetAfterCreation();
    }

    private void ResetAfterCreation()
    {
        titleInputField.text = "";
        descriptionInputField.text = "";
        repeat.value = false;
        repeat.ResetButton();
        ToggleRepeatDays();
    }

    private void ScheduleNotification()
    {
        var task = taskManager.taskList.tasks[taskManager.taskList.tasks.Count - 1];
        notificationManager.ScheduleNotification(task.title, task.description, task.randomTime);
    }

    public void InputValidationCheck()
    {
        if (!string.IsNullOrEmpty(titleInputField.text) && repeat.value && AtleastOneDaySelected() || !string.IsNullOrEmpty(titleInputField.text) && !repeat.value)
            savebutton.GetComponent<DefaultButton>().SetGreyedOut(false);
        else
            savebutton.GetComponent<DefaultButton>().SetGreyedOut(true);
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

    public void ToggleRepeatDays()
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

            timePickerSection.transform.DOScale(1f, duration).SetEase(ease);
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
            timePickerSection.transform.DOScale(0f, duration).SetEase(ease);
        }
    }

    public DateTime GetRandomTimePeriod(DateTime start, DateTime end)
    {
        if (end <= start)
        {
            Debug.LogWarning("End time must be after start time!");
            end = start.AddHours(1);
        }

        double totalSeconds = (end - start).TotalSeconds;
        double randomSeconds = UnityEngine.Random.Range(0f, (float)totalSeconds);
        DateTime randomTime = start.AddSeconds(randomSeconds);

        return randomTime;
    }

}
