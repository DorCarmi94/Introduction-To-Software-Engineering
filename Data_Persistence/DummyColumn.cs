using System;
using System.Collections.Generic;

namespace Milestone2.Data_Persistence
{
    [Serializable]
    public class DummyColumn 
    {
        public String columnName;
        public String columnID;
        
        public List<DummyTask> tasksList;
        

        public DummyColumn(string columnName, string columnID)
        {
            this.columnName = columnName;
            this.columnID = columnID;           
            this.tasksList = new List<DummyTask>();
        }

        public DummyColumn(string columnName, string columnID,List<DummyTask> dummyTasks)
        {
            this.columnName = columnName;
            this.columnID = columnID;
            this.tasksList = dummyTasks;
        }
        public void addNewDummyTask(DummyTask modifiedTask)
        {
            this.tasksList.Add(modifiedTask);
        }
        
        public void removeTask(DummyTask dummyTask)
        {
            this.tasksList.Remove(dummyTask);
        }
        public DummyTask getTask(String taskID)
        {
            DummyTask task = this.tasksList.Find(item => item.TaskID.Equals(taskID));
            return task;
        }

        public string ToString()
        {
            string str = "Column Name: " + this.columnName +
                "\n Column ID: " + this.columnID;
            foreach (DummyTask task in this.tasksList)
            {
                str += task.ToString() + "\n";
            }
            return str;
        }

        
    }
}
