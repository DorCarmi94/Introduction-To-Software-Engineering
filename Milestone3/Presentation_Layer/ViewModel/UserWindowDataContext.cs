using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milestone3.Bussiness_Layer.Interface;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Milestone3.Presentation_Layer.Functionality;

namespace Milestone3.Presentation_Layer.ViewModel
{
    class UserWindowDataContext 
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public UserFunc userFunc = new UserFunc();
        
        public string Email_Bind
        {
            get
            {
                return userFunc.email;
            }

            set
            {
                userFunc.email=value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Email_Bind"));
                }
            }
        }
        public string Pass_Bind
        {
            get
            {
                return userFunc.pass;
            }
            set
            {
                userFunc.pass = value;
                if(PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Pass_Bind"));
                }
            }
        }

        public string NewUserEmail
        {

            get
            {
                return this.userFunc.newUserEmail;
            }
            set
            {
                userFunc.newUserEmail=value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("NewUserEmail"));
                }
            }
        }

        public string NewUserPass
        {

            get
            {
                return this.userFunc.newUserPass;
            }
            set
            {
                userFunc.newUserPass = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("NewUserPass"));
                }
            }
        }

        public string Login()
        {
            string loginSucceed = userFunc.Login(Email_Bind,Pass_Bind);
            if (loginSucceed.Equals(userFunc.email))
            {
                return "";
            }
            return loginSucceed;
        }

        public string CreateNewUser()
        {
            if (userFunc.newUserEmail == null || userFunc.newUserPass == null)
            {
                return "Pleas make sure you entered both email and password";
            }
            else
            {
                return userFunc.CreateNewUser();
            }
            

            
        }

        
        public bool EmptyFields()
        {
            Pass_Bind = "";
            Email_Bind = "";

            return true;
        }

    }
}
