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
using Milestone3.Bussiness_Layer.Interface;
using Milestone3.Presentation_Layer.ViewModel;

namespace Milestone3.Presentation_Layer.Windows
{
    /// <summary>
    /// Interaction logic for UserPropertiesWindow.xaml
    /// </summary>
    public partial class UserPropertiesWindow : Window
    {
        UserPropWinDataContext userPropDC;

        public UserPropertiesWindow(string loggedInEmail, IConnect service)
        {
            InitializeComponent();

            this.userPropDC = new UserPropWinDataContext(loggedInEmail,service);

            this.DataContext = this.userPropDC;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)//Change User Password
        {
            string ans=userPropDC.ChangeUserPasword();
            if(ans.Equals(""))
            {
                MessageBox.Show("Update succeeded.");
                this.Close();
            }
            else
            {
                MessageBox.Show("Something went wrong: " + ans);
            }
        }
    }
}
