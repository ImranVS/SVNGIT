using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDO;
using System.Data;

namespace VSWebDAL.ConfiguratorDAL
{
   public class NetworkDevicesDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static NetworkDevicesDAL _self = new NetworkDevicesDAL();

        public static NetworkDevicesDAL Ins
        {
            get { return _self; }
        }
        /// <summary>
        /// Get all Data from NetworkDevices
        /// </summary>
		public DataTable GetNetworkdevicedData()
		{
			DataTable dt = new DataTable();
			try
			{
                //5/5/2016 NS modified
                //Changed -ND to -Network Device
				string Query = "select nd.Name, nd.ImageURL, st.StatusCode,st.Lastupdate from [Network Devices] nd, Status st,Users us where nd.UserName=us.LoginName and st.TypeANDName= nd.Name+'-Network Device' and us.NetworkInfrastucture='true' union select sd.Name, sd.ImageURL, st.StatusCode,st.Lastupdate from [SNMPDevices] sd, Status st,Users us where sd.UserName=us.LoginName and st.TypeANDName= sd.Name+'-SNMP' and us.NetworkInfrastucture='true'";
				dt = objAdaptor.FetchData(Query);
			}
			catch (Exception ex)
			{	
				throw ex;
			}
			return dt;
		}
		public DataTable GetNetworkdevicevisibleData()
		{
			DataTable dt = new DataTable();
			try
			{
				//2/6/15 WS Commented out.  Same as CloudApps not appearing, no need to do a where condition from VS User to ND User names
				//string Query = "select nd.Name, nd.ImageURL, st.StatusCode,st.Lastupdate from [Network Devices] nd, Status st,Users us where nd.UserName=us.LoginName and st.TypeANDName= nd.Name+'-ND'and nd.IncludeOnDashBoard='true'  union select sd.Name, sd.ImageURL, st.StatusCode,st.Lastupdate from [SNMPDevices] sd, Status st,Users us where sd.UserName=us.LoginName and st.TypeANDName= sd.Name+'-SNMP' and sd.IncludeOnDashBoard='true'";
                //5/5/2016 NS modified
                //Changed -ND to -Network Device
				string Query = "select nd.Name, nd.ImageURL, st.StatusCode,st.Lastupdate from [Network Devices] nd, Status st where st.TypeANDName= nd.Name+'-Network Device'and nd.IncludeOnDashBoard='true'  union select sd.Name, sd.ImageURL, st.StatusCode,st.Lastupdate from [SNMPDevices] sd, Status st where st.TypeANDName= sd.Name+'-SNMP' and sd.IncludeOnDashBoard='true'";
				dt = objAdaptor.FetchData(Query);
			}
			catch (Exception ex)
			{	
				throw ex;
			}
			return dt;
		}

		public DataTable GetNetworkdevicevisibleDatadashboard()
		{
			DataTable dt = new DataTable();
			try
			{
                //5/5/2016 NS modified
                //Changed -ND to -Network Device
				string Query = "select nd.Name, nd.ImageURL, st.StatusCode,st.Lastupdate from [Network Devices] nd, Status st where st.TypeANDName= nd.Name+'-Network Device' and nd.IncludeOnDashBoard='true'  union select sd.Name, sd.ImageURL, st.StatusCode,st.Lastupdate from [SNMPDevices] sd, Status st,Users us where  st.TypeANDName= sd.Name+'-SNMP' and sd.IncludeOnDashBoard='true'";
				dt = objAdaptor.FetchData(Query);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}
        public DataTable GetAllData()
        {

            DataTable NetworkDevicesDataTable = new DataTable();
            NetworkDevices ReturnObject = new NetworkDevices();
            try
            {
                string SqlQuery = "select * from [Network Devices]";

                NetworkDevicesDataTable = objAdaptor.FetchData(SqlQuery);

            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return NetworkDevicesDataTable;
        }
        /// <summary>
        /// Get Data from NetworkDevices based on Key
        /// </summary>
        public NetworkDevices GetData(NetworkDevices DCObject)
        {
            DataTable NetworkDevicesDataTable = new DataTable();
            NetworkDevices ReturnObject = new NetworkDevices();
            try
            {
				string SqlQuery = "Select nd.*,t2.Location as LocationText,nm.Name as imagename from [Network Devices] nd  INNER JOIN [Locations] t2 ON nd.[LocationID] = t2.[ID] Inner join NetworkMaster nm on nd.ImageURL=nm.Image  where nd.ID=" + DCObject.ID.ToString();
                NetworkDevicesDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
                if (NetworkDevicesDataTable.Rows.Count > 0)
                {
                    ReturnObject.ImageURL = NetworkDevicesDataTable.Rows[0]["ImageURL"].ToString();
                    ReturnObject.imagename = NetworkDevicesDataTable.Rows[0]["imagename"].ToString();
                    ReturnObject.Password = NetworkDevicesDataTable.Rows[0]["Password"].ToString();
                    ReturnObject.Location = NetworkDevicesDataTable.Rows[0]["Location"].ToString();
                    ReturnObject.LocationID = int.Parse(NetworkDevicesDataTable.Rows[0]["LocationID"].ToString());
                    if (NetworkDevicesDataTable.Rows[0]["Port"].ToString() != "")
                        ReturnObject.Port = int.Parse(NetworkDevicesDataTable.Rows[0]["Port"].ToString());
                    if (NetworkDevicesDataTable.Rows[0]["ResponseThreshold"].ToString() != "")
                        ReturnObject.ResponseThreshold = int.Parse(NetworkDevicesDataTable.Rows[0]["ResponseThreshold"].ToString());
                    ReturnObject.Username = NetworkDevicesDataTable.Rows[0]["Username"].ToString();
                    ReturnObject.Description = NetworkDevicesDataTable.Rows[0]["Description"].ToString();
                    ReturnObject.Address = NetworkDevicesDataTable.Rows[0]["Address"].ToString();
                    ReturnObject.Category = NetworkDevicesDataTable.Rows[0]["Category"].ToString();
                    if (NetworkDevicesDataTable.Rows[0]["Enabled"].ToString() != "")
                        ReturnObject.Enabled = bool.Parse(NetworkDevicesDataTable.Rows[0]["Enabled"].ToString());
                    if (NetworkDevicesDataTable.Rows[0]["IncludeOnDashBoard"].ToString() != "")
                        ReturnObject.IncludeOnDashBoard = bool.Parse(NetworkDevicesDataTable.Rows[0]["IncludeOnDashBoard"].ToString());

                    ReturnObject.Name = NetworkDevicesDataTable.Rows[0]["Name"].ToString();
                    if (NetworkDevicesDataTable.Rows[0]["Scanning Interval"].ToString() != "")
                        ReturnObject.ScanningInterval = int.Parse(NetworkDevicesDataTable.Rows[0]["Scanning Interval"].ToString());
                    if (NetworkDevicesDataTable.Rows[0]["OffHoursScanInterval"].ToString() != "")
                        ReturnObject.OffHoursScanInterval = int.Parse(NetworkDevicesDataTable.Rows[0]["OffHoursScanInterval"].ToString());
                    if (NetworkDevicesDataTable.Rows[0]["RetryInterval"].ToString() != "")
                        ReturnObject.RetryInterval = int.Parse(NetworkDevicesDataTable.Rows[0]["RetryInterval"].ToString());

                    ReturnObject.NetworkType = NetworkDevicesDataTable.Rows[0]["NetworkType"].ToString();
                }
                
            }
            catch
            {

            }
            finally
            {
            }
            return ReturnObject;
			
        }
        /// <summary>
        /// Insert data into NetworkDevices table
        /// </summary>
        /// <param name="DSObject">DominoServers object</param>
        /// <returns></returns>

        public bool InsertData(NetworkDevices NDObject)
        {
            bool Insert = false;
            try
            {
                DataTable NetworkDevicesDataTable = new DataTable();
                string SqlQuery = "select ID from servertypes where servertype='Network Device'";
                NetworkDevicesDataTable = objAdaptor.FetchData(SqlQuery);
                if (NetworkDevicesDataTable.Rows.Count > 0)
                    NDObject.ServerTypeId =int.Parse(NetworkDevicesDataTable.Rows[0][0].ToString());
				SqlQuery = "INSERT INTO [Network Devices] (Description,Category,Port,Username,Password,[Scanning Interval]" +
					",OffHoursScanInterval,Enabled,IncludeOnDashBoard,Location,Name,ResponseThreshold,RetryInterval,Address,LocationID,ImageURL,ServerTypeId,NetworkType) " +
					"VALUES('" + NDObject.Description + "','" + NDObject.Category + "'," + NDObject.Port + ",'" + NDObject.Username + "','" + NDObject.Password +
					"'," + NDObject.ScanningInterval + "," + NDObject.OffHoursScanInterval + ",'" + NDObject.Enabled +
					"','" + NDObject.IncludeOnDashBoard + "', '" + NDObject.Location + "','" + NDObject.Name + "'," + NDObject.ResponseThreshold + "," + NDObject.RetryInterval + ",'" + NDObject.Address +
					"'," + NDObject.LocationID + ",'"+NDObject.ImageURL+"'," + NDObject.ServerTypeId + ",'" + NDObject.NetworkType + "')";
				 //SqlQuery = "INSERT INTO [Network Devices] (Description,Category,Port,Username,Password,[Scanning Interval]" +
				 //   ",OffHoursScanInterval,Enabled,Location,ImageURL,Name,ResponseThreshold,RetryInterval,Address,LocationID,ServerTypeId) " +
				 //   "VALUES('" + NDObject.Description + "','" + NDObject.Category + "'," + NDObject.Port + ",'"+NDObject.Username+"','" + NDObject.Password +
				 //   "'," + NDObject.ScanningInterval + "," + NDObject.OffHoursScanInterval + ",'" + NDObject.Enabled +
				 //   "','" + NDObject.Location + "','" + NDObject.ImageURL + "', '" + NDObject.Name + "'," + NDObject.ResponseThreshold + "," + NDObject.RetryInterval + ",'" + NDObject.Address +
				 //   "'," + NDObject.LocationID + "," + NDObject.ServerTypeId + ")";


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
        /// Update data into NetworkDevices table
        /// </summary>
        /// <param name="DSObject">DominoServers object</param>
        /// <returns></returns>
        public Object UpdateData(NetworkDevices NDObject)
        {
            Object Update;
            try
            {
                string SqlQuery = "UPDATE [Network Devices] SET Description='"+NDObject.Description+"',Category='"+NDObject.Category+
                    "',Port="+NDObject.Port+",Username='"+NDObject.Username+"',Password='"+NDObject.Password+"',[Scanning Interval]="+
                    NDObject.ScanningInterval+",OffHoursScanInterval="+NDObject.OffHoursScanInterval+",Enabled='"+NDObject.Enabled+
					"', IncludeOnDashBoard='" + NDObject.IncludeOnDashBoard + "',  Location='" + NDObject.Location + "',ImageURL='" + NDObject.ImageURL + "', Name='" + NDObject.Name + "',ResponseThreshold=" + NDObject.ResponseThreshold +
                    ",RetryInterval=" + NDObject.RetryInterval + ",Address='" + NDObject.Address + "',LocationID=" + NDObject.LocationID + ",NetworkType='" + NDObject.NetworkType + "'" +
                    " where [ID]=" + NDObject.ID + "";

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

        //delete Data from NetworkDevices Table

        public Object DeleteData(NetworkDevices NDObject)
        {
            Object Update;
            try
            {

                string SqlQuery = "Delete [Network Devices] Where ID=" + NDObject.ID;

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
        public DataTable GetIPAddress(NetworkDevices NDObj)
        {
            //SametimeServers SametimeObj = new SametimeServers();
            DataTable NDTable = new DataTable();
            try
            {
                if (NDObj.ID == 0)
                {
                    string sqlQuery = "Select * from [Network Devices] where Address='" + NDObj.Address + "' or Name='" + NDObj.Name + "' ";
                    NDTable = objAdaptor.FetchData(sqlQuery);
                }
                else
                {
                    string sqlQuery = "Select * from [Network Devices] where (Address='" + NDObj.Address + "' or Name='" + NDObj.Name + "')and ID<>"+ NDObj.ID+" ";
                    NDTable = objAdaptor.FetchData(sqlQuery);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return NDTable;

        }
        public DataTable GetLocation()
        {
            DataTable loc = new DataTable();
			try
			{
				string SqlQuery = "select * from Locations";
				loc = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{
				throw ex;
			}
            
            return loc;
        }

        public Int32 GetServerIDbyServerName(string serverName)
        {
            DataTable ServersDataTable = new DataTable();
            int ID = 0;
            try
            {
                string SqlQuery = "SELECT ID FROM [Network Devices] WHERE Name='" + serverName + "'";
                ServersDataTable = objAdaptor.FetchData(SqlQuery);
                ID = Convert.ToInt32(ServersDataTable.Rows[0]["ID"]);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {

            }
            return ID;
        }

        //9/14/2015 NS added for VSPLUS-2148
        public DataTable GetDataByID(string id)
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT t1.Name AS ServerName, ServerType AS Type, LastUpdate " +
                    "FROM [Network Devices] t1 INNER JOIN ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                    "INNER JOIN Status t3 ON t3.Name=t1.Name AND t3.Type=t2.ServerType " +
                    "WHERE t1.ID=" + id;
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }
        //06/05/2016 sowmya added for VPLUS-2902
        public string GetNetworktype(string serverName)
        {
            DataTable dt = new DataTable();
            string Networktype;
            try
            {
                string SqlQuery = "SELECT [NetworkType] FROM [Network Devices] WHERE Name='" + serverName + "'";
                dt = objAdaptor.FetchData(SqlQuery);
                 Networktype = dt.Rows[0]["NetworkType"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return Networktype;
        }
    }
}
