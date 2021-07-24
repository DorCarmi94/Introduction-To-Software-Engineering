using Milestone2.Bussiness_Layer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milestone2.Data_Persistence;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Milestone2.Bussiness_Layer.Interface;
using Milestone2.Data_Persistence;
using log4net;

namespace Milestone2
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public static IConnect connect;

        static void Main(string[] args)
        {
            connect = new IConnect();
            connect.stratUpAll();

            string str = "-1";
            while (!str.Equals("0"))
            {
                Console.WriteLine("Choose what you want to do");
                Console.WriteLine("1-Create new user");
                Console.WriteLine("2-Login");
                Console.WriteLine("0-Leave");

                str = Console.ReadLine();
                if(str.Equals("1"))
                {
                    CreateNewUserTest();
                }
                if (str.Equals("2"))
                {
                    LoginTest();
                    Console.WriteLine("Choose what you want to do next");
                    while (!str.Equals("0"))
                    {
                        Console.WriteLine("Boards:");
                        Console.WriteLine("1-Print board");

                        Console.WriteLine("\nColumns:");
                        Console.WriteLine("2-Add column");
                        Console.WriteLine("3- Remove column");
                        Console.WriteLine("4- Change column order");
                        Console.WriteLine("5- Check next");

                        Console.WriteLine("\nTasks:");
                        Console.WriteLine("6-Add new task");
                        Console.WriteLine("7- modify task");
                        Console.WriteLine("8- Promote");
                        Console.WriteLine("9- Limit");
                        Console.WriteLine("10- unLimit");
                        Console.WriteLine("11- Filter");
                        Console.WriteLine("12- Sort");


                        str = Console.ReadLine();
                        if(str.Equals("1"))
                        {
                            printBoard();
                        }
                        if (str.Equals("2"))
                        {
                            AddNewColumn();
                            
                        }
                        if (str.Equals("3"))
                        {
                            RemoveColumn();
                            
                        }
                        if (str.Equals("4"))
                        {
                            ChangeColumnOrder();
                            
                        }
                        if (str.Equals("5"))
                        {
                            CheckNext();
                        }

                        if (str.Equals("6"))
                        {
                            createNewTask();
                        }
                        if (str.Equals("7"))
                        {
                            modifyTask();
                        }
                        if (str.Equals("8"))
                        {
                            promoteTask();
                        }
                        if (str.Equals("9"))
                        {
                            limitColumn();
                        }
                        if (str.Equals("10"))
                        {
                            unlimitColumn();
                        }
                        if (str.Equals("11"))
                        {
                            filter();
                        }
                        if (str.Equals("12"))
                        {
                            sort();
                        }
                    }
                }
            }
            
            
            
            
            
            
            
        }

        public static void CreateNewUserTest()
        {
            Console.WriteLine("//////////////////////////////////////////////////");
            string stopTest = "1";
            string output;
            while (!stopTest.Equals("0"))
            {
                Console.WriteLine("email:");
                string email = Console.ReadLine();
                Console.WriteLine("pass:");
                string pass = Console.ReadLine();
                output = connect.createNewUser(email, pass);
                if(output.Equals(""))
                    Console.WriteLine("Creation succeed");
                else
                    Console.WriteLine(output);
                Console.WriteLine("press 0 for stop CreateTest:");
                stopTest = Console.ReadLine();

            }
            Console.WriteLine("//////////////////////////////////////////////////");
        }
        public static void LoginTest()
        {
            Console.WriteLine("//////////////////////////////////////////////////");
            
            string output;
                Console.WriteLine("email:");
                string email = Console.ReadLine();
                Console.WriteLine("pass:");
                string pass = Console.ReadLine();
                output = connect.login(email, pass);
                Console.WriteLine(output);
                
            Console.WriteLine("//////////////////////////////////////////////////");

        }
        public static void AddNewColumn()
        {
            printBoard();
            Console.WriteLine("//////////////////////////////////////////////////");
            string colToAdd;
            Console.WriteLine("addCol:");
            Console.WriteLine("enter col name:");
            colToAdd = Console.ReadLine();
            connect.addColumnToBoard(colToAdd);
            Console.WriteLine("//////////////////////////////////////////////////");
            printBoard();


        }
        public static void RemoveColumn()
        {
            Console.WriteLine("//////////////////////////////////////////////////");
            string colToRemove;
            Console.WriteLine("removeCol:");
            Console.WriteLine("enter col id:");
            colToRemove = Console.ReadLine();
            connect.removeColumnFromBoard(colToRemove);
            Console.WriteLine("//////////////////////////////////////////////////");
        }
        public static void ChangeColumnOrder()
        {
            Console.WriteLine("//////////////////////////////////////////////////");
            Console.WriteLine("enter two column IDs you want to switch");
            Console.WriteLine("colID1");
            
            string colID1 = Console.ReadLine();
            Console.WriteLine("colID2");
            string colID2 = Console.ReadLine();

            if(connect.changeColumnsOrder(colID1, colID2))
            {
                Console.WriteLine("Swap succeded");
            }
            else
            {
                Console.WriteLine("Swap failed");
            }
            Console.WriteLine("//////////////////////////////////////////////////");
        }
        public static void CheckNext()
        {
            Console.WriteLine("//////////////////////////////////////////////////");
            Console.WriteLine("enter the column IDs you want to check it's next:");

            string colID = Console.ReadLine();

            Console.WriteLine("Next col ID:");
            Console.WriteLine(connect.checkNext(colID));
        }

        public static void printBoard()
        {
            Console.WriteLine("//////////////////////////////////////////////////");
            List<String> columns = connect.getColumnsIDList();
            if (columns != null)
            {
                foreach (var col in columns)
                {
                    List<String> colInfo = connect.getColumn(col);
                    Console.WriteLine("Column ID: " + colInfo[0]);
                    Console.WriteLine("Column Name: " + colInfo[1]);
                    Console.WriteLine("Column's Tasks:");
                    List<string> tasks = connect.getColumnsTasksIDs(col);


                    foreach (var task in tasks)
                    {
                        List<string> theTask = connect.getTask(col, task);
                        Console.WriteLine("\tTask ID: " + theTask[0]);
                        Console.WriteLine("\tCreation Date: " + theTask[1]);
                        Console.WriteLine("\tCreator " + theTask[2]);
                        Console.WriteLine("\tTitle: " + theTask[3]);
                        Console.WriteLine("\tDescription: " + theTask[4]);
                        Console.WriteLine("\tDue Date: " + theTask[5]);
                        Console.WriteLine("");
                    }
                    Console.WriteLine("");

                }
            }
            Console.WriteLine("//////////////////////////////////////////////////");


        }

        public static void createNewTask()
        {
            Console.WriteLine("//////////////////////////////////////////////////");
            Console.WriteLine("Enter you task information:");
            Console.WriteLine("Enter title:");
            string title = Console.ReadLine();

            Console.WriteLine("Enter description:");
            string description = Console.ReadLine();

            Console.WriteLine("Enter due date:");
            Console.WriteLine("day:");
            int day = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("month:");
            int month = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("year:");
            int year = Convert.ToInt32(Console.ReadLine());

            string newTaskID=connect.createTask(title, description, day, month, year);
            Console.WriteLine("Your new task id:");
            Console.WriteLine(newTaskID);
            Console.WriteLine("//////////////////////////////////////////////////");

        }

        public static void modifyTask()
        {
            Console.WriteLine("//////////////////////////////////////////////////");
            Console.WriteLine("Enter column id:");
            string colID = Console.ReadLine();
            Console.WriteLine("Enter taskID:");
            string taskID = Console.ReadLine();

            Console.WriteLine("Enter you modified task information:");
            Console.WriteLine("Enter title:");
            string title = Console.ReadLine();

            Console.WriteLine("Enter description:");
            string description = Console.ReadLine();

            Console.WriteLine("Enter due date:");
            Console.WriteLine("day:");
            int day = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("month:");
            int month = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("year:");
            int year = Convert.ToInt32(Console.ReadLine());

            bool modificationSuccess = connect.modifyTask(colID,taskID,title, description, day, month, year);
            Console.WriteLine("Did modification succeeded:");
            Console.WriteLine(modificationSuccess);

            Console.WriteLine("//////////////////////////////////////////////////");
        }

        public static void promoteTask()
        {
            Console.WriteLine("//////////////////////////////////////////////////");
            Console.WriteLine("Please enter ids for columns and task you want to promote");
            Console.WriteLine("TaskID:");
            string taskID = Console.ReadLine();
            Console.WriteLine("ColumnID:");
            string colID = Console.ReadLine();

            Console.WriteLine("The new column is:");
            Console.WriteLine(connect.promote(taskID, colID));

            Console.WriteLine("//////////////////////////////////////////////////");
        }

        public static void limitColumn()
        {
            Console.WriteLine("//////////////////////////////////////////////////");
            Console.WriteLine("Enter col id to limit:");
            string colID = Console.ReadLine();
            Console.WriteLine("Enter col id to limit:");
            int limit = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("The limit succeeded?:");
            Console.WriteLine(connect.limitNumOfTasks(colID, limit));

            Console.WriteLine("//////////////////////////////////////////////////");
        }

        public static void unlimitColumn()
        {
            Console.WriteLine("//////////////////////////////////////////////////");


            Console.WriteLine("//////////////////////////////////////////////////");

        }

        public static void filter()
        {
            Console.WriteLine("//////////////////////////////////////////////////");
            Console.WriteLine("Filtering tasks in column");
            Console.WriteLine("Enter column ID:");
            string columnID=Console.ReadLine();
            Console.WriteLine("Enter title phrase you want to filter:");
            string phrase= Console.ReadLine();

            List<String> filteredTasks = connect.filterTasksByTitlePhrase(columnID, phrase);
            foreach (var task in filteredTasks)
            {
                Console.WriteLine("ID:"+task);
                Console.WriteLine("Title:"+connect.getTask(columnID,task)[3]);
            }

            Console.WriteLine("//////////////////////////////////////////////////");

        }

        public static void sort()
        {
            Console.WriteLine("//////////////////////////////////////////////////");
            Console.WriteLine("Enter column ID to sort by date:");
            string columnID = Console.ReadLine();
            Console.WriteLine("Sorting succeeded:");
            Console.WriteLine(connect.sortTasksIDsByDueDate(columnID));

            Console.WriteLine("//////////////////////////////////////////////////");

        }

        /*
        public static void officialTest()
        {
            
            IUser.startUp();
            iboard.startUp();
            icolumn.startUp();

            string bye = "0";
            while (!(bye.Equals("-1")))
            {
                Console.WriteLine("Hello, Welcome to Kanban! \n " +
                "please press: \n 1 - Create new User \n 2 - Login \n 3 - Close Kanban");
            String decision = Console.ReadLine();
            
                if (decision.Equals("1"))
                    TestRegister(iuser);
                else if (decision.Equals("2"))
                    TestLogin(iuser);
                else if (decision.Equals("3"))
                    bye = "-1";
            }
            
        }

        public static void TestLogin(IUser iuser)
        {

            Console.WriteLine("Please enter an Email:");
            String email_lgn = Console.ReadLine();
            Console.WriteLine("Please enter a Password");
            String pass = Console.ReadLine();
            string ans = iuser.Login(email_lgn, pass);
            if (ans.Equals(email_lgn))
            {
                email = email_lgn;
                Console.WriteLine(email_lgn + " Logged in");
                loadBoardsForUser(email_lgn);
                loadMenuToCtrlBoard();

            }
            else
                Console.WriteLine(ans);





        }

        public static void loadMenuToCtrlBoard()
        {
            Console.WriteLine("Choose from the following:");
            Console.WriteLine("1- Create new task");
            Console.WriteLine("2- Promote task");
            Console.WriteLine("3- Print your board");
            Console.WriteLine("4- ByeBye");

            string decision = Console.ReadLine();

            if(decision.Equals("1"))
            {
                createNewTask(lstCol[0][1], email);
                loadMenuToCtrlBoard();
        
            }
            else if(decision.Equals("2"))
            {
                Console.WriteLine("This is your board. Choose the task that you want to promote and enter the column's ID and the task ID");
                Console.WriteLine(icolumn.toStringByIDB(lst_boards[0]));

                Console.WriteLine("Enter column ID:");
                string idColToPromt = Console.ReadLine();
                Console.WriteLine("Enter task ID:");
                string idTaskToPromt = Console.ReadLine();
                icolumn.Promote(idColToPromt, idTaskToPromt);
                loadMenuToCtrlBoard();

            }
            else if(decision.Equals("3"))
            {
                Console.WriteLine("This is your board.");
                Console.WriteLine(icolumn.toStringByIDB(lst_boards[0]));
                loadMenuToCtrlBoard();

            }
            else if(decision.Equals("4"))
            {
                lstCol = null;
                email = null;
                lst_boards = null;
                

            }
        }

        public static void TestRegister(IUser iuser)
        {
            Console.WriteLine("please enter a new Email");

                String email=Console.ReadLine();
            Console.WriteLine("please enter a new Password");

            String pass =Console.ReadLine();
            if (iuser.CreateNewUser(email, pass))
            {
                Console.WriteLine("Register went successfully");
                createAllNesseseryFornewUser(email);
                
            }
            else
                Console.WriteLine("Register went Bad");

        }
        public static void loadBoardsForUser(string email)
        {
            lst_boards= iuser.getBoards(email);
            string boardId = lst_boards[0];
            
            lstCol = icolumn.LoadColumnsByIDB(boardId);
            




        }

        public static void createAllNesseseryFornewUser(string email)
        {
            string newBoardID= iboard.createNewBoard("default: " + email);
            iuser.AssignBoardToUser(email, newBoardID);

            icolumn.createDeafaultColumnsForNewBoard(newBoardID);
            List<List<String>> col_list = icolumn.LoadColumnsByIDB(newBoardID);
            string backlogID = col_list[0][1];

            


        }

        public static void createNewTask(string columnID, string UserID)
        {
            Console.WriteLine("Now it's your time to create your first task. wow!");
            Console.WriteLine("Please enter a new title: (must be under 50 characters)");

            string taskTitle = Console.ReadLine();
            Console.WriteLine("Excellent");
            Console.WriteLine("Please enter a full description (optional, under 300 characters");

            string description= Console.ReadLine();

            Console.WriteLine("Please enter the due date: day, month, year");
            Console.WriteLine("Day:");
            int day=Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Month:");
            int month= Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Year:");
            int year= Convert.ToInt32(Console.ReadLine()); ;

            string taskID= icolumn.createTask(columnID, UserID, description, taskTitle, day, month, year);
            if (!taskID.Equals(""))
            {
                Console.WriteLine("Creation succeded: "+taskID);
            }
            else
            {
                Console.WriteLine("There was a problem with the one or more of the inputs. Please check and try again");
            }
        }


        public static void TestBoard()
        {
            IBoard myIBoard = new IBoard();
            IColumn myIColumn = new IColumn();

            myIBoard.startUp();
            myIColumn.startUp();

            String boardName;
            String newBoardID;

            Console.WriteLine("Welcome to RRD Kanban");
            Console.WriteLine("For your convenience you have a menu for you to choose your choices.");
            Console.WriteLine("Choose what would you like to do:");
            Console.WriteLine("Enter '1' for creating a new board");
            Console.WriteLine("Enter '2' for loading all existing boards.");
            String input = Console.ReadLine();
            if (input.Equals("1"))
            {
                Console.WriteLine("You chose to create new board");
                Console.WriteLine("Please enter the board's name");
                boardName = Console.ReadLine();
                newBoardID = myIBoard.createNewBoard(boardName);
                Console.WriteLine("Your new board was created:\nBoardID: " + newBoardID + "\nBoard Name: " + boardName);
                Console.WriteLine("Would you like to create the default columns for your new board?\nEnter 'y' if you do");
                input = Console.ReadLine();
                if (input.Equals("y"))
                {
                    myIColumn.createDeafaultColumnsForNewBoard(newBoardID);
                    List<List<String>> columnsList = myIColumn.LoadColumnsByIDB(newBoardID);
                    List<String> columnsIDs = new List<string>();
                    if (columnsList != null)
                    {
                        foreach (List<String> lst in columnsList)
                        {
                            columnsIDs.Add(lst[0]);
                            Console.WriteLine(lst[1]);
                        }
                    }
                }
            }
            else if (input.Equals("2"))
            {
                Console.WriteLine("These are the existing boards:");
                Console.Write(myIBoard.ToString());
                Console.WriteLine("Please enter a board's ID which you want to see it's columns");

                string idb = Console.ReadLine();
                List<List<String>> myLst = myIColumn.LoadColumnsByIDB(idb);
                List<String> colIDs = new List<string>();
                foreach (List<string> lst in myLst)
                {

                    colIDs.Add(lst[0]);
                    Console.WriteLine(lst[1]);
                }


            }
            else
            {
                Console.WriteLine("Your input is invalid");
            }
        }
        public static void testDummys()
        {
            DummyTask t1 = new DummyTask("1", "12", "Hello", "Good file", "1/1/2019", "1/2/2019");
            DummyTask t2 = new DummyTask("2", "123", "World", "The best", "1/3/2019", "1/4/2019");

            DummyColumn c = new DummyColumn("Backlog", "111", "222", "111");
            c.addNewDummyTask(t1);
            c.addNewDummyTask(t2);

            DummyBoard board = new DummyBoard("Bazuka", "b157");
            //board.addDummyColumn(c);

            Stream myFileStream = File.Create("boards.bin");
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            binaryFormatter.Serialize(myFileStream, board);
            myFileStream.Close();

            myFileStream = File.OpenWrite("boards.bin");
            board.boardName = "Sabababababab";
            binaryFormatter.Serialize(myFileStream, board);
            myFileStream.Close();

            DummyBoard newBoard=null;
            if (File.Exists("boards.bin"))
            {
                Stream myOtherFileStrem = File.OpenRead("boards.bin");
                BinaryFormatter desirializer = new BinaryFormatter();
                newBoard = (DummyBoard)desirializer.Deserialize(myOtherFileStrem);
                myOtherFileStrem.Close();
            }

            


            Console.WriteLine("After Serialize:");
            if (newBoard != null)
            {
                Console.WriteLine(newBoard.ToString());
            }
            
        }

        public static void testDummys2()
        {
            List<DummyColumn> list = new List<DummyColumn>();
            DummyColumn a = new DummyColumn("Back", "111", "aaa", "AAA");
            DummyColumn b = new DummyColumn("InPro", "222", "bbb", "BBB");
            DummyColumn c = new DummyColumn("Done", "333", "ccc", "CCC");

            list.Add(a);
            list.Add(b);
            list.Add(c);

            Stream myFileStream = File.Create("DorCheck.dat");
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            binaryFormatter.Serialize(myFileStream, list);
            myFileStream.Close();

            list = null;

            myFileStream = File.Open("DorCheck.dat",FileMode.Open);
            list = (List<DummyColumn>)binaryFormatter.Deserialize(myFileStream);
            Console.ReadLine();

        }

        public static void AllKindsOfTests()
        {
            //testDummys2();
            IBoard myIBoard = new IBoard();
            IColumn myIColumn = new IColumn();
            //Console.WriteLine(myIBoard.startUp());
            myIColumn.startUp();

            Console.WriteLine(myIColumn.ToString());
            Console.WriteLine("If you want to create new board enter '1'");
            Console.WriteLine("If you want to create new column enter '2'");
            String input = Console.ReadLine();
            if (input.Equals("1"))
            {
                Console.WriteLine("Enter the new board's name:");
                String inBoardName = Console.ReadLine();
                Console.WriteLine(myIBoard.createNewBoard(inBoardName));
            }
            if (input.Equals("2"))
            {
                Console.WriteLine("Enter the new boards idb:");
                String inBoardIDB = Console.ReadLine();
                myIColumn.createDeafaultColumnsForNewBoard(inBoardIDB);
                Console.WriteLine(myIColumn.ToString());
            }

        }

        public static void Test1User()
        {
            IUser iUser = new IUser();
            //CreatNewUser
            iUser.CreateNewUser("Roee@gmail.com", "12aB12"); //email: .. pass: .. is saved.
            iUser.CreateNewUser("Raviv@yahoo.com", "1Aa5");  //email: .. pass: .. is saved.
            iUser.CreateNewUser("Dor@walla.com", "Ac56H");  //email: .. pass: .. is saved.
            iUser.CreateNewUser("Dor@walla.com", "123456");//Email alredy exist in the system!
            iUser.CreateNewUser("Roee@walla", "123Bb");//Email entered invalid
            iUser.CreateNewUser("Dor12@walla.com", "123"); //illegal Password
            iUser.CreateNewUser("Dor12@walla.com", "12ART"); //illegal Password
            iUser.CreateNewUser("Dor12@walla.com", "12art"); //illegal Password


            IUser.startUp(); //Users uploaded

            Console.WriteLine(iUser.Login("Roee@gmail.com", "12aB12")); //Roee@gmail.com
            Console.WriteLine(iUser.Login("Roee@gmail.com", "12ab12")); //Wrong password!
            Console.WriteLine(iUser.Login("Roe3e@gmail.com", "12aB12")); //Wrong User Name!

            iUser.ChangeUserPass("Dor@walla.com", "Ac56H", "1234");//illegal Password
            iUser.ChangeUserPass("Dor@walla.com", "Ac56H", "12AaBa");//password has changed to: 12AaBa 

            Console.ReadKey();
        }
        */

    }

}
