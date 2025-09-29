using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public TaskList taskList;

    void Start()
    {
        taskList = TaskStorage.LoadTasks();
    }

    public void AddTaskToStorage(string title, string description, System.DateTime time)
    {
        TaskData newTask = new TaskData
        {
            id = taskList.tasks.Count > 0 ? taskList.tasks[^1].id + 1 : 1,
            title = title,
            description = description,
            done = false,
            dueDate = time.ToString()
        };

        taskList.tasks.Add(newTask);
        TaskStorage.SaveTasks(taskList);
    }

    [ContextMenu("Clear All Tasks")]
    public void ClearAllTasks()
    {
        taskList.tasks.Clear();
        TaskStorage.SaveTasks(taskList);
    }

    public void MarkTaskAsDone(int id)
    {
        var task = taskList.tasks.Find(t => t.id == id);
        if (task != null)
        {
            task.done = true;
            TaskStorage.SaveTasks(taskList);
        }
    }

    private void OnApplicationQuit()
    {
        TaskStorage.SaveTasks(taskList);
    }
}
