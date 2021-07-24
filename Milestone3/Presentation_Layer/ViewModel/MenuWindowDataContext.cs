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

namespace Milestone3.Presentation_Layer.ViewModel
{
    class MenuWindowDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public IConnect service;
        string loggedInEmail;

        public string getLoggedInEmail() { return this.loggedEmailUser; }

        public MenuWindowDataContext(string loginEmail, IConnect service)
        {
            

            this.loggedInEmail = loginEmail;

            loggedEmailUser = this.loggedInEmail;
            this.service = service;
        }

        public string loggedEmailUser
        {
            get
            {
                return loggedInEmail;
            }

            set
            {
                this.loggedInEmail = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("loggedEmailUser"));
                }
            }
        }


    }
}
