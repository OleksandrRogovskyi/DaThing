using TMPro;
using UnityEngine;

public class UITaskCreationScreen : MonoBehaviour
{
    private TaskManager taskManager;
    private Notification_Manager notificationManager;

    [SerializeField] private TMP_InputField titleInputField;
    [SerializeField] private GameObject discardButton;
    [SerializeField] private GameObject savebutton;

    private void Start()
    {
        taskManager = FindAnyObjectByType<TaskManager>(FindObjectsInactive.Include);
        notificationManager = FindAnyObjectByType<Notification_Manager>(FindObjectsInactive.Include);
        CheckIfNameIsNull();
    }

    public void SaveNewTask()
    {
        taskManager.AddTaskToStorage(titleInputField.text, "Just do it", System.DateTime.Now.AddSeconds(10));

        var newTask = taskManager.taskList.tasks[taskManager.taskList.tasks.Count - 1];
        notificationManager.ScheduleNotification(newTask.title, newTask.description, newTask.dueDate);

        titleInputField.text = "";
    }

    public void CheckIfNameIsNull()
    {
        savebutton.GetComponent<DefaultButton>().SetGreyedOut(string.IsNullOrEmpty(titleInputField.text));
    }
}
