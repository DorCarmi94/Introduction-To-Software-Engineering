using System;
using System.Collections.Generic;

namespace Milestone3.DataPersistence_Layer
{
    [Serializable]
    class DummyBoard //: ISerializable
    {
        public String boardName;
        public string boardId;
        public List<String> columnsIDs;
        public DummyBoard(string boardName, string idb, List<String> columnsList)
        {
            this.boardName = boardName;
            this.boardId = idb;
            this.columnsIDs = columnsList;
        }
        public DummyBoard(string boardName, string idb)
        {
            this.boardName = boardName;
            this.boardId = idb;
        }
        public override string ToString()
        {
            string str = "BoardName: " + this.boardName + "\n Board IDB: "
                + this.boardId + "\n";
            return str;
        }


    }


}
