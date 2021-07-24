using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milestone2.Bussiness_Layer.Interface
{
    class IConnect
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IColumn myIColumn;
        private IBoard myIBoard;
        private IUser myIUser;
        private string userID;
        private string boardID;

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
            IUser.startUp();
            myIBoard.startUp();
            myIColumn.startUp();

        }
        //GETS

        public string getBoardID()
        {
            return this.boardID;
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
            if(columnID==null | boardID ==null)
            {
                log.Error(" board or column null");
                return (new List<String>());
            }
            if (myIBoard.ColumnInBoard(boardID,columnID))
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

        public List<String> getTask(string columnID,string taskID)
        {
            if(getColumn(columnID).Count>0)
                return myIColumn.getTask(columnID, taskID);
            return (new List<String>());
        }


        //USER
        public string login(String email, string pass)
        {
            if (email == null || pass == null)
            {
                log.Warn("trying to login with email or password that is null");
                return "Invalid string input- doesn't except null";
            }
            string validEmail = IConnect.checkIfEmailValid(email);
            if (!validEmail.Equals(""))
            {
                log.Warn("bad email inserted " + email + ". "+validEmail);
                return validEmail;
            }
            if (!IUser.checkUserPass(pass))
            {
                log.Warn("bad email inserted " + pass + ". " +IUser.checkUserPassReason(pass));
                return IUser.checkUserPassReason(pass);
            }
            string loggedInEmail = this.myIUser.Login(email, pass);
            if (loggedInEmail.Equals(email))
            {
                log.Info("user " + email +"logged in!");
                this.userID = email;
                this.boardID = this.getUserBoards()[0];
                return email;
            }
            else
            {
                log.Warn("bad email - password combination, email entered: " + email + " password: " + pass);
                return "There is a problem with the email addresss you entered";
            }
        }
        public List<String> getUserBoards()
        {
            if (!checkIfLoggedIn())
            {
                log.Error("user tried to access boards though he shouldn't be able to do it due to the fact that no one is logged in");
                return null;
            }
            return myIUser.getBoards(this.userID);
        }
        public void logOut()
        {
            if (userID == null)
                log.Info("Arya Stark has logged out successfully");
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
                log.Error("cannot create user, email or pass equals null. email: " + email+" pass: " +pass);
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
            myIUser.CreateNewUser(email, pass); //TODO check bool
            string boardID = myIBoard.createNewBoard(email); // TODO check if string not empty
            string assignSucceed = assignBoardToUser(email, boardID);
            if (!assignSucceed.Equals(""))
                return assignSucceed; //if the assign failed, returns the reason
            List<String> newColumnsIDsList = createDeafaultColumnsForNewBoard();
            bool assignColsBoardSucceed = newColumnsIDsList != null;
            
            if (assignColsBoardSucceed == false)
                return "assign Col To Board failed";
            return "";
        }
        private string createNewBoard(String boardName)
        {
            return myIBoard.createNewBoard(boardName);
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
        public List<String> createDeafaultColumnsForNewBoard()
        {
            List<String> newColumnsIDsList = new List<string>();
            newColumnsIDsList = myIColumn.createDeafaultColumnsForNewBoard();
            foreach (var newCol in newColumnsIDsList)
            {
                this.assignColumnToBoard(newCol);
            }
            return newColumnsIDsList;
        }
        public bool assignColumnToBoard(string columnID)
        {
            return myIBoard.assignColumnToBoard(this.boardID, columnID);
        }
        //---
        public string changeUserPass(string currPass, string newPass)
        {
            if (!checkIfLoggedIn())
            {
                return "User is not logged in";
            }
            if (currPass == null || newPass == null)
            {
                return "Invalid string input- doesn't except null";
            }
            if (myIUser.ChangeUserPass(this.userID, currPass, newPass))
            {
                return "";
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
        public static string checkIfPassValid(String pass)
        {
            if (pass == null)
            {
                return "Invalid string input- doesn't except null"; ;
            }
            return IUser.checkUserPassReason(pass);

        }
        public bool checkIfLoggedIn()
        {
            return this.userID == null || this.boardID == null;
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
        public List<List<List<String>>> loadBoardColumns()
        {
            List < List < List < String >>> output= new List<List<List<string>>>();
            if (!checkIfLoggedIn())
            {
                return null;
            }
            List<String> boardColumnsIDs = myIBoard.getColumnsIDsListOfBoard(this.boardID);
            if (boardColumnsIDs == null)
                return null;
            int i = 0, j = 0;
            foreach(String column in boardColumnsIDs)
            {
                List<String> columnStringLst = myIColumn.getColumn(column);
                if(columnStringLst!=null && columnStringLst.Count>1)
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
        public string addColumnToBoard(String colName)
        {

            if(colName==null || colName.Length<1)
            {
                return "";
            }
            string newColID= myIColumn.addNewColumn(colName);
            if (newColID.Equals(""))
            {
                return "";
            }
            else
            {
                if (this.assignColumnToBoard(newColID))
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
            if (columnID == null)
            {
                return false;
            }
            else
            {
                string ans=myIBoard.checkIfPossibleToUnAssignColumnFormBoard(this.boardID,columnID);
                if (ans.Equals(""))
                {
                    string iColumnAns = myIColumn.RemoveColumn(columnID);
                    if (!iColumnAns.Equals(Guid.Empty.ToString()))
                    {
                        bool iBoardAns = myIBoard.unAssignColumnToBoard(this.boardID, columnID);
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

        public string checkIfCouldRemoveColumnFromBoard(string columnID)
        {
            return myIBoard.checkIfPossibleToUnAssignColumnFormBoard(this.boardID, columnID);
        }
        public bool changeColumnsOrder(string colID1, string colID2)
        {
            if(colID1==null || colID2==null)
            {
                return false;
            }
            return myIBoard.changeColumnsOrder(this.boardID, colID1, colID2);
        }
        public string checkNext(string colID)
        {
            if(colID==null)
            {
                return "";
            }
            string nextCol = myIBoard.checkNext(this.boardID, colID);
            return nextCol;
        }

        //TASKS
        public string promote(string taskID, string CurrentColID)
        {
            string nextCol = this.checkNext(CurrentColID);
            
            if (nextCol.Equals(Guid.Empty.ToString()))
            {
                return "Cannot promote, either the task is in the last column or there is a problem with the inputs";
            }
            else if(myIColumn.Promote(CurrentColID, nextCol,taskID))
            {
                return "";
            }
            else
            {
                return "Something went wront, the promote didn't succeed. Check your inputs";
            }
            
        }
        public string createTask(String taskTitle,String Description, int day, int month, int year)
        {
            if(Description!=null && taskTitle!=null)
            {
                List<String> columnIDs = this.getColumnsIDList();
                if (columnIDs != null && columnIDs.Count>0)
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
        public bool modifyTask(string columnID, string taskID,  String taskTitle, String Description, int day, int month, int year)
        {
            if (columnID == null || taskID == null || Description == null || taskTitle == null)
            {
                return false;
            }
            else
            {
                return myIColumn.TaskModify(columnID, taskID, Description, taskTitle, day, month, year);
            }
        }
        public bool limitNumOfTasks(string colID, int limit)
        {
            if(limit<=0 || colID==null || myIColumn.getNumOfTasksInColumn(colID)>limit)
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
        public List<String> filterTasksByTitlePhrase(string columnID, string titlePhrase)
        {
            if(titlePhrase==null)
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
                if(myIColumn.SortTasksInColumnByCreationDate(columnID))
                {
                    return true;
                    
                }
            }
            return false;
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
    }
}
