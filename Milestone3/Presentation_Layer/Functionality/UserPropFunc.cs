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
    class UserPropFunc : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        IConnect service;

        string loggedInEmail;
        string currPass;
        string newPass;

        public UserPropFunc(IConnect service)
        {
            this.service = service;
        }

        //public string getEmail() { return this.loggedInEmail; }
        //public string getCurrPass() { return this.currPass; }
        //public string getNewPass() { return this.newPass; }
        
        public void setCurrPass(string currPass) { this.currPass = currPass; }
        public void setNewPass(string newPass) { this.newPass = newPass; }

        public string ChangeYourPass(string currPass, string newPass)
        {
            string ans = service.changeUserPass(currPass, newPass);
            return ans;
                
        }
        
    }
}
