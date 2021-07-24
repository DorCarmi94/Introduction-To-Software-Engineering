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
using Milestone3.Presentation_Layer;
using Milestone3.Presentation_Layer.Windows;
using Menu = Milestone3.Presentation_Layer.ViewModel.Menu;
using Milestone3;
using Milestone3.Presentation_Layer.ViewModel;
using Milestone3.Presentation_Layer.Functionality;


namespace Milestone3.Presentation_Layer
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Login : Window
    {
        ViewModel.UserWindowDataContext UserDC;
        public Login()
        {
            InitializeComponent();
            fixPic();
            this.UserDC = new UserWindowDataContext();

            this.DataContext = this.UserDC;
        }

        private void fixPic()
        {
            
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)//Login
        {
            string loginEmail = this.UserDC.Login();

            if (loginEmail.Equals(""))
            {
                MessageBox.Show(UserDC.Email_Bind + " logged in");
                Menu menu = new Menu(UserDC.Email_Bind,UserDC.userFunc.service);
                menu.Show();
                
                this.Close();
            }
            else
            {
                MessageBox.Show("Something went wrong:\n"+loginEmail);
            }

        }

        private void CreateNewUser_Button_Click(object sender, RoutedEventArgs e)//CreateNewUser
        {
            Presentation_Layer.Windows.NewUserCreation newUserWindow = new NewUserCreation(this.UserDC);
            newUserWindow.Show();
        }
    }
}
