using Milestone3.Presentation_Layer.Functionality;
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
    /// Interaction logic for 
    /// </summary>
    public partial class TaskModification : Window
    {
        BoardDataContext BDC;
        TaskWindowModes mode;
        
        public TaskModification(BoardDataContext BDC, TaskWindowModes mode)
        {
            //New Task
            this.BDC = BDC;
            this.DataContext = BDC;
            this.mode = mode;
            InitializeComponent();
        }
         
        private void SubmitButton(object sender, RoutedEventArgs e)
        {
            if (this.mode == TaskWindowModes.NewTask)
            {
                string ans = BDC.createNewTask();
                if (ans != null && ans.Equals(""))
                {
                    MessageBox.Show("Creating new task succeeded");
                    BDC.SelectedColumn = BDC.SelectedColumn;
                    this.Close();
                }
                else if (ans == null)
                {
                    MessageBox.Show("Something went wrong. We recomend to close the window and try again");
                }
                else
                {
                    MessageBox.Show(ans);
                }
            }
            if(this.mode == TaskWindowModes.ModifyTask)
            {
                MessageBox.Show("Modifying");
                
                string ans = BDC.modifyTask();
                if (ans != null && ans.Equals(""))
                {
                    MessageBox.Show("Modification succeeded");
                    BDC.SelectedColumn = BDC.SelectedColumn;
                    this.Close();
                }
                else if (ans == null)
                {
                    MessageBox.Show("Something went wrong. We recomend to close the window and try again");
                }
                else
                {
                    MessageBox.Show(ans);
                }
            }
        }
    }

    public enum TaskWindowModes
    {
        NewTask,
        ModifyTask
    }
}
