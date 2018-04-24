using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDO;
using System.Data;

namespace VSWebDAL.ConfiguratorDAL
{
    public class ServerTaskSettingsDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static ServerTaskSettingsDAL _self = new ServerTaskSettingsDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static ServerTaskSettingsDAL Ins
        {
            get { return _self; }
        }

        /// <summary>
        /// Update data of ServerTaskSettings table
        /// </summary>
        /// <param name="objDominoServers">ServerTaskSettings object</param>
        /// <returns></returns>
		public Object UpdateData(ServerTaskSettings STSettingsObject)
		{
			Object Update;
			try
			{
				//string SqlQuery = "Update ServerTaskSettings SET Enabled= " + STSettingsObject.Enabled + 
				//    ", RestartOffHours=" + STSettingsObject.RestartOffHours + ", SendLoadCommand=" +
				//    STSettingsObject.SendLoadCommand + ", SendExitCommand=" + STSettingsObject.SendExitCommand +
				//    ", SendRestartCommand=" + STSettingsObject.SendRestartCommand + " " +
				//                "Where MyID=" + STSettingsObject.MyID + " AND TaskID=" + STSettingsObject.TaskID;

				string SqlQuery = "Update ServerTaskSettings SET Enabled= '" + STSettingsObject.Enabled +
					"', IsMinTasksEnabled='" + STSettingsObject.IsMinTasksEnabled + "',SendMinTasksLoadCmd='" + STSettingsObject.SendMinTasksLoadCmd + "', MinNoOfTasks=" + STSettingsObject.MinNoOfTasks + ", RestartOffHours='" + STSettingsObject.RestartOffHours + "', SendLoadCommand='" +
					STSettingsObject.SendLoadCommand + "', SendExitCommand='" + STSettingsObject.SendExitCommand +
					"', SendRestartCommand='" + STSettingsObject.SendRestartCommand + "',TaskID=" + STSettingsObject.TaskID +
								" Where MyID=" + STSettingsObject.MyID;

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
    
        /// <summary>
        /// Delete data of ServerTaskSettings table
        /// </summary>
        /// <param name="objDominoServers">ServerTaskSettings object</param>
        /// <returns></returns>
        public Object DeleteData(ServerTaskSettings STSettingsObject)
        {
            Object Update;
            try
            {
                
                string SqlQuery = "Delete ServerTaskSettings Where MyID=" + STSettingsObject.MyID;

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
        public Object DeleteData1(WebSphereNodes STSettingsObject)
        {
            Object Update;
            try
            {

                string SqlQuery = "Delete  from  WebSphereNodes Where ID=" + STSettingsObject.ID;

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
          /// <summary>
        /// Insert data into ServerTaskSettings table
        /// </summary>
        /// <param name="objDominoServers">ServerTaskSettings object</param>
        /// <returns></returns>
		public Object InsertData(ServerTaskSettings STSettingsObject)
		{
			Object Update;
			try
			{

				string SqlQuery = "INSERT INTO ServerTaskSettings(TaskID ,ServerID,[Enabled],SendLoadCommand,SendRestartCommand," +
					"SendExitCommand,RestartOffHours,MinNoOfTasks,IsMinTasksEnabled,SendMinTasksLoadCmd)" +
							   " VALUES(" + STSettingsObject.TaskID + "," + STSettingsObject.ServerID +
								",'" + STSettingsObject.Enabled + "','" + STSettingsObject.SendLoadCommand +
								 "','" + STSettingsObject.SendRestartCommand + "','" + STSettingsObject.SendExitCommand +
								 "','" + STSettingsObject.RestartOffHours + "'," + STSettingsObject.MinNoOfTasks +
								 ",'" + STSettingsObject.IsMinTasksEnabled + "','" + STSettingsObject.SendMinTasksLoadCmd + "')";
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
      
      

        public DataTable SelectData(string serverid, string taskid) 
        {
            DataTable Update;
            try
            {

                string SqlQuery = "SELECT * FROM ServerTaskSettings WHERE TaskID=" + taskid + " AND ServerID=" + serverid;
                Update = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
                Update = null;
            }
            finally
            {
            }
            return Update;
        }
        
    }
}
