using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.ConfiguratorDAL
{
    public class NotesMailProbeDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static NotesMailProbeDAL _self = new NotesMailProbeDAL();

        public static NotesMailProbeDAL Ins
        {
            get { return _self; }
        }

        /// <summary>
        /// Get all Data from NotesMailProbe
        /// </summary>

        public DataTable GetAllData()
        {

            DataTable ProbeDataTable = new DataTable();
            NotesMailProbe ReturnObject = new NotesMailProbe();
            try
            {
                string SqlQuery = "SELECT Enabled,Name,NotesMailAddress,Category,ScanInterval,OffHoursScanInterval,DeliveryThreshold,"+
                    "RetryInterval,(select ServerName from Servers where ID= DestinationServerID) as DestinationServer,DestinationDatabase,SourceServer,EchoService,ReplyTo,Filename FROM NotesMailProbe";

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
        /// Get Data from NotesMailProbe based on mail-Id
        /// </summary>
        public NotesMailProbe GetData(NotesMailProbe ProbObject)
        {
            DataTable NotesMailProbeDataTable = new DataTable();
            NotesMailProbe ReturnObject = new NotesMailProbe();
            try
            {
                //12/7/2012 NS modified - need to return Server Name, it will be used in the Target Server combo box.
                //Currently, the combo box erroneously displays Server ID value
                string SqlQuery = "SELECT *, ServerName FROM NotesMailProbe " +
                    "INNER JOIN Servers ON ID = DestinationServerID where [Name]='" + 
                    ProbObject.Name + "'";
                NotesMailProbeDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
                ReturnObject.Name = NotesMailProbeDataTable.Rows[0]["Name"].ToString();
                if (NotesMailProbeDataTable.Rows[0]["Enabled"].ToString() != "")
                    ReturnObject.Enabled = bool.Parse(NotesMailProbeDataTable.Rows[0]["Enabled"].ToString());
               
                if (NotesMailProbeDataTable.Rows[0]["ScanInterval"].ToString() != "")
                    ReturnObject.ScanInterval = int.Parse(NotesMailProbeDataTable.Rows[0]["ScanInterval"].ToString());
                if (NotesMailProbeDataTable.Rows[0]["OffHoursScanInterval"].ToString() != "")
                    ReturnObject.OffHoursScanInterval = int.Parse(NotesMailProbeDataTable.Rows[0]["OffHoursScanInterval"].ToString());
                if (NotesMailProbeDataTable.Rows[0]["RetryInterval"].ToString() != "")
                    ReturnObject.RetryInterval = int.Parse(NotesMailProbeDataTable.Rows[0]["RetryInterval"].ToString());
                ReturnObject.Category = NotesMailProbeDataTable.Rows[0]["Category"].ToString();
                ReturnObject.Filename = NotesMailProbeDataTable.Rows[0]["Filename"].ToString();
                if( NotesMailProbeDataTable.Rows[0]["EchoService"].ToString()!="")
                ReturnObject.EchoService =bool.Parse( NotesMailProbeDataTable.Rows[0]["EchoService"].ToString());
                ReturnObject.NotesMailAddress = NotesMailProbeDataTable.Rows[0]["NotesMailAddress"].ToString();
               if(NotesMailProbeDataTable.Rows[0]["DestinationServerID"].ToString()!=""&&NotesMailProbeDataTable.Rows[0]["DestinationServerID"].ToString()!=null)
                ReturnObject.DestinationServerID = int.Parse(NotesMailProbeDataTable.Rows[0]["DestinationServerID"].ToString());
                ReturnObject.DestinationDatabase = NotesMailProbeDataTable.Rows[0]["DestinationDatabase"].ToString();
                if(NotesMailProbeDataTable.Rows[0]["DeliveryThreshold"].ToString()!="")
                    ReturnObject.DeliveryThreshold = int.Parse(NotesMailProbeDataTable.Rows[0]["DeliveryThreshold"].ToString());
                ReturnObject.SourceServer = NotesMailProbeDataTable.Rows[0]["SourceServer"].ToString();
                ReturnObject.ReplyTo=NotesMailProbeDataTable.Rows[0]["ReplyTo"].ToString();
                ReturnObject.ServerName = NotesMailProbeDataTable.Rows[0]["ServerName"].ToString();
               
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
        /// Insert data into NotesMailProbe table
        /// </summary>
        /// <param name="DSObject">NotesMailProbe object</param>
        /// <returns></returns>

        public bool InsertData(NotesMailProbe ProbeObject)
        {
            bool Insert = false;
            try
            {
                string SqlQuery = "INSERT INTO NotesMailProbe (Enabled,Name,NotesMailAddress,Category,ScanInterval"+
                    ",OffHoursScanInterval,DeliveryThreshold,RetryInterval,DestinationServerID,DestinationDatabase"+
                    ",SourceServer,EchoService,ReplyTo,Filename)"+
                    "VALUES('" + ProbeObject.Enabled + "','" + ProbeObject.Name+
                    "','" + ProbeObject.NotesMailAddress+ "','" + ProbeObject.Category+
                    "','" + ProbeObject.ScanInterval+ "','" + ProbeObject.OffHoursScanInterval+
                    "'," + ProbeObject.DeliveryThreshold+ "," + ProbeObject.RetryInterval+
                    ",'" + ProbeObject.DestinationServerID+ "','" + ProbeObject.DestinationDatabase+ "','" + ProbeObject.SourceServer+
                    "','" + ProbeObject.EchoService+
                    "','" + ProbeObject.ReplyTo+ "','" + ProbeObject.Filename+ "')";


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
        /// Update data into NotesMailProbe table
        /// </summary>
        /// <param name="ProbeObject">NotesMailProbe object</param>
        /// <returns></returns>
        public Object UpdateData(NotesMailProbe ProbeObject, string name)
        {
            
            Object Update;
            try
            {
                string SqlQuery = "UPDATE NotesMailProbe SET Enabled='" + ProbeObject.Enabled + "',NotesMailAddress='" + ProbeObject.NotesMailAddress +
                    "',Category='" + ProbeObject.Category + "',ScanInterval=" + ProbeObject.ScanInterval +
                    ",OffHoursScanInterval=" + ProbeObject.OffHoursScanInterval + ",RetryInterval=" + ProbeObject.RetryInterval +
                    ",DeliveryThreshold=" + ProbeObject.DeliveryThreshold + ",DestinationServerID='"+ProbeObject.DestinationServerID+
                   "',SourceServer='"+ProbeObject.SourceServer+ "',Name='"+ProbeObject.Name+
                    "',EchoService='" + ProbeObject.EchoService + "',ReplyTo='" + ProbeObject.ReplyTo + "',Filename='" + ProbeObject.Filename + "',DestinationDatabase='" + ProbeObject.DestinationDatabase +
                    "' WHERE Name= '" + name + "'";

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

        //delete Data from NotesMailProbe Table

        public Object DeleteData(NotesMailProbe ProbObject)
        {
            Object Update;
            try
            {

                string SqlQuery = "Delete NotesMailProbe Where Name='" + ProbObject.Name + "'";

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
            NotesMailProbeHistory ReturnObject = new NotesMailProbeHistory();
            try
            {
                string SqlQuery = "SELECT * FROM NotesMailProbeHistory";

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

        public DataTable GetIPAddress(NotesMailProbe MailObj, string name)
        {
            //SametimeServers SametimeObj = new SametimeServers();
            DataTable MailProbTable = new DataTable();
            try
            {
                string sqlQuery = "";
                if (name == "")
                {
                    sqlQuery = "Select * from NotesMailProbe where Name='" + MailObj.Name + "'";
                }
                else
                {
                    sqlQuery = "Select * from NotesMailProbe where Name='" + MailObj.Name + "' and name<>'" + name + "'";                
                }
                   MailProbTable = objAdaptor.FetchData(sqlQuery);
                //if (MailObj.NotesMailAddress == "" && MailObj.NotesMailAddress!=null)
                //{
                //    string sqlQuery = "Select * from NotesMailProbe where NotesMailAddress='" + MailObj.NotesMailAddress + "' or Name='"+MailObj.Name+"'";
                //    MailProbTable = objAdaptor.FetchData(sqlQuery);
                //}          
                //else
                //{
                //    string sqlQuery = "Select * from NotesMailProbe where  (Name='" + MailObj.Name + "' and NotesMailAddress!='" + MailObj.NotesMailAddress + "') " +
                //        " or (Name!='" + MailObj.Name + "' and NotesMailAddress='" + MailObj.NotesMailAddress + "') ";
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
            NotesMailProbe ReturnObject = new NotesMailProbe();
            try
            {
                //1/29/2013 NS modified - added sort and distinct to make sure server names are not duplicated in the drop down list
                string SqlQuery = "SELECT DISTINCT ServerName,ID,LocationID FROM Servers ORDER BY ServerName";

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

        public DataTable GetAllDataByName(NotesMailProbe MailObj)
        {

            DataTable ProbeDataTable = new DataTable();
            NotesMailProbe ReturnObject = new NotesMailProbe();
            try
            {
                string SqlQuery = "SELECT Enabled,Name,NotesMailAddress,Category,ScanInterval,OffHoursScanInterval,DeliveryThreshold," +
                    "RetryInterval,(select ServerName from Servers where ID= DestinationServerID) as DestinationServer,DestinationDatabase,SourceServer,EchoService,ReplyTo,Filename FROM NotesMailProbe where Name='" + MailObj.Name + "'";

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

		public DataTable GetDominoServerNames()
		{

			DataTable ProbeDataTable = new DataTable();
			NotesMailProbe ReturnObject = new NotesMailProbe();
			try
			{
				//1/29/2013 NS modified - added sort and distinct to make sure server names are not duplicated in the drop down list
				string SqlQuery = "SELECT DISTINCT ServerName,ID,LocationID FROM Servers WHERE ServerTypeID = 1 ORDER BY ServerName";

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
