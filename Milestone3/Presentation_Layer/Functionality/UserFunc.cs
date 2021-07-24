using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milestone3.Bussiness_Layer.Interface;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Milestone3.Presentation_Layer.Functionality;

namespace Milestone3.Presentation_Layer.Functionality
{
    class UserFunc : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public IConnect service;
        public string email { get; set; }
        public string pass { get; set; }
        public string newUserEmail { get; set; }
        public string newUserPass { get; set; }

        public UserFunc()
        {
            service = new IConnect();
            email = ""; pass = "";
            service.stratUpAll();
        }


        public string Login(string email, string pass)
        {
            string loginSucceed = service.login(email,pass);
            if (loginSucceed.Equals(email))
            {
                return loginSucceed;
            }
            return loginSucceed;
        }

        public string CreateNewUser()
        {
            string ans = service.createNewUser(this.newUserEmail, this.newUserPass);
            if (ans.Equals(""))
            {
                return "";
            }
            return ans;
        }

    }
}
