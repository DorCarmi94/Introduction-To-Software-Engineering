using System;
using System.Collections.Generic;
using System.Linq;
using Milestone2.Data_Persistence;

namespace Milestone2.Bussiness_Layer.Logic
{
    class Columns
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
              System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private List<Column> allColumnsList;
        private ColumnHandler myColumnHandler;

        public Columns()
        {
            allColumnsList = new List<Column>();
            myColumnHandler = new ColumnHandler();
        }
        public void saveColumn(DummyColumn dummyColumn)
        {
            myColumnHandler.saveColumn(dummyColumn);
        }
        public void startUp()
        {
            List<DummyColumn> dummyColumnsList = myColumnHandler.LoadColumns();
            if (dummyColumnsList != null)
            {
                foreach (DummyColumn dummyColumn in dummyColumnsList)
                {
                    Column newCol = new Column(dummyColumn);
                    this.allColumnsList.Add(newCol);
                }
            }
        }
        public Column getByColumnID(Guid columnID)
        {
            Column retCol = null;
            int numOfCols = this.allColumnsList.Count;
            int i = 0;
            bool found = false;
            while (i < numOfCols && !found)
            {
                if (allColumnsList[i].getColId().Equals(columnID))
                {
                    retCol = allColumnsList[i];
                    found = true;
                }
                else
                {
                    i++;
                }
            }

            return retCol;
        }
        
        public List<Column> createDefaultColumns()
        {
            List<Column> newCols = new List<Column>();
            Column backlog = new Column("Backlog");
            Column InProgress = new Column("In Progress");
            Column IsDone = new Column("Is Done");
            
            this.allColumnsList.Add(backlog);
            this.allColumnsList.Add(InProgress);
            this.allColumnsList.Add(IsDone);

            newCols.Add(backlog);
            newCols.Add(InProgress);
            newCols.Add(IsDone);

            DummyColumn backlogDummy = new DummyColumn(backlog.getColName(), 
                backlog.getColId().ToString());

            DummyColumn InProgressDummy = new DummyColumn(InProgress.getColName(),
                InProgress.getColId().ToString());

            DummyColumn IsDoneDummy = new DummyColumn(IsDone.getColName(),
                IsDone.getColId().ToString());

            myColumnHandler.saveColumn(backlogDummy);
            myColumnHandler.saveColumn(InProgressDummy);
            myColumnHandler.saveColumn(IsDoneDummy);

            return newCols;
        }

        public Column searchForTaskColumn(Guid taskID)
        {
            Column theCol = null;
            foreach (var col in this.allColumnsList)
            {
                Task t=col.getTask(taskID);
                if (t!=null)
                {
                    theCol = col;
                    break;
                }
            }

            return theCol;
        }


        public bool promoteTask(Guid columnIDFrom_guid, Guid columnIDTo_guid, Guid taskID_guid)
        {
            bool ans = false;
            Column colFrom = this.getByColumnID(columnIDFrom_guid);
            Column colToMoveTo = this.getByColumnID(columnIDTo_guid);
            if(colFrom==null || colToMoveTo==null)
            {
                log.Error("One of the columns id to promote doesn't exist");
                ans = false;
            }
            else if(colToMoveTo.getColumnLimit()!=-1 && colToMoveTo.getNumOfTasks()+1>colToMoveTo.getColumnLimit())
            {
                log.Error("Cannot promote. Number of tasks in the designated column will cross the limit");
                ans = false;
            }
            else
            {
                Task taskToMove=colFrom.RemoveTask(taskID_guid);
                if (taskToMove == null)
                {
                    log.Error("The task id to promote doesn't exist in this column");
                }
                else
                {
                    colToMoveTo.AddTask(taskToMove);
                    this.saveColumn(colFrom.turnThisColumnToDummy());
                    this.saveColumn(colToMoveTo.turnThisColumnToDummy());
                    ans = true;
                }
                
            }
            return ans;
        }
        

        public Guid addNewColumn(string colName)
        {
            Column col = this.allColumnsList.Find(item => item.getColName().Equals(colName));
            if (col !=null)
            {
                return Guid.Empty;
            }

            Column newCol = new Column(colName);
            this.allColumnsList.Add(newCol);
            DummyColumn dummyColumn = new DummyColumn(newCol.getColName(), newCol.getColId().ToString());
            myColumnHandler.saveColumn(dummyColumn);
            return newCol.getColId();
        }

        internal Guid removeColumn(Guid colID_guid)
        {
            Column col = this.getByColumnID(colID_guid);
            if(col!= null)
            {
                this.allColumnsList.Remove(col);
                myColumnHandler.removeColumn(col.turnThisColumnToDummy());
                return col.getColId();
            }
            log.Error("Column to remove doesn't exist");
            return Guid.Empty;
        }

        
    }

}
