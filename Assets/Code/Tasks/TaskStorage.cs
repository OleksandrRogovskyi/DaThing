using System.IO;
using UnityEngine;

public class TaskStorage
{
    private static string filePath = Path.Combine(Application.persistentDataPath, "tasks.json");

    public static void SaveTasks(TaskList taskList)
    {
        string json = JsonUtility.ToJson(taskList, true); // pretty print
        File.WriteAllText(filePath, json);
        Debug.Log("Tasks saved to " + filePath);
    }

    public static TaskList LoadTasks()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<TaskList>(json);
        }
        else
        {
            return new TaskList();
        }
    }
}
