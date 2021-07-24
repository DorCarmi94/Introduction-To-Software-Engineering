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
    class UserPropWinDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public UserPropFunc userPropFunc;

        public UserPropWinDataContext(string loginEmail, IConnect service)
        {
            this.userPropFunc = new UserPropFunc(service);
            this.loggedInEmail = loginEmail;
        }

        
        private string loggedInEmail;
        public string LoggedEmailUser
        {
            get
            {
                return loggedInEmail;
            }

            
        }

        string currPass;
        public string CurrPass
        {
            get
            {
                return currPass;
            }

            set
            {
                currPass=value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrPass"));
                }
            }
        }

        string newPass;
        public string NewPass
        {
            get
            {
                return newPass;
            }

            set
            {
                newPass=value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("newPass"));
                }
            }
        }

        public string ChangeUserPasword()
        {

            return userPropFunc.ChangeYourPass(currPass,newPass);

        }


    }
}
