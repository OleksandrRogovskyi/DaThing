using TMPro;
using UnityEngine;

public class UITaskCreationScreen : MonoBehaviour
{
    private TaskManager taskManager;
    [SerializeField] private TMP_InputField titleInputField;
    [SerializeField] private GameObject discardButton;
    [SerializeField] private GameObject savebutton;

    private void Start()
    {
        taskManager = FindAnyObjectByType<TaskManager>(FindObjectsInactive.Include);
        CheckIfNameIsNull();
    }

    public void SaveNewTask()
    {
        taskManager.AddTaskToStorage(titleInputField.text, "Description", System.DateTime.Now);
        titleInputField.text = "";
    }

    public void CheckIfNameIsNull()
    {
        savebutton.GetComponent<DefaultButton>().SetGreyedOut(string.IsNullOrEmpty(titleInputField.text));
    }
}
