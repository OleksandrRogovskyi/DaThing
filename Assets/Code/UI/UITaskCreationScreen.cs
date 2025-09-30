using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITaskCreationScreen : MonoBehaviour
{
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private Ease ease;

    private TaskManager taskManager;
    private Notification_Manager notificationManager;

    [SerializeField] private TMP_InputField titleInputField;
    [SerializeField] private TMP_InputField descriptionInputField;
    [SerializeField] private ToggleButton repeat;
    [SerializeField] private GameObject discardButton;
    [SerializeField] private GameObject savebutton;

    [SerializeField] private List<Transform> repeatDays = new List<Transform>();
    [SerializeField] private GameObject repeatDaysSection;

    private void Start()
    {
        taskManager = FindAnyObjectByType<TaskManager>(FindObjectsInactive.Include);
        notificationManager = FindAnyObjectByType<Notification_Manager>(FindObjectsInactive.Include);
        InputValidationCheck();
        ToggleRepeatDays();
    }

    public void SaveNewTask()
    {
        taskManager.AddTaskToStorage(titleInputField.text, descriptionInputField.text, repeat.value, GetRepeatDays(), System.TimeSpan.MaxValue);

        var newTask = taskManager.taskList.tasks[taskManager.taskList.tasks.Count - 1];
        //notificationManager.ScheduleNotification(newTask.title, newTask.description, newTask.date);
        foreach (var day in newTask.repeatDays.value)
        {
            Debug.Log(day);
        }

        titleInputField.text = "";
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
        }
    }
}
