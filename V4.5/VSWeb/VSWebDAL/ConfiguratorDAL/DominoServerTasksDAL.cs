using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDO;
using System.Data;

namespace VSWebDAL.ConfiguratorDAL
{
    public class DominoServerTasksDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static DominoServerTasksDAL _self = new DominoServerTasksDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static DominoServerTasksDAL Ins
        {
            get { return _self; }
        }

        /// <summary>
        /// Get Data based on TaskID
        /// </summary>
        /// <param name="DSTasksObject"></param>
        /// <returns></returns>
        public DominoServerTasks GetDataForID(DominoServerTasks DSTasksObject)
        {
            DataTable DSTasksDataTable = new DataTable();
            DominoServerTasks ReturnDSTObject = new DominoServerTasks();
            try
            {
                string SqlQuery = "Select * from DominoServerTasks where TaskID=" + DSTasksObject.TaskID.ToString();
                DSTasksDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
                if (DSTasksDataTable.Rows[0]["TaskID"].ToString() != " ")
                    ReturnDSTObject.TaskID = int.Parse(DSTasksDataTable.Rows[0]["TaskID"].ToString());
                ReturnDSTObject.TaskName = DSTasksDataTable.Rows[0]["TaskName"].ToString();
                ReturnDSTObject.ConsoleString = DSTasksDataTable.Rows[0]["ConsoleString"].ToString();
                if (DSTasksDataTable.Rows[0]["RetryCount"].ToString() != "")
                    ReturnDSTObject.RetryCount = int.Parse(DSTasksDataTable.Rows[0]["RetryCount"].ToString());
                if (DSTasksDataTable.Rows[0]["FreezeDetect"].ToString() != "")
                    ReturnDSTObject.FreezeDetect = bool.Parse(DSTasksDataTable.Rows[0]["FreezeDetect"].ToString());
                if (DSTasksDataTable.Rows[0]["MaxBusyTime"].ToString() != "")
                    ReturnDSTObject.MaxBusyTime = int.Parse(DSTasksDataTable.Rows[0]["MaxBusyTime"].ToString());
                ReturnDSTObject.IdleString = DSTasksDataTable.Rows[0]["IdleString"].ToString();
                ReturnDSTObject.LoadString = DSTasksDataTable.Rows[0]["LoadString"].ToString();
            }
            catch(Exception ex)
            {
				throw ex;
            }
            finally
            {
            }
            return ReturnDSTObject;
        }

        /// <summary>
        /// Get Data based on TaskName
        /// </summary>
        /// <param name="DSTasksObject"></param>
        /// <returns></returns>
        public DominoServerTasks GetDataForTaskName(DominoServerTasks DSTasksObject)
        {
            DataTable DSTasksDataTable = new DataTable();
            DominoServerTasks ReturnDSTObject = new DominoServerTasks();
            try
            {
                string SqlQuery = "Select * from DominoServerTasks where TaskName='" + DSTasksObject.TaskName + "'";
                DSTasksDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
                if (DSTasksDataTable.Rows[0]["TaskID"].ToString() != "")
                    ReturnDSTObject.TaskID = int.Parse(DSTasksDataTable.Rows[0]["TaskID"].ToString());
                ReturnDSTObject.TaskName = DSTasksDataTable.Rows[0]["TaskName"].ToString();
                ReturnDSTObject.ConsoleString = DSTasksDataTable.Rows[0]["ConsoleString"].ToString();
                if (DSTasksDataTable.Rows[0]["RetryCount"].ToString() != "")
                    ReturnDSTObject.RetryCount = int.Parse(DSTasksDataTable.Rows[0]["RetryCount"].ToString());
                if (DSTasksDataTable.Rows[0]["FreezeDetect"].ToString() != "")
                    ReturnDSTObject.FreezeDetect = bool.Parse(DSTasksDataTable.Rows[0]["FreezeDetect"].ToString());
                if (DSTasksDataTable.Rows[0]["MaxBusyTime"].ToString() != "")
                    ReturnDSTObject.MaxBusyTime = int.Parse(DSTasksDataTable.Rows[0]["MaxBusyTime"].ToString());
                ReturnDSTObject.IdleString = DSTasksDataTable.Rows[0]["IdleString"].ToString();
                ReturnDSTObject.LoadString = DSTasksDataTable.Rows[0]["LoadString"].ToString();
            }
            catch(Exception ex)
            {
				throw ex;
            }
            finally
            {
            }
            return ReturnDSTObject;
        }

        /// <summary>
        /// Get total Data
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllData()
        {
            DataTable DSTasksDataTable = new DataTable();

            try
            {
                //2/5/2013 NS added sort by task name
                string SqlQuery = "Select *,'' as MyID,'false' as isSelected,'false' as SendLoadCommand,'false' as SendRestartCommand,'false' as RestartOffHours,'false' as SendExitCommand from DominoServerTasks ORDER BY TaskName";
                DSTasksDataTable = objAdaptor.FetchData(SqlQuery);

            }
            catch(Exception ex)
            {
				throw ex;
            }
            finally
            {
            }
            return DSTasksDataTable;
        }
        /// <summary>
        /// Insert data into DominoServerTasks table
        /// </summary>
        /// <param name="DSTObject">DominoServerTasks object</param>
        /// <returns></returns>
        public bool InsertData(DominoServerTasks DSTObject)
        {
            bool Insert = false;
            try
            {
                string SqlQuery = "INSERT INTO DominoServerTasks (TaskName,ConsoleString,RetryCount,FreezeDetect," +
               " MaxBusyTime,IdleString,LoadString)VALUES('" + DSTObject.TaskName +
                    "','" + DSTObject.ConsoleString + "'," + DSTObject.RetryCount + ",'" + DSTObject.FreezeDetect +
                    "'," + DSTObject.MaxBusyTime + ",'" + DSTObject.IdleString +
                    "','" + DSTObject.LoadString + "')";


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
        /// Update data into DominoServerTasks table
        /// </summary>
        /// <param name="DSTObject">DominoServerTasks object</param>
        /// <returns></returns>
        public Object UpdateData(DominoServerTasks DSTObject)
        {
            Object Update;
            try
            {
                string SqlQuery = "UPDATE DominoServerTasks SET TaskName='" + DSTObject.TaskName +
                    "',ConsoleString='" + DSTObject.ConsoleString + "',RetryCount=" + DSTObject.RetryCount +
                    ",FreezeDetect='" + DSTObject.FreezeDetect + "',MaxBusyTime=" + DSTObject.MaxBusyTime +
                    ",IdleString='" + DSTObject.IdleString + "',LoadString='" + DSTObject.LoadString +
                    "'WHERE TaskID = " + DSTObject.TaskID + "";


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
        //delete Data from DominoServerTasks Table

        public Object DeleteData(DominoServerTasks DSTObject)
        {
            Object Update;
            try
            {

                string SqlQuery = "Delete DominoServerTasks Where TaskID=" + DSTObject.TaskID;

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

        //public DataTable GetTaskName(string TaskName)
        //{
        //    DataTable TasknameTable=new DataTable();
        //    try
        //    {
        //        string sqlQuery = "Select   from DominoServerTasks ";

        //        TasknameTable = objAdaptor.FetchData(sqlQuery);
        //    }
        //    catch 
        //    {


        //    }
        //    return TasknameTable;
        //}
        public DataTable GetName(DominoServerTasks NDObj)
        {
            //SametimeServers SametimeObj = new SametimeServers();
            DataTable NDTable = new DataTable();
            try
            {
                if (NDObj.TaskID == 0)
                {
                    string sqlQuery = "Select * from [DominoServerTasks] where TaskName='" + NDObj.TaskName + "' ";
                    NDTable = objAdaptor.FetchData(sqlQuery);
                }
                else
                {
                    string sqlQuery = "Select * from [DominoServerTasks] where TaskName='" + NDObj.TaskName + "' and TaskID<>" + NDObj.TaskID + " ";
                    NDTable = objAdaptor.FetchData(sqlQuery);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return NDTable;


        }

        public int UpdateDominoServerTasks(int TaskID, int ServerID, int Enabled,int SendLoadCommand, int SendRestartCommand, int RestartOffHours, int SendExitCommand)
        {
            int Update = 0;
            try
            {
                if (Enabled == 0)
                {
                    string SqlQuery1 = "Delete from ServerTaskSettings WHERE TaskId = " + TaskID + " AND ServerID = " + ServerID;
                    Update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery1);
                }
                else
                {
                    string SqlQuery = "SELECT * FROM ServerTaskSettings where TaskId = " + TaskID + " AND ServerID = " + ServerID;
                    if (objAdaptor.FetchData(SqlQuery).Rows.Count > 0)
                    {
                        string SqlQuery1 = "Update ServerTaskSettings SET Enabled = " + Enabled + ",SendLoadCommand = " + SendLoadCommand + ",SendRestartCommand = " + SendRestartCommand + ",RestartOffHours = " + RestartOffHours + ",SendExitCommand = " + SendExitCommand + " WHERE TaskId = " + TaskID + " AND ServerID = " + ServerID;
                        Update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery1);
                    }
                    else
                    {
                        string SqlQuery1 = "INSERT INTO ServerTaskSettings (TaskID,ServerID,Enabled,SendLoadCommand,SendRestartCommand,RestartOffHours,SendExitCommand) VALUES (" + TaskID + "," + ServerID + "," + Enabled + "," + SendLoadCommand + "," + SendRestartCommand + "," + RestartOffHours + "," + SendExitCommand + ")";
                        Update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery1);
                    }
                }                
            }
            catch
            {
                Update = 0;
            }
            finally
            {
            }
            return Update;
        }


        /// <summary>
        /// Get Exchange Service Data
        /// </summary>
        /// <returns></returns>
        public DataTable GetExchangeServiceData(string roleType)
        {
            DataTable DSTasksDataTable = new DataTable();

            try
            {
                //2/5/2013 NS added sort by task name
                string SqlQuery = "Select Distinct Ws.DisplayName, Ws.Service_Name, 'false' as isSelected, case when [ServerRequired] = '1' then 'True' else 'False' end as Monitored " +
                                " from WindowsServices Ws Where ServerTypeID = '"+roleType+"' ORDER BY DisplayName ";
                DSTasksDataTable = objAdaptor.FetchData(SqlQuery);
                //ServiceMaster
            }
            catch(Exception ex)
            {
				throw ex;
            }
            finally
            {
            }
            return DSTasksDataTable;
        }

        public DataTable GetDisksData()
        {
            DataTable DisksDataTable = new DataTable();

            try
            {
                //2/5/2013 NS added sort by task name
                //VE-23 24-Jun-14, Mukund added union with Diskspace
                string SqlQuery = "select distinct(DiskName) as DiskName,'' as Threshold, 'GB' as ThresholdType, 'false' as isSelected from DominoDiskSpace"+
                    " union "+
                    " select distinct(DiskName) as DiskName,'' as Threshold, 'GB' as ThresholdType, 'false' as isSelected from DiskSpace";
                
                
                DisksDataTable = objAdaptor.FetchData(SqlQuery);
                
            }
            catch(Exception ex)
            {
				throw ex;
            }
            finally
            {
            }
            return DisksDataTable;
        }
    }
}