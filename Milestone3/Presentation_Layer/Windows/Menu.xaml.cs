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
using Milestone3.Presentation_Layer.Windows;

namespace Milestone3.Presentation_Layer.ViewModel
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        MenuWindowDataContext menuDC;

        public Menu(string loginEmail, IConnect service)
        {
            InitializeComponent();
            
            this.menuDC = new MenuWindowDataContext(loginEmail, service);

            this.DataContext = this.menuDC;
            
        }

        private void OpenBoardButton(object sender, RoutedEventArgs e)//Board
        {
            BoardsSelect boardsWindow = new BoardsSelect(this.menuDC.getLoggedInEmail(), menuDC.service);
            boardsWindow.Show();
            this.Close();
        }

        private void UserPropertiesButton(object sender, RoutedEventArgs e)//user Properties
        {
            UserPropertiesWindow UPW = new UserPropertiesWindow(this.menuDC.getLoggedInEmail(),menuDC.service);
            UPW.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}
