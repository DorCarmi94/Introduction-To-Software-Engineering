using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milestone3.Bussiness_Layer.Interface
{
    public class IConnect : INotifyPropertyChanged
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IColumn myIColumn;
        private IBoard myIBoard;
        private IUser myIUser;
        private string userID;
        private string boardID;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //CONSTRUCTOR
        public IConnect()
        {
            myIBoard = new IBoard();
            myIColumn = new IColumn();
            myIUser = new IUser();

            userID = null;
            boardID = null;
        }

        //STARTUP
        public void stratUpAll()
        {
            myIColumn.startUp();
            myIBoard.startUp();

            IUser.startUp();

        }
        //GETS

        public String getBoard(String boardID)
        {
            if (userID == null)
            {
                log.Error(" unsigned user. Cannot access boards");
                return ("");
            }
            if (boardID == null)
            {
                log.Error(" board or column null");
                return ("");
            }

            return myIBoard.LoadSpecificBoard(boardID);
        }

        public List<String> getColumnsIDList()
        {
            if (this.boardID != null)
            {
                return myIBoard.getColumnsIDsListOfBoard(this.boardID);
            }
            else
            {
                return null;
            }
        }

        public List<String> getColumn(string columnID)
        {
            if (columnID == null | boardID == null)
            {
                log.Error(" board or column null");
                return (new List<String>());
            }
            if (myIBoard.ColumnInBoard(boardID, columnID))
                return myIColumn.getColumn(columnID);
            return (new List<String>());
        }

        public List<string> getColumnsTasksIDs(string columnID)
        {
            if (columnID == null | boardID == null)
            {
                log.Error(" board or column null");
                return (new List<string>());
            }
            if (myIBoard.ColumnInBoard(boardID, columnID))
                return myIColumn.getColumnTasksIDList(columnID);
            return (new List<String>());
        }

        /// <summary>
        /// 0-getTaskID()
        /// 1-getCreationDate()
        /// 2-getCreatorId()
        /// 3-getTitle());
        /// 4-getDescription());
        /// 5-getDueDate()
        /// </summary>
        /// <param name="columnID"></param>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public List<String> getTask(string columnID, string taskID)
        {
            if (getColumn(columnID).Count > 0)
                return myIColumn.getTask(columnID, taskID);
            return (new List<String>());
        }


        //USER
        public string login(String email, string pass)
        {
            if (email == null || pass == null )
            {
                log.Warn("trying to login with email or password that is null");
                return "Invalid string input- doesn't except null";
            }
            string validEmail = IConnect.checkIfEmailValid(email);
            if (!validEmail.Equals(""))
            {
                log.Warn("bad email inserted " + email + ". " + validEmail);
                return "Unvalid email has been entered";
            }
            if (!IUser.checkUserPass(pass))
            {
                log.Warn("bad email inserted " + pass + ". " + IUser.checkUserPassReason(pass));
                return IUser.checkUserPassReason(pass);
            }
            string loggedInEmail = this.myIUser.Login(email, pass);
            if (loggedInEmail.Equals(email))
            {
                log.Info("user " + email + "logged in!");
                this.userID = email;
                //List<string> currBoardIDlst= this.getUserBoards(); 
                //if(currBoardIDlst==null)
                //{
                //    log.Error("Something went wrong with loading board of the user: " +email);
                //    return "Something went wrong with loading your board.";
                //}
                //this.boardID = currBoardIDlst[0];
                return email;
            }
            else
            {
                log.Warn("bad email - password combination, email entered: " + email + " password: " + pass);
                return "Something went wrong:"+loggedInEmail;
            }
        }
        public List<String> getUserBoards()
        {
            if (this.userID==null)
            {
                log.Error("user tried to access boards though he shouldn't be able to do it due to the fact that no one is logged in");
                return null;
            }
            return myIUser.getBoards(this.userID);
        }
        public void logOut()
        {
            if (userID == null)
                log.Info("Arya Stark has logged out successfully!");
            else
            {
                log.Info("user " + this.userID + "logged out!");
                this.boardID = null;
                this.userID = null;
            }
        }
        //create new user---
        public string createNewUser(String email, string pass)
        {
            if (email == null || pass == null)
            {
                log.Error("cannot create user, email or pass equals null. email: " + email + " pass: " + pass);
                return "Invalid string input- doesn't except null";
            }
            if (!(IUser.checkEmail(email)))
            {
                log.Error("cannot create user, email is not good. email: " + email + ". " + IUser.checkEmailReason(email)); ;

                return IUser.checkEmailReason(email);
            }
            if (!(IUser.checkUserPass(pass)))
            {
                log.Error("cannot create user, pass is not good. pass: " + pass + ". " + IUser.checkUserPassReason(pass)); ;

                return IUser.checkUserPassReason(pass);
            }
            if (!myIUser.CreateNewUser(email, pass))
            {
                return "Problem with creating new user";
            }
            else
            {
                this.userID = email;
                string ans= createNewBoard_private(email);
                this.userID = null;
                return ans;
            }

            //myIUser.CreateNewUser(email, pass); //TODO check bool
            //this.boardID = myIBoard.createNewBoard(email,email); // TODO check if string not empty. Important to keep it with this because assign columns to board uses it

            //string assignSucceed = assignBoardToUser(email, boardID);
            //if (!assignSucceed.Equals(""))
            //    return assignSucceed; //if the assign failed, returns the reason
            //List<String> newColumnsIDsList = createDeafaultColumnsForNewBoard(boardID);
            //bool assignColsBoardSucceed = newColumnsIDsList != null;
            //this.boardID = null;
            //if (assignColsBoardSucceed == false)
            //    return "assign Col To Board failed";
            //return "";
        }
        
        private string assignBoardToUser(string email, string boardID)
        {
            /*if (!checkIfLoggedIn())
            {
                return "User is not logged in";
            }*/
            Guid g;
            if (boardID == null)
            {
                return "Invalid string input - doesn't except null";
            }
            if (!(Guid.TryParse(boardID, out g)) | boardID.Equals(""))
            {
                return "Board ID is invalid";
            }
            myIUser.AssignBoardToUser(email, boardID);
            return "";
        }
        public List<String> createDeafaultColumnsForNewBoard(string boardID)
        {
            List<String> newColumnsIDsList = new List<string>();
            newColumnsIDsList = myIColumn.createDeafaultColumnsForNewBoard(boardID);
            foreach (var newCol in newColumnsIDsList)
            {
                this.assignColumnToBoard(newCol, boardID);
            }
            return newColumnsIDsList;
        }
        public bool assignColumnToBoard(string columnID, string boardID)
        {
            if (boardID != null && columnID != null)
            {
                return myIBoard.assignColumnToBoard(boardID, columnID, this.userID);
            }
            else
            {
                return false;
            }
        }
        //---
        public string changeUserPass(string currPass, string newPass)
        {
            bool ans = checkIfLoggedIn();
            if (ans==false)
            {
                return "User is not logged in";
            }
            if (currPass == null || newPass == null)
            {
                return "Invalid string input- doesn't except null";
            }
            if (!IUser.checkUserPass(currPass))
            {
                return "There has been a problem with the " +
                    "current pass you entered: " +
                    IUser.checkUserPassReason(currPass);
            }
            if (!IUser.checkUserPass(newPass))
            {
                return "There has been a problem with the " +
                    "new pass you entered: " +
                    IUser.checkUserPassReason(newPass);
            }
            if (myIUser.ChangeUserPass(this.userID, currPass, newPass))
            {
                return "";
            }
            else
            {
                return "There has been a problem with you input. Please check";
            }
        }

        //CHECKS
        public static string checkIfEmailValid(String email)
        {
            if (email == null)
            {
                return "Invalid string input- doesn't except null"; ;
            }
            if (!email.Contains('@') | !email.EndsWith(".com"))
            {
                return "Email entered invalid";

            }
            return "";

        }
        public bool checkIfLoggedIn()
        {
            bool checkUser = (userID == null);

            return (checkUser == false);
        }

        //Boards
        
        public string createNewBoard(string newBoardName)
        {
            return this.createNewBoard_private(newBoardName);

        }//UserCreating newBoard
        private string createNewBoard_private(String boardName)
        {
            string newBoardID = myIBoard.createNewBoard(boardName, this.userID);
            string assignSucceed = assignBoardToUser(this.userID, newBoardID);
            if (!assignSucceed.Equals(""))
                return assignSucceed; //if the assign failed, returns the reason
            List<String> newColumnsIDsList = createDeafaultColumnsForNewBoard(newBoardID);
            bool assignColsBoardSucceed = newColumnsIDsList != null;
            this.boardID = null;
            if (assignColsBoardSucceed == false)
                return "assign Col To Board failed";
            return "";
        }

        public string loadSpecificBoard(String boardID)
        {
            if (userID == null)
            {
                return "Cannot load board. User is unassigned. ";
            }
            if (boardID == null)
            {
                return "Invalid input";
            }
            else if (!myIUser.checkIfBoardBelongsToUser(userID, boardID))
            {
                return "Cannot load this board. Doesn't belong to this user.";
            }
            else
            {
                this.boardID = boardID;
                return "";
            }
        }

        public string removeBoard(String boardID)
        {
            if (userID == null)
            {
                return "Cannot remove board. User is unassigned. ";
            }
            if (boardID == null)
            {
                return "Invalid input";
            }
            else if (!myIUser.checkIfBoardBelongsToUser(userID, boardID))
            {
                return "Cannot remove this board. Doesn't belong to this user.";
            }
            else
            {
                return deleteBoard(boardID, userID);
            }
        }

        private string deleteBoard(string boardID, string userEmail)
        {
            List<String> columnInBoard=myIBoard.getColumnsIDsListOfBoard(boardID);
            bool ans = true;
            foreach (var col in columnInBoard)
            {
                ans= ans & this.removeColumnFromBoard(col,boardID, "force");
            }
            if(ans)
            {
                return myIBoard.deleteBoard(boardID);
            }
            else
            {
                return "Something went wrong with deleting the columns of this board";
            }
        }

        //COLUMNS
        /// <summary>
        /// Returns lists of strings, that represents the board's columns info
        /// Each list represent a column.
        /// List[0]-Column 1
        /// List[0][1][0]-Column 1's id
        /// List[0][1][0]-Column 1's name
        /// List[0][1]-Column's tasks 
        /// </summary>
        /// <returns></returns>
        public string addColumnToBoard(String colName)
        {

            if (colName == null || colName.Length < 1)
            {
                return "";
            }
            string newColID = myIColumn.addNewColumn(colName, this.boardID);
            if (newColID.Equals(""))
            {
                return "";
            }
            else
            {
                if (this.assignColumnToBoard(newColID, this.boardID))
                {
                    return newColID;
                }
                else
                {
                    return "";
                }
            }

        }

        public bool removeColumnFromBoard(string columnID)
        {
            return removeColumnFromBoard(columnID, "user request");
        }
        private bool removeColumnFromBoard(string columnID, string mode)
        {
            if(this.boardID==null)
            {
                return false;
            }
            else
            {
                return removeColumnFromBoard(columnID, this.boardID, mode);
            }
        }
        private bool removeColumnFromBoard(string columnID, string boardID, string mode)
        {
            if (columnID == null)
            {
                return false;
            }
            else
            {
                string ans = "";
                if (!mode.Equals("force"))
                {
                    ans = myIBoard.checkIfPossibleToUnAssignColumnFormBoard(boardID, columnID);
                }
                if (ans.Equals(""))
                {
                    string iColumnAns = myIColumn.RemoveColumn(columnID);
                    if (!iColumnAns.Equals(Guid.Empty.ToString()))
                    {
                        bool iBoardAns=true;
                        if (!mode.Equals("force"))
                        {
                            iBoardAns= myIBoard.unAssignColumnToBoard(boardID, columnID, this.userID);
                            refreshColumnsPosition();
                        }
                        return iBoardAns;
                    }

                    return false;
                }
                else
                {
                    return false;

                }
            }
        }

        public void refreshColumnsPosition()
        {
            List<String> colInCurrBoard = myIBoard.getColumnsIDsListOfBoard(boardID);
            int i = 0;
            foreach (var col in colInCurrBoard)
            {
                myIColumn.refreshPosition(col, i);
                i++;
            }
        }


        public bool changeColumnsOrder(string colID1, string colID2)
        {
            if (colID1 == null || colID2 == null)
            {
                return false;
            }
            
                bool ans= myIBoard.changeColumnsOrder(this.boardID, colID1, colID2);
            refreshColumnsPosition();
            return ans;

        }
        public string checkNext(string colID)
        {
            if (colID == null)
            {
                return "";
            }
            string nextCol = myIBoard.checkNext(this.boardID, colID);
            return nextCol;
        }

        //TASKS
        public string promote(string CurrentColID,string taskID) //Promote task!!!
        {
            string nextCol = this.checkNext(CurrentColID);

            if (nextCol.Equals(Guid.Empty.ToString()))
            {
                return "Cannot promote, either the task is in the last column or there is a problem with the inputs";
            }
            else if (myIColumn.Promote(CurrentColID, nextCol, taskID))
            {
               
                return "";
            }
            else
            {
                return "Something went wront, the promote didn't succeed. Check your inputs";
            }

        }
        public string checkIfCanCreateTask(String taskTitle, String Description, int day, int month, int year)
        {
            if (Description != null && taskTitle != null)
            {
                List<String> columnIDs = this.getColumnsIDList();
                if (columnIDs != null && columnIDs.Count > 0)
                {
                    return myIColumn.checkIfCanCreateNewTask(columnIDs[0], this.userID, Description, taskTitle, day, month, year);
                }
                else
                {
                    return "Problem with the chosen column";
                }
            }

            else
            {
                return "Can't accept empty inputs for title";
            }

        }

        public string createTask(String taskTitle, String Description, int day, int month, int year)
        {
            if (Description != null && taskTitle != null)
            {
                List<String> columnIDs = this.getColumnsIDList();
                if (columnIDs != null && columnIDs.Count > 0)
                {
                    return myIColumn.createNewTask(columnIDs[0], this.userID, Description, taskTitle, day, month, year);
                }
                else
                {
                    return "";
                }
            }

            else
            {
                return "";
            }

        }
        public string modifyTask(string columnID, string taskID, String taskTitle, String Description, int day, int month, int year)
        {
            if (userID==null || boardID==null || columnID == null || taskID == null || Description == null || taskTitle == null)
            {
                return "Missing information. Null inputs is unexcepted";
            }
            else
            {
                int position= myIBoard.getPositionOfColumn(this.boardID, columnID);
                int numOfColumns = myIBoard.getColumnsIDsListOfBoard(this.boardID).Count;
                if (position == -1 || position + 1 >= numOfColumns)
                {
                    return "Can't modify tasks in the last column";
                }
                else
                {
                    return myIColumn.TaskModify(columnID, taskID, Description, taskTitle, day, month, year);
                }
            }
        }

        public int getColumnLimit(string columnID)
        {
            if(columnID==null)
            {
                return -2;
            }
            else
            {
                return myIColumn.getColumnLimit(columnID);

            }
        }
        public bool limitNumOfTasks(string colID, int limit)
        {
            if (limit <= 0 || colID == null || myIColumn.getNumOfTasksInColumn(colID) > limit)
            {
                return false;
            }
            else
            {
                return myIColumn.limitNumOfTasksInColum(colID, limit);
            }
        }
        public bool unlimitNumOfTasks(string columnID)
        {
            if (columnID == null)
            {
                return false;
            }
            else
            {
                return myIColumn.unlimitNumOfTasksInColum(columnID);
            }
        }
      
        public int getColumnPosition(string columnID)
        {
            if(columnID==null || this.boardID==null)
            {
                return -1;
            }
            else
            {
                return myIBoard.getPositionOfColumn(this.boardID, columnID);
            }
        }

        public List<String> filterTasksByTitlePhrase(string columnID, string titlePhrase)
        {
            if (titlePhrase == null)
            {
                return null;
            }
            else
            {
                return myIColumn.filterTaskByTitle(columnID, titlePhrase);
            }
        }
        public bool sortTasksIDsByCreationDate(string columnID)
        {
            if (columnID == null)
            {
                return false;
            }
            else
            {
                if (myIColumn.SortTasksInColumnByCreationDate(columnID))
                {
                    return true;

                }
            }
            return false;
        }
        public List<List<List<String>>> loadBoardColumns()
        {
            List<List<List<String>>> output = new List<List<List<string>>>();
            if (!checkIfLoggedIn())
            {
                return null;
            }
            List<String> boardColumnsIDs = myIBoard.getColumnsIDsListOfBoard(this.boardID);
            if (boardColumnsIDs == null)
                return null;
            int i = 0, j = 0;
            foreach (String column in boardColumnsIDs)
            {
                List<String> columnStringLst = myIColumn.getColumn(column);
                if (columnStringLst != null && columnStringLst.Count > 1)
                {
                    output[i][0][0] = columnStringLst[0];
                    output[i][1][1] = columnStringLst[1];
                    List<String> tasksList = myIColumn.getColumnTasksIDList(column);
                    foreach (var task in tasksList)
                    {
                        output[i][2 + j] = myIColumn.getTask(column, task);
                        j++;
                    }
                    j = 0;
                    i++;
                }

            }
            return output;

        }

        public bool sortTasksIDsByDueDate(string columnID)
        {
            if (columnID == null)
            {
                return false;
            }
            else
            {
                if (myIColumn.SortTasksInColumnByDueDate(columnID))
                {
                    return true;

                }
            }
            return false;
        }
        public bool checkIfColumnIDisValid(string columnID)
        {
            if (columnID == null || !myIColumn.checkIfColumnExistByID(columnID))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string checkIfCouldRemoveColumnFromBoard(string columnID)
        {
            return myIBoard.checkIfPossibleToUnAssignColumnFormBoard(this.boardID, columnID);
        }
        public static string checkIfPassValid(String pass)
        {
            if (pass == null)
            {
                return "Invalid string input- doesn't except null"; ;
            }
            return IUser.checkUserPassReason(pass);

        }
        public string getBoardID()
        {
            return this.boardID;
        }

    }
}
