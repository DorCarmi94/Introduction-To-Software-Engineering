using Milestone3.Bussiness_Layer.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using log4net;

namespace Milestone3.Bussiness_Layer.Interface
{
    public class IUser
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static bool checkEmail(String email)
        {
            if (User.hashUsers.ContainsKey(email))
            {
                log.Warn("Email already exist in the system!");
                return false;
            }
            if (!email.Contains('@') | !email.EndsWith(".com"))
            {
                log.Warn("Email entered invalid");
                return false;
            }
            return true;
        }
        public static string checkEmailReason(String email)
        {
            if (User.hashUsers.ContainsKey(email))
            {
                return "Email already exist in the system!";
            }
            if (!email.Contains('@') | !email.EndsWith(".com"))
            {
                return "Email entered invalid";
            }
            //Console.WriteLine("Valid Email");
            return "";
        }
        public static bool checkUserPass(String pass)
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
        public static string checkUserPassReason(String pass)
        {
            //checkLength
            if (pass.Length > 20 | pass.Length < 4)
            {
                return "illegal Password";

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
                return "illegal Password";

            }

            //Console.WriteLine("legal Password");
            return "";
        }
        public void AssignBoardToUser(String email, String boardId)
        {
            User.AssignBoardToUser(email, boardId);
        }
        public bool ChangeUserPass(String email, String currUserPass, String newUserPass)
        {
            if (!checkUserPass(newUserPass))
                return false;
            User u = User.getUserByEmail(email);

            return u.ChangePass(email, currUserPass, newUserPass);
        }
        public bool CreateNewUser(String email, String pass)
        {
            if (!checkEmail(email) || !checkUserPass(pass))
                return false;
            return User.createUser(email, pass);
        }
        public List<String> getBoards(String email) { return User.getBoardsByEmail(email); }
        public String Login(String email, String pass)
        {
            User user = User.getUserByEmail(email);
            if (user == null)
                return "Wrong User Name!";
            if (!(user.loginCheckPass(pass)))
                return "Wrong password!";
            return user.getEmail();
        }
        public bool checkIfLoginValid(String email, String pass)
        {
            User user = User.getUserByEmail(email);
            if (user == null)
                return false;
            if (!(user.loginCheckPass(pass)))
                return false;
            return true;
        }
        public static bool startUp() { return User.startUp(); }
    }
}
