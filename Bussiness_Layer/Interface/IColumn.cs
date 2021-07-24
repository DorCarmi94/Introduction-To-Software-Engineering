using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milestone2.Bussiness_Layer.Logic;
using log4net;
using Task = Milestone2.Bussiness_Layer.Logic.Task;

namespace Milestone2.Bussiness_Layer.Interface
{
    class IColumn
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Columns myColumns;

        public IColumn()
        {
            myColumns = new Columns();
        }

        public void startUp() { myColumns.startUp(); }



        private Guid guidTryParse(String guid, string error_meesage)
        {
            Guid toReturn;
            if(!(Guid.TryParse(guid,out toReturn)))
            {
                log.Error(error_meesage);
                return Guid.Empty;
            }
            else
            {
                return toReturn;
            }            

        }

        /// <summary>
        /// return a full List of strings that represent the column
        /// so we can print the column
        /// </summary>
        /// <param name="columnID"></param>
        /// <returns>List of string</returns>
        public List<String> getColumn(String columnID)
        {
            if (!(Guid.TryParse(columnID, out Guid ColumnID)))
            {
                log.Error("Error trying to parse column\task ID entered");
                return null;
            }
            else
            {
                Column c = myColumns.getByColumnID(ColumnID);
                if (c != null)
                {

                    return c.getColInfoStrings();
                }
                return null;
            }
        }

        /// <summary>
        /// Retruns this column's tasks IDs list
        /// </summary>
        /// <param name="columnID"></param>
        /// <returns></returns>
        public List<String> getColumnTasksIDList (String columnID)
        {
            if (!(Guid.TryParse(columnID, out Guid ColumnID)) )
            {
                log.Error("Error trying to parse column or task ID entered");
                return null;
            }
            else
            {
                Column c = myColumns.getByColumnID(ColumnID);

                if (c != null )
                {
                    List<Task> tasksListIDs = c.getTaskList();
                    List<String> tasksListIDsToString = new List<string>();
                    foreach (var item in tasksListIDs)
                    {
                        tasksListIDsToString.Add(item.getTaskID().ToString());
                    }
                    return tasksListIDsToString;
                }
                return null;
            }
        }
        /// <summary>
        /// Returns a list of strings that represent a task with all the task's information
        /// </summary>
        /// <param name="columnID"></param>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public List<String> getTask(String columnID ,String taskID)

        {
            if (!(Guid.TryParse(columnID, out Guid ColumnID)) ||
                !(Guid.TryParse(taskID, out Guid TaskID)))
            {
                log.Error("Error trying to parse column or task ID entered");
                return null ;
            }
            else
            {
                Column c = myColumns.getByColumnID(ColumnID);

                
                if (c != null)
                {
                    List<String> output = new List<string>();
                    Task thetask = c.getTask(TaskID);
                    if (thetask == null)
                    {
                        log.Error("task " + taskID + "isn't in " + columnID);
                        return null;
                    }
                    output.Add(thetask.getTaskID().ToString());
                    output.Add(thetask.getCreationDate().ToString());
                    output.Add(thetask.getCreatorId().ToString());
                    output.Add(thetask.getTitle());
                    output.Add(thetask.getDescription());
                    output.Add(thetask.getDueDate().ToString());
                    return output;
                }
                return null;
            }
        }
        
        /// <summary>
        /// Creating default three colunmns and returns list of their IDs
        /// Returns List of strings with the new columns IDs
        /// </summary>
        /// <returns></returns>
        public List<String> createDeafaultColumnsForNewBoard()
        {            
            List<Column> newColumns = myColumns.createDefaultColumns();
            List<String> newColumnsIDsList = new List<string>();

            foreach (var item in newColumns)
            {
                newColumnsIDsList.Add(item.getColId().ToString());
            }

            return newColumnsIDsList;
        }

        /// <summary>
        /// Returns all Columns toString in one big string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return myColumns.ToString();
        }

        public bool checkIfColumnExistByID(String col_id)
        {
            Guid colID_guid;
            if(!(Guid.TryParse(col_id,out colID_guid)))
            {
                return false;
            }

            Column c = myColumns.getByColumnID(colID_guid);
            return c == null;
        }

        public String addNewColumn(String colName)
        {
            Guid ans = myColumns.addNewColumn(colName);
            if (ans.Equals(Guid.Empty))
            {
                return "";
            }
            return ans.ToString();
        }

        public String RemoveColumn(String colID)
        {
            Guid colID_guid = this.guidTryParse(colID, "Problem parsing colID for remove column");
            if(!(colID_guid.Equals(Guid.Empty)))
            {
                Guid retGuid = myColumns.removeColumn(colID_guid);
                if (!(retGuid.Equals(Guid.Empty)))
                {
                    return retGuid.ToString();
                }
                
            }
            return "";
                
        }

        public String getColumnName(String colID)
        {
            Guid colID_guid = this.guidTryParse(colID, "Problem parsing colID for getColumnName");
            if (!(colID_guid.Equals(Guid.Empty)))
            {
                Column col = myColumns.getByColumnID(colID_guid);
                if (col != null)
                {
                    return col.getColName();
                }
            }
            return "";
        }

        public String createNewTask(String columnID, String UserID,  String Description, String taskTitle,int day,int month,int year)
        {
            if(!(Guid.TryParse(columnID, out Guid ColumnID)))
            {
                log.Error("Error trying to parse column ID entered");
                return "";
            }
            else
            {
                Column theCol = myColumns.getByColumnID(ColumnID);
                Task newTask = theCol.createNewTask(UserID, Description, taskTitle, day, month, year);
                if (newTask == null)
                {
                    log.Error("Couldnt create new task. Problem related to limit of tasks number");
                    return "";
                }
                else
                {
                    myColumns.saveColumn(theCol.turnThisColumnToDummy());
                    return newTask.getTaskID().ToString();
                }
            }
        }

        public bool IsTaskDone(String taskID)
        {
            if ( !(Guid.TryParse(taskID, out Guid TaskID)))
            {
                log.Error("Error trying to parse colum or task ID entered");
                return false;
            }
            else
            {
                Column foundTaskInColumn = myColumns.searchForTaskColumn(TaskID);

                if (foundTaskInColumn != null)
                {
                    string colName = foundTaskInColumn.getColName();
                    return (colName.Contains("Done") || colName.Contains("done") || colName.Contains("DONE"));
                }
                else
                {
                    log.Error("Task asked to be checked if done doesn't exist according to this task ID");
                    return false;
                }
            }
        }

        public bool Promote(String columnIDFrom,String columnIDTo, String taskID)

        {
            if (!(Guid.TryParse(columnIDFrom, out Guid ColumnIDFrom_guid)) || !(Guid.TryParse(columnIDTo, out Guid ColumnIDTo_guid)) || !(Guid.TryParse(taskID, out Guid TaskID_guid)))
            {
                log.Error("Error trying to parse column ID entered");
                return false;
            }
            else
            {
                return myColumns.promoteTask(ColumnIDFrom_guid, ColumnIDTo_guid, TaskID_guid);
            }
        }

        public bool TaskModify(String colID,String taskID, String Description, String taskTitle, int day, int month, int year)

        {
            if ( !(Guid.TryParse(taskID, out Guid TaskID_guid)) & !(Guid.TryParse(colID, out Guid colID_guid)))
            {
                log.Error("Error trying to parse column ID or task ID that was entered");
                return false;
            }
            else
            {
                Column theCol = myColumns.getByColumnID(colID_guid);
                
                if(theCol==null)
                {
                    log.Error("The column doesn't exist");
                    return false;
                }
                else
                {
                    bool ans=theCol.TaskModify(TaskID_guid, day, month, year, taskTitle, Description);
                    myColumns.saveColumn(theCol.turnThisColumnToDummy());
                    return ans;
                }
                
            }
        }

        public bool limitNumOfTasksInColum(String columnID, int limit)
        {
            if (!(Guid.TryParse(columnID, out Guid ColID_guid)))
            {
                log.Error("Error trying to parse column ID for limiting number of tasks");
                return false;
            }
            else
            {
                Column theCol = this.myColumns.getByColumnID(ColID_guid);
                if (theCol != null)
                {
                    theCol.limitTasksNum(limit);
                    return true;
                }
                return false;
            }
        }

        public int getNumOfTasksInColumn(string columnID)
        {
            if (!(Guid.TryParse(columnID, out Guid ColID_guid)))
            {
                log.Error("Error trying to parse column ID for limiting number of tasks");
                return -1;
            }
            else
            {
                Column theCol = this.myColumns.getByColumnID(ColID_guid);
                if (theCol != null)
                {
                    return theCol.getNumOfTasks();
                    
                }
                return -1;
            }
        }

        public bool unlimitNumOfTasksInColum(String columnID)
        {
            if (!(Guid.TryParse(columnID, out Guid ColID_guid)))
            {
                log.Error("Error trying to parse column ID for limiting number of tasks");
                return false;
            }
            else
            {
                Column theCol = this.myColumns.getByColumnID(ColID_guid);
                if (theCol != null)
                {
                    theCol.unlimitTasksNum();
                    return true;
                }
                return false;
            }
        }

        public bool SortTasksInColumnByCreationDate(string columniD)
        {
            Guid colID = this.guidTryParse(columniD, "The column id input for sorting by date is invalid");
            if(colID!=Guid.Empty)
            {
                Column theCol= myColumns.getByColumnID(colID);
                
                return theCol.sortTasksInColumnByCreationDate();   
            }
            return false;
        }

        public bool SortTasksInColumnByDueDate(string columniD)
        {
            Guid colID = this.guidTryParse(columniD, "The column id input for sorting by date is invalid");
            if (colID != Guid.Empty)
            {
                Column theCol = myColumns.getByColumnID(colID);

                return theCol.sortTasksInColumnByDueDate();
            }
            return false;
        }

        public List<String> filterTaskByTitle(String columnID,string titlePhrase)
        {
            List<String> listOfTasksIDs = null;
            
            
            Guid col_guid = this.guidTryParse(columnID, "Problem with column id entered in filtering tasks by title");
            if(!(col_guid.Equals(Guid.Empty)))
            {
                listOfTasksIDs = new List<string>();
                Column c = this.myColumns.getByColumnID(col_guid);
                List<Task> filteredTasks = c.filterTaskByTitle(titlePhrase);

                foreach (var item in filteredTasks)
                {
                    listOfTasksIDs.Add(item.getTaskID().ToString());
                }
            }
            return listOfTasksIDs;

        }

        

    }
}
