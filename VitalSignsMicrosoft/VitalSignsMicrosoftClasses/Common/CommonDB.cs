using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using VSFramework;
using System.Data;
using System.Threading;
using System.Globalization;
using MongoDB.Driver;
namespace VitalSignsMicrosoftClasses
{
	class CommonDB
	{
		VSFramework.VSAdaptor myAdapter = new VSFramework.VSAdaptor();
		VSFramework.XMLOperation myxmlAdapter = new VSFramework.XMLOperation();

        public static readonly String DB_VITALSIGNS = "VitalSigns";
        public static readonly String DB_VSS_STATISTICS = "VSS_Statistics";

		//Determines the verbosity of the log file
		//enum LogLevel
		//{
		//    Verbose = 0,
		//    Normal = 1
		//}
		//'MyLogLevel is used throughout to control the volume of the log file output
		//LogLevel MyLogLevel;
		//CultureInfo culture = CultureInfo.CurrentCulture;
		//SqlConnection con = new SqlConnection("Data Source=USER-PC;Initial Catalog=vitalsigns;Integrated Security=true;");//Persist Security Info=True;multipleactiveresultsets=true; Min Pool Size=20;Max Pool Size=500; Connection Timeout=30; " />");
		//SqlConnection con = new SqlConnection("Data Source=174.46.239.207,443;Initial Catalog=vitalsigns;User ID=sa;Password=vsadmin123!;Persist Security Info=True;");//Persist Security Info=True;multipleactiveresultsets=true; Min Pool Size=20;Max Pool Size=500; Connection Timeout=30; " />");
		string sqlcon = "";
		SqlConnection con;

		public CommonDB()
		{
			//InitializeComponent();
			try
			{
				sqlcon = myxmlAdapter.GetDBConnectionString("VitalSigns");
				con = new SqlConnection(sqlcon);
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry("ALL", "Microsoft", "Error in CommonDB. Error: " + ex.Message, Common.LogLevel.Normal);
			}
		}

        public CommonDB(String dbName)
        {
            //InitializeComponent();
            sqlcon = myxmlAdapter.GetDBConnectionString(dbName);
            con = new SqlConnection(sqlcon);
        }
		public bool RecordExists(string SqlQuery)
		{
			bool retVal = false;
			try
			{
				if (con.State == ConnectionState.Closed) con.Open();
				DataTable dt = new DataTable();
				DataSet ds = new DataSet();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();
				myCommand.Connection = con;
				// SqlQuery = "select * from " + TableName + " where " + ForField + "='" + FieldValue + "'";
				myCommand.CommandText = SqlQuery;
				myAdapter.SelectCommand = myCommand;
				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myCommand.Dispose();
				//con.Close();
				if (dt.Rows.Count > 0)
				{
					retVal= true;
				}
				else
				{
					retVal= false;
				}
			}
			catch
			{
			}
			finally
			{
				if (con.State == ConnectionState.Open)
				{
					con.Close();
					//con.Dispose();
				}
			}
			return retVal;

		}

		public DataTable GetData(string SqlQuery)
		{
			DataTable dt = new DataTable();
			try
			{

				//Common.WriteDeviceHistoryEntry("Exchange", "", DateTime.Now.ToString() + " Connection string: " + con.ConnectionString, Common.LogLevel.Normal);

				if (con.State == ConnectionState.Closed) con.Open();
				DataSet ds = new DataSet();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();
				myCommand.Connection = con;
				// SqlQuery = "select * from " + TableName + " where " + ForField + "='" + FieldValue + "'";
				myCommand.CommandText = SqlQuery;
				myAdapter.SelectCommand = myCommand;
				myAdapter.Fill(ds, "dt");
				dt = ds.Tables[0];
				myCommand.Dispose();
				//con.Close();

			}
			catch (Exception ex)
			{

				Common.WriteDeviceHistoryEntry("ALL", "Microsoft", "Error in GetData.  Sql: " + SqlQuery + "... Error: " + ex.Message, Common.LogLevel.Normal);

			}
			finally
			{
				if (con.State == ConnectionState.Open)
				{
					con.Close();
					//con.Dispose();
				}
			}
			return dt;
		}

		public int Execute(string SqlQuery)
		{
			int iresults = 0;

			try
			{
				if (con.State == ConnectionState.Closed) con.Open();
				DataTable dt = new DataTable();
				DataSet ds = new DataSet();
				SqlDataAdapter myAdapter = new SqlDataAdapter();
				SqlCommand myCommand = new SqlCommand();
				myCommand.Connection = con;
				myCommand.CommandText = SqlQuery;
				//iresults = myCommand.ExecuteNonQuery();
				myCommand.Dispose();
				//con.Close();
				
			}
			catch
			{
			}
			finally
			{
				if (con.State == ConnectionState.Open)
				{
					con.Close();
					//con.Dispose();
				}
			}
			return iresults;
			}

        public string GetMongoConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["VitalSignsMongo"].ToString();
            
            //return "mongodb://192.168.1.10:27017/vitalsigns_reference";
        }

		public void UpdateAllTests(TestResults AllTestsList, MonitoredItems.MicrosoftServer Server, string ServerType)
		{
			
				//drs
				ProcessStatusDetails(AllTestsList, Server, ServerType);
				//ProcessSQLStatements(AllTestsList, Server, ServerType);
                ProcessMongoStatements(AllTestsList, Server, ServerType);
				ProcessAlerts(AllTestsList, Server, ServerType);

			}
        private void ProcessMongoStatements(TestResults AllTestsList, MonitoredItems.MicrosoftServer Server, string ServerType)
        {
            try
            {
                // TODO: Start transaction

                //Loop through all the different Entity types. Must make a new Repo for each type.
                foreach (MongoStatements mongoStatement in AllTestsList.MongoEntity)
                {
                    mongoStatement.Execute();
                }
            }
            catch (Exception ex)
            {
                Common.WriteDeviceHistoryEntry(ServerType, Server.Name, "Exception while executing Mongo Statements. Error: " + ex.Message, Common.LogLevel.Normal);
            }
            // TODO: End transaction
            finally
            {
                Common.WriteDeviceHistoryEntry(ServerType, Server.Name, "End of SQL Statement");
            }


        }

		public void UpdateSQLStatements(TestResults AllTestsList, MonitoredItems.MicrosoftServer Server)
		{
			//ProcessSQLStatements(AllTestsList, Server);
            ProcessMongoStatements(AllTestsList, Server, Server.ServerType);
			ProcessAlerts(AllTestsList, Server);
		}

		private void ProcessStatusDetails(TestResults AllTestsList, MonitoredItems.MicrosoftServer Server)
		{
			ProcessStatusDetails(AllTestsList, Server, Server.ServerType);
		}
		private void ProcessStatusDetails(TestResults AllTestsList, MonitoredItems.MicrosoftServer Server, string serverType)
        {

			string currentCul = Thread.CurrentThread.CurrentCulture.ToString();
            //assume all tests passed.
            bool TestStatusPass = true;
            commonEnums.AlertType alertType=commonEnums.AlertType.None;
            DateTime time = DateTime.Now;              // Use current time
			string format = "t";//"ddd, MMM d HH:mm yyyy";    // Use this format


			string strSQL = "";
            try
            {
                // Figure out if all tests passed, and if not, which one
                string OverallStatus = "";
                string Details = "";
                CommonDB DB = new CommonDB();
                //Delete the StatusDetails for this server here
				if (!Server.FastScan)
				{
				if (serverType =="Office365")
					strSQL = "DELETE FROM StatusDetail WHERE TYPEANDNAME='" + Server.Name + "-" + Server.Location + "'";
				else
                    strSQL = "DELETE FROM StatusDetail WHERE TYPEANDNAME='" + Server.Name + "-" + serverType + "' AND Category != 'Mailbox' AND TestName not in ('" + (Server.HourlyAlerts != null ? string.Join("','", Array.ConvertAll(Server.HourlyAlerts.ToArray(), i => i.AlertType.ToString().Replace('_', ' '))) : "") + "')";
                
                DB.Execute(strSQL);
				}
                //first get all the fails
                foreach (Alerting T in AllTestsList.AlertDetails)//.TakeWhile(j => j.Result == commonEnums.ServerResult.Fail))
                {
                    alertType=T.AlertType;
                    //if there's one  FAIL record. set the flag and exit
					if (T.ResetAlertQueue == commonEnums.ResetAlert.No)
					{
						TestStatusPass = false;
						if (T.AlertTypeString.ToString().Contains("Services") || T.AlertType.ToString() == "DAG_Member_Health" || T.AlertType.ToString() == "DAG_Database_Health" || T.AlertType.ToString() == "DAG_Activation_Preference" || T.AlertType.ToString() == "DAG_Replay_Queue" || T.AlertType.ToString() == "DAG_Copy_Queue"

								|| T.AlertType.ToString() == "Create_Mail_Folder" || T.AlertType.ToString() == "Mail_flow" || T.AlertType.ToString() == "Create_Site" || T.AlertType.ToString() == "OneDrive_Upload_Document" || T.AlertType.ToString() == "OneDrive_Download_Document")
						{
							Details = T.Details;
							//break;
						}
						else if(T.AlertType != commonEnums.AlertType.None)
						{
                            Details = T.AlertType.ToString().Replace("_", " ") + ":  " + (T.Details.Trim().EndsWith(".") ? T.Details.TrimEnd('.') : T.Details) + " at " + time.ToString(format);
							break;
						}
                        else
                        {
                            Details = (T.Details.Trim().EndsWith(".") ? T.Details.TrimEnd('.') : T.Details) + " at " + time.ToString(format);
                            break;
                        }
						break;
					}
                }

                if(TestStatusPass == true && Server.HourlyAlerts != null)
                {

                    foreach (MonitoredItems.MicrosoftServer.HourlyAlert T in Server.HourlyAlerts)
                    {
                        if (T.AlertRaised == true)
                        {
                            TestStatusPass = false;
                            if (T.AlertType.ToString().Contains("Services") || T.AlertType.ToString() == "DAG_Member_Health" || T.AlertType.ToString() == "DAG_Database_Health" || T.AlertType.ToString() == "DAG_Activation_Preference" || T.AlertType.ToString() == "DAG_Replay_Queue" || T.AlertType.ToString() == "DAG_Copy_Queue")
                            {
                                Details = T.AlertDetails;
                                //break;
                            }
                            else
                            {
                                Details = T.AlertType.ToString().Replace("_", " ") + ":  " + T.AlertDetails + " at " + time.ToString(format);
                                break;
                            }
                            break;
                        }
                    }

                }

             
                             
                if (TestStatusPass)
                {
					Common.WriteDeviceHistoryEntry(serverType, Server.Name, "Status is set to OK", commonEnums.ServerRoles.Empty, Common.LogLevel.Verbose);
					Common.WriteDeviceHistoryEntry(serverType, Server.Name, "Status details are " + Details, commonEnums.ServerRoles.Empty, Common.LogLevel.Verbose);
                    OverallStatus = "OK";
					if (Server.FastScan)
						Details = "Successfully connected via fast scan at " + time.ToString(format);
					else
						Details = "All tests passed successfully at " + time.ToString(format);
					Server.FastScan = !Server.FastScan;
                }
                else
                {
					Common.WriteDeviceHistoryEntry(serverType, Server.Name, "Status is set to Issue", commonEnums.ServerRoles.Empty, Common.LogLevel.Verbose);
                    if (alertType == commonEnums.AlertType.Not_Responding)
                    {
                        OverallStatus = "Not Responding";

                    }
                    else
                    {
                        OverallStatus = "Issue";

                    }
                    Common.WriteDeviceHistoryEntry(serverType, Server.Name, "Status details are " + Details, commonEnums.ServerRoles.Empty, Common.LogLevel.Verbose);
					if(Details == "")
						Details = "An issue was detected.";
					Server.FastScan = false;
                }

				Server.Status = OverallStatus;
				Server.StatusCode = OverallStatus;
				Server.LastScan = DateTime.Now;

				if (Details.Length > 255)
					Details = Details.Substring(0, 250) + "...";

				if (serverType == "Office365")
					strSQL = " SELECT * FROM STATUS WHERE TYPEANDNAME='" + Server.Name + "-" + Server.Location + "'";
				else
					strSQL = " SELECT * FROM STATUS WHERE TYPEANDNAME='" + Server.Name + "-" + serverType + "'";

				if(Server.GetType() == typeof(MonitoredItems.ExchangeServer))
				{
					MonitoredItems.ExchangeServer exServer = Server as MonitoredItems.ExchangeServer;
					if (OverallStatus == "OK" && (exServer.OWAUsers > 2140000000 || exServer.RPCClientAccesUsers > 2140000000))
                    {
                        OverallStatus = "Issue";
						Details += ". The user count is unreasonably high, and this condition is due to an IIS bug. The count can be reset to the correct value by restarting either IIS or the server itself.";
				}
				}

				bool  isExist = DB.RecordExists(strSQL);
				if (isExist)
				{
                //finally Update the Status table with OverallStatus for this server
                strSQL = "UPDATE STATUS SET " + 
					"STATUS='" + OverallStatus + "', " + 
					"DETAILS='" + Details + "', " +
					"STATUSCODE='" + OverallStatus + "', " +
					"LASTUPDATE='" + DateTime.Now.ToString() + "', " +
					"Name='" + Server.Name +"', " +
					"UserCount='" + Server.UserCount.ToString() + "', " +
					"ResponseTime='" + Server.ResponseTime.ToString() + "', " +
					"SecondaryRole='', " +
					"Location='" + Server.Location + "', " +
					"Category='" + Server.Category + "', " +
					"ResponseThreshold='" + Server.ResponseThreshold.ToString() + "', " +
					"DominoVersion='" + serverType +" " + (Server.VersionNo=="0" ? "Version Unknown" : Server.VersionNo) + "', " +
					"OperatingSystem='" + Server.OperatingSystem + "', " +
					"NextScan='" + Server.NextScan.ToString() + "' " ;
					if (serverType == "Office365")
						strSQL += " WHERE TYPEANDNAME='" + Server.Name + "-" + Server.Location + "'";
					else
						strSQL += " WHERE TYPEANDNAME='" + Server.Name + "-" + serverType + "'"; 
                int retCount = DB.Execute(strSQL);
				}
				else
                {
                    //insert 
					if (serverType == "Office365")
					{
						strSQL = "" +
					   "INSERT INTO STATUS (NAME, STATUS, STATUSCODE, LASTUPDATE, TYPE, LOCATION, CATEGORY, " +
					   "TYPEANDNAME, DESCRIPTION, UserCount, ResponseTime, " +
					   "SecondaryRole," +
					   "ResponseThreshold, " +
					   "DominoVersion, OperatingSystem, NextScan, Details) " +
					   "VALUES ('" + Server.Name + "', '" + OverallStatus + "', '" + OverallStatus + "', '" + DateTime.Now.ToString() + "','" + serverType + "','" + Server.Location + "','" + Server.Category + "','" +
					   Server.Name + "-" + Server.Location + "', 'Microsoft " + serverType + " Server', '" + Server.UserCount.ToString() + "', '" + Server.ResponseTime.ToString() + "', " +
					   "'', " +
					   "'" + Server.ResponseThreshold.ToString() + "', " +
					   "'" + Server.ServerType + " " + Server.VersionNo + "', '" + Server.OperatingSystem + "', '" + Server.NextScan.ToString() + "', '" + Details + "' )";
					}
					else
					{
						strSQL = "" +
						   "INSERT INTO STATUS (NAME, STATUS, STATUSCODE, LASTUPDATE, TYPE, LOCATION, CATEGORY, " +
						   "TYPEANDNAME, DESCRIPTION, UserCount, ResponseTime, " +
						   "SecondaryRole," +
						   "ResponseThreshold, " +
						   "DominoVersion, OperatingSystem, NextScan, Details) " +
						   "VALUES ('" + Server.Name + "', '" + OverallStatus + "', '" + OverallStatus + "', '" + DateTime.Now.ToString() + "','" + serverType + "','" + Server.Location + "','" + Server.Category + "','" +
						   Server.Name + "-" + serverType + "', 'Microsoft " + serverType + " Server', '" + Server.UserCount.ToString() + "', '" + Server.ResponseTime.ToString() + "', " +
						   "'', " +
						   "'" + Server.ResponseThreshold.ToString() + "', " +
						   "'" + Server.ServerType + " " + Server.VersionNo + "', '" + Server.OperatingSystem + "', '" + Server.NextScan.ToString() + "', '" + Details + "' )";

					}
                    DB.Execute(strSQL);

                    

                }


                MongoStatementsUpsert<VSNext.Mongo.Entities.Status> mongoStatement = new MongoStatementsUpsert<VSNext.Mongo.Entities.Status>();
                mongoStatement.filterDef = mongoStatement.repo.Filter.Where(i => i.TypeAndName == Server.TypeANDName);
                mongoStatement.updateDef = mongoStatement.repo.Updater
                    .Set(i => i.Name, Server.Name)
                    .Set(i => i.CurrentStatus, Server.Status)
                    .Set(i => i.StatusCode, Server.StatusCode)
                    .Set(i => i.LastUpdated, DateTime.Now)
                    .Set(i => i.NextScan, Server.NextScan)
                    .Set(i => i.Type, Server.ServerType)
                    .Set(i => i.Location, Server.Location)
                    .Set(i => i.Category, Server.Category)
                    .Set(i => i.TypeAndName, Server.TypeANDName)
                    .Set(i => i.Description, "Microsoft")
                    .Set(i => i.UserCount, int.Parse(Server.UserCount.ToString()))
                    .Set(i => i.ResponseTime, int.Parse(Server.ResponseTime.ToString()))
                    .Set(i => i.ResponseThreshold, int.Parse(Server.ResponseThreshold.ToString()))
                    .Set(i => i.SoftwareVersion, Server.VersionNo)
                    .Set(i => i.OperatingSystem, Server.OperatingSystem)
                    .Set(i => i.Details, Details);

                mongoStatement.Execute();

                int TestFailures = 0;
                foreach (TestList T in AllTestsList.StatusDetails)
                     
                {
                    //loop thru all the servers and update the status details
                   
                    int DetailsLenth = T.Details.Length;
                    if (DetailsLenth > 100)
                        DetailsLenth = 100;
                    //if (T.TestName.ToString().Contains("Services"))
                    //{

                    //    string[] newStrings = Details.Split(new string[] { " " }, StringSplitOptions.None);
                    //    string services = "";
                       
                    //    services = newStrings[2].ToString();
                    //    T.TestName = T.TestName + ":" + services;
                    //    strSQL = "Insert INTO StatusDetail (TypeAndName, Category, TestName, Result, Details, LastUpdate) VALUES ( '" + Server.Name + "-" + serverType + "', '" + T.Category + "', '" + T.TestName.ToString() + "', '" + T.Result.ToString() + "', '" + T.Details.ToString().Substring(0, DetailsLenth) + "', '" + DateTime.Now.ToString() + "')";
                    //}
                    //else
                    //{
                        strSQL = "Insert INTO StatusDetail (TypeAndName, Category, TestName, Result, Details, LastUpdate) VALUES ( '" + Server.Name + "-" + serverType + "', '" + T.Category + "', '" + T.TestName.ToString() + "', '" + T.Result.ToString() + "', '" + T.Details.ToString().Substring(0, DetailsLenth) + "', '" + DateTime.Now.ToString() + "')";
                    //}
                    try
                    {
                       // DB.Execute(strSQL);
                    }
                    catch (Exception ex)
                    {
						Common.WriteDeviceHistoryEntry(Server.ServerType, Server.Name, "Error Executing Query =  " + strSQL + " with error message of " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                    }
                }

               


            }
            catch (Exception ex)
            {
                //Common.WriteTestResults(Server.Name, "exchange", "Exception", "Exception", ex.Message.ToString());
				Common.WriteDeviceHistoryEntry(serverType, Server.Name, "Error Executing Query =  " + strSQL + " with error message of " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);

            }
            // TODO: End transaction
            finally
            {
                //commit or rollback

            }


        }

		public void ProcessAlerts(TestResults AllTestsList, MonitoredItems.MicrosoftServer Server, string ServerType)
        {
            try
            {
                DateTime time = DateTime.Now;              // Use current time
                string format = "t";
                string Details = "";
                AlertLibrary.Alertdll myAlert = new AlertLibrary.Alertdll();

                // TODO: Start transaction
                List<Alerting> AlertStmts = AllTestsList.AlertDetails;

                foreach (Alerting AlertStmt in AlertStmts)
                {

                    try
                    {
                        if (AlertStmt.ResetAlertQueue == commonEnums.ResetAlert.No)
                        {

							if (AlertStmt.AlertTypeString.ToString().Contains("Services") || AlertStmt.AlertType.ToString() == "DAG_Member_Health" || AlertStmt.AlertType.ToString() == "DAG_Database_Health" || AlertStmt.AlertType.ToString() == "DAG_Activation_Preference" || AlertStmt.AlertType.ToString() == "DAG_Replay_Queue" || AlertStmt.AlertType.ToString() == "DAG_Copy_Queue"

								|| AlertStmt.AlertType.ToString() == "Create_Mail_Folder" || AlertStmt.AlertType.ToString() == "Mail_flow" || AlertStmt.AlertType.ToString() == "Create_Site" || AlertStmt.AlertType.ToString() == "OneDrive_Upload_Document" || AlertStmt.AlertType.ToString() == "OneDrive_Download_Document")
                            {
                                AlertStmt.Details = AlertStmt.Details;
                                //break;
                            }
                            else if( AlertStmt.AlertType != commonEnums.AlertType.None)
                            {
                                AlertStmt.Details = AlertStmt.AlertType.ToString().Replace("_", " ") + ":  " + AlertStmt.Details + " at " + time.ToString(format);
                            }
                        }
                        if (AlertStmt.ResetAlertQueue == commonEnums.ResetAlert.No)
                        {
							//if (AlertStmt.DeviceType.ToString() =="Office365")
							//    myAlert.QueueAlert(AlertStmt.Category, AlertStmt.DeviceName, AlertStmt.AlertType.ToString().Replace('_', ' '), AlertStmt.Details, AlertStmt.Location, AlertStmt.Category);
							//else
                            myAlert.QueueAlert(AlertStmt.DeviceType.ToString().Replace("_", " "), AlertStmt.DeviceName, string.IsNullOrEmpty(AlertStmt.AlertTypeString) ? AlertStmt.AlertType.ToString().Replace('_', ' ') : AlertStmt.AlertTypeString, AlertStmt.Details, AlertStmt.Location, AlertStmt.Category);
                        }
                        else
                        {
							//if (AlertStmt.DeviceType.ToString() == "Office365")
							//    myAlert.ResetAlert(AlertStmt.Category, AlertStmt.DeviceName, AlertStmt.AlertType.ToString().Replace('_', ' '), AlertStmt.Location, AlertStmt.Details, AlertStmt.Category);
							//else
                            myAlert.ResetAlert(AlertStmt.DeviceType.ToString().Replace("_", " "), AlertStmt.DeviceName, string.IsNullOrEmpty(AlertStmt.AlertTypeString) ? AlertStmt.AlertType.ToString().Replace('_', ' ') : AlertStmt.AlertTypeString, AlertStmt.Location, AlertStmt.Details, AlertStmt.Category);
                        }

                    }
                    catch (Exception ex)
                    {
						Common.WriteDeviceHistoryEntry(ServerType, Server.Name, "Error Executing Alert =  " + AlertStmt.Details + " with error message of " + ex.Message, commonEnums.ServerRoles.Empty, Common.LogLevel.Normal);
                    }
                }

            }
            catch (Exception ex)
            {
				Common.WriteTestResults(ServerType, Server.Name, "Exception", "Exception", ex.Message.ToString());
            }
            // TODO: End transaction
            finally
            {
                
            }
        }
		private void ProcessAlerts(TestResults AllTestsList, MonitoredItems.MicrosoftServer Server)
		{
			ProcessAlerts(AllTestsList, Server, Server.ServerType);
		}

        private void ProcessSQLStatements(TestResults AllTestsList, MonitoredItems.MicrosoftServer Server, string ServerType)
        {
            CommonDB VSDB = new CommonDB();
            CommonDB VssStatsDB = new CommonDB(DB_VSS_STATISTICS);

            try
            {
                // TODO: Start transaction
                List <SQLstatements> SQLStmts = AllTestsList.SQLStatements;
				Common.WriteDeviceHistoryEntry(ServerType, Server.Name, "Start of SQL Statement.  There are " + SQLStmts.Count + " statements");
                foreach (SQLstatements strSQL in SQLStmts)
                {
                    //loop thru all the servers and update the status details
                    try
                    {
                        if (strSQL.DatabaseName.ToLower().Equals(DB_VITALSIGNS.ToLower())){
                            VSDB.Execute(strSQL.SQL);
                            Common.WriteDeviceHistoryEntry(ServerType, Server.Name, "Executed SQL Statement " + strSQL.SQL,Common.LogLevel.Verbose);
                        }else{
                            VssStatsDB.Execute(strSQL.SQL);
							Common.WriteDeviceHistoryEntry(ServerType, Server.Name, "Executed SQL Statement " + strSQL.SQL, Common.LogLevel.Verbose);
                        }
                    }
                    catch (Exception ex)
                    {
						Common.WriteDeviceHistoryEntry(ServerType, Server.Name, "Error Executing Query =  " + strSQL.SQL + " with error message of " + ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
				Common.WriteTestResults(ServerType, Server.Name, "Exception", "Exception", ex.Message.ToString());
            }
            // TODO: End transaction
            finally
            {
				Common.WriteDeviceHistoryEntry(ServerType, Server.Name, "End of SQL Statement");

                if (VSDB.con.State == ConnectionState.Open)
                {
                    VSDB.con.Close();
                    VSDB.con.Dispose();
                }

                if (VssStatsDB.con.State == ConnectionState.Open)
                {
                    VssStatsDB.con.Close();
                    VssStatsDB.con.Dispose();
                }
                //commit or rollback
            }


        }
		public void ProcessSQLStatements(TestResults AllTestsList, MonitoredItems.MicrosoftServer Server)
		{
			ProcessSQLStatements(AllTestsList, Server, Server.ServerType);
		}

		public void NotRespondingQueries(MonitoredItems.MicrosoftServer Server, string ServerType, String Details = "")
		{
			string strSQL = "";
            try
            {

                string NotResponding = "Not Responding";

                CommonDB DB = new CommonDB();
				Server.Status = NotResponding;

				//Status Details Table
				if (ServerType == "Office365")
					strSQL = "DELETE FROM StatusDetail WHERE TYPEANDNAME='" + Server.Name + "-" + Server.Location + "'";
				else
					strSQL = "DELETE FROM StatusDetail WHERE TYPEANDNAME='" + Server.Name + "-" + ServerType + "'";
                DB.Execute(strSQL);

				//DAG Table
				TestResults SQLStatementsList = new TestResults();
				
				string dagMessage = "";
				if (Server.ServerType == "Exchange")
				{
					strSQL = "DELETE FROM ExgMailHealthDetails WHERE ServerName='" + Server.Name + "'";
					DB.Execute(strSQL);
					strSQL = "DELETE FROM ExgMailHealth WHERE ServerName='" + Server.Name + "'";
					DB.Execute(strSQL);
				}
				else if (Server.ServerType == "Active Directory")
				{
					strSQL = "UPDATE ActiveDirectoryTest set LogOnTest='Fail', QueryTest='Fail', LDAPPortTest='Fail' WHERE ServerID=" + Server.ServerTypeId;
					DB.Execute(strSQL);
				}
				else if (Server.ServerType == "SharePoint")
				{

				}
				//strSQL = "DELETE FROM DiskSpace WHERE ServerName='" + Server.Name + "'";
				//DB.Execute(strSQL);

				ProcessSQLStatements(SQLStatementsList, Server, ServerType);

				Server.LastScan = DateTime.Now;
				string TypeAndName = Server.Name + "-" + ServerType;
				if (ServerType == "Office365")
					TypeAndName = Server.Name + "-" + Server.Location ;

				else
					TypeAndName = Server.Name + "-" + ServerType;

                //finally Update the Status table with OverallStatus for this server

				if (Server.GetType() == typeof(MonitoredItems.Office365Server) && ((MonitoredItems.Office365Server)(Server)).AuthenticationTest == false)
				{
					Details = "The Authentication test failed to connect to the tenant.";
				}
                else if (Server.GetType() == typeof(MonitoredItems.Office365Server) && (((MonitoredItems.Office365Server)(Server)).AuthenticationTest == true))
                {
					Details = "The Authentication test passed, PowerShell Connection failed to connect to the tenant.";

                }
                else
                {
                    if(Details == "")
                        Details = "The service failed to connect to the server after several attempts.";
                    
                }

                strSQL = "UPDATE STATUS SET " +
					"STATUS='" + Server.Status + "', " +
					"DETAILS='"+Details+"', " +
					"STATUSCODE='" + NotResponding + "', " +
					"LASTUPDATE='" + DateTime.Now.ToString() + "', " +

					//"TypeAndName='" + TypeAndName + "', " +
					"UserCount='0', " +
					"ResponseTime='0', " +
					"SecondaryRole='', " +
					"Location='" + Server.Location + "', " +
					"Category='" + Server.Category + "', " +
					"ResponseThreshold='" + Server.ResponseThreshold + "', " +
					"DominoVersion='', " +
					"OperatingSystem='', " +
					"NextScan='" + Server.NextScan + "', " +
					"CPU=0, " + 
					"Memory=0 " +
					"WHERE TypeAndName='" + TypeAndName + "'";
                int retCount = DB.Execute(strSQL);
                if (retCount == 0)
                {
                    //insert 
					
                    strSQL = "" + 
						"INSERT INTO STATUS (NAME, STATUS,DETAILS, STATUSCODE, LASTUPDATE, TYPE, LOCATION, CATEGORY, " + 
						"TYPEANDNAME, DESCRIPTION, UserCount, ResponseTime, " +
						"SecondaryRole," +
						"ResponseThreshold, " +
						"DominoVersion, OperatingSystem, NextScan, CPU, Memory) " +
						"VALUES ('" + Server.Name + "', '" + NotResponding + "','"+Details+"','" + NotResponding + "', '" + DateTime.Now.ToString() + "','" + ServerType + "','" + Server.Location + "','" + Server.Category + "','" +
						TypeAndName + "', 'Microsoft " + ServerType + " Server', '0', '0', " +
						"'', " + 
						"'" + Server.ResponseThreshold + "', " +
						"'', '', '" + Server.NextScan + "', 0, 0 )";
                    DB.Execute(strSQL);
                }

                MongoStatementsUpsert<VSNext.Mongo.Entities.Status> mongoStatement = new MongoStatementsUpsert<VSNext.Mongo.Entities.Status>();
                mongoStatement.filterDef = mongoStatement.repo.Filter.Where(i => i.TypeAndName == Server.TypeANDName);
                mongoStatement.updateDef = mongoStatement.repo.Updater
                    .Set(i => i.Name, Server.Name)
                    .Set(i => i.CurrentStatus, NotResponding)
                    .Set(i => i.StatusCode, NotResponding)
                    .Set(i => i.LastUpdated, DateTime.Now)
                    .Set(i => i.NextScan, Server.NextScan)
                    .Set(i => i.Type, Server.ServerType)
                    .Set(i => i.Location, Server.Location)
                    .Set(i => i.Category, Server.Category)
                    .Set(i => i.TypeAndName, Server.TypeANDName)
                    .Set(i => i.Description, "Microsoft")
                    .Set(i => i.UserCount, 0)
                    .Set(i => i.ResponseTime, 0)
                    .Set(i => i.ResponseThreshold, int.Parse(Server.ResponseThreshold.ToString()))
                    .Set(i => i.SoftwareVersion, Server.VersionNo)
                    .Set(i => i.OperatingSystem, Server.OperatingSystem)
                    .Set(i => i.Details, Details);

                mongoStatement.Execute();
               


            }
            catch (Exception ex)
            {
                //Common.WriteTestResults(Server.Name, "exchange", "Exception", "Exception", ex.Message.ToString());
				Common.WriteDeviceHistoryEntry(ServerType , Server.Name, "Error in Not Responding...Executing Query =  " + strSQL + " with error message of " + ex.Message);

            }
            // TODO: End transaction
            finally
            {
                //commit or rollback

            }
		}

        public void SetServerStatus(MonitoredItems.MicrosoftServer Server, string ServerType, String Status = "OK", String Details = "")
        {
            
            string strSQL = "";
            try
            {
                CommonDB DB = new CommonDB();
                Server.Status = Status;

                //DAG Table
                TestResults SQLStatementsList = new TestResults();

                Server.LastScan = DateTime.Now;
                string TypeAndName = Server.Name + "-" + ServerType;
                if (ServerType == "Office365")
                    TypeAndName = Server.Name + "-" + Server.Location;

                else
                    TypeAndName = Server.Name + "-" + ServerType;

                //finally Update the Status table with OverallStatus for this server

               

                strSQL = "UPDATE STATUS SET " +
                    "STATUS='" + Server.Status + "', " +
                    "DETAILS='" + Details + "', " +
                    "STATUSCODE='" + Server.Status + "', " +
                    "LASTUPDATE='" + DateTime.Now.ToString() + "', " +
                    "TypeAndName='" + TypeAndName + "', " +
                    "Location='" + Server.Location + "', " +
                    "Category='" + Server.Category + "', " +
                    "ResponseThreshold='" + Server.ResponseThreshold + "', " +
                    "NextScan='" + Server.NextScan + "' " +
                    "WHERE NAME='" + Server.Name + "'";
                int retCount = DB.Execute(strSQL);
                if (retCount == 0)
                {
                    //insert 

                    strSQL = "" +
                        "INSERT INTO STATUS (NAME, STATUS,DETAILS, STATUSCODE, LASTUPDATE, TYPE, LOCATION, CATEGORY, " +
                        "TYPEANDNAME, DESCRIPTION, " +
                        "ResponseThreshold, " +
                        "NextScan) " +
                        "VALUES ('" + Server.Name + "', '" + Server.Status + "','" + Details + "','" + Server.Status + "', '" + DateTime.Now.ToString() + "','" + ServerType + "','" + Server.Location + "','" + Server.Category + "','" +
                        TypeAndName + "', 'Microsoft " + ServerType + " Server', " +
                        "'" + Server.ResponseThreshold + "', " +
                        "'" + Server.NextScan + "')";
                    DB.Execute(strSQL);
                }





            }
            catch (Exception ex)
            {
                //Common.WriteTestResults(Server.Name, "exchange", "Exception", "Exception", ex.Message.ToString());
                Common.WriteDeviceHistoryEntry(ServerType, Server.Name, "Error in Not Responding...Executing Query =  " + strSQL + " with error message of " + ex.Message);

            }
            // TODO: End transaction
            finally
            {
                //commit or rollback

            }
        }

	}
}