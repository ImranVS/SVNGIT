using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.ConfiguratorDAL
{
    public class ExchangeMailProbeDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static ExchangeMailProbeDAL _self = new ExchangeMailProbeDAL();

        public static ExchangeMailProbeDAL Ins
        {
            get { return _self; }
        }

        /// <summary>
        /// Get all Data from ExchangeMailProbe
        /// </summary>

        public DataTable GetAllData()
        {

            DataTable ProbeDataTable = new DataTable();
            ExchangeMailProbeClass ReturnObject = new ExchangeMailProbeClass();
            try
            {
                string SqlQuery = "SELECT Enabled,Name,ExchangeMailAddress,Category,ScanInterval,OffHoursScanInterval,DeliveryThreshold," +
					"RetryInterval,(select ServerName from Servers where ID= SourceServerID) as SourceServer,Filename FROM ExchangeMailProbe";

                ProbeDataTable = objAdaptor.FetchData(SqlQuery);

            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return ProbeDataTable;
        }

        /// <summary>
        /// Get Data from ExchangeMailProbe based on mail-Id
        /// </summary>
        public ExchangeMailProbeClass GetData(ExchangeMailProbeClass ProbObject)
        {
            DataTable ExchangeMailProbeDataTable = new DataTable();
            ExchangeMailProbeClass ReturnObject = new ExchangeMailProbeClass();
            try
            {
                //12/7/2012 NS modified - need to return Server Name, it will be used in the Target Server combo box.
                //Currently, the combo box erroneously displays Server ID value
                string SqlQuery = "SELECT *, ServerName as SourceServer FROM ExchangeMailProbe " +
					"INNER JOIN Servers ON ID = SourceServerID where [Name]='" +
                    ProbObject.Name + "'";
                ExchangeMailProbeDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
                ReturnObject.Name = ExchangeMailProbeDataTable.Rows[0]["Name"].ToString();
                if (ExchangeMailProbeDataTable.Rows[0]["Enabled"].ToString() != "")
                    ReturnObject.Enabled = bool.Parse(ExchangeMailProbeDataTable.Rows[0]["Enabled"].ToString());

                if (ExchangeMailProbeDataTable.Rows[0]["ScanInterval"].ToString() != "")
                    ReturnObject.ScanInterval = int.Parse(ExchangeMailProbeDataTable.Rows[0]["ScanInterval"].ToString());
                if (ExchangeMailProbeDataTable.Rows[0]["OffHoursScanInterval"].ToString() != "")
                    ReturnObject.OffHoursScanInterval = int.Parse(ExchangeMailProbeDataTable.Rows[0]["OffHoursScanInterval"].ToString());
                if (ExchangeMailProbeDataTable.Rows[0]["RetryInterval"].ToString() != "")
                    ReturnObject.RetryInterval = int.Parse(ExchangeMailProbeDataTable.Rows[0]["RetryInterval"].ToString());
                ReturnObject.Category = ExchangeMailProbeDataTable.Rows[0]["Category"].ToString();
                ReturnObject.Filename = ExchangeMailProbeDataTable.Rows[0]["Filename"].ToString();
                ReturnObject.ExchangeMailAddress = ExchangeMailProbeDataTable.Rows[0]["ExchangeMailAddress"].ToString();
				if (ExchangeMailProbeDataTable.Rows[0]["SourceServerID"].ToString() != "" && ExchangeMailProbeDataTable.Rows[0]["SourceServerID"].ToString() != null)
					ReturnObject.SourceServerID = int.Parse(ExchangeMailProbeDataTable.Rows[0]["SourceServerID"].ToString());
                if (ExchangeMailProbeDataTable.Rows[0]["DeliveryThreshold"].ToString() != "")
                    ReturnObject.DeliveryThreshold = int.Parse(ExchangeMailProbeDataTable.Rows[0]["DeliveryThreshold"].ToString());
				ReturnObject.SourceServer = ExchangeMailProbeDataTable.Rows[0]["SourceServer"].ToString();

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
        /// Insert data into ExchangeMailProbe table
        /// </summary>
        /// <param name="DSObject">ExchangeMailProbe object</param>
        /// <returns></returns>

        public bool InsertData(ExchangeMailProbeClass ProbeObject)
        {
            bool Insert = false;
            try
            {
                string SqlQuery = "INSERT INTO ExchangeMailProbe (Enabled,Name,ExchangeMailAddress,Category,ScanInterval" +
					",OffHoursScanInterval,DeliveryThreshold,RetryInterval,SourceServerID,Filename)" +
                    "VALUES('" + ProbeObject.Enabled + "','" + ProbeObject.Name +
                    "','" + ProbeObject.ExchangeMailAddress + "','" + ProbeObject.Category +
                    "','" + ProbeObject.ScanInterval + "','" + ProbeObject.OffHoursScanInterval +
                    "'," + ProbeObject.DeliveryThreshold + "," + ProbeObject.RetryInterval +
                    ",'" + ProbeObject.SourceServerID + "','" + ProbeObject.Filename + "')";


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
        /// Update data into ExchangeMailProbe table
        /// </summary>
        /// <param name="ProbeObject">ExchangeMailProbe object</param>
        /// <returns></returns>
        public Object UpdateData(ExchangeMailProbeClass ProbeObject, string name)
        {

            Object Update;
            try
            {
                string SqlQuery = "UPDATE ExchangeMailProbe SET Enabled='" + ProbeObject.Enabled + "',ExchangeMailAddress='" + ProbeObject.ExchangeMailAddress +
                    "',Category='" + ProbeObject.Category + "',ScanInterval=" + ProbeObject.ScanInterval +
                    ",OffHoursScanInterval=" + ProbeObject.OffHoursScanInterval + ",RetryInterval=" + ProbeObject.RetryInterval +
					",DeliveryThreshold=" + ProbeObject.DeliveryThreshold + ",SourceServerID='" + ProbeObject.SourceServerID +
                   "',Name='" + ProbeObject.Name +
                    "',Filename='" + ProbeObject.Filename + "'" + 
                    " WHERE Name= '" + name + "'";

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

        //delete Data from ExchangeMailProbe Table

        public Object DeleteData(ExchangeMailProbeClass ProbObject)
        {
            Object Update;
            try
            {

                string SqlQuery = "Delete ExchangeMailProbe Where Name='" + ProbObject.Name + "'";

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
        public DataTable GetAllHistoryData()
        {

            DataTable ProbeHistoryDataTable = new DataTable();
            ExchangeMailProbeHistory ReturnObject = new ExchangeMailProbeHistory();
            try
            {
                string SqlQuery = "SELECT * FROM ExchangeMailProbeHistory";

                ProbeHistoryDataTable = objAdaptor.FetchData(SqlQuery);

            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return ProbeHistoryDataTable;
        }

        public DataTable GetIPAddress(ExchangeMailProbeClass MailObj, string name)
        {
            //SametimeServers SametimeObj = new SametimeServers();
            DataTable MailProbTable = new DataTable();
            try
            {
                string sqlQuery = "";
                if (name == "")
                {
                    sqlQuery = "Select * from ExchangeMailProbe where Name='" + MailObj.Name + "'";
                }
                else
                {
                    sqlQuery = "Select * from ExchangeMailProbe where Name='" + MailObj.Name + "' and name<>'" + name + "'";
                }
                MailProbTable = objAdaptor.FetchData(sqlQuery);
                //if (MailObj.ExchangeMailAddress == "" && MailObj.ExchangeMailAddress!=null)
                //{
                //    string sqlQuery = "Select * from ExchangeMailProbe where ExchangeMailAddress='" + MailObj.ExchangeMailAddress + "' or Name='"+MailObj.Name+"'";
                //    MailProbTable = objAdaptor.FetchData(sqlQuery);
                //}          
                //else
                //{
                //    string sqlQuery = "Select * from ExchangeMailProbe where  (Name='" + MailObj.Name + "' and ExchangeMailAddress!='" + MailObj.ExchangeMailAddress + "') " +
                //        " or (Name!='" + MailObj.Name + "' and ExchangeMailAddress='" + MailObj.ExchangeMailAddress + "') ";
                //    MailProbTable = objAdaptor.FetchData(sqlQuery);
                //}
            }
            catch (Exception ex)
			{
				throw ex;
			}
            return MailProbTable;

        }
        public DataTable GetServername()
        {

            DataTable ProbeDataTable = new DataTable();
            ExchangeMailProbeClass ReturnObject = new ExchangeMailProbeClass();
            try
            {
                //1/29/2013 NS modified - added sort and distinct to make sure server names are not duplicated in the drop down list
                string SqlQuery = "SELECT DISTINCT ServerName,ID,LocationID FROM Servers where ServerTypeId=(Select ID from ServerTypes where ServerType='Exchange') ORDER BY ServerName";

                ProbeDataTable = objAdaptor.FetchData(SqlQuery);

            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return ProbeDataTable;
        }

        public DataTable GetAllDataByName(ExchangeMailProbeClass MailObj)
        {

            DataTable ProbeDataTable = new DataTable();
            ExchangeMailProbeClass ReturnObject = new ExchangeMailProbeClass();
            try
            {
                string SqlQuery = "SELECT Enabled,Name,ExchangeMailAddress,Category,ScanInterval,OffHoursScanInterval,DeliveryThreshold," +
					"RetryInterval,(select ServerName from Servers where ID= SourceServerID) as SourceServer,Filename FROM ExchangeMailProbe where Name='" + MailObj.Name + "'";

                ProbeDataTable = objAdaptor.FetchData(SqlQuery);

            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return ProbeDataTable;
        }
    }
}
