using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milestone2.Bussiness_Layer.Logic;

namespace Milestone2.Bussiness_Layer.Interface
{
    class IColumn
    {
        private List<Column> columns;
        public void startUp()
        {

        }
        public String getColum(String columID)
        {
            return null;
        }
        public String getTask(String taskID)
        {
            return null;
        }
        public IColumn(String IDB)
        
        {
            
        }
        public bool createNewTask_submit(String UserID,  String Description, String taskTitle, String creationDate, String dueDate)
        {
            return false;
        }

        public bool IsTaskDone(String columnID, String taskID)
        {
            return false;
        }

        public bool Promote(String columnID,String taskID)
        {
            return false;
        }

        public string TaskModify(String columnID, String TaskID, String dueDate, String Title, String Description)
        {
            return null;
        }
        
        public void limitNumOfTasksInColum(String columnID, int limitNumber)
        {

        }

    }
}
