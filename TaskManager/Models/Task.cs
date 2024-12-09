using System;

namespace TaskManager.Models;

public enum TaskPriority
{
    Low, Medium, High
}

public class Task
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateEnd{ get; set; }
    public TaskPriority Priority { get; set; }
}