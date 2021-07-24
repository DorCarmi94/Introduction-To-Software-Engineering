using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Configuration;
using Milestone3.Presentation_Layer.ViewModel;

namespace Milestone3.Presentation_Layer.Windows
{
    /// <summary>
    /// Interaction logic for NewUserCreation.xaml
    /// </summary>
    /// 
    public partial class NewUserCreation : Window
    {
        UserWindowDataContext UserDC;
        public NewUserCreation(object DataContext)
        {
            InitializeComponent();
            UserDC = (UserWindowDataContext)DataContext;
            this.DataContext = DataContext;
        }

        private void CreateNewUser(object sender, RoutedEventArgs e)
        {
            if (!PassVerify.Text.Equals(TxtBoxNewPswd.Text))
            {
                //MessageBox.Show("Passwords are not matching! Please try again");  MessageBox.Show("Creation succeed: "+ this.UserDC.NewUserEmail);
                MessageBox.Show(PassVerify.Text);
            }
            else
            {
                string ans = this.UserDC.CreateNewUser();
                if (ans.Equals(""))
                {
                    MessageBox.Show("Creation succeed: " + this.UserDC.NewUserEmail);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("There was a problem creating a new user:\n" +
                        ans);
                }
            }
        }

        
    }
}
