using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using log4net;
using System.Data.SQLite;
using System.Windows;

namespace Milestone3.DataPersistence_Layer
{
    class ColumnHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Stream myFileStream;
        private BinaryFormatter binaryFormatter;
        private bool fileIsNew;
        private List<DummyColumn> dummyColumns;
        private String fileName = "ColumnsTasks.dat";

        public ColumnHandler()
        {
            dummyColumns = new List<DummyColumn>();
            if (File.Exists(fileName))
            {
                fileIsNew = false;
            }
            else
            {
                try
                {
                    myFileStream = File.Open(fileName, FileMode.Create);
                    binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(myFileStream, dummyColumns);
                    fileIsNew = true;
                    myFileStream.Dispose();
                    dummyColumns = new List<DummyColumn>();
                }
                catch (Exception e)
                {
                    binaryFormatter = new BinaryFormatter();

                    log.Fatal("There has been a problem creating new file. exception message: " + e.Message);

                }
            }
        }

        public List<DummyColumn> LoadColumns()
        {
            List<DummyColumn> dummyColumns = new List<DummyColumn>();
            string connection_string = null;
            string sql = null;
            string database_name = "KanBanDb.sqlite";

            SQLiteConnection connection;
            SQLiteCommand command;

            connection_string = $"Data Source={database_name};Version=3";
            connection = new SQLiteConnection(connection_string);
            SQLiteDataReader dataReader;

            try
            {
                connection.Open();

                sql = "SELECT * from Columns";

                command = new SQLiteCommand(sql, connection);
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    string columnID = (string)dataReader["ColumnID"];
                    string name = (string)dataReader["ColumnName"];
                    
                    List<DummyTask> tasks = loadTasks(columnID);
                    DummyColumn DC = new DummyColumn(name, columnID, tasks); //update hashTable
                    dummyColumns.Add(DC);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                log.Error("Cannot load columns. problem: " + e);
                dummyColumns = new List<DummyColumn>();

            }
            finally
            {
                connection.Close();
            }


            return dummyColumns;

        }

        private List<DummyTask> loadTasks(string columnID)
        {
            List<DummyTask> dummyTasks = new List<DummyTask>();
            string connection_string = null;
            string sql = null;
            string database_name = "KanBanDb.sqlite";

            SQLiteConnection connection;
            SQLiteCommand command;

            connection_string = $"Data Source={database_name};Version=3";
            connection = new SQLiteConnection(connection_string);
            SQLiteDataReader dataReader;

            try
            {
                connection.Open();

                sql = "SELECT * from Tasks " +
                    "WHERE ColumnID= '" +columnID+ "'";

                command = new SQLiteCommand(sql, connection);
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    string TaskId = (string)dataReader["TaskID"];
                    string title = (string)dataReader["Title"];
                    string desc = (string)dataReader["Description"];
                    string creationD = (string)dataReader["CreationDate"];
                    string dueD = (string)dataReader["DueDate"];
                    DummyTask DT = new DummyTask(TaskId, title, desc, creationD, dueD);
                    dummyTasks.Add(DT);
                }
            }
            catch (Exception e)
            {
                log.Error("Cannot load Tasks for columns" +columnID+" problem: " + e);
                dummyColumns = new List<DummyColumn>();

            }
            finally
            {
                connection.Close();
            }


            return dummyTasks;

        }

        public void promoteTask(String taskID, String colID)
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


                String sql = "UPDATE Tasks " +
                         "SET ColumnID = '" + colID + "' " +
                              "WHERE TaskID = '" + taskID + "'";

                command = new SQLiteCommand(sql, connection);
                //command.ExecuteReader();

                command.ExecuteNonQuery();
                command.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show(":" + e.Message);
                log.Error(e.Message);
            }
        }

        public void saveTask(DummyTask dummyTask, string colID)
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
                  "INSERT INTO Tasks (TaskID, Title, Description, CreationDate, DueDate, ColumnID)" +
                        "VALUES (@taskID, @title, @descreption, @creationDate, @dueDate, @colID)";

                SQLiteParameter taskID_param = new SQLiteParameter(@"taskID", dummyTask.TaskID.ToString());
                SQLiteParameter title_param = new SQLiteParameter(@"title", dummyTask.Title);
                SQLiteParameter descreption_param = new SQLiteParameter(@"descreption", dummyTask.Description);
                SQLiteParameter creationDate_param = new SQLiteParameter(@"creationDate", dummyTask.creationDate.ToString());
                SQLiteParameter dueDate_param = new SQLiteParameter(@"dueDate", dummyTask.dueDate.ToString());
                SQLiteParameter colID_param = new SQLiteParameter(@"colID", colID);

                command.Parameters.Add(taskID_param);
                command.Parameters.Add(title_param);
                command.Parameters.Add(descreption_param);
                command.Parameters.Add(creationDate_param);
                command.Parameters.Add(dueDate_param);
                command.Parameters.Add(colID_param);

                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show(":" + e.Message);
                log.Error(e.Message);
            }

        }

        public void saveExistingTask(DummyTask dummyTask, string colID)
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
                

                String sql = "UPDATE Tasks " +
                         "SET Title = '" + dummyTask.Title + "', " +
                            "Description = '" + dummyTask.Description + "', " +
                            "DueDate = '" + dummyTask.dueDate + "' " +
                              "WHERE TaskID = '" + dummyTask.TaskID + "'";

                command = new SQLiteCommand(sql, connection);
                //command.ExecuteReader();

                command.ExecuteNonQuery();
                command.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show(":" + e.Message);
                log.Error(e.Message);
            }

        }

        public void saveColumn(DummyColumn dummyColumn, string boardID, int index)
        {
            DummyColumn theCol = this.GetColumn(dummyColumn.columnID);
            if (theCol != null)
            {
                this.dummyColumns.Remove(theCol);
            }

            this.dummyColumns.Add(dummyColumn);
            saveColumnToFile(dummyColumn, boardID, index);
        }

        private void saveColumnToFile(DummyColumn dummyColumn, string boardID, int index)
        {

            if(index == -1)
                index = findPrevIndex(boardID) + 1;

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
                  "INSERT INTO Columns (ColumnID, ColumnName, BoardID, IndexOfColInBoard)" +
                        "VALUES (@columnID, @columnName, @boardID, @index)";

                SQLiteParameter columnID_param = new SQLiteParameter(@"columnID", dummyColumn.columnID);
                SQLiteParameter columnName_param = new SQLiteParameter(@"columnName", dummyColumn.columnName);
                SQLiteParameter boardID_param = new SQLiteParameter(@"boardID", boardID);
                SQLiteParameter index_param = new SQLiteParameter(@"index", index);

                command.Parameters.Add(columnID_param);
                command.Parameters.Add(columnName_param);
                command.Parameters.Add(boardID_param);
                command.Parameters.Add(index_param);

                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show(":" + e.Message);
                log.Error(e.Message);
            }

        }

        public void refreshColumns(String columnID,  int index)
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


                String sql = "UPDATE Columns " +
                         "SET IndexOfColInBoard = '" + index + "' " +
                              "WHERE ColumnID = '" + columnID + "'";

                command = new SQLiteCommand(sql, connection);
                //command.ExecuteReader();

                command.ExecuteNonQuery();
                command.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show(":" + e.Message);
                log.Error(e.Message);
            }
        }

        private int findPrevIndex(string boardID)
        {
            string connection_string = null;
            string database_name = "KanBanDb.sqlite";
            int index;

            SQLiteConnection connection;
            SQLiteCommand command;
            SQLiteDataReader dataReader;


            try
            {
                connection_string = $"Data Source={database_name};Version=3";
                connection = new SQLiteConnection(connection_string);
                connection.Open();
                command = new SQLiteCommand(null, connection);

                command.CommandText =
                                "SELECT MAX(IndexOfColInBoard) " +
                                    "FROM Columns " +
                                        "WHERE BoardID ='" + boardID + "'";

                dataReader = command.ExecuteReader();
                index = 0;
            

                while (dataReader.Read())
                { long i = (long)dataReader[0];
                    index = (int)i; }


                connection.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show(":" + e.Message);
                log.Error(e.Message);
                index = -1;
            }

            return index;

        }

        internal void removeColumn(DummyColumn Column)

        {
            DummyColumn theCol = this.GetColumn(Column.columnID);
            if (theCol != null)
            {
                this.dummyColumns.Remove(theCol);
            }

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
                  "DELETE FROM Tasks Where ColumnID = '" + Column.columnID + "'";
                command.ExecuteNonQuery();

                command.CommandText =
                  "DELETE FROM Columns Where ColumnID = '" + Column.columnID + "'";
                command.ExecuteNonQuery();

                command.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show(":" + e.Message);
                log.Error(e.Message);
            }





        }

        private DummyColumn GetColumn(String columnID)
        {
            DummyColumn theCol = this.dummyColumns.Find(item => item.columnID.Equals(columnID));
            return theCol;

        }
    }


}
