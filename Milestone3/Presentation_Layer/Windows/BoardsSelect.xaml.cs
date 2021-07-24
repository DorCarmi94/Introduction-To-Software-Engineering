using Milestone3.Bussiness_Layer.Interface;
using Milestone3.Presentation_Layer.ViewModel;
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

namespace Milestone3.Presentation_Layer.Windows
{
    /// <summary>
    /// Interaction logic for BoardsSelect.xaml
    /// </summary>
    public partial class BoardsSelect : Window
    {
        BoardSelectDataContext boardSelectDataConext;
        public BoardsSelect(string loggedInEmail, IConnect service)
        {
            InitializeComponent();
            this.boardSelectDataConext = new BoardSelectDataContext(loggedInEmail, service);
            this.DataContext = this.boardSelectDataConext;
        }

        private void LoadSpecificBoard(object sender, RoutedEventArgs e)
        {
            string loadAns = this.boardSelectDataConext.loadSpecificBoard();
            if(loadAns.Equals(""))
            {
                BoardWindow boardWindow = new BoardWindow(this.boardSelectDataConext.boardSelectFunc.loggedInEmail, this.boardSelectDataConext.boardSelectFunc.service);
                boardWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Something went wrong: \n" + loadAns);
            }
        }

        private void AddingNewBoard(object sender, RoutedEventArgs e)
        {
            AddButtonSend.Visibility = Visibility.Visible;
            NewBoardName.Visibility = Visibility.Visible;

        }


        private void AddSend(object sender, RoutedEventArgs e)
        {
            string ans= boardSelectDataConext.createNewBoard();
            if(ans.Equals(""))
            {
                MessageBox.Show("Board creation succeeded.\n Board's Name: " +this.boardSelectDataConext.NewBoardName);
                boardSelectDataConext.SelectedBoard = boardSelectDataConext.SelectedBoard;
                Binding binding = new Binding
                {
                    Source = boardSelectDataConext.BoardsGridView,
                    
                };

                BoardsSelectDataGrid.SetBinding(ItemsControl.ItemsSourceProperty, binding);
                AddButtonSend.Visibility = Visibility.Hidden;
                NewBoardName.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Something went wrong:\n" + ans);
            }

        }

        private void RemoveBoardButton(object sender, RoutedEventArgs e)
        {
            var dialogResult = MessageBox.Show("Are you sure you want to remove this Board?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (dialogResult == MessageBoxResult.Yes)
            {
                string ans = this.boardSelectDataConext.removeBoard();
                if (ans.Equals(""))
                {
                    MessageBox.Show("Board has been remvoed");
                    Binding binding = new Binding
                    {
                        Source = boardSelectDataConext.BoardsGridView

                    };

                    BoardsSelectDataGrid.SetBinding(ItemsControl.ItemsSourceProperty, binding);
                }
                else
                {
                    MessageBox.Show("Error: "+ans);
                }
            }
        }
    }
}
