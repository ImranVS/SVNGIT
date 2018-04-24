using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDO;
using System.Data;

namespace VSWebDAL.ConfiguratorDAL
{
    public class NotesDatabaseDAL
    {

        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static NotesDatabaseDAL _self = new NotesDatabaseDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static NotesDatabaseDAL Ins
        {
            get { return _self; }
        }

        /// <summary>
        /// Get all Data from NotesDatabase 
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllData()
        {

            DataTable NotesDatabasesDataTable = new DataTable();
            
            try
            {
                string SqlQuery = "SELECT *,(select servername from servers where id=NotesDatabases.serverid) as servernames FROM NotesDatabases";

                NotesDatabasesDataTable = objAdaptor.FetchData(SqlQuery);

            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return NotesDatabasesDataTable;
        }
        /// <summary>
        /// Get  Data from NotesDatabase based on ID
        /// </summary>
        /// <returns></returns>
        public NotesDatabases GetDataOnID(NotesDatabases NDObject)
        {
            NotesDatabases ReturnNDObject = new NotesDatabases();
            DataTable NotesDatabasesDataTable = new DataTable();
        
            try
            {
                string SqlQuery = "SELECT *,(select servername from servers where id=NotesDatabases.serverid) as servernames  FROM NotesDatabases where ID=" + NDObject.ID.ToString();

                NotesDatabasesDataTable = objAdaptor.FetchData(SqlQuery);
                if (NotesDatabasesDataTable.Rows.Count > 0)
                {
                    ReturnNDObject.ID = int.Parse(NotesDatabasesDataTable.Rows[0]["ID"].ToString());
                    ReturnNDObject.Name = NotesDatabasesDataTable.Rows[0]["Name"].ToString();
                    ReturnNDObject.Category = NotesDatabasesDataTable.Rows[0]["TriggerType"].ToString();
                    ReturnNDObject.ScanInterval = int.Parse(NotesDatabasesDataTable.Rows[0]["ScanInterval"].ToString());
                    ReturnNDObject.OffHoursScanInterval = int.Parse(NotesDatabasesDataTable.Rows[0]["OffHoursScanInterval"].ToString());
                    ReturnNDObject.Enabled = bool.Parse(NotesDatabasesDataTable.Rows[0]["Enabled"].ToString());
                    ReturnNDObject.ResponseThreshold = int.Parse(NotesDatabasesDataTable.Rows[0]["ResponseThreshold"].ToString());
                    ReturnNDObject.RetryInterval = int.Parse(NotesDatabasesDataTable.Rows[0]["RetryInterval"].ToString());
                    ReturnNDObject.ServerName = NotesDatabasesDataTable.Rows[0]["servernames"].ToString();
                    ReturnNDObject.FileName = NotesDatabasesDataTable.Rows[0]["FileName"].ToString();
                    ReturnNDObject.TriggerType = NotesDatabasesDataTable.Rows[0]["TriggerType"].ToString();
                    ReturnNDObject.TriggerValue = float.Parse(NotesDatabasesDataTable.Rows[0]["TriggerValue"].ToString());
                    ReturnNDObject.AboveBelow = NotesDatabasesDataTable.Rows[0]["AboveBelow"].ToString();
                    ReturnNDObject.ReplicationDestination = NotesDatabasesDataTable.Rows[0]["ReplicationDestination"].ToString();
                    ReturnNDObject.InitiateReplication =bool.Parse(NotesDatabasesDataTable.Rows[0]["InitiateReplication"].ToString());
                }


            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return ReturnNDObject;
        }

        /// <summary>
        /// Insert data into NotesDatabases table
        /// </summary>
        /// <param name="DSObject">NotesDatabases object</param>
        /// <returns></returns>

        public bool InsertData(NotesDatabases NDObject)
        {
            bool Insert = false;
            //1/15/2016 NS modified for VSPLUS-2501
            int paramnum = 0;
            string[] paramnames = new string[16];
            string[] paramvalues = new string[16];
            try
            {
                paramnum = 16;
                paramnames[0] = "@name";
                paramnames[1] = "@category";
                paramnames[2] = "@scaninterval";
                paramnames[3] = "@offhrscaninterval";
                paramnames[4] = "@serverid";
                paramnames[5] = "@enabled";
                paramnames[6] = "@respthreshold";
                paramnames[7] = "@retryinterval";
                paramnames[8] = "@servername";
                paramnames[9] = "@filename";
                paramnames[10] = "@triggertype";
                paramnames[11] = "@triggerval";
                paramnames[12] = "@abovebelow";
                paramnames[13] = "@repdest";
                paramnames[14] = "@initrep";
                paramnames[15] = "@id";
                paramvalues[0] = NDObject.Name;
                paramvalues[1] = NDObject.Category;
                paramvalues[2] = Convert.ToString(NDObject.ScanInterval);
                paramvalues[3] = Convert.ToString(NDObject.OffHoursScanInterval);
                paramvalues[4] = Convert.ToString(NDObject.ServerID);
                paramvalues[5] = Convert.ToString(NDObject.Enabled);
                paramvalues[6] = Convert.ToString(NDObject.ResponseThreshold);
                paramvalues[7] = Convert.ToString(NDObject.RetryInterval);
                paramvalues[8] = NDObject.ServerName;
                paramvalues[9] = NDObject.FileName;
                paramvalues[10] = NDObject.TriggerType;
                paramvalues[11] = Convert.ToString(NDObject.TriggerValue);
                paramvalues[12] = NDObject.AboveBelow;
                paramvalues[13] = NDObject.ReplicationDestination;
                paramvalues[14] = Convert.ToString(NDObject.InitiateReplication);
                paramvalues[15] = Convert.ToString(NDObject.ID);
                string SqlQuery = "INSERT INTO NotesDatabases (Name,Category,ScanInterval,OffHoursScanInterval,Enabled,ResponseThreshold" +
                ",RetryInterval,ServerID,ServerName,FileName,TriggerType,TriggerValue,AboveBelow,ReplicationDestination,InitiateReplication)" +
                    "VALUES(" + paramnames[0] + "," + paramnames[1] + "," + paramnames[2] + "," + paramnames[3] +
                    "," + paramnames[5] + "," + paramnames[6] + "," + paramnames[7] + "," + paramnames[4] + "," + paramnames[8] + "," + paramnames[9] +
                    "," + paramnames[10] + "," + paramnames[11] + "," + paramnames[12] + "," + paramnames[13] +
                    "," + paramnames[14] + ")";


                Insert = objAdaptor.ExecuteQueryWithParams(SqlQuery, paramnum, paramnames, paramvalues);
            }
            catch
            {
                Insert = false;
            }
            return Insert;
        }

        /// <summary>
        /// Update data into NotesDatabases table
        /// </summary>
        /// <param name="NDObject">NotesDatabases object</param>
        /// <returns></returns>
        public Object UpdateData(NotesDatabases NDObject)
        {
            Object Update;
            int paramnum = 0;
            string[] paramnames = new string[16];
            string[] paramvalues = new string[16];
            try
            {
                paramnum = 16;
                paramnames[0] = "@name";
                paramnames[1] = "@category";
                paramnames[2] = "@scaninterval";
                paramnames[3] = "@offhrscaninterval";
                paramnames[4] = "@serverid";
                paramnames[5] = "@enabled";
                paramnames[6] = "@respthreshold";
                paramnames[7] = "@retryinterval";
                paramnames[8] = "@servername";
                paramnames[9] = "@filename";
                paramnames[10] = "@triggertype";
                paramnames[11] = "@triggerval";
                paramnames[12] = "@abovebelow";
                paramnames[13] = "@repdest";
                paramnames[14] = "@initrep";
                paramnames[15] = "@id";
                paramvalues[0] = NDObject.Name;
                paramvalues[1] = NDObject.Category;
                paramvalues[2] = Convert.ToString(NDObject.ScanInterval);
                paramvalues[3] = Convert.ToString(NDObject.OffHoursScanInterval);
                paramvalues[4] = Convert.ToString(NDObject.ServerID);
                paramvalues[5] = Convert.ToString(NDObject.Enabled);
                paramvalues[6] = Convert.ToString(NDObject.ResponseThreshold);
                paramvalues[7] = Convert.ToString(NDObject.RetryInterval);
                paramvalues[8] = NDObject.ServerName;
                paramvalues[9] = NDObject.FileName;
                paramvalues[10] = NDObject.TriggerType;
                paramvalues[11] = Convert.ToString(NDObject.TriggerValue);
                paramvalues[12] = NDObject.AboveBelow;
                paramvalues[13] = NDObject.ReplicationDestination;
                paramvalues[14] = Convert.ToString(NDObject.InitiateReplication);
                paramvalues[15] = Convert.ToString(NDObject.ID);
                //string SqlQuery = "UPDATE NotesDatabases SET Name='" + NDObject.Name + "',Category='" + NDObject.Category +
                //    "',ScanInterval='" + NDObject.ScanInterval + "',OffHoursScanInterval='" + NDObject.OffHoursScanInterval + 
                //    "',ServerID="+NDObject.ServerID+",Enabled='" + NDObject.Enabled +
                //    "',ResponseThreshold='" + NDObject.ResponseThreshold + "', RetryInterval='" + NDObject.RetryInterval + 
                //    "',ServerName='" + NDObject.ServerName +
                //    "',FileName='" + NDObject.FileName + "',TriggerType='" + NDObject.TriggerType + "',TriggerValue=" + NDObject.TriggerValue +
                //    ",AboveBelow='" + NDObject.AboveBelow + "',ReplicationDestination='" + NDObject.ReplicationDestination + 
                //    "',InitiateReplication='" + NDObject.InitiateReplication + "' WHERE ID = " + NDObject.ID + "";
                //Update = objAdaptor.ExecuteNonQuery(SqlQuery);
                string SqlQuery = "UPDATE NotesDatabases SET Name=" + paramnames[0] + ",Category=" + paramnames[1] +
                    ",ScanInterval=" + paramnames[2] + ",OffHoursScanInterval=" + paramnames[3] + 
                    ",ServerID=" + paramnames[4] + ",Enabled=" + paramnames[5] +
                    ",ResponseThreshold=" + paramnames[6] + ",RetryInterval=" + paramnames[7] + 
                    ",ServerName=" + paramnames[8] +
                    ",FileName=" + paramnames[9] + ",TriggerType=" + paramnames[10] + ",TriggerValue=" + paramnames[11] +
                    ",AboveBelow=" + paramnames[12] + ",ReplicationDestination=" + paramnames[13] + 
                    ",InitiateReplication=" + paramnames[14] + " WHERE ID = " + paramnames[15];
                Update = objAdaptor.ExecuteQueryWithParams(SqlQuery,paramnum,paramnames,paramvalues);
                
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
        //delete Data from NotesDatabases Table

        public Object DeleteData(NotesDatabases NDObject)
        {
            Object Update;
            try
            {

                string SqlQuery = "Delete NotesDatabases Where ID=" + NDObject.ID;

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

        public DataTable GetName(NotesDatabases NDObj)
        {
            //SametimeServers SametimeObj = new SametimeServers();
            DataTable NotesTable = new DataTable();
            try
            {
                string sqlQuery = "Select * from NotesDatabases where Name='" + NDObj.Name + "'";
                NotesTable = objAdaptor.FetchData(sqlQuery);

            }
			catch (Exception ex)
			{
				throw ex;
			}
            return NotesTable;

        }


    }
}
