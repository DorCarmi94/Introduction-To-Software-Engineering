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
using System.Windows;

namespace Milestone3.DataPersistence_Layer
{
    class BoardHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private BinaryFormatter binaryFormatter;
        private static String fileName = "MyBoards.dat";
        private static string database_name = "KanBanDb.sqlite";

        private List<DummyBoard> dummyBoards;

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
            dummyBoards = new List<DummyBoard>();
            SQLiteConnection connection;
            string connection_string = $"Data Source={database_name};Version=3";
            connection = new SQLiteConnection(connection_string);

            try
            {
                connection.Open();
                string sql = "select * from Boards";
                SQLiteCommand c = new SQLiteCommand(sql, connection);
                SQLiteDataReader reader = c.ExecuteReader();
                while (reader.Read())
                {
                    string boardName = reader["BoardName"].ToString();
                    string boardId = reader["BoardID"].ToString();

                    DummyBoard dummyBoard = new DummyBoard(boardName, boardId);
                    dummyBoards.Add(dummyBoard);
                    Console.WriteLine(boardName);
                }
                connection.Close();
                foreach (DummyBoard db in dummyBoards)
                {
                    db.columnsIDs = getColumnsOfBoards(connection, db.boardId);

                }
            }
            catch (Exception e)
            {
                log.Error("error reading boards from database. problem message: " + e.Message);
                dummyBoards = new List<DummyBoard>();
            }
            return dummyBoards;
        }
        private List<string> getColumnsOfBoards(SQLiteConnection connection, string boardID)
        {
            List<string> columns = new List<string>();
            try
            {
                connection.Open();

                string sql = "SELECT ColumnID from Columns WHERE BoardID='" + boardID + "' Order by IndexOfColInBoard";


                SQLiteCommand c = new SQLiteCommand(sql, connection);
                SQLiteDataReader reader = c.ExecuteReader();
                while (reader.Read())
                {
                    string colID = reader["ColumnID"].ToString();
                    columns.Add(colID);
                }
                connection.Close    ();


            }
            catch (Exception e)
            {
                log.Error("problem reading column from database. problem message: " + e.Message);
                columns = new List<string>();
            }
            return columns;
        }

        public string deleteBoard(string boardID)
        {
            DummyBoard toFind = this.dummyBoards.
                Find(item => item.boardId.Equals(boardID));
            if (toFind == null)
            {
                return "The board wasn't found in the presistence layer";
            }

            if (this.dummyBoards.Remove(toFind))
            {
                string connection_string = null;
                string database_name = "KanBanDb.sqlite";

                SQLiteConnection connection;
                SQLiteCommand command;


                try
                {
                    connection_string = $"Data Source={database_name};Version=3";
                    connection = new SQLiteConnection(connection_string);
                    connection.Open();
                    command = new SQLiteCommand(null, connection);

                    command.CommandText =
                      "DELETE FROM Boards Where BoardID = '" + boardID + "'";

                    command.ExecuteNonQuery();
                    command.Dispose();
                    return "";
                }
                catch (Exception e)
                {
                    MessageBox.Show(":" + e.Message);
                    log.Error(e.Message);
                    return "Something went wrong: e.Message";
                }
            }
            else
            {
                return "Something went wrong";
            }
        }

        public bool saveBoard(DummyBoard dummyBoard, string user)
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
            else

                this.dummyBoards.Add(dummyBoard);

            return saveInSQLDataBase(dummyBoard, user); //change to user 
        }
        private void deleteFromSql(DummyBoard toDelete)
        {

        }
        private bool saveInSQLDataBase(DummyBoard toSave, String user)
        {
            SQLiteConnection connection;
            string connection_string = $"Data Source={database_name};Version=3";
            connection = new SQLiteConnection(connection_string);

            try
            {
                connection.Open();

                String query = "INSERT INTO Boards (BoardID,BoardName,Email_UserID) VALUES (@boardID,@boardName,@user)";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteParameter param0 = new SQLiteParameter("@boardID", toSave.boardId);
                SQLiteParameter param1 = new SQLiteParameter("@boardName", toSave.boardName);
                SQLiteParameter param2 = new SQLiteParameter("@user", user);

                command.Parameters.Add(param0);
                command.Parameters.Add(param1);
                command.Parameters.Add(param2);

                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception e)
            {
                log.Error("Problem saving board: " + toSave.boardId + " problem message: " + e.Message);
                return false;
            }
        }

        private bool updateSQLDataBase(DummyBoard toUpdate, String user)
        {
            SQLiteConnection connection;
            string connection_string = $"Data Source={database_name};Version=3";
            connection = new SQLiteConnection(connection_string);

            try
            {
                connection.Open();
                string sql = "update Boards SET BoardID=@param0    ";
                SQLiteCommand c = new SQLiteCommand(sql, connection);
                SQLiteDataReader reader = c.ExecuteReader();
                connection.Close();

                return true;
            }
            catch (Exception e)
            {
                log.Error("Problem saving board: " + toUpdate.boardId + " problem message: " + e.Message);
                return false;
            }
        }
        //TODO change update!
    }


}