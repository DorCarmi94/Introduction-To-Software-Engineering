using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milestone3.Bussiness_Layer.Interface;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milestone3.Bussiness_Layer.Logic;
using System.Data.SQLite;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace Milestone3.Bussiness_Layer.Interface.Tests
{
    [TestClass()]
    public class IUserTests
    {

        [TestMethod()]
        public void checkEmailTest()
        {
           string email = "roi@gmail.com";
           Assert.IsTrue(IUser.checkEmail(email));
        }

        [TestMethod()]
        public void checkUserPassTest()
        {
            string pass = "er2";
            Assert.IsFalse(IUser.checkUserPass(pass));
        }

        [TestMethod()]
        public void checkUserPassTest2()
        {
            string pass = "er2t5";
            Assert.IsFalse(IUser.checkUserPass(pass));
        }

        [TestMethod()]
        public void checkUserPassTest3()
        {
            string pass = "er2TD45";
            Assert.IsTrue(IUser.checkUserPass(pass));
        }

        [TestMethod()]
        public void checkCreateNewTest()
        {
            string email = "hadas@gmail.com";
            string pass = "Test1234";
            IUser iUser = new IUser();
            Assert.IsTrue(iUser.CreateNewUser(email, pass));
            DeleteTestUser("hadas@gmail.com");
        }


        private void DeleteTestUser(string email)
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
                  "DELETE FROM Users " +
                    "Where Email_UserID = '" + email + "'";

                command.ExecuteNonQuery();
                command.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception("thert was a problem in delete user from test DB: " + e);
            }
        }
    }
}