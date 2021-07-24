using Milestone3.DataPersistence_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using log4net;

namespace Milestone3.Bussiness_Layer.Logic
{
    class User
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Hashtable hashUsers = new Hashtable();
        private string email;
        private string userPass;
        private UserHandler userHandler;
        private List<Guid> listOfBoardsID;

        public User(String email, String pass)
        {
            this.email = email; this.userPass = pass;
            userHandler = new UserHandler();
            listOfBoardsID = new List<Guid>();
            //hashUsers.Add(email, this);
        }

        public static void AssignBoardToUser(String email, String IDB)
        {
            User user = getUserByEmail(email);
            Guid gBoardID = new Guid();
            if (!(Guid.TryParse(IDB, out gBoardID)))
            {
                log.Error("Parsing IDB doesn't work");
            }
            else
            {
                user.addBoard(gBoardID);
                hashUsers[email] = user;
            }
            user.GetHandler().setNewIDB(email, user.getUserPass(), IDB);
        }
        public static bool createUser(String email, String pass)
        {
            if (email == null | pass == null)
            {
                log.Warn("trying to creater user with null email or pass");
                return false;
            }
            if (!User.checkEmail(email) || !User.checkPass(pass))
                return false;
            User u = new User(email, pass);
            hashUsers.Add(email, u);
            //return u.GetHandler().SaveNewUser(email, pass);
            return u.GetHandler().SaveNewUser(email, pass);
        }
        public static User getUserByEmail(String email) //TODO check null with the function using it
        {
            if (email == null)
            {
                log.Warn("trying to get user with null email");
                return null;
            }
            if (hashUsers.ContainsKey(email))
                return (User)hashUsers[email];
            else
            {
                startUp();
                if (hashUsers.ContainsKey(email))
                    return (User)hashUsers[email];
            }
            return null;
        }
        public static bool checkEmail(String email)
        {
            if (User.hashUsers.ContainsKey(email))
            {
                log.Error("cannot add user " + email + " it  already exists");
                return false;
            }
            if (!email.Contains('@') | !email.EndsWith(".com"))
            {
                log.Warn("email isn't valid email: " + email);
                return false;
            }
            return true;
        }
        public static bool checkPass(String pass)
        {
            //checkLength
            if (pass.Length > 20 | pass.Length < 4)
            {
                Console.WriteLine("illegal Password");
                return false;
            }
            //checkCharactersValid
            int countNum = 0; int countCapital = 0; int countLower = 0;
            for (int i = 0; i < pass.Length; i++)
            {
                if (pass[i] >= 'a' & pass[i] <= 'z')
                    countLower++;
                if (pass[i] >= 'A' & pass[i] <= 'Z')
                    countCapital++;
                if (pass[i] >= '0' & pass[i] <= '9')
                    countNum++;
            }
            if (countNum == 0 | countLower == 0 | countCapital == 0)
            {
                Console.WriteLine("illegal Password");
                return false;
            }

            //Console.WriteLine("legal Password");
            return true;
        }
        public static List<String> getBoardsByEmail(String email)
        {
            User u = getUserByEmail(email);
            List<String> strListIDB = new List<String>();
            foreach (Guid g in u.GetIDBGuids())
            {
                strListIDB.Add(g.ToString());
            }
            return strListIDB;
        }
        public static bool startUp()
        {
            UserHandler.StartUpUsers();
            Hashtable hashDummyUsers = DummyUser.hashDummyUsers;
            foreach (DictionaryEntry hash in hashDummyUsers)
            {
                DummyUser dummyUser = (DummyUser)hash.Value;
                if (!hashUsers.ContainsKey(dummyUser.getEmail()))
                {
                    User u = new User(dummyUser.getEmail(), dummyUser.getPass());
                    foreach (String guid in dummyUser.gListOfIDB())
                    {
                        u.addBoard(Guid.Parse(guid));
                    }
                    hashUsers.Add(dummyUser.getEmail(), u);
                }
            }
            return true;
        }

        public void addBoard(Guid newBoard)
        {
            if (listOfBoardsID.Contains(newBoard))
                log.Error("New Board ID already exist in this User");
            else
                listOfBoardsID.Add(newBoard);
        }
        public bool loginCheckPass(String pass)
        {
            return this.getUserPass() == pass;
        }
        public bool ChangePass(String email, String currUserPass, String newUserPass)
        {
            if (!hashUsers.ContainsKey(email))
                return false;
            if (!checkPass(newUserPass))
                return false;


            User user = User.getUserByEmail(email);
            if (!(user.userPass.Equals(currUserPass)))
            {
                return false;
            }
            user.setPass(newUserPass);
            hashUsers.Remove(email);
            hashUsers.Add(email, user);

            
            return userHandler.ChangeDummyUserPass(email, currUserPass, newUserPass);
        }

        public string getEmail() { return this.email; }
        public UserHandler GetHandler() { return this.userHandler; }
        public List<Guid> GetIDBGuids() { return this.listOfBoardsID; } //change name
        public string getUserPass() { return this.userPass; }
        public void SetEmail(String email) { this.email = email; }
        private void setPass(String newUserPass) { this.userPass = newUserPass; }

    }
}
