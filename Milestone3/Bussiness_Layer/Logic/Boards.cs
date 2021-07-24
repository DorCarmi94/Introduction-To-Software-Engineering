using System;
using System.Collections.Generic;
using Milestone3.DataPersistence_Layer;


namespace Milestone3.Bussiness_Layer.Logic
{
    class Boards
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private List<Board> BoardsList;
        private List<Board> filteredBoardsList;
        private bool isFiltered;
        private BoardHandler myBoardHandler;

        public Boards()
        {

            myBoardHandler = new BoardHandler();
            isFiltered = false;
            this.BoardsList = new List<Board>();
        }

        public string createNewBoard(string BoardName)
        {
            Board newBoard = new Board(BoardName);
            DummyBoard newDummyBoard = newBoard.boardToDummy();
            if (myBoardHandler.saveBoard(newDummyBoard))
            {
                this.BoardsList.Add(newBoard);
                return newBoard.getBoardId().ToString();
            }
            return "";
        }
        public bool saveBoard(Board board)
        {
            if (board == null)
            {
                log.Error("trying to save a null board");
                return false;
            }
            return myBoardHandler.saveBoard(board.boardToDummy());
        }
        public Board getByBoardId(Guid boardID)
        {

            Board retBoard = null;
            int numOfBoards = this.BoardsList.Count;

            int i = 0;
            bool found = false;
            while (i < numOfBoards && !found)
            {

                if (BoardsList[i].getBoardId().Equals(boardID))
                {
                    retBoard = BoardsList[i];
                    found = true;
                }
                else
                {
                    i++;
                }
            }
            if (retBoard == null)
                log.Info("no such board with the following ID: " + boardID.ToString());
            return retBoard;
        }
        public void startUp()
        {
            List<DummyBoard> dummyBoardsList = myBoardHandler.LoadBoards();
            if (dummyBoardsList != null)
            {
                foreach (DummyBoard dummyBoard in dummyBoardsList)
                {
                    Board newBoard = new Board(dummyBoard);
                    this.BoardsList.Add(newBoard);
                }
            }
        }
        public override string ToString()
        {
            string str = "";
            foreach (Board board in this.BoardsList)
            {
                str = board.ToString() + "\n";
            }
            return str;
        }
    }
}
