using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Milestone3.Bussiness_Layer.Logic;
using Task = Milestone3.Bussiness_Layer.Logic.Task;

public class TaskCreationDateComperator : IComparer<Task>
{
    public int Compare(Task x, Task y)
    {

        return DateTime.Compare(x.getCreationDate(), y.getCreationDate());
    }
}

public class TaskDueDateComperator : IComparer<Task>
{
    public int Compare(Task x, Task y)
    {

        return DateTime.Compare(x.getDueDate(), y.getDueDate());
    }
}
