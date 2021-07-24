using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milestone2.Bussiness_Layer.Logic
{
    class TestTask
    {
        public static void main(String[] args)
        {
            Task task = new Task("2", "Learn about log4net", new DateTime(2019, 5, 21),"Log4net");
            Console.WriteLine(task.getCreationDate());
            Console.WriteLine(task.getCreatorId());
            Console.WriteLine(task.getTaskID());
            Console.WriteLine(task.getTitle());
            Console.WriteLine(task.getDueDate());
            Console.WriteLine(task.getDescription());
            task.changeDueDate(new DateTime(2020, 5, 2));
            Console.WriteLine(task.getDueDate());
            task.changeTaskTitle("raviv");
            task.changeTaskDesc("king");
            Console.WriteLine(task.getTitle());
            Console.WriteLine(task.getDescription());
            Console.ReadKey();
        }
    }
}
