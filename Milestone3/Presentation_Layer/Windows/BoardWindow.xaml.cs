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
using Milestone3.Presentation_Layer.Functionality;
using Milestone3.Presentation_Layer.ViewModel;
using Microsoft.VisualBasic;
using Milestone3.Bussiness_Layer.Interface;

namespace Milestone3.Presentation_Layer.Windows
{
    /// <summary>
    /// Interaction logic for BoardWindow.xaml
    /// </summary>
    public partial class BoardWindow : Window
    {
        BoardDataContext BDC;
        
        public BoardWindow(string loggedInEmail, IConnect service)
        {
            InitializeComponent();
            this.BDC = new BoardDataContext(loggedInEmail, service);
            this.DataContext = this.BDC;
        }

        private void CreateNewTaskWindowButton(object sender, RoutedEventArgs e)
        {

            TaskModification createNewTaskWindow = new TaskModification(this.BDC,TaskWindowModes.NewTask);
            createNewTaskWindow.Show();
            
        }
        private void ModifyTaskWindowButton(object sender, RoutedEventArgs e)
        {
            
            if (BDC.checkIfCanModifyTaskByCol())
            {
                TaskModification ModifyTaskWindow = new TaskModification(this.BDC, TaskWindowModes.ModifyTask);
                ModifyTaskWindow.Show();
            }
            else
            {
                MessageBox.Show("Tasks from the last column in the board's order can't be modified");
            }

        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BDC.TaskTitleSearchTerm = SearchTermTxtBox.Text;
        }

        private void PromoteTaskWindowButton(object sender, RoutedEventArgs e)
        {
            var dialogResult=MessageBox.Show("Are you sure you want to promote?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (dialogResult==MessageBoxResult.Yes)
            {
                string ans = BDC.taskPromote();
                if (ans.Equals(""))
                {
                    MessageBox.Show("Promotion succeeded");
                    BDC.SelectedColumn = BDC.SelectedColumn;
                }
                else
                {
                    MessageBox.Show("Something went wrong, please note the following message:\n" + ans);
                }
            }
        }
        private void AddNewColumn(object sender, RoutedEventArgs e)
        {
            NewColumnLabel.Visibility = Visibility.Visible;
            NewColumnSendButton.Visibility = Visibility.Visible;
            NewColumnTxtBox.Visibility= Visibility.Visible;
            AddNewColumnButton.Visibility = Visibility.Hidden;

        }

        private void AddNewColumnSend(object sender, RoutedEventArgs e)
        {
            string ans=BDC.addNewColumn();
            if(ans==null || ans.Equals(""))
            {
                MessageBox.Show("Something went wrong. Please check your input.\n Make sure you have entered a new column name and try again");
            }
            else
            {
                MessageBox.Show("Your new columns has been added succesfully");
                NewColumnLabel.Visibility = Visibility.Hidden;
                NewColumnSendButton.Visibility = Visibility.Hidden;
                NewColumnTxtBox.Visibility = Visibility.Hidden;
                AddNewColumnButton.Visibility = Visibility.Visible;
            }

            

        }

        private void RemoveColumnButton(object sender, RoutedEventArgs e)
        {
            var dialogResult = MessageBox.Show("Are you sure you want to remove this column?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (dialogResult == MessageBoxResult.Yes)
            {
                bool ans = BDC.removeColumn();
                if (ans)
                {
                    MessageBox.Show("Column has been remvoed");
                    
                }
                else
                {
                    MessageBox.Show("Something went wrong, the column hasn't been removed");
                }
            }
        }

        private void ColumnMoveUpButton(object sender, RoutedEventArgs e)
        {
            bool ans=BDC.moveColumnUp();
            if(!ans)
            {
                MessageBox.Show("Something went wrong.\nEither it can't be moved up, or else your input is invlid");
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            LimitAmountTxtBox.Visibility = Visibility.Visible;
            LimitSendButton.Visibility = Visibility.Visible;

        }
        private void limitTasksSend(object sender, RoutedEventArgs e)
        {
            if (LimitAmountTxtBox != null)
            {
                LimitTasks();
            }
        }

        private void UnlimitTasksSend(object sender, RoutedEventArgs e)
        {

            UnlimitTasks();
            
        }

        private void CheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            bool ans=BDC.unLimitNumberOfTasks();
            if (ans)
            {
                MessageBox.Show("Unlimiting succeeded");
                LimitAmountTxtBox.Visibility = Visibility.Hidden;
                LimitSendButton.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Something went wrong.");
            }


        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LogOut();
        }

        private void LogOut()
        {
            BDC.Logout();
            Login login = new Login();
            this.BDC = null;
            login.Show();
            this.Close();

        }

        private void UnlimitTasks()
        {

            bool ans = BDC.unLimitNumberOfTasks();
            if (ans)
            {
                MessageBox.Show("Unlimiting succeeded");  
            }
            else
            {
                MessageBox.Show("Something went wrong.");
            }
        }
        private void LimitTasks()
        {
            bool ans = BDC.limitNumOfTasksInThisColumn();
            if (ans)
            {
                MessageBox.Show("Limit succeeded");

            }
            else
            {
                MessageBox.Show("Problem when limiting. Either the input is invalid or the number of tasks\nthat are currently in this column is bigger then the limit\nand you need to fix this first");
            }
        }


    }

}
