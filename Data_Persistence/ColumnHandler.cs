using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using log4net;

namespace Milestone2.Data_Persistence
{
    class ColumnHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Stream myFileStream;
        private BinaryFormatter binaryFormatter;
        private bool fileIsNew;
        private List<DummyColumn> dummyColumns;
        private String fileName = "ColumnsTasks.dat";

        public ColumnHandler()
        {
            dummyColumns = new List<DummyColumn>();
            if (File.Exists(fileName))
            {
                fileIsNew = false;
            }
            else
            {
                try
                {
                    myFileStream = File.Open(fileName, FileMode.Create);
                    binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(myFileStream, dummyColumns);
                    fileIsNew = true;
                    myFileStream.Dispose();
                    dummyColumns = new List<DummyColumn>();
                }
                catch (Exception e)
                {
                    binaryFormatter = new BinaryFormatter();

                    log.Fatal("There has been a problem creating new file. exception message: " + e.Message);

                }
            }
        }

        public List<DummyColumn> LoadColumns()
        {
            if (!fileIsNew)
            {
                try
                {
                    myFileStream = File.Open(fileName, FileMode.Open);
                    binaryFormatter = new BinaryFormatter();
                    dummyColumns = (List<DummyColumn>)binaryFormatter.Deserialize(myFileStream);
                    myFileStream.Dispose();
                }
                catch (Exception e)
                {
                    log.Fatal("Error desirializing the Columns file. exception message: " + e.Message);
                    dummyColumns = new List<DummyColumn>();
                }
            }
           

            return dummyColumns;

        }

        public void saveColumn(DummyColumn dummyColumn)
        {
            DummyColumn theCol = this.GetColumn(dummyColumn.columnID);
            if (theCol != null)
            {
                this.dummyColumns.Remove(theCol);
            }

            this.dummyColumns.Add(dummyColumn);
            saveChangesToFile();
        }

        private void saveChangesToFile()
        { 
            try
            {
                myFileStream = File.Open(fileName, FileMode.Open); 
                binaryFormatter.Serialize(myFileStream, this.dummyColumns);
                myFileStream.Dispose();
            }
            catch (Exception e)
            {
                log.Error("Error saving changes to Columns file");
            }
            
        }

        internal void removeColumn(DummyColumn Column)

        {
            DummyColumn theCol = this.GetColumn(Column.columnID);
            if (theCol!=null)
            {
                this.dummyColumns.Remove(theCol);
            }
            saveChangesToFile();
        }


        private DummyColumn GetColumn(String columnID)
        {
            DummyColumn theCol = this.dummyColumns.Find(item => item.columnID.Equals(columnID));
            return theCol;

        }
    }


}
