using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using log4net;

namespace Milestone3.DataPersistence_Layer
{

    [Serializable]

    public class DummyTask//: ISerializable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public String TaskID;
        public String Title;
        public String Description;
        public String creationDate;
        public String dueDate;

        public DummyTask(string taskID,  string Title, string Description, string creationDate, string dueDate)
        {
            this.TaskID = taskID;
            this.Title = Title;
            this.Description = Description;
            this.creationDate = creationDate;
            this.dueDate = dueDate;
        }


        public override string ToString()
        {
            string str = "Task ID: " + this.TaskID + "\n Title: " + this.Title
                 + "\n Description: " +
                this.Description + "\n Creation Date: " + this.Description +
                "\n Due Date: " + this.dueDate;

            return str;
        }
    }
}

