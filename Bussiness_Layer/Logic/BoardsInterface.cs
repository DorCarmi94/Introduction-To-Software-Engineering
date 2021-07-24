using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milestone2.Bussiness_Layer.Logic
{
    interface BoardsInterface
    {
        void startUp();
         void LogOut();
         Board getByIDB(Guid IDB);
        string createNewBoard(string BoardName);
    }




    interface BoardInterface
    {
        String getTitle();
        Guid getIDB();
        string ToString();
    }
}
