using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDO;
using System.Data;

namespace VSWebDAL.ConfiguratorDAL
{
   public class SNMPDevicesDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static SNMPDevicesDAL _self = new SNMPDevicesDAL();

        public static SNMPDevicesDAL Ins
        {
            get { return _self; }
        }
        /// <summary>
        /// Get all Data from NetworkDevices
        /// </summary>

        public DataTable GetAllData()
        {

            DataTable SNMPDevicesDataTable = new DataTable();
            SNMPDevices ReturnObject = new SNMPDevices();
            try
            {
                string SqlQuery = "select * from SNMPDevices";

                SNMPDevicesDataTable = objAdaptor.FetchData(SqlQuery);

            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return SNMPDevicesDataTable;
        }
        /// <summary>
        /// Get Data from NetworkDevices based on Key
        /// </summary>
		public DataTable GetSNMPdevicedData()
		{
			DataTable dt = new DataTable();
			try
			{
				string Query = "select sd.Name, sd.ImageURL, st.StatusCode,st.Lastupdate from [SNMPDevices] sd, Status st,Users us where sd.UserName=us.LoginName and st.TypeANDName= sd.Name+'-SNMP' and us.NetworkInfrastucture='true'";
				dt = objAdaptor.FetchData(Query);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}
        public SNMPDevices GetData(SNMPDevices DCObject)
        {
            DataTable SNMPDevicesDataTable = new DataTable();
            SNMPDevices ReturnObject = new SNMPDevices();
            try
            {
				//string SqlQuery = "Select nd.*,t2.Location as LocationText from [SNMPDevices] nd  INNER JOIN [Locations] t2 ON nd.[LocationID] = t2.[ID] where nd.ID=" + DCObject.ID.ToString();
				string SqlQuery = "Select nd.*,t2.Location as LocationText,nm.Name as imagename from [SNMPDevices] nd  INNER JOIN [Locations] t2 ON nd.[LocationID] = t2.[ID] Inner join NetworkMaster nm on nd.ImageURL=nm.Image  where nd.ID=" + DCObject.ID.ToString();
				SNMPDevicesDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
                ReturnObject.Password = SNMPDevicesDataTable.Rows[0]["Password"].ToString();
                ReturnObject.Location = SNMPDevicesDataTable.Rows[0]["Location"].ToString();
				ReturnObject.ImageURL = SNMPDevicesDataTable.Rows[0]["ImageURL"].ToString();
				ReturnObject.imagename = SNMPDevicesDataTable.Rows[0]["imagename"].ToString();
                ReturnObject.LocationID = int.Parse(SNMPDevicesDataTable.Rows[0]["LocationID"].ToString());
                if (SNMPDevicesDataTable.Rows[0]["Port"].ToString() != "")
                    ReturnObject.Port = int.Parse(SNMPDevicesDataTable.Rows[0]["Port"].ToString());
                if (SNMPDevicesDataTable.Rows[0]["ResponseThreshold"].ToString() != "")
                    ReturnObject.ResponseThreshold = int.Parse(SNMPDevicesDataTable.Rows[0]["ResponseThreshold"].ToString());
                ReturnObject.Username = SNMPDevicesDataTable.Rows[0]["Username"].ToString();
                ReturnObject.Description = SNMPDevicesDataTable.Rows[0]["Description"].ToString();
                ReturnObject.Address = SNMPDevicesDataTable.Rows[0]["Address"].ToString();
                ReturnObject.Category = SNMPDevicesDataTable.Rows[0]["Category"].ToString();
                if (SNMPDevicesDataTable.Rows[0]["Enabled"].ToString() != "")
					ReturnObject.Enabled = bool.Parse(SNMPDevicesDataTable.Rows[0]["Enabled"].ToString());
				if (SNMPDevicesDataTable.Rows[0]["IncludeOnDashBoard"].ToString() != "")
					ReturnObject.IncludeOnDashBoard = bool.Parse(SNMPDevicesDataTable.Rows[0]["IncludeOnDashBoard"].ToString());
                ReturnObject.Name = SNMPDevicesDataTable.Rows[0]["Name"].ToString();
                if (SNMPDevicesDataTable.Rows[0]["Scanning Interval"].ToString() != "")
                    ReturnObject.ScanningInterval = int.Parse(SNMPDevicesDataTable.Rows[0]["Scanning Interval"].ToString());
                if (SNMPDevicesDataTable.Rows[0]["OffHoursScanInterval"].ToString() != "")
                    ReturnObject.OffHoursScanInterval = int.Parse(SNMPDevicesDataTable.Rows[0]["OffHoursScanInterval"].ToString());
                if (SNMPDevicesDataTable.Rows[0]["RetryInterval"].ToString() != "")
                    ReturnObject.RetryInterval = int.Parse(SNMPDevicesDataTable.Rows[0]["RetryInterval"].ToString());
                ReturnObject.OID = SNMPDevicesDataTable.Rows[0]["OID"].ToString();
              
                
            }
			catch (Exception ex)
			{
				throw ex;
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

        public bool InsertData(SNMPDevices SNMPObject)
        {
            bool Insert = false;
            try
            {
                DataTable SNMPDevicesDataTable = new DataTable();
                string SqlQuery = "select ID from servertypes where servertype='SNMP Devices'";
                SNMPDevicesDataTable = objAdaptor.FetchData(SqlQuery);
                if (SNMPDevicesDataTable.Rows.Count > 0)
                    SNMPObject.ServerTypeId = int.Parse(SNMPDevicesDataTable.Rows[0][0].ToString());

                SqlQuery = "INSERT INTO [SNMPDevices] (Description,Category,Port,Username,Password,[Scanning Interval]" +
					",OffHoursScanInterval,Enabled,IncludeOnDashBoard,Location,ImageURL,Name,ResponseThreshold,RetryInterval,Address,LocationID,ServerTypeId,OID) " +
                    "VALUES('" + SNMPObject.Description + "','" + SNMPObject.Category + "'," + SNMPObject.Port + ",'" + SNMPObject.Username + "','" + SNMPObject.Password +
                    "'," + SNMPObject.ScanningInterval + "," + SNMPObject.OffHoursScanInterval + ",'" + SNMPObject.Enabled +
					"','" + SNMPObject.IncludeOnDashBoard + "', '" + SNMPObject.Location + "','" + SNMPObject.ImageURL + "','" + SNMPObject.Name + "'," + SNMPObject.ResponseThreshold + "," + SNMPObject.RetryInterval + ",'" + SNMPObject.Address +
                    "'," + SNMPObject.LocationID + "," + SNMPObject.ServerTypeId + ",'" + SNMPObject.OID + "')";


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
        public Object UpdateData(SNMPDevices SNMPObject)
        {
            Object Update;
            try
            {
                string SqlQuery = "UPDATE [SNMPDevices] SET Description='" + SNMPObject.Description + "',Category='" + SNMPObject.Category +
                    "',Port=" + SNMPObject.Port + ",Username='" + SNMPObject.Username + "',Password='" + SNMPObject.Password + "',[Scanning Interval]=" +
                    SNMPObject.ScanningInterval + ",OffHoursScanInterval=" + SNMPObject.OffHoursScanInterval + ",Enabled='" + SNMPObject.Enabled +
					"',IncludeOnDashBoard='" + SNMPObject.IncludeOnDashBoard + "',Location='" + SNMPObject.Location + "',ImageURL='" + SNMPObject.ImageURL + "',Name='" + SNMPObject.Name + "',ResponseThreshold=" + SNMPObject.ResponseThreshold +
                    ",RetryInterval=" + SNMPObject.RetryInterval + ",Address='" + SNMPObject.Address + "',LocationID=" + SNMPObject.LocationID +
                    " where [ID]=" + SNMPObject.ID +" and OID='"+SNMPObject.OID+ "'";

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

        public Object DeleteData(SNMPDevices SNMPObject)
        {
            Object Update;
            try
            {

                string SqlQuery = "Delete SNMPDevices Where ID=" + SNMPObject.ID;

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
        public DataTable GetIPAddress(SNMPDevices SNMPObj)
        {
            //SametimeServers SametimeObj = new SametimeServers();
            DataTable SNMPTable = new DataTable();
            try
            {
                if (SNMPObj.ID == 0)
                {
                    string sqlQuery = "Select * from SNMPDevices where Address='" + SNMPObj.Address + "' or Name='" + SNMPObj.Name + "' ";
                    SNMPTable = objAdaptor.FetchData(sqlQuery);
                }
                else
                {
                    string sqlQuery = "Select * from SNMPDevices where (Address='" + SNMPObj.Address + "' or Name='" + SNMPObj.Name + "')and ID<>" + SNMPObj.ID + " ";
                    SNMPTable = objAdaptor.FetchData(sqlQuery);
                }
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return SNMPTable;

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
        public DataTable GetOID()
        {
            DataTable dt = new DataTable();
			try
			{
				string SqlQuery = "select OID from SNMPDevices";
				dt = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{
				throw ex;
			}
            
            return dt;
        }

        public Int32 GetServerIDbyServerName(string serverName)
        {
            DataTable ServersDataTable = new DataTable();
            int ID = 0;
            try
            {
                string SqlQuery = "SELECT ID FROM SNMPDevices WHERE Name='" + serverName + "'";
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
    }
}
