using UnityEngine;

public class UITaskScreen : MonoBehaviour
{
    private TaskManager taskManager;

    [SerializeField] private GameObject dailyTaskPrefab;
    [SerializeField] private Transform dailyTaskParent;

    private void Start()
    {
        Invoke("PopulateDailyTasks", 0.5f);
        taskManager = FindAnyObjectByType<TaskManager>(FindObjectsInactive.Include);
    }

    private void OnEnable()
    {
        PopulateDailyTasks();
    }

    private void PopulateDailyTasks()
    {
        for (int i = dailyTaskParent.childCount - 1; i >= 0; i--)
        {
            Transform child = dailyTaskParent.GetChild(i);
            Destroy(child.gameObject);
        }

        if (taskManager != null)
        {
            foreach (var task in taskManager.taskList.tasks)
            {
                if (!task.done)
                    SpawnDailyTask(task);
            }
        }
        else
        {
            Debug.LogWarning("TaskManager reference is missing in UITaskScreen.");
        }
    }

    private void SpawnDailyTask(TaskData task)
    {
        var newTask = Instantiate(dailyTaskPrefab, dailyTaskParent).GetComponent<SlidingButton>();
        newTask.info.text = task.title;
        newTask.id = task.id;
    }


}
