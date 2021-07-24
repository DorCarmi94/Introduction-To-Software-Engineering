using Milestone3.Bussiness_Layer.Interface;
using Milestone3.Presentation_Layer.Functionality;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Milestone3.Presentation_Layer.ViewModel
{
    class BoardSelectDataContext
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public BoardSelectFunc boardSelectFunc;

        private ObservableCollection<PresentBoards> boards;
        public ObservableCollection<PresentBoards> Boards
        {
            get
            {
                return this.boards;
            }
            set
            {
                boards = value;
                updateBoardsGridView();
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Boards"));
            }

        }



        private void updateBoardsGridView()
        {

            CollectionViewSource cvs = new CollectionViewSource() { Source = boards };
            BoardsGridView = cvs.View;
        }

        public string loadSpecificBoard()
        {
            String loadAns= boardSelectFunc.loadSpecificBoard(selectedBoard.BoardID);
            return loadAns;
        }

        private ICollectionView boardsGridView;
        public ICollectionView BoardsGridView
        {
            get
            {
                return boardsGridView;
            }
            set
            {
                boardsGridView = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("BoardsGridView"));
            }
        }

        

        private PresentBoards selectedBoard;
        public PresentBoards SelectedBoard
        {
            get
            {
                return this.selectedBoard;
            }
            set
            {
                selectedBoard = value;
                
                
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedBoard"));
            }
        }

        private String newBoardName;
        public String NewBoardName
        {
            get
            {
                return this.newBoardName;
            }
            set
            {
                newBoardName = value;


                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("NewBoardName"));
            }
        }

        public string createNewBoard()
        {

            if (NewBoardName == null || NewBoardName.Length <= 0)
            {
                return "Can't create board with empty name";
            }
            else
            {
                List<PresentBoards> boardLst = this.boards.ToList<PresentBoards>();
                var board = boardLst.Find(item => item.BoardName.Equals(NewBoardName));
                if (board == null)
                {
                    string ans = boardSelectFunc.createNewBoard(NewBoardName);
                    this.Boards = boardSelectFunc.LoadBoards();
                    return ans;

                }
                else
                {
                    return "A board with the same name already exists in this user's list";
                }
            }


            
           
        }

        public string removeBoard()
        {
            if(selectedBoard==null)
            {
                return "No board was selected";
            }
            else
            {
                
                string ans=boardSelectFunc.removeBoard(selectedBoard.BoardID);
                this.Boards = boardSelectFunc.LoadBoards();
                return ans;
            }
        }

        public BoardSelectDataContext(string loggedInEmail, IConnect service)
        {
            boardSelectFunc = new BoardSelectFunc(loggedInEmail, service);
            this.Boards = boardSelectFunc.LoadBoards();

        }
    }
    
}
