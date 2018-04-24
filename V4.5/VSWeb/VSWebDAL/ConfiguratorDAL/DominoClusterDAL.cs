using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.ConfiguratorDAL
{
   public class DominoClusterDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static DominoClusterDAL _self = new DominoClusterDAL();

        public static DominoClusterDAL Ins
        {
            get { return _self; }
        }
        /// <summary>
        /// Get all Data from DominoCluster
        /// </summary>

        public DataTable GetAllData()
        {

            DataTable DominoClusterDataTable = new DataTable();
            DominoCluster ReturnDCObject = new DominoCluster();
            try
            {
                string SqlQuery = "SELECT ID,(select ServerName from Servers where ID in (ServerID_A)) as ServerA,Server_A_Directory,Server_A_ExcludeList,(select ServerName from Servers where ID in (ServerID_B)) as ServerB,Server_B_Directory,Server_B_ExcludeList" +
                    ", (select ServerName from Servers where ID in (ServerID_C)) as ServerC,Server_C_Directory,Server_C_ExcludeList,Missing_Replica_Alert,First_Alert_Threshold" +
                    ",Enabled,Name,ScanInterval,OffHoursScanInterval,RetryInterval,Category FROM DominoCluster";

                DominoClusterDataTable = objAdaptor.FetchData(SqlQuery);

            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return DominoClusterDataTable;
        }
        /// <summary>
        /// Get Data from DominoServers based on Key
        /// </summary>
        public DominoCluster GetData(DominoCluster DCObject)
        {
            DataTable DominoClusterDataTable = new DataTable();
            DominoCluster ReturnDCObject = new DominoCluster();
            try
            {
                string SqlQuery = "Select * from DominoCluster where [ID]=" + DCObject.ID.ToString();
                DominoClusterDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
                ReturnDCObject.Server_A_Directory = DominoClusterDataTable.Rows[0]["Server_A_Directory"].ToString();
                ReturnDCObject.Server_A_ExcludeList = DominoClusterDataTable.Rows[0]["Server_A_ExcludeList"].ToString();
                ReturnDCObject.ServerID_A = int.Parse(DominoClusterDataTable.Rows[0]["ServerID_A"].ToString());
                ReturnDCObject.Server_B_Directory = DominoClusterDataTable.Rows[0]["Server_B_Directory"].ToString();
                ReturnDCObject.Server_B_ExcludeList = DominoClusterDataTable.Rows[0]["Server_B_ExcludeList"].ToString();
                ReturnDCObject.ServerID_B = int.Parse(DominoClusterDataTable.Rows[0]["ServerID_B"].ToString());
                ReturnDCObject.Server_C_Directory = DominoClusterDataTable.Rows[0]["Server_C_Directory"].ToString();
                ReturnDCObject.Server_C_ExcludeList = DominoClusterDataTable.Rows[0]["Server_C_ExcludeList"].ToString();
                ReturnDCObject.ServerID_C = int.Parse(DominoClusterDataTable.Rows[0]["ServerID_C"].ToString());
                if (DominoClusterDataTable.Rows[0]["Missing_Replica_Alert"].ToString() != "")
                    ReturnDCObject.Missing_Replica_Alert = bool.Parse(DominoClusterDataTable.Rows[0]["Missing_Replica_Alert"].ToString());
                if (DominoClusterDataTable.Rows[0]["First_Alert_Threshold"].ToString() != "")
                    ReturnDCObject.First_Alert_Threshold = float.Parse(DominoClusterDataTable.Rows[0]["First_Alert_Threshold"].ToString());
                //ReturnDCObject.Second_Alert_Threshold = float.Parse(DominoClusterDataTable.Rows[0]["Second_Alert_Threshold"].ToString());
                //if (DominoClusterDataTable.Rows[0]["Second_Alert_Threshold"].ToString() != "") 
                // ReturnDCObject.Second_Alert_Threshold = float.Parse(DominoClusterDataTable.Rows[0]["Second_Alert_Threshold"].ToString());
                if (DominoClusterDataTable.Rows[0]["Enabled"].ToString() != "")
                    ReturnDCObject.Enabled = bool.Parse(DominoClusterDataTable.Rows[0]["Enabled"].ToString());
                ReturnDCObject.Name = DominoClusterDataTable.Rows[0]["Name"].ToString();
                if (DominoClusterDataTable.Rows[0]["ScanInterval"].ToString() != "")
                    ReturnDCObject.ScanInterval = int.Parse(DominoClusterDataTable.Rows[0]["ScanInterval"].ToString());
                if (DominoClusterDataTable.Rows[0]["OffHoursScanInterval"].ToString() != "")
                    ReturnDCObject.OffHoursScanInterval = int.Parse(DominoClusterDataTable.Rows[0]["OffHoursScanInterval"].ToString());
                if (DominoClusterDataTable.Rows[0]["RetryInterval"].ToString() != "")
                    ReturnDCObject.RetryInterval = int.Parse(DominoClusterDataTable.Rows[0]["RetryInterval"].ToString());
                ReturnDCObject.Category = DominoClusterDataTable.Rows[0]["Category"].ToString();
                ReturnDCObject.ServerAName = DominoClusterDataTable.Rows[0]["ServerAName"].ToString();
                ReturnDCObject.ServerBName = DominoClusterDataTable.Rows[0]["ServerBName"].ToString();
                ReturnDCObject.ServerCName = DominoClusterDataTable.Rows[0]["ServerCName"].ToString();
                // AdvNtwrkConCheckBox.Checked = (AdvIPAddressTextBox.Text != "" ? true : false);
            }
            catch(Exception ex)
            {
				throw ex;
            }
            finally
            {
            }
            return ReturnDCObject;
        }
        /// <summary>
        /// Insert data into DominoCluster table
        /// </summary>
        /// <param name="DSObject">DominoServers object</param>
        /// <returns></returns>

        public bool InsertData(DominoCluster DCObject)
        {
            bool Insert = false;
            try
            {
                //4/27/2016 NS modified for VSPLUS-2724
                string SqlQuery = "INSERT INTO DominoCluster (ServerID_A,Server_A_Directory,Server_A_ExcludeList,ServerID_B,Server_B_Directory," +
                    "Server_B_ExcludeList,ServerID_C,Server_C_Directory,Server_C_ExcludeList,Missing_Replica_Alert,First_Alert_Threshold," +
                    "Enabled,Name,ScanInterval,OffHoursScanInterval,RetryInterval,Category,ServerAName,ServerBName,ServerCName) " +
                    "VALUES('" + DCObject.ServerID_A + "','" + DCObject.Server_A_Directory + "','" + DCObject.Server_A_ExcludeList +
                    "','" + DCObject.ServerID_B + "','" + DCObject.Server_B_Directory + "','" + DCObject.Server_B_ExcludeList +
                    "','" + DCObject.ServerID_C + "','" + DCObject.Server_C_Directory + "','" + DCObject.Server_C_ExcludeList +
                    "','" + DCObject.Missing_Replica_Alert + "'," + DCObject.First_Alert_Threshold +
                    ",'" + DCObject.Enabled + "','" + DCObject.Name + "'," + DCObject.ScanInterval + "," + DCObject.OffHoursScanInterval +
                    "," + DCObject.RetryInterval + ",'" + DCObject.Category + "', "+
                    "'" + DCObject.ServerAName + "', '" + DCObject.ServerBName + "', " +
                    "'" + DCObject.ServerCName + "')"; 


              


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
        /// Update data into DominoCluster table
        /// </summary>
        /// <param name="DSObject">DominoServers object</param>
        /// <returns></returns>
        public Object UpdateData(DominoCluster DCObject)
        {
            Object Update;
            try
            {
                string SqlQuery = "UPDATE DominoCluster SET ServerID_A='" + DCObject.ServerID_A + "', " +
                    "Name='" + DCObject.Name+ "', Server_A_Directory='" + DCObject.Server_A_Directory + "', " + 
                    "Server_A_ExcludeList='" + DCObject.Server_A_ExcludeList + "', " + 
                    "ServerID_B='" + DCObject.ServerID_B + "', Server_B_Directory='" + DCObject.Server_B_Directory + "', " + 
                    "Server_B_ExcludeList='" + DCObject.Server_B_ExcludeList + "', ServerID_C='" + DCObject.ServerID_C + "', " +
                    "Server_C_Directory='" + DCObject.Server_C_Directory + "', " + 
                    "Server_C_ExcludeList='" + DCObject.Server_C_ExcludeList + "', " + 
                    "Missing_Replica_Alert='" + DCObject.Missing_Replica_Alert + "', " + 
                    "First_Alert_Threshold=" + DCObject.First_Alert_Threshold + ", Enabled='" + DCObject.Enabled + "', " + 
                    "ScanInterval=" + DCObject.ScanInterval + ", OffHoursScanInterval=" + DCObject.OffHoursScanInterval + ", " + 
                    "RetryInterval=" + DCObject.RetryInterval + ", Category='" + DCObject.Category + "', " + 
                    "ServerAName='" + DCObject.ServerAName + "', ServerBName='" + DCObject.ServerBName + "', " +
                    "ServerCName='" + DCObject.ServerCName + "' " + 
                    "WHERE ID = " + DCObject.ID + "";

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

       //delete Data from DominoCluster Table

        public Object DeleteData(DominoCluster DCObject)
        {
            Object Update;
            try
            {

                string SqlQuery = "Delete DominoCluster Where ID=" + DCObject.ID;

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

        public DataTable GetIPAddress(DominoCluster ClusterObj)
        {
            //SametimeServers SametimeObj = new SametimeServers();
            DataTable ClusterTable = new DataTable();
			try
			{
				if (ClusterObj.ID == 0)
				{

					string sqlQuery = "Select * from DominoCluster where Name='" + ClusterObj.Name + "' ";
					ClusterTable = objAdaptor.FetchData(sqlQuery);

				}
				else
				{
					string sqlQuery = "Select * from DominoCluster where Name='" + ClusterObj.Name + "' and ID <>" + ClusterObj.ID + " ";
					ClusterTable = objAdaptor.FetchData(sqlQuery);
				}
			}

			catch (Exception ex)
			{

				throw ex;
			}
            return ClusterTable;

        }

        public DataTable GetServer()
        {
            DataTable ClusterTable = new DataTable();
            try
            {
                string sqlQuery = "SELECT ServerName,ID,LocationID FROM Servers " + 
                    "WHERE ServerTypeID=(SELECT ID FROM ServerTypes WHERE ServerType='Domino') " + 
                    "ORDER BY ServerName";
                ClusterTable = objAdaptor.FetchData(sqlQuery);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            return ClusterTable;
        
        }

		public DataTable GetNameforStatus(DominoCluster ClusterObj)
		{
			//SametimeServers SametimeObj = new SametimeServers();
			DataTable ClusterTable = new DataTable();
			try
			{
					string sqlQuery = "Select * from Status where Name='" + ClusterObj.Name + "' ";
					ClusterTable = objAdaptor.FetchData(sqlQuery);	
			}

			catch (Exception ex)
			{

				throw ex;
			}
			return ClusterTable;

		}

        //10/2/2016 Sowmya Added for VSPLUS 2455
        public bool Updateclusterscandata(DominoCluster DCObject)
        {
            bool Updateclusterscan=false;
            try
            {
                string SqlQuery = "UPDATE DominoCluster SET ClusterScan='" + DCObject.ClusterScan + "' " 
                     +"WHERE Name = '" + DCObject.Name + "'";

                Updateclusterscan = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Updateclusterscan = false;
            }
            finally
            {
            }
            return Updateclusterscan;
        }


    }
}

