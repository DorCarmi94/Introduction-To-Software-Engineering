using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using log4net;
using System.Data.SQLite;

namespace Milestone3.DataPersistence_Layer
{
    class BoardHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private BinaryFormatter binaryFormatter;
        List<DummyBoard> dummyBoards;
        private static String fileName = "MyBoards.dat";

        public BoardHandler()
        {
            binaryFormatter = new BinaryFormatter();

            if (!File.Exists(fileName))
            {
                dummyBoards = new List<DummyBoard>();
                try
                {
                    FileStream myFileStream = File.Create(fileName);
                    binaryFormatter.Serialize(myFileStream, dummyBoards);//serilize an empty list - avoid deserializing an empty stream
                    myFileStream.Dispose();
                }
                catch (Exception e)
                {
                    log.Error("Error creating boards file, exeption:" + e.Message);
                    binaryFormatter = new BinaryFormatter();
                }
            }

        }
        public List<DummyBoard> LoadBoards()
        {
            string connection_string = null;
            string sql_query = null;
            string database_name = "KanBanDb.sqlite";

            SQLiteConnection connection;
            SQLiteCommand command;

            connection_string = $"Data Source={database_name};Version=3";
            connection = new SQLiteConnection(connection_string);
            SQLiteDataReader dataReader;

            try
            {
                connection.Open();

                string sql = "select * from Boards";

                
                SQLiteCommand c = new SQLiteCommand(sql, connection);
                SQLiteDataReader reader = c.ExecuteReader();
                while (reader.Read())
                {
                    string boardName =(string) reader["BoardName"];
                    long boardID = (long)reader["BoardID"];
                    string boardIdStr = Convert.ToString(boardID);
                    
                    List<String> columnIDs = this.getColumnsOfBoards(connection, boardID);
                    DummyBoard dummyBoard = new DummyBoard(boardName, boardIdStr, columnIDs);
                    Console.WriteLine(boardName);
                }
                

            }
            catch (Exception e)
            {

            }





            try
            {
                FileStream myFileStream = File.Open(fileName, FileMode.Open);
                dummyBoards = (List<DummyBoard>)binaryFormatter.Deserialize(myFileStream);
                myFileStream.Dispose();
            }
            catch (Exception e)
            {
                dummyBoards = new List<DummyBoard>();
                log.Error("Error Deserializing boards file, exception:" + e.Message);
                binaryFormatter = new BinaryFormatter();
            }

            return dummyBoards;
        }

        public List<String> getColumnsOfBoards(SQLiteConnection connection,long boardID)
        {
            List<String> BoardCols = new List<string>();
            try
            {

                string sql2 = "Select ColumnID From Columns Where BoardID = @boardID ORDER BY IndexOfColInBoard";
                SQLiteCommand commandGetColumns = new SQLiteCommand(sql2, connection);
                SQLiteParameter paramBoardID = new SQLiteParameter(@"boardID", boardID);
                commandGetColumns.Parameters.Add(paramBoardID);


                SQLiteDataReader reader2 = commandGetColumns.ExecuteReader();
                while(reader2.Read())
                {
                    long l = (long)reader2.GetValue(0);
                    BoardCols.Add(Convert.ToString(l));
                }
            }
            catch(Exception e)
            {
                return new List<string>();
            }

            return BoardCols;
        }
        public bool saveBoard(DummyBoard dummyBoard)
        {
            if (dummyBoard == null)
            {
                log.Error("trying to save a null dummy board");
                return false;
            }
            DummyBoard toFind = this.dummyBoards.
                Find(item => item.boardId.Equals(dummyBoard.boardId));
            if (toFind != null)
            {
                this.dummyBoards.Remove(toFind);

            }

            this.dummyBoards.Add(dummyBoard);
            try
            {
                FileStream myFileStream = File.Open(fileName, FileMode.Open);
                binaryFormatter.Serialize(myFileStream, dummyBoards);
                myFileStream.Dispose();
                log.Info("Board ID: " + dummyBoard.boardId + " saved successfully to hard disk");
                return true;
            }
            catch (Exception e)
            {
                binaryFormatter = new BinaryFormatter();
                log.Error("Error saving changes to boards file when trying to save the board with the following ID: " + dummyBoard.boardId + " , exception messeage:" + e.Message);
                return false;
            }
        }
    }


}
