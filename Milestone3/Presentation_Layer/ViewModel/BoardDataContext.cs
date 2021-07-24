using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milestone3.Bussiness_Layer.Interface;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Milestone3.Presentation_Layer.Functionality;
using System.Collections.ObjectModel;

namespace Milestone3.Presentation_Layer.ViewModel
{
    public class BoardDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public BoardFunc boardFunc;
        
        //Columns
        private ObservableCollection<PresentColumns> columns;
        public ObservableCollection<PresentColumns> Columns
        {
            get
            {
                return this.columns;
            }
            set
            {
                columns = value;
                updateColumnsGridView();
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Columns"));
            }

        }

       

        private void updateColumnsGridView()
        {
            
            CollectionViewSource cvs = new CollectionViewSource() { Source = columns };
            ColumnsGridView = cvs.View;
        }

        private ICollectionView columnsGridView;
        public ICollectionView ColumnsGridView
        {
            get
            {
                return columnsGridView;
            }
            set
            {
                columnsGridView = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ColumnsGridView"));
            }
        }

        

        private PresentColumns selectedColumn;
        public PresentColumns SelectedColumn
        {
            get
            {
                return this.selectedColumn;
            }
            set
            {
                selectedColumn = value;
                UpdateTasksGrid();
                this.LimitTasksNum = boardFunc.getColumnLimit(selectedColumn.ColumnID);
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedColumn"));
            }
        }
        public void createNewColumn()
        {
            boardFunc.createNewColumn();
        }

        public string newColumnName;
        public string NewColumnName
        {
            get
            {
                return this.newColumnName;
            }
            set
            {
                this.newColumnName = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("NewColumnName"));
            }
        }

        public string addNewColumn()
        {

            if(newColumnName==null || newColumnName.Length<=0)
            {
                return "";
            }
            else
            {
                List<PresentColumns> colLst = this.columns.ToList<PresentColumns>();
                var col = colLst.Find(item => item.ColumnName.Equals(newColumnName));
                if (col == null)
                {
                    string ans = boardFunc.addNewColumn(newColumnName);
                    this.Columns = boardFunc.LoadColumns();

                    return ans;

                }
                else
                {
                    return "";
                }
            }
        }
        public bool removeColumn()
        {
            if(this.selectedColumn==null)
            {
                return false;
            }
            else
            {
                bool ans= boardFunc.removeColumn(selectedColumn.ColumnID);
                if(ans)
                {
                    this.Columns = boardFunc.LoadColumns();
                    return ans;
                }
                else
                {
                    return false;
                }
                
            }
        }

        public bool moveColumnUp()
        {
            int colToMoveIdx = columns.IndexOf(selectedColumn);
            int numOfCols = columns.Count;
            if(colToMoveIdx+1>=numOfCols)
            {
                return false;

            }
            else
            {
                bool ans=boardFunc.moveColumnUp(selectedColumn.ColumnID, columns[colToMoveIdx + 1].ColumnID);
                if (ans)
                {
                    this.Columns = boardFunc.LoadColumns();
                }
                return ans;
            }
        }

        //Tasks
        //SelectedTask
        private PresentTasks selectedTask;
        public PresentTasks SelectedTask
        {
            get
            {
                return this.selectedTask;
            }
            set
            {
                selectedTask = value;
                
                
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedColumn"));
            }
        }
        private void UpdateTasksGrid()
        {
            this.taskTitleSearchTerm = "";
            if(selectedColumn==null)
            {
                    SelectedColumn = Columns[0];
                    
                
            }
            this.TasksByCols = boardFunc.LoadTasks(selectedColumn.ColumnID);
            UpdateFilter();
        }
        private ObservableCollection<PresentTasks> tasksByCols;
        public ObservableCollection<PresentTasks> TasksByCols
        {
            get
            {
                return this.tasksByCols;
            }
            set
            {
                tasksByCols = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("tasksByCols"));
            }

        }

        internal void Logout()
        {
            boardFunc.Logout();
        }

        string taskTitleSearchTerm = "";
        public string TaskTitleSearchTerm
        {
            get
            {
                return taskTitleSearchTerm;
            }
            set
            {
                taskTitleSearchTerm = value;
                UpdateFilter();
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SearchTerm"));
            }
        }

        

        private ICollectionView tasksGridView;
        public ICollectionView TasksGridView
        {
            get
            {
                return tasksGridView;
            }
            set
            {
                tasksGridView = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("TasksGridView"));
            }
        }
        private void UpdateFilter()
        {
            if (tasksByCols != null)
            {
                CollectionViewSource cvs = new CollectionViewSource() { Source = tasksByCols };
                ICollectionView cv = cvs.View;
                cv.Filter = o =>
                {
                    PresentTasks p = o as PresentTasks;
                    return (p.TaskTitle.ToUpper().Contains(TaskTitleSearchTerm.ToUpper()))||
                    (p.Description.ToUpper().Contains(TaskTitleSearchTerm.ToUpper()));
                };
                TasksGridView = cv;
            }
        }


        public BoardDataContext(string loggedInEmail, IConnect service)
        {
            boardFunc = new BoardFunc(loggedInEmail, service);
            this.Columns = boardFunc.LoadColumns();
            
        }

        string newTaskTitle;
        public string NewTaskTitle
        {
            get
            {
                return this.newTaskTitle;
            }
            set
            {
                this.newTaskTitle = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("NewTaskTitle"));
            }
        }

        string newTaskDescription;
        public string NewTaskDescription
        {
            get
            {
                return this.newTaskDescription;
            }
            set
            {
                this.newTaskDescription = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("NewTaskDescription"));
            }
        }

        DateTime newTaskDueDate;
        public DateTime NewTaskDueDate
        {
            get
            {
                return this.newTaskDueDate;
            }
            set
            {
                this.newTaskDueDate = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("NewTaskDueDate"));
            }
        }

        public string createNewTask()
        {
            if (newTaskTitle == null)
            {
                return "Can't create with no title";
            }
            if(newTaskDueDate==null)
            {
                return "Can't create with no due date";
            }
            
            else
            {
                if (NewTaskDescription == null)
                    NewTaskDescription = "";
                return boardFunc.createNewTask(newTaskTitle, newTaskDescription, newTaskDueDate);
            }
        }
        
        public string modifyTask()
        {
            if (this.selectedTask == null || this.selectedColumn == null)
            {
                return "Can't modify with no due date";
            }
            else
            {
                string ans = boardFunc.taskModify(selectedColumn.ColumnID, selectedTask.TaskID, newTaskTitle, newTaskDescription, newTaskDueDate);
                if (ans.Equals(""))
                {
                    return "";
                }
                else
                {
                    return ("Something went wrong:\n"+ans);
                }
            }
        }

        public bool checkIfCanModifyTaskByCol()
        {
            return boardFunc.checkIfTaskCanBeModyfiedByCol(selectedColumn.ColumnID);
        }

        public string taskPromote()
        {
            if(this.selectedTask==null ||this.selectedColumn==null)
            {
                return "You need to choose both column and task";
            }
            else
            {
                return boardFunc.taskPromote(this.selectedColumn.ColumnID,this.selectedTask.TaskID);
            }
        }

        private string limitTasksNum;
        public string LimitTasksNum
        {
            get
            {
                return this.limitTasksNum;
            }
            set
            {
                this.limitTasksNum = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("LimitTasksNum"));
            }
        }

        public bool limitNumOfTasksInThisColumn()
        {
            if(selectedColumn==null || limitTasksNum==null)
            {
                return false;
            }
            int limit;
            bool parseAns= Int32.TryParse(limitTasksNum, out limit);
            if(!parseAns)
            {
                return false;
            }
            else
            {
                return boardFunc.limitNumOfTasksInColumn(selectedColumn.ColumnID, limit);
                
            }
        }

        public bool unLimitNumberOfTasks()
        {
            if (selectedColumn == null )
            {
                return false;
            }
            if(boardFunc.unLimitNumberOfTasks(selectedColumn.ColumnID))
            {
                LimitTasksNum = "none";
                return true;

            }
            return false;
        }

        
    }
}
