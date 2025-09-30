[System.Serializable]
public class DailyTaskData
{
    public int id;
    public string title;
    public string description;
    public bool done;
    public bool repeat;
    public TaskRepeatDays repeatDays;
    public System.TimeSpan repeatTime;
}
