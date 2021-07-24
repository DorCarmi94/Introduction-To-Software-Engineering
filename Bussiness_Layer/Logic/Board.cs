using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milestone2.Data_Persistence;
using log4net;
using System.Collections.Generic;
using System.Collections;

namespace Milestone2.Bussiness_Layer.Logic
{
    

    class Board

    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Guid boardID;
        private String boardTitle;
        private List<Guid> columnsIDsList;

        public Board(String BoardTitle)
        {
            this.boardID = Guid.NewGuid();
            this.boardTitle = BoardTitle;
            this.columnsIDsList = new List<Guid>();
        }
        public Board(DummyBoard dummyBoard)
        {
            if (!(Guid.TryParse(dummyBoard.boardId, out this.boardID)))
            {
               log.Error("Error parsing board IDB");
            }
            this.boardTitle = dummyBoard.boardName;
            this.columnsIDsList = new List<Guid>();
            dummyBoard.columnsIDs.ForEach
                (u => columnsIDsList.Add(Guid.Parse(u)));
            

        }

        public bool assignColumnToBoard(Guid columnID_guid)
        {
            Guid col = this.columnsIDsList.Find(item => item.Equals(columnID_guid));
            if(col!=Guid.Empty)
            {
                log.Warn("Trying to assign the following column: " + columnID_guid.ToString() + " to the board " + this.boardID.ToString() + ", though he already assinged to it.");
                return false;
            }
            else
            {
                this.columnsIDsList.Add(columnID_guid);
                return true;
            }
            
        }
        public bool changeColumnsOrder(Guid col1ID_guid, Guid col2ID_guid)
        {
            int col1index=this.columnsIDsList.FindIndex(item => item.Equals(col1ID_guid));
            int col2index = this.columnsIDsList.FindIndex(item => item.Equals(col2ID_guid));

            if (col1index == -1)
            {
                log.Error("Change columns Order couldn't happen  because the column: " + col1ID_guid.ToString() + " doesn't exist in the context of board: " + this.boardID.ToString());
                return false;
            }
            if ( col2index == -1)
            {
                log.Error("Change columns Order couldn't happen  because the column: " + col2ID_guid.ToString() + " doesn't exist in the context of board: " + this.boardID.ToString());
                return false;
            }
            columnsIDsList[col1index] = col2ID_guid;
            columnsIDsList[col2index] = col1ID_guid;
            return true;
        }
        public bool checkIfCouldUnAssign(Guid columnID_guid)
        {
            Guid col = this.columnsIDsList.Find(item => item.Equals(columnID_guid));
            if (col != Guid.Empty)
<<<<<<< HEAD
            {                
=======
            {
>>>>>>> 12fe8fe5a08793861f969678fb915581a632da1c
                return true;
            }
            else
            {
                return false;
            }
        } //TODO check if neccessery 
        public Guid checkNext(Guid colID_guid)
        { 
            int currCol = this.columnsIDsList.
                FindIndex(item => item.Equals(colID_guid));
            if (currCol == -1)
            {
                log.Warn("trying to extract the column after column " + colID_guid.ToString() + " in board " + this.boardID.ToString() + " though it doesn't exist in this board ");
                return Guid.Empty;
            }
            if (currCol+1==this.columnsIDsList.Count)
            {
                log.Warn("trying to extract the column after column " + colID_guid.ToString() + " in board " + this.boardID.ToString() + " though it is the last");
                return Guid.Empty;
            }
            return columnsIDsList[currCol + 1];
        }
        public DummyBoard boardToDummy()
        {
            List<String> columnsList = new List<string>();
            foreach (var col in this.columnsIDsList)
            {
                columnsList.Add(col.ToString());
            }
            return new DummyBoard(this.boardTitle, this.boardID.ToString(), columnsList);
        }
        public int NumOfCoulmns() { return columnsIDsList.Count; }
        public int positionOfColumn(Guid columnId) //return -1 if col not exists in board.
        {
            return this.columnsIDsList.
                FindIndex(item => item.Equals(columnId));
        }
        public override string ToString()
        {
            string str = "Board Title: " + this.boardTitle + "\nBoard ID: " + this.boardID;
            return str;
        }
        public bool unAssignColumnToBoard(Guid columnID_guid)
        {
            if (this.columnsIDsList.Remove(columnID_guid))
                return true;
            log.Error("trying to remove column " + columnID_guid.ToString() + " though it doesn't exist in board " + this.boardID.ToString());
            return false;
           
        }

        public Guid getBoardId()
        {
            return this.boardID;
        }
        public String getTitle()
        {
            return this.boardTitle;
        }
        public List<Guid> getColumnsIDsList()
        {
            return this.columnsIDsList;
        }

    }
}
