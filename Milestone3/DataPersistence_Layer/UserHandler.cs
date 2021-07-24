using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using log4net;
using System.Data.SQLite;

namespace Milestone3.DataPersistence_Layer
{
    [Serializable]
    class UserHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static String fileName = "dummyUsers.bin";

        public bool ChangeDummyUserPass(String email, String currUserPass, String newUserPass)
        {
            if (!DummyUser.hashDummyUsers.Contains(email))
                return false;
            DummyUser dummyU = (DummyUser)DummyUser.hashDummyUsers[email];
            if (!dummyU.getPass().Equals(currUserPass))
                return false;

            dummyU.setPass(newUserPass);
            DummyUser.hashDummyUsers.Remove(email);
            DummyUser.hashDummyUsers.Add(email, dummyU);
            SaveUser(dummyU);
            return true;
        }
        public bool SaveUser(DummyUser dummyUser)
        {
            DummyUser newDummyUser = dummyUser;

            try
            {
                FileStream fileStream = new FileStream(fileName, FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fileStream, DummyUser.hashDummyUsers);
                fileStream.Dispose();
            }
            catch (Exception e)
            {
                log.Error("Creat or Update dummyUsers to file - exception detail: " + e.Message);
            }


            //Console.WriteLine("email:" + dummyUser.getEmail() + "." + "pass:" + dummyUser.getPass() + "--" + "is saved");
            return true;
        }
        public bool SaveNewUser(String email, String pass)
        {
            DummyUser newDummyUser = new DummyUser(email, pass);

            try
            {
                FileStream fileStream = new FileStream(fileName, FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fileStream, DummyUser.hashDummyUsers);
                fileStream.Dispose();
            }
            catch (Exception e)
            {
                log.Error("Creat or Update dummyUsers to file - exception detail: " + e.Message);
            }


            //Console.WriteLine("email:" + email + "." + "pass:" + pass + "--" + "is saved");
            return true;
        }

        public bool SaveExistingUser(DummyUser dummyUser)
        {
            DummyUser newDummyUser = new DummyUser(dummyUser.getEmail(), dummyUser.getPass(),dummyUser.gListOfIDB());

            try
            {
                FileStream fileStream = new FileStream(fileName, FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fileStream, DummyUser.hashDummyUsers);
                fileStream.Dispose();
            }
            catch (Exception e)
            {
                log.Error("Creat or Update dummyUsers to file - exception detail: " + e.Message);
            }


            //Console.WriteLine("email:" + email + "." + "pass:" + pass + "--" + "is saved");
            return true;
        }

        public void setNewIDB(String email, String pass, String IDB)
        {
            Hashtable hash = DummyUser.hashDummyUsers;
            DummyUser dummyUser = (DummyUser)hash[email];
            dummyUser.SetNewIDBoard(IDB);
            DummyUser.hashDummyUsers[email] = dummyUser;
            SaveUser(dummyUser);

        }
 /*       
        public static Hashtable StartUpUsers()
        {
            Hashtable uploadHashDummyUsers = new Hashtable();
            if (File.Exists(fileName))
            {
                try
                {
                    FileStream fileStream = new FileStream(fileName, FileMode.Open);
                    BinaryFormatter bf = new BinaryFormatter();
                    uploadHashDummyUsers = (Hashtable)bf.Deserialize(fileStream);
                    fileStream.Dispose();
                }
                catch (Exception e)
                {
                    log.Error("Users Loaded problem exception details: " + e.Message);
                }
            }
            else
            {
                try
                {
                    FileStream fileStream = new FileStream(fileName, FileMode.Create);
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fileStream, uploadHashDummyUsers);
                    fileStream.Dispose();
                }
                catch (Exception e)
                {
                    log.Error("Creat new users file problem exception details: " + e.Message);
                }
            }
            DummyUser.hashDummyUsers = uploadHashDummyUsers;
            return uploadHashDummyUsers;
        }
   */     

        public static void StartUpUsers()
        {
            string connection_string = null;
            string sql = null;
            string database_name = "KanBanDataBase.db";

            SQLiteConnection connection;
            SQLiteCommand command;

            connection_string = $"Data Source={database_name};Version=3";
            connection = new SQLiteConnection(connection_string);
            SQLiteDataReader dataReader;

            try
            {
                connection.Open();

                sql = "SELECT * from Users";

                command = new SQLiteCommand(sql, connection);
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    string email = (string) dataReader["Email_userID"];
                    string pass= (string) dataReader["Password"];
                    List<String> boardsIDs = getBoardIDsOfUser(connection, email);

                    DummyUser dummyUser = new DummyUser(email, pass, boardsIDs); //update hashTable
                }
            }
            catch (Exception e)
            {
                throw new FileLoadException("Exception:" + e);
            }
            Console.ReadLine();
        }
        
        public static List<String> getBoardIDsOfUser (SQLiteConnection connection, string email)
        {
            List<string> currEmailBoardIDs = new List<string>();

            try
            {
                string sql2 = "SELECT Board_ID " +
                                "FROM Boards " +
                                 "WHERE Email_userID = @Email_userID";

                SQLiteCommand command = new SQLiteCommand(sql2, connection);
                SQLiteParameter paramEmail_userID = new SQLiteParameter("@Email_userID", email);
                command.Parameters.Add(paramEmail_userID);

                SQLiteDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    long boardID = (long)dataReader["Board_ID"];
                    currEmailBoardIDs.Add(Convert.ToString(boardID));
                }
            }
            catch (Exception e)
            {
                throw new Exception("Exception:" + e);
            }

            return currEmailBoardIDs;
           
        }

    }

}
[Serializable]
class DummyUser
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public static Hashtable hashDummyUsers = new Hashtable();
    private String email;
    private String pass;
    private List<String> listOfBoardsID;

    public DummyUser(String email, String pass)
    {
        this.email = email; this.pass = pass;
        hashDummyUsers.Add(email, this);

        listOfBoardsID = new List<String>();
    }

    public DummyUser(String email, String pass, List<String> boardsList)
    {
        this.email = email; this.pass = pass;
        hashDummyUsers.Add(email, this);

        listOfBoardsID = boardsList;
    }

    //gets
    public String getEmail() { return this.email; }
    public String getPass() { return this.pass; }
    public Hashtable GetHashDummyUsers() { return DummyUser.hashDummyUsers; }
    public List<String> gListOfIDB() { return this.listOfBoardsID; }

    //sets
    public void setEmail(String newName) { this.email = newName; }
    public void setPass(String newPass) { this.pass = newPass; }
    public void SetNewIDBoard(String newBoard)
    {
        if (listOfBoardsID.Contains(newBoard))
            log.Error("New Board ID already exist in this User");
        else
            listOfBoardsID.Add(newBoard);
    }
}

