using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Milestone3.DataPersistence_Layer;
using log4net;
using System.Collections;

namespace Milestone3.Bussiness_Layer.Logic
{
    class Column
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private string name;
        private Guid col_id;
        private int MaxTasks = -1;
        private List<Task> tasksList;
        private int numOfTasksInThisColum;

        public string getColName()
        {
            return this.name;
        }

        public Guid getColId()

        {
            return col_id;
        }

        public int getColumnLimit()
        {
            return MaxTasks;
        }

        public List<Task> getTaskList()
        {
            return this.tasksList;
        }
        public Task getTask(Guid taskID)

        {
            int i;
            Task t;
            bool found = false;
            for (i = 0; i < numOfTasksInThisColum & !found; i++)
            {
                if (tasksList.ElementAt(i).getTaskID().Equals(taskID))
                {
                    t = tasksList.ElementAt(i);
                    found = true;
                }
            }
            if (found)
                return tasksList.ElementAt(i - 1);
            return null;

        }
        public int getNumOfTasks()

        {
            return this.numOfTasksInThisColum;
        }

        public List<String> getColInfoStrings()
        {
            List<String> listStrings = new List<string>();
            listStrings.Add(this.col_id.ToString());
            listStrings.Add(this.name);


            return listStrings;

        }

        public Column(String ColumeName)

        {
            this.name = ColumeName;
            this.col_id = Guid.NewGuid();
            tasksList = new List<Task>();

            numOfTasksInThisColum = 0;
        }


        public Column(DummyColumn dummyColumn)
        {
            this.name = dummyColumn.columnName;
            if (!(Guid.TryParse(dummyColumn.columnID, out this.col_id)))
            {
                log.Error("Error parsing column ID");
            }

            this.numOfTasksInThisColum = 0;
            this.tasksList = new List<Task>();
            foreach (DummyTask dummyTask in dummyColumn.tasksList)
            {
                Task newTask = new Task(dummyTask);
                this.numOfTasksInThisColum++;
                this.tasksList.Add(newTask);

            }



        }

        public DummyColumn turnThisColumnToDummy()
        {
            List<DummyTask> dummyTasks = new List<DummyTask>();
            foreach (var task in this.tasksList)
            {
                dummyTasks.Add(task.turnThisToDummyTask());
            }

            DummyColumn dummyColumn = new DummyColumn(this.name, this.col_id.ToString(), dummyTasks);
            return dummyColumn;
        }

        public Task createNewTask(String UserID, String Description, String taskTitle, int day, int month, int year)

        {
            try
            {
                Task add = new Task(Description, day, month, year, taskTitle);
                if ((MaxTasks == -1 | numOfTasksInThisColum+1 <= MaxTasks))
                {
                    this.tasksList.Add(add);

                    this.numOfTasksInThisColum++;
                    return add;

                }
                return null;
            }
            catch (Exception e)
            {
                log.Error("Could not create task. problem message: " + e.Message);
                return null;

            }



        }

        public void AddTask(Task t)
        {
            if (!this.tasksList.Contains(t))

            {
                if (MaxTasks == -1 | (this.numOfTasksInThisColum+1 <= MaxTasks))
                {
                    this.tasksList.Add(t);
                    numOfTasksInThisColum++;
                }

            }
        }
        public Task RemoveTask(Guid taskID)
        {
            Task toRemove = this.getTask(taskID);
            if (toRemove != null)
            {

                this.tasksList.Remove(toRemove);
                this.numOfTasksInThisColum--;

            }
            return toRemove;
        }

        public bool TaskModify(Guid TaskID, int day, int month, int year, String Title, String Description)
        {
            Task t = this.getTask(TaskID);
            if (t != null)
            {
                try
                {
                    t.ModifyTask(day, month, year, Title, Description);
                    return true;
                }
                catch (Exception e)
                {
                    log.Error("Could not create task. problem message: " + e.Message);
                    return false;
                }

            }
            return false;
        }




        public void limitTasksNum(int limit)
        {
            if (!(this.numOfTasksInThisColum > limit))
            {
                this.MaxTasks = limit;
            }
            else
            {

                throw new Exception("Can't limit the tasks num, you need to remove tasks first");

            }
        }
        public void unlimitTasksNum()
        {
            this.MaxTasks = -1;
        }


        public override string ToString()
        {
            string str = "Column name: " + this.name + "\nColumn ID: " + this.col_id + "\n";
            if (this.tasksList != null)
            {
                foreach (Task task in this.tasksList)
                {
                    str += task.ToString() + "\n";
                }
            }
            return str;
        }


        public bool sortTasksInColumnByCreationDate()
        {
            TaskCreationDateComperator dateComperator = new TaskCreationDateComperator();
            this.tasksList.Sort(dateComperator);
            return true;

        }

        public bool sortTasksInColumnByDueDate()
        {
            TaskDueDateComperator dateComperator = new TaskDueDateComperator();
            this.tasksList.Sort(dateComperator);
            return true;

        }


        public List<Task> filterTaskByTitle(String title)
        {
            var filteredList = this.tasksList.Where(item => item.getTitle().Contains(title));

            return filteredList.ToList<Task>();
        }

        
    }

}
