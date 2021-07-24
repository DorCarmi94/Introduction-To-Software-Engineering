using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Milestone3.Bussiness_Layer.Interface;

namespace Milestone3.Presentation_Layer.Functionality
{
    public class BoardFunc : INotifyPropertyChanged
    {
        public IConnect service;
        string loggedInEmail;
        public BoardFunc(string loggedInEmail, IConnect service)
        {

            this.service = service;
            this.loggedInEmail = loggedInEmail;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<PresentColumnsTasks> LoadColumnsTasks()
        {
            List<String> columnsIDs = service.getColumnsIDList();

            ObservableCollection<PresentColumnsTasks> colTasks = new ObservableCollection<PresentColumnsTasks>();
            foreach (var column in columnsIDs)
            {
                List<String> colInfo = service.getColumn(column);
                List<String> taskInThisCol = service.getColumnsTasksIDs(column);
                int i = 0;
                foreach (var task in taskInThisCol)
                {
                    List<string> taskInfo = service.getTask(column,task);
                    i++;
                    PresentColumnsTasks p = new PresentColumnsTasks()
                    {
                        
                        ColumnID = column,
                        ColumnName = colInfo[1],
                        TaskID = task,
                        TaskTitle = taskInfo[4]
                    };
                    
                    colTasks.Add(p);
                }
                if(i==0)
                {
                    PresentColumnsTasks p = new PresentColumnsTasks()
                    {
                        ColumnID = colInfo[0],
                        ColumnName = colInfo[1],
                        TaskID="",
                        TaskTitle=""

                    };
                    colTasks.Add(p);
                }

            }
            return colTasks;
        }


        public ObservableCollection<PresentColumns> LoadColumns()
        {
            List<String> columnsIDs = service.getColumnsIDList();

            ObservableCollection<PresentColumns> columns = new ObservableCollection<PresentColumns>();
            foreach (var column in columnsIDs)
            {
                List<String> colInfo = service.getColumn(column);

                PresentColumns newCol = new PresentColumns()
                {

                    ColumnID = column,
                    ColumnName = colInfo[1],
                    ColumnPositionInOreder = service.getColumnPosition(column)
                    };

                    columns.Add(newCol);
                }

            return columns;            
        }

        public string getColumnLimit(string columnID)
        {
            if(columnID==null)
            {
                return "error";
            }
            int ans = service.getColumnLimit(columnID);
            if(ans==-2)
            {
                return "error";
            }
            if(ans==-1)
            {
                return "none";
            }
            else
            {
                return ans.ToString();
            }
        }

        public ObservableCollection<PresentTasks> LoadTasks(string columnID)
        {


            ObservableCollection<PresentTasks> tasks = new ObservableCollection<PresentTasks>();

            if (columnID != null)
            {
                List<String> tasksIDinThisColumn = service.getColumnsTasksIDs(columnID);
                foreach (var taskID in tasksIDinThisColumn)
                {

                    List<string> currentTaskInfo = service.getTask(columnID, taskID);
                    PresentTasks newTask = new PresentTasks()
                    {
                        TaskID = currentTaskInfo[0],
                        CreationDate= DateTime.Parse(currentTaskInfo[1]),
                        Description= currentTaskInfo[3],
                        TaskTitle= currentTaskInfo[2],
                        DueDate= DateTime.Parse(currentTaskInfo[4])
                    };

                    tasks.Add(newTask);
                }
            }
        

            return tasks;
        }

        public string createNewTask(string newTaskTitle, string newTaskDescription, DateTime newTaskDueDate)
        {
            string ans=service.checkIfCanCreateTask(newTaskTitle, newTaskDescription, newTaskDueDate.Day, newTaskDueDate.Month, newTaskDueDate.Year);
            if(ans.Equals(""))
            {
                string newTaskID=service.createTask(newTaskTitle, newTaskDescription, newTaskDueDate.Day, newTaskDueDate.Month, newTaskDueDate.Year);
                
                return "";
            }
            else
            {
                return ans ;
            }
        }

        public bool removeColumn(string columnID)
        {

            return service.removeColumnFromBoard(columnID);
        }

        public bool moveColumnUp(string col1,string col2)
        {
            return service.changeColumnsOrder(col1,col2);
        }

        public string taskModify(string columnID,string taskID, string newTaskTitle, string newTaskDescription, DateTime newTaskDueDate)
        {
            if (columnID != null && taskID != null && newTaskTitle != null && newTaskDescription != null && newTaskDueDate != null)
            {
                string ans=service.checkIfCanCreateTask(newTaskTitle,newTaskDescription, newTaskDueDate.Day, newTaskDueDate.Month, newTaskDueDate.Year);
                if (!ans.Equals(""))
                {
                    return ans;
                }
                else
                {
                    return service.modifyTask(columnID, taskID, newTaskTitle, newTaskDescription, newTaskDueDate.Day, newTaskDueDate.Month, newTaskDueDate.Year);
                }
            }
            else
            {
                return "Can't except null inputs";
            }
        }

        public bool checkIfTaskCanBeModyfiedByCol(string columnID)
        {
            int position=service.getColumnPosition(columnID);
            int numOfColumns = service.getColumnsIDList().Count;
            if(position==-1 || position+1>=numOfColumns)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        internal void Logout()
        {
            service.logOut();
        }

        public string taskPromote(string columnID, string taskID)
        {
            return service.promote(columnID, taskID);
        }

        public void createNewColumn()
        {
            service.addColumnToBoard("");
        }

        public string addNewColumn(string newColumnName)
        {
            return service.addColumnToBoard(newColumnName);
        }

        public bool limitNumOfTasksInColumn(string columnID, int numToLimit)
        {
            return service.limitNumOfTasks(columnID, numToLimit);
            
        }

        public bool unLimitNumberOfTasks(string columnID)
        {
            return service.unlimitNumOfTasks(columnID);
            
        }

        internal string GetUsername()
        {
            return "";
        }

    }
    public class PresentColumnsTasks
    {
        public string ColumnID { get; set; }
        public string ColumnName { get; set; }
        public string TaskTitle { get; set; }
        
        public string TaskID { get; set; }
    }

    public class PresentColumns
    {
        public string ColumnName { get; set; }
        public int ColumnPositionInOreder { get; set; }

        public string ColumnID { get; set; }
    }

    public class PresentTasks
    {
        public string TaskTitle { get; set; }
        
        public DateTime CreationDate { get; set; }

        public DateTime DueDate { get; set; }

        public string Description { get; set; }

        public string TaskID { get; set; }
    }
}
