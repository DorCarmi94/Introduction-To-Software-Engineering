using Milestone3.Bussiness_Layer.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milestone3.Presentation_Layer.Functionality
{
    class BoardSelectFunc
    {
        public IConnect service;
        public string loggedInEmail;
        public BoardSelectFunc(string loggedInEmail, IConnect service)
        {

            this.service = service;
            this.loggedInEmail = loggedInEmail;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<PresentBoards> LoadBoards()
        {
            List<String> boardIDs = service.getUserBoards();

            ObservableCollection<PresentBoards> boards = new ObservableCollection<PresentBoards>();
            foreach (var board in boardIDs)
            {
                String boardName = service.getBoard(board);
                if (!boardName.Equals(""))
                {
                    PresentBoards newBoard = new PresentBoards()
                    {
                        BoardName=boardName,
                        BoardID=board

                    };
                    boards.Add(newBoard);
                }

            }
            return boards;
        }

        public string loadSpecificBoard(string boardID)
        {
            string ans=service.loadSpecificBoard(boardID);
            return ans;
        }

        public string createNewBoard(string newBoardName)
        {
            return service.createNewBoard(newBoardName);
        }

        public string removeBoard(string boardID)
        {
            if(boardID==null)
            {
                return "null boardID error";
            }
            else
            {
                return this.service.removeBoard(boardID);
            }
        }
    }

   
    public class PresentBoards
    {
        public string BoardName { get; set; }

        public string BoardID { get; set; }
    }
}
