using System;
using System.Collections.Generic;

[System.Serializable]
public class DailyTaskData
{
    public int id;
    public string title;
    public string description;
    public bool done;
    public bool repeat;
    public TaskRepeatDays repeatDays;
    public System.DateTime startTime;
    public System.DateTime endTime;
    public List<DateTime> scheduledNotifications = new List<DateTime>();
}
