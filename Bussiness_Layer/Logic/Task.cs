using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milestone2.Data_Persistence;
using log4net;

namespace Milestone2.Bussiness_Layer.Logic
{
    public class Task
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private static int maxCharsDesc = 300;
        private static int maxCharsTitle = 50;
        private static DateTime ERROR_DATE = new DateTime(0);

        private string creatorID;
        private DateTime creationTime;
        private string description;
        private DateTime dueDate;
        private Guid taskID;
        private string title;



        public Task(string creatorID, string description, DateTime dueDate, string title)
        {

            dueDate = checkDate(dueDate);
            if (dueDate.Equals(ERROR_DATE)) // compare if equal to DateTime returned that indicate an error (see this.checkDate())   
                throw new ArgumentException("invalid date parameters / date is in the past");


            if (!checkDescription(description) | !checkTitle(title))
                throw new ArgumentException("empty title/ too long title/description"); //enforcing Task limitation

            this.creatorID = creatorID;
            this.creationTime = DateTime.Now;
            this.description = description;
            this.dueDate = dueDate;
            this.taskID = Guid.NewGuid();

            this.title = title;
        }
        public Task(DummyTask dummy)
        {

            this.dueDate = DateTime.Parse(dummy.dueDate);
            this.creatorID = dummy.UserID;
            this.creationTime =DateTime.Parse(dummy.creationDate);
            this.description = dummy.Description;
            this.taskID = Guid.Parse(dummy.TaskID);
            this.title = dummy.Title;
        }
        public Task(string creatorID, string description, int day, int month, int year, string title)
        {
            DateTime dueDate = checkDate(year, month, day);
            if (dueDate.Equals(ERROR_DATE)) // compare if equal to DateTime returned that indicate an error (see this.checkDate())   
                throw new ArgumentException("invalid date parameters / date is in the past");

            if (!checkDescription(description) | !checkTitle(title))
                throw new ArgumentException("empty title or too long title/description"); //enforcing Task limitation




            this.creatorID = creatorID;
            this.creationTime = DateTime.Now;
            this.description = description;
            this.dueDate = dueDate;
            this.taskID = Guid.NewGuid();
            this.title = title;
        }

        public DummyTask turnThisToDummyTask()
        {
            DummyTask dummyTask = new DummyTask(this.taskID.ToString(), this.creatorID, this.title, this.description,
                this.creationTime.ToString(), this.dueDate.ToString());

            return dummyTask;
        }

        public void changeDueDate(DateTime dueDate) { this.dueDate = dueDate; }
        public void changeDueDate(int year, int month, int day)
        {
            DateTime dueDate = checkDate(year, month, day);
            if (dueDate.Equals(ERROR_DATE)) // compare if equal to DateTime returned that indicate an error (see this.checkDate())   
                throw new ArgumentException("invalid date parameters");
            this.dueDate = dueDate;

        }
        public bool changeTaskTitle(string title)
        {
            if (checkTitle(title))
            {
                this.title = title;
                return true;
            }
            return false;
        }
        public bool changeTaskDesc(string description)
        {
            if (checkDescription(description))
            {
                this.description = description;
                return true;
            }
            return false;

        }


        


        


        public string getCreatorId() { return creatorID; }
        public DateTime getCreationDate() { return creationTime; }
        public string getDescription() { return description; }
        public DateTime getDueDate() { return dueDate; }
        public Guid getTaskID() { return taskID; }
        public string getTitle() { return title; }

        private bool checkTitle(string title)
        {
            if (title.Equals("") | title.Length > maxCharsTitle)
                return false;

            return true;

        }

        public void ModifyTask(int day, int month, int year, string title, string description)
        {
            DateTime newDate=this.checkDate(year, month, day);
            bool ans = this.checkTitle(title) && this.checkDescription(description);
            if(newDate!=null && ans)
            {
                this.changeTaskDesc(description);
                this.changeTaskTitle(title);
                this.changeDueDate(year, month, day);
            }

        }

        //return true iff  description matches limitations

        private bool checkDescription(string description)
        {
            if (description.Length > maxCharsDesc)
                return false;

            return true;
        }//return true iff  description matches limitations             
        private DateTime checkDate(int year, int month, int day)

        {
            DateTime reDueDate;

            try
            {
                reDueDate = new DateTime(year, month, day);
                if (DateTime.Compare(reDueDate, DateTime.Now) < 0)
                {
                    reDueDate = ERROR_DATE;
                    log.Warn("Date is in the past, returning an empty Datetime object");
                }
            }
            catch (Exception e)
            {
                log.Error("Problem with constructing new DateTime object, probably incorrect output");
                reDueDate = ERROR_DATE;

            }
            return reDueDate;
        }//returns a valid DateTime object if input was valid, and an empty DateTime object otherwise(01/01/0001 00:00)
        private DateTime checkDate(DateTime dueDate)
        {

            if (DateTime.Compare(dueDate, DateTime.Now) < 0)
            {
                dueDate = ERROR_DATE;
                log.Warn("Date is in the past, returning an empty Datetime object");
            }
            return dueDate;
        }

        public override string ToString()
        {
            string str = "Task ID: " + this.taskID + "\n Title: " + this.title
                + "\n User ID: " + this.creatorID + "\n Description: " +
                this.description + "\n Creation Date: " + this.creationTime.ToString() +
                "\n Due Date: " + this.dueDate.ToString();

            return str;
        }

        
    }
}
