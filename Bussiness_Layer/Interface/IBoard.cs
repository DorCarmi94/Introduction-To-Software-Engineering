using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milestone2.Bussiness_Layer.Logic;
using log4net;

namespace Milestone2.Bussiness_Layer.Interface
{
    class IBoard
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
               System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Boards boards;
        private Board currentBoard;

        public IBoard()
        {

            boards = new Boards();
            
        }
<<<<<<< HEAD
        public string createNewBoard(String boardName)
        {
            return boards.createNewBoard(boardName);
        }
        public String LoadSpecificBoard(String guid_str)
        {
            if (!(Guid.TryParse(guid_str, out Guid g)))
            {
                log.Error("Error trying to parse the input board id the following string was the input: " + guid_str);
                return "";
            }
            else
            {
                currentBoard = boards.getByBoardId(g);
                if (currentBoard == null)
                {
                    return "There are no boards available";

                }
                return currentBoard.ToString();
            }
        }

=======
>>>>>>> 12fe8fe5a08793861f969678fb915581a632da1c
        public bool assignColumnToBoard(string boardID, string columnID)
        {
            if (!(Guid.TryParse(boardID, out Guid boardID_guid)))
            {
                log.Warn("the following string couldn't parse to Guid object: boardID string: " + boardID);
                return false;
            }
            if (!(Guid.TryParse(columnID, out Guid columnID_guid)))
            {
                log.Warn("the following string couldn't parse to Guid object: columnID string: " + columnID);
                return false;
            }
            Board b = boards.getByBoardId(boardID_guid);
            if (b == null)
                return false;
            return (b.assignColumnToBoard(columnID_guid) && boards.saveBoard(b));
           
        }
        public bool changeColumnsOrder(string boardID,string colID1, string colID2)
        {
            if(!(Guid.TryParse(boardID,out Guid boardID_guid)) || !(Guid.TryParse(colID1, out Guid col1ID_guid)) || !(Guid.TryParse(colID2, out Guid col2ID_guid)))
            {
                log.Warn("One or more of the following strings couldn't parse to Guid object: boardID string: " + boardID + " colID1 string: " + colID1 + " colID2 string: "+ colID2);
                return false;
            }
            
            Board board = boards.getByBoardId(boardID_guid);
            if (board == null)
                return false;
            return board.changeColumnsOrder(col1ID_guid, col2ID_guid);
            
            
        }
        public string checkIfPossibleToUnAssignColumnFormBoard(string boardID, string columnID)
        {
            string answer = "";
            if (!(Guid.TryParse(boardID, out Guid boardID_guid)))
            {

                answer = "There is a problem parsing board ID";
                return answer;
            }
            if (!(Guid.TryParse(columnID, out Guid columnID_guid)))
            {
                answer = "There is a problem parsing column ID";
                return answer;

            }
            Board b = boards.getByBoardId(boardID_guid);
            if (b.NumOfCoulmns() <= 1)
            {
                answer = "Cannot remove all columns from board. Must remain one";
                return answer;
            }
            bool ans = b.checkIfCouldUnAssign(columnID_guid);
            if(!ans)
            {
                
                answer = "The column you requested to remove dosen't exist on this board";
                return answer;
            }
            return answer;
<<<<<<< HEAD
        }

        public List<String> getColumnsIDsListOfBoard(String boardID)
=======
        }  //TODO check if neccessery 
        public string checkNext(string boardId,string colID)
>>>>>>> 12fe8fe5a08793861f969678fb915581a632da1c
        {
            if(!(Guid.TryParse(colID,out Guid colID_guid)) || !(Guid.TryParse(boardId, out Guid boardID_guid)))
            {

                return Guid.Empty.ToString();
            }
            Board board = boards.getByBoardId(boardID_guid);
            if (board == null)
                return Guid.Empty.ToString();
            Guid ans= board.checkNext(colID_guid);
            if(ans.Equals(Guid.Empty))
            {
                return Guid.Empty.ToString();
            }
            return ans.ToString();
            
        }
        public string createNewBoard(String boardName)
        {
            return boards.createNewBoard(boardName);
        }
        public List<String> getColumnsIDsListOfBoard(String boardID)
        {
            Board board = boards.getByBoardId(Guid.Parse(boardID));
            if (board==null)
                return null;
            List<String> idsLstStr= new List<string>();
            board.getColumnsIDsList().ForEach
                (u => idsLstStr.Add(u.ToString()));
            return idsLstStr;
        
        }
        public String LoadSpecificBoard(String guid_str)
        {
            if (!(Guid.TryParse(guid_str, out Guid g)))
            {
                log.Error("Error trying to parse the input board id the following string was the input: " + guid_str);
                return "";
            }
            else
            {
                currentBoard = boards.getByBoardId(g);
                if (currentBoard == null)
                {
                    log.Error("Error trying to load unexisted board with the following ID: " + guid_str);
                    return ""; //TODO check if return an empty string
                }
                return currentBoard.ToString();
            }
        }
        public void startUp()
        {
            boards.startUp();    
        }     
        public override string ToString()
        {
            return boards.ToString();
        }
        public bool unAssignColumnToBoard(string boardID, string columnID)
        {
            if (!(Guid.TryParse(boardID, out Guid boardID_guid)))
            {

                return false;
            }
            if (!(Guid.TryParse(columnID, out Guid columnID_guid)))
            {

                return false;
            }
            Board board = boards.getByBoardId(boardID_guid);
            if (board == null)
                return false;
            if (board.NumOfCoulmns()<=1)
            {
                return false;
            }
            bool ans = board.unAssignColumnToBoard(columnID_guid);
            boards.saveBoard(board);
            return ans;
        }
        public bool ColumnInBoard (string boardID,string columnID)
        {
            if (!(Guid.TryParse(boardID, out Guid boardID_guid)))
            {
                log.Warn("the following string couldn't parse to Guid object: boardID string: " + boardID);
                return false;
            }
            if (!(Guid.TryParse(columnID, out Guid columnID_guid)))
            {
                log.Warn("the following string couldn't parse to Guid object: columnID string: " + columnID);
                return false;
            }
            Board board = boards.getByBoardId(boardID_guid);
            if (board == null)
                return false;
            return (board.positionOfColumn(columnID_guid) != -1);
        }
    }
}
