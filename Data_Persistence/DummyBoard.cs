using System;
using System.Collections.Generic;

namespace Milestone2.Data_Persistence
{
    [Serializable]
    class DummyBoard //: ISerializable
    {
        public String boardName;
        public string boardId;
        public List<String> columnsIDs;
        public DummyBoard(string boardName, string idb,List<String> columnsList)
        {
            this.boardName = boardName;
            this.boardId = idb;
            this.columnsIDs = columnsList;
        }
        public override string ToString()
        {
            string str = "BoardName: " + this.boardName + "\n Board IDB: "
                + this.boardId + "\n";
            return str;
        }

        
    }

    
}
