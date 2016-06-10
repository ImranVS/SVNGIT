using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.ConfiguratorDAL
{
   public class LogFileDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static LogFileDAL _self = new LogFileDAL();

        public static LogFileDAL Ins
        {
            get { return _self; }
        }
        /// <summary>
        /// Get all Data from LocationsDAL
        /// </summary>

        public DataTable GetAllData()
        {

            DataTable LogFileDataTable = new DataTable();
            LogFile ReturnLOCbject = new LogFile();
            try
            {
                string SqlQuery = "SELECT * FROM [LogFile]";

                LogFileDataTable = objAdaptor.FetchData(SqlQuery);

            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return LogFileDataTable;
        }

        /// <summary>
        /// Get Data from DominoServers based on Key
        /// </summary>
        public LogFile GetData(LogFile LogFilebject)
        {
            DataTable LogFileDataTable = new DataTable();
            LogFile ReturnLogobject = new LogFile();
            try
            {
                string SqlQuery = "Select * from LogFile where [ID]=" + LogFilebject.ID.ToString();
                LogFileDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
                ReturnLogobject.Keyword = LogFileDataTable.Rows[0]["Keyword"].ToString();
                ReturnLogobject.NotRequiredKeyword = LogFileDataTable.Rows[0]["NotRequiredKeyword"].ToString();
                if(LogFileDataTable.Rows[0]["RepeatOnce"].ToString()!="")
                ReturnLogobject.RepeatOnce =bool.Parse(LogFileDataTable.Rows[0]["RepeatOnce"].ToString());
				if (LogFileDataTable.Rows[0]["Log"].ToString() != "")
					ReturnLogobject.RepeatOnce = bool.Parse(LogFileDataTable.Rows[0]["Log"].ToString());
				if (LogFileDataTable.Rows[0]["AgentLog"].ToString() != "")
					ReturnLogobject.RepeatOnce = bool.Parse(LogFileDataTable.Rows[0]["AgentLog"].ToString());



            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return ReturnLogobject;
        }


        /// <summary>
        /// Insert data into LogFile table
        /// </summary>
        /// <param name="DSObject">LogFile object</param>
        /// <returns></returns>

        public bool InsertData(LogFile LOgobject)
        {
            bool Insert = false;
            try
            {
				string SqlQuery = "INSERT INTO [LogFile] (Keyword,RepeatOnce,Log,AgentLog,NotRequiredKeyword) VALUES('" + LOgobject.Keyword + "','" + LOgobject.RepeatOnce + "','" + LOgobject.Log + "','" + LOgobject.AgentLog + "','" + LOgobject.NotRequiredKeyword + "')";


                Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Insert = false;
            }
            finally
            {
            }
            return Insert;
        }


        /// <summary>
        /// Update data into LogFile table
        /// </summary>
        /// <param name="LOgobject">LogFile object</param>
        /// <returns></returns>
        public Object UpdateData(LogFile LOgobject)
        {
            Object Update;
            try
            {
				string SqlQuery = "UPDATE LogFile SET Keyword='" + LOgobject.Keyword + "',NotRequiredKeyword='" + LOgobject.NotRequiredKeyword + "',RepeatOnce='" + LOgobject.RepeatOnce + "',Log='" + LOgobject.Log + "',AgentLog='" + LOgobject.AgentLog + "' WHERE ID = " + LOgobject.ID + "";
                Update = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Update = false;
            }
            finally
            {
            }
            return Update;
        }

        //delete Data from LogFile Table

        public Object DeleteData(LogFile LOgobject)
        {
            Object Update;
            try
            {

                string SqlQuery = "Delete LogFile Where ID=" + LOgobject.ID;

                Update = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Update = false;
            }
            finally
            {
            }
            return Update;
        }

    }
}
