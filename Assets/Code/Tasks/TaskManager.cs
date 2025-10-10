using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public TaskList taskList;

    void Start()
    {
        taskList = TaskStorage.LoadTasks();
    }

    public void AddTaskToStorage(string title, string description, bool repeat, TaskRepeatDays repeatDays, System.DateTime startTime, System.DateTime endTime, System.DateTime randomTime)
    {
        DailyTaskData newTask = new DailyTaskData
        {
            id = taskList.tasks.Count,
            title = title,
            description = description,
            done = false,
            repeat = repeat,
            repeatDays = repeatDays,
            startTime = startTime,
            endTime = endTime,
            randomTime = randomTime
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
