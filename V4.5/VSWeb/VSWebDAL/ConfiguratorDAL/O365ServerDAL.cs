using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.ConfiguratorDAL
{
	public class O365ServerDAL
	{
		///<summary>
		///Declarations
		///</summary>
		private Adaptor objAdaptor = new Adaptor();
		private static O365ServerDAL _self = new O365ServerDAL();

		public static O365ServerDAL Ins
		{

			get { return _self; }
		}

		/// <summary>
		/// Get all Data from URLs
		/// </summary>

		public DataTable GetAllData()
		{

			DataTable O365ServerDataTable = new DataTable();
			URLs ReturnDCObject = new URLs();
			try
			{
				//2/5/2014 NS modified the query - adde sort by url
				string SqlQuery = "SELECT * FROM O365Server " +
					"ORDER BY Name";

				O365ServerDataTable = objAdaptor.FetchData(SqlQuery);

			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return O365ServerDataTable;
		}

		/// <summary>
		/// Get Data from O365Server based on Key
		/// </summary>
		public O365Server GetData(O365Server URLObject)
		{
			DataTable O365ServerDataTable = new DataTable();
			O365Server ReturnObject = new O365Server();
			try
			{
				string SqlQuery = "Select * from O365Server where ID=" + URLObject.ID;
				O365ServerDataTable = objAdaptor.FetchData(SqlQuery);
				//populate & return data object
				//11/19/2013 NS added
				ReturnObject.ID = O365ServerDataTable.Rows[0]["ID"].ToString();
				ReturnObject.Name = O365ServerDataTable.Rows[0]["Name"].ToString();
				if (O365ServerDataTable.Rows[0]["ScanInterval"].ToString() != "")
					ReturnObject.ScanInterval = int.Parse(O365ServerDataTable.Rows[0]["ScanInterval"].ToString());
				if (O365ServerDataTable.Rows[0]["OffHoursScanInterval"].ToString() != "")
					ReturnObject.OffHoursScanInterval = int.Parse(O365ServerDataTable.Rows[0]["OffHoursScanInterval"].ToString());
				ReturnObject.Category = O365ServerDataTable.Rows[0]["Category"].ToString();
				if (O365ServerDataTable.Rows[0]["Enabled"].ToString() != "")
					ReturnObject.Enabled = bool.Parse(O365ServerDataTable.Rows[0]["Enabled"].ToString());

				if (O365ServerDataTable.Rows[0]["RetryInterval"].ToString() != "")
					ReturnObject.RetryInterval = int.Parse(O365ServerDataTable.Rows[0]["RetryInterval"].ToString());
				if (O365ServerDataTable.Rows[0]["ResponseThreshold"].ToString() != "")
					ReturnObject.ResponseThreshold = int.Parse(O365ServerDataTable.Rows[0]["ResponseThreshold"].ToString());
				ReturnObject.URL = O365ServerDataTable.Rows[0]["URL"].ToString();
				ReturnObject.SearchStringNotFound = O365ServerDataTable.Rows[0]["SearchString"].ToString();
				ReturnObject.UserName = O365ServerDataTable.Rows[0]["UserName"].ToString();
				ReturnObject.PW = O365ServerDataTable.Rows[0]["PW"].ToString();
				ReturnObject.Location = O365ServerDataTable.Rows[0]["Location"].ToString();
				ReturnObject.imageurl = O365ServerDataTable.Rows[0]["imageurl"].ToString();
				if (O365ServerDataTable.Rows[0]["FailureThreshold"].ToString() != null)
					ReturnObject.FailureThreshold = Convert.ToInt32(O365ServerDataTable.Rows[0]["FailureThreshold"]);
				ReturnObject.mode = O365ServerDataTable.Rows[0]["Mode"].ToString();
				ReturnObject.servername = O365ServerDataTable.Rows[0]["ServerName"].ToString();
                if (O365ServerDataTable.Rows[0]["Costperuser"].ToString() != null)
                    ReturnObject.Costperuser = O365ServerDataTable.Rows[0]["Costperuser"].ToString();

				if (O365ServerDataTable.Rows[0]["CredentialsId"].ToString() == "")
				{
					ReturnObject.CredentialsId = -1;
				}
				else
				{
					ReturnObject.CredentialsId = int.Parse(O365ServerDataTable.Rows[0]["CredentialsId"].ToString());
				}
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

		public DataTable GetOffice365TestsData(int ID)
		{
			DataTable Office365TestsDataTable = new DataTable();
			Office365Tests ReturnObject = new Office365Tests();
			try
			{
				string SqlQuery = "Select * from Office365Tests where ServerId=" + ID;
				Office365TestsDataTable = objAdaptor.FetchData(SqlQuery);


			}
			catch
			{
			}
			finally
			{
			}
			return Office365TestsDataTable;
		}
		/// <summary>
		/// Insert data into URLs table
		/// </summary>
		/// <param name="DSObject">URLs object</param>
		/// <returns></returns>

		public bool InsertData(O365Server O365ServerObject)
		{
			bool Insert = false;
			try
			{
				string SqlQuery = "INSERT INTO O365Server (URL,Name,Category,ScanInterval,OffHoursScanInterval,Enabled" +
                    ",ResponseThreshold,RetryInterval,SearchString,AlertStringFound,UserName,PW,Location,ServerTypeId,FailureThreshold,Imageurl,[Mode],[ServerName],[CredentialsId],Costperuser)" +
					"VALUES('" + O365ServerObject.URL + "','" + O365ServerObject.Name + "','" + O365ServerObject.Category + "'," + O365ServerObject.ScanInterval +
					"," + O365ServerObject.OffHoursScanInterval + ",'" + O365ServerObject.Enabled + "'," + O365ServerObject.ResponseThreshold + "," + O365ServerObject.RetryInterval +
					",'" + O365ServerObject.SearchStringNotFound + "','" + O365ServerObject.SearchStringFound + "','" + O365ServerObject.UserName + "','" + O365ServerObject.PW + "'," +
                    O365ServerObject.LocationId + "," + O365ServerObject.ServerTypeId + "," + O365ServerObject.FailureThreshold + ",'" + O365ServerObject.imageurl + "','" + O365ServerObject.mode + "','" + O365ServerObject.servername + "','" + O365ServerObject.CredentialsId + "','" + O365ServerObject.Costperuser + "')";



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
		/// Update data into URLs table
		/// </summary>
		/// <param name="DSObject">DominoServers object</param>
		/// <returns></returns>
		public Object UpdateData(O365Server O365ServerObject)
		{
			Object Update;
			try
			{
				//11/19/2013 NS modified
				//string SqlQuery = "UPDATE URLs SET Name='" + URLObject.Name + "',Category='" + URLObject.Category + "',ScanInterval=" + URLObject.ScanInterval +
				//",ResponseThreshold=" + URLObject.ResponseThreshold + ",Enabled='" + URLObject.Enabled + "',OffHoursScanInterval=" + URLObject.OffHoursScanInterval +
				//",RetryInterval=" + URLObject.RetryInterval +",SearchString='" + URLObject.SearchString +"',UserName='" + URLObject.UserName + "',PW='" + URLObject.PW+
				//"',LocationId=" + URLObject.LocationId + ",ServerTypeId=" + URLObject.ServerTypeId + ",FailureThreshold=" + URLObject.FailureThreshold + " WHERE [TheURL]='" + URLObject.TheURL + "'";
				string SqlQuery = "UPDATE O365Server SET Name='" + O365ServerObject.Name + "',Url='" + O365ServerObject.URL + "',Category='" + O365ServerObject.Category + "',ScanInterval=" + O365ServerObject.ScanInterval +
				",ResponseThreshold=" + O365ServerObject.ResponseThreshold + ",Enabled='" + O365ServerObject.Enabled + "',OffHoursScanInterval=" + O365ServerObject.OffHoursScanInterval +
				",RetryInterval=" + O365ServerObject.RetryInterval + ",SearchString='" + O365ServerObject.SearchStringNotFound + "',AlertStringFound='" + O365ServerObject.SearchStringFound + "',UserName='" + O365ServerObject.UserName + "',PW='" + O365ServerObject.PW +
                "',Location='" + O365ServerObject.Location + "', Costperuser=" + O365ServerObject.Costperuser + ", ServerTypeId=" + O365ServerObject.ServerTypeId + ",FailureThreshold=" + O365ServerObject.FailureThreshold + " ,Mode='" + O365ServerObject.mode + "',ServerName='" + O365ServerObject.servername + "',CredentialsId='" + O365ServerObject.CredentialsId + "' WHERE [ID]=" + O365ServerObject.ID;
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

		public Object UpdateTestsData(O365Server O365ServerObject)
		{
			Object Update;
			try
			{
				//11/19/2013 NS modified
				//string SqlQuery = "UPDATE URLs SET Name='" + URLObject.Name + "',Category='" + URLObject.Category + "',ScanInterval=" + URLObject.ScanInterval +
				//",ResponseThreshold=" + URLObject.ResponseThreshold + ",Enabled='" + URLObject.Enabled + "',OffHoursScanInterval=" + URLObject.OffHoursScanInterval +
				//",RetryInterval=" + URLObject.RetryInterval +",SearchString='" + URLObject.SearchString +"',UserName='" + URLObject.UserName + "',PW='" + URLObject.PW+
				//"',LocationId=" + URLObject.LocationId + ",ServerTypeId=" + URLObject.ServerTypeId + ",FailureThreshold=" + URLObject.FailureThreshold + " WHERE [TheURL]='" + URLObject.TheURL + "'";
				string SqlQuery = "UPDATE O365Server SET Name='" + O365ServerObject.Name + "',Url='" + O365ServerObject.URL + "',Category='" + O365ServerObject.Category + "',ScanInterval=" + O365ServerObject.ScanInterval +
				",ResponseThreshold=" + O365ServerObject.ResponseThreshold + ",Enabled='" + O365ServerObject.Enabled + "',OffHoursScanInterval=" + O365ServerObject.OffHoursScanInterval +
				",RetryInterval=" + O365ServerObject.RetryInterval + ",SearchString='" + O365ServerObject.SearchStringNotFound + "',AlertStringFound='" + O365ServerObject.SearchStringFound + "',UserName='" + O365ServerObject.UserName + "',PW='" + O365ServerObject.PW +
				"',Location='" + O365ServerObject.Location + "',ServerTypeId=" + O365ServerObject.ServerTypeId + ",FailureThreshold=" + O365ServerObject.FailureThreshold +
				" WHERE [ID]=" + O365ServerObject.ID;
				Update = objAdaptor.ExecuteNonQuery(SqlQuery);
			}
			catch
			{
				Update = false;
			}



			return Update;
		}

		//delete Data from URLs Table

		public Object DeleteData(O365Server O365ServerObject)
		{
			Object Update;
			try
			{
				//11/19/2013 NS modified
				//string SqlQuery = "Delete URLs Where TheURL='" + URLObject.TheURL+"'";
				string SqlQuery = "Delete O365Server  Where O365Server.ID=" + O365ServerObject.ID + " ; Delete Status  where Status.Name='" + O365ServerObject.Name + "' and Status.Type='Office365' ";

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
		public DataTable GetIPAddress(O365Server UrlObj, string mode)
		{
			DataTable UrlTable = new DataTable();
			try
			{
				if (mode == "Insert")
				{
					string sqlQuery = "Select * from O365Server where NAME='" + UrlObj.Name + "'";
					UrlTable = objAdaptor.FetchData(sqlQuery);
				}
				else
				{
					string sqlQuery = "Select * from O365Server where  ID<>" + UrlObj.ID + " AND NAME='" + UrlObj.Name + "' ";
					UrlTable = objAdaptor.FetchData(sqlQuery);

				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return UrlTable;

		}
		public DataTable Get(string id)
		{
			DataTable UrlTable = new DataTable();
			try
			{

				string sqlQuery = "Select * from O365Server where ID=" + id;
				//string sqlQuery = "Select * from O365Server";
				UrlTable = objAdaptor.FetchData(sqlQuery);

			}
			catch (Exception ex)
			{
				throw ex;
			}
			return UrlTable;

		}
		public DataTable GetIdFromName(string name)
		{
			DataTable UrlTable = new DataTable();
			try
			{

				string sqlQuery = "Select * from O365Server where name='" + name + "'";
				//string sqlQuery = "Select * from O365Server";
				UrlTable = objAdaptor.FetchData(sqlQuery);

			}
			catch (Exception ex)
			{
				throw ex;
			}
			return UrlTable;

		}
		public bool InsertCustomPageValue(string userID, string URLval, string titleval, bool isprivate, string ID, bool doinsert)
		{
			bool Insert = false;
			string SqlQuery = "";
			try
			{
				if (doinsert)
				{
					SqlQuery = "INSERT INTO UserCustomPages (UserID,URL,Title,IsPrivate) VALUES('" + userID + "','" + URLval + "','" + titleval + "','" + isprivate + "')";
				}
				else
				{
					SqlQuery = "UPDATE UserCustomPages SET URL='" + URLval + "', Title='" + titleval + "', IsPrivate='" + isprivate + "' " +
						"WHERE UserID=" + userID + " AND ID=" + ID;
				}
				Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return Insert;
		}
		public DataTable GetCustomPageValue(string userID)
		{
			DataTable dt = new DataTable();
			try
			{
				string query = "SELECT URL FROM UserCustomPages WHERE UserID=" + userID;
				dt = objAdaptor.FetchData(query);
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
				string SqlQuery = "SELECT ID FROM O365Server WHERE Name='" + serverName + "'";
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
		public DataTable GetCloudData()
		{
			DataTable dt = new DataTable();
			try
			{
				//string Query = "select cd.Name, cd.Imageurl, cd.URL,st.StatusCode,st.Lastupdate from O365Server cd, Status st where st.TypeANDName= cd.Name+'-Cloud'";
				string Query = "select nd.Name, nd.ImageURL, st.StatusCode,st.Lastupdate from [O365Server] nd, Status st,Users us where nd.UserName=us.LoginName and st.TypeANDName= nd.Name+'-Cloud' and us.CloudApplications='true'";

				dt = objAdaptor.FetchData(Query);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}
		public DataTable GetCloudDatavisible()
		{
			DataTable dt = new DataTable();
			try
			{
				//string Query = "select cd.Name, cd.Imageurl, cd.URL,st.StatusCode,st.Lastupdate from O365Server cd, Status st where st.TypeANDName= cd.Name+'-Cloud'";
				string Query = "select nd.Name, nd.ImageURL, st.StatusCode,st.Lastupdate from [O365Server] nd, Status st,Users us where nd.UserName=us.LoginName and st.TypeANDName= nd.Name+'-Cloud' ";

				dt = objAdaptor.FetchData(Query);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}
		public DataTable GetCloudStatuses()
		{
			DataTable dt = new DataTable();
			try
			{
				//string Query = "select cd.Name, cd.Imageurl, cd.URL,st.StatusCode,st.Lastupdate from O365Server cd, Status st where st.TypeANDName= cd.Name+'-Cloud'";
				string Query = "select nd.Name, nd.ImageURL, st.StatusCode,st.Lastupdate from [O365Server] nd, Status st where st.TypeANDName= nd.Name+'-Cloud' ";

				dt = objAdaptor.FetchData(Query);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}
		public DataTable GetTestsTab(int ID)
		{
			DataTable TestsDetails = new DataTable();
			try
			{
				string sqlQuery;
				//if (ID == 0)
				//{
				sqlQuery = "(Select Id,Type,Tests,EnableSimulationTests,ResponseThreshold,ServerId from Office365Tests where ServerId='" + ID + "' union Select f1.Id, f1.Type,f1.Tests,convert(bit,'false') as EnableSimulationTests,'' as ResponseThreshold,'' as ServerId from  TestsMaster f1 Left outer join Office365Tests f2 on f1.Id=f2.Id where f1.ServerType=21 and f1.Tests not in (Select Tests from Office365Tests where ServerId='" + ID + "')) order by Tests ";


				TestsDetails = objAdaptor.FetchData(sqlQuery);
				if (TestsDetails.Rows.Count == 0)
				{

					sqlQuery = "Select f1.Id, f1.Type,f1.Tests,convert(bit,'false') as EnableSimulationTests,'' as ResponseThreshold,'' as ServerId from  TestsMaster f1 Left outer join Office365Tests f2 on f1.Id=f2.Id where f1.ServerTypeId=21 order by f1.Tests ";
					TestsDetails = objAdaptor.FetchData(sqlQuery);
				}
				//}
				//else
				//{


				//}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return TestsDetails;
		}
		public DataTable GetTestsTabadd()
		{
			string sqlQuery;
			DataTable TestsDetails = new DataTable();
			try
			{

				//if(mode=="Update")
				//{
				//sqlQuery = "Select f1.Id, f1.Type,f1.Tests,f2.EnableSimulationTests as EnableSimulationTests,f2.ResponseThreshold as ResponseThreshold,f2.ServerId as ServerId from  TestsMaster f1 Left outer join Office365Tests f2 on f1.Id=f2.Id  order by f1.Tests";
				//TestsDetails = objAdaptor.FetchData(sqlQuery);
				//}
				//else
				//{
				sqlQuery = "Select distinct f1.Id, f1.Type,f1.Tests,convert(bit,'false') as EnableSimulationTests,'' as ResponseThreshold,'' as ServerId from  TestsMaster f1 Left outer join Office365Tests f2 on f1.Id=f2.Id  where f1.ServerType=21 order by f1.Tests";//Somaraj
				TestsDetails = objAdaptor.FetchData(sqlQuery);

				//}



			}
			catch (Exception ex)
			{
				throw ex;
			}
			return TestsDetails;
		}
		public DataTable GetNodesTabadd(string id)
		{
			DataTable NodesDetails = new DataTable();
			try
			{
				string sqlQuery = "";
				sqlQuery += "select distinct n.Id,N.Name,N.HostName,Ln.Location ,'true'   as SelectedNodes from ";
				sqlQuery += "Nodes N inner join O365Nodes O on N.ID=O.NodeID left outer join Locations as Ln on Ln.ID=N.LocationID where O.O365ServerID =" + id;
				sqlQuery += "union all ";
				sqlQuery += "select distinct n.Id,N.Name,N.HostName,Ln.Location ,'false' SelectedNodes from ";
				sqlQuery += "Nodes N  left outer join Locations as Ln on Ln.ID=N.LocationID where N.ID not in(select nodeid from O365Nodes where O365ServerID =" + id + ") ";

				//sqlQuery = "Select O.SelectedNodes,N.ID,N.Name,N.HostName,Ln.Location from O365Nodes as O left outer join Nodes as N on O.NodeID=N.ID  inner join Locations as Ln on Ln.ID=N.LocationID";
				//sqlQuery = "Select N.ID,convert(bit,'false') as SelectedNodes,N.Name,N.HostName,Ln.Location from O365Nodes as O left outer join Nodes as N on O.NodeID=N.ID  inner join Locations as Ln on Ln.ID=N.LocationID";
				//sqlQuery = "select n.Id,O.O365ServerID,N.Name,N.HostName,Ln.Location ,O.NodeID,convert(bit,'false') as SelectedNodes from Nodes N left outer join O365Nodes O on N.ID=O.NodeID inner join Locations as Ln on Ln.ID=N.LocationID ";
				NodesDetails = objAdaptor.FetchData(sqlQuery);


			}
			catch (Exception ex)
			{
				throw ex;
			}
			return NodesDetails;
		}
		public DataTable GetO365Nodes()
		{
			DataTable NodesDetails = new DataTable();
			try
			{
				string sqlQuery;


				sqlQuery = "Select * from O365Nodes";
				NodesDetails = objAdaptor.FetchData(sqlQuery);


			}
			catch (Exception ex)
			{
				throw ex;
			}
			return NodesDetails;
		}
		public DataTable GetNodes()
		{
			DataTable NodesDetails = new DataTable();
			try
			{
				string sqlQuery;

				//sqlQuery = "Select O.SelectedNodes,N.ID,N.Name,N.HostName,Ln.Location from O365Nodes as O left outer join Nodes as N on O.NodeID=N.ID  inner join Locations as Ln on Ln.ID=N.LocationID";
				//sqlQuery = "select n.Id,O.O365ServerID,N.Name,N.HostName,Ln.Location ,O.NodeID,case when O.NodeID is null then 'false' else 'true' end  as SelectedNodes from Nodes N left outer join O365Nodes O on N.ID=O.NodeID inner join Locations as Ln on Ln.ID=N.LocationID";

				sqlQuery = "select n.Id,O.O365ServerID,N.Name,N.HostName,Ln.Location ,O.NodeID,case when O.NodeID is null then 'false' else 'true' end  as SelectedNodes from Nodes N left outer join O365Nodes O on N.ID=O.NodeID left outer join Locations as Ln on Ln.ID=N.LocationID";

				//sqlQuery = "Select * from Nodes";
				NodesDetails = objAdaptor.FetchData(sqlQuery);


			}
			catch (Exception ex)
			{
				throw ex;
			}
			return NodesDetails;
		}
		public Boolean UpdateTests(int ID, List<object> fieldValues)
		{
			bool Update = false;

			try
			{
				string SqlQuery;
				string sqlCheck;
				string sqlquery;

				SqlQuery = "UPDATE Office365Tests set  EnableSimulationTests='False'  WHERE ServerId='" + ID + "'";

				Update = objAdaptor.ExecuteNonQuery(SqlQuery);

				for (int i = 0; i < fieldValues.Count; i++)
				{


					sqlCheck = "Select * from Office365Tests where ServerId= " + ID + " and tests='" + ((object[])(fieldValues[i]))[3] + "'";
					DataTable dt = objAdaptor.FetchData(sqlCheck);

					if (dt.Rows.Count == 0)
					{
						DataTable id = new DataTable();


						sqlquery = "Select * from TestsMaster where ServerType=21 and Tests='" + ((object[])(fieldValues[i]))[3] + "'";
						id = objAdaptor.FetchData(sqlquery);
						string SqlQuery1 = "INSERT INTO Office365Tests (Id,ServerId,Tests,EnableSimulationTests,ResponseThreshold,Type) VALUES(" + Convert.ToInt32(id.Rows[0]["id"]) + "," + ID + ",'" + ((object[])(fieldValues[i]))[3] + "','" + ((object[])(fieldValues[i]))[1] + "'," + ((object[])(fieldValues[i]))[2] + ",'" + ((object[])(fieldValues[i]))[4] + "')";
						objAdaptor.ExecuteNonQuery(SqlQuery1);
					}

				}

				//SqlQuery = "UPDATE Office365Tests set  EnableSimulationTests='False' WHERE ServerId='" + ID + "'";
				//Update = objAdaptor.ExecuteNonQuery(SqlQuery);
				for (int i = 0; i < fieldValues.Count; i++)
				{

					int threshold = Convert.ToInt32(((object[])(fieldValues[i]))[2]);
					//SqlQuery = "UPDATE Office365Tests set  EnableSimulationTests='True' WHERE Id='" + ((object[])(fieldValues[i]))[0] + "'";
					SqlQuery = "UPDATE Office365Tests set  EnableSimulationTests='True' WHERE ServerId='" + ID + "' and  Tests = '" + ((object[])(fieldValues[i]))[3] + "'";
					Update = objAdaptor.ExecuteNonQuery(SqlQuery);
					//SqlQuery = "UPDATE Office365Tests set EnableSimulationTests='" + ((object[])(fieldValues[i]))[1] + "', ResponseThreshold='" + threshold + "' WHERE Id=" + ((object[])(fieldValues[i]))[0];
					SqlQuery = "UPDATE Office365Tests set EnableSimulationTests='" + ((object[])(fieldValues[i]))[1] + "', ResponseThreshold='" + threshold + "' WHERE ServerId='" + ID + "' and Tests = '" + ((object[])(fieldValues[i]))[3] + "'";
					Update = objAdaptor.ExecuteNonQuery(SqlQuery);
				}
			}
			catch
			{
				Update = false;

			}
			//bool Insert = false;
			//if (Update == false)
			//{
			//    try
			//    {


			//        for (int i = 0; i < fieldValues.Count; i++)
			//        {

			//            string SqlQuery1 = "INSERT INTO Office365Tests (ServerId,Tests,EnableSimulationTests,ResponseThreshold,Type) VALUES(" + ID + ",'" + ((object[])(fieldValues[i]))[3] + "','" + ((object[])(fieldValues[i]))[1] + "'," + ((object[])(fieldValues[i]))[2] + ",'" + ((object[])(fieldValues[i]))[4] + "')";
			//            Insert = objAdaptor.ExecuteNonQuery(SqlQuery1);
			//        }
			//    }

			//    catch
			//    {
			//        Insert = false;
			//    }
			//}

			return Update;
		}
		public Boolean UpdateNodes(int ID, List<object> fieldValues)
		{
			bool Update = false;

			try
			{
				string SqlQuery;
				string sqlCheck;
				SqlQuery = "delete from  O365Nodes  WHERE  O365ServerId='" + ID + "'";
				Update = objAdaptor.ExecuteNonQuery(SqlQuery);
				for (int i = 0; i < fieldValues.Count; i++)
				{

					string SqlQuery1 = "INSERT INTO O365Nodes (O365ServerID, NodeID) VALUES(" + ID + ",'" + ((object[])(fieldValues[i]))[0] + "')";
					objAdaptor.ExecuteNonQuery(SqlQuery1);
				}


				//for (int i = 0; i < fieldValues.Count; i++)
				//{

				//    //int threshold = Convert.ToInt32(((object[])(fieldValues[i]))[2]);
				//    SqlQuery = "UPDATE O365Nodes set  SelectedNodes='True' WHERE NodeID='" + ((object[])(fieldValues[i]))[0] + "'";
				//    Update = objAdaptor.ExecuteNonQuery(SqlQuery);
				//    SqlQuery = "UPDATE O365Nodes set SelectedNodes='" + ((object[])(fieldValues[i]))[1] + "' WHERE NodeID=" + ((object[])(fieldValues[i]))[0];
				//    Update = objAdaptor.ExecuteNonQuery(SqlQuery);
				//}
			}
			catch
			{
				Update = false;

			}


			return Update;
		}
		//public bool InsertDatafortests(List<object> fieldValues,string servername)
		//{
		//    string sqlQuery;
		//    bool Insert = false;
		//    try
		//    {
		//        DataTable TestsDetails = new DataTable();
		//        DataTable TestsData = new DataTable();

		//        sqlQuery = "Select id from O365Server where name='" + servername + "'";
		//        TestsDetails = objAdaptor.FetchData(sqlQuery);

		//        if (TestsDetails.Rows.Count > 0)
		//        {
		//            for (int i = 0; i < fieldValues.Count; i++)
		//            {
		//                //sqlQuery = "Select Id from Office365Tests where Tests='" + ((object[])(fieldValues[i]))[3] + "' and ServerId="+ Convert.ToInt32(TestsDetails.Rows[0]["id"]+"");
		//                //TestsData = objAdaptor.FetchData(sqlQuery);
		//                //if (TestsData.Rows.Count > 0)
		//                //{
		//                //    string SqlQuery = "UPDATE Office365Tests set EnableSimulationTests='" + ((object[])(fieldValues[i]))[1] + "', ResponseThreshold='" + ((object[])(fieldValues[i]))[2] + "' WHERE Id=" + ((object[])(fieldValues[i]))[0];
		//                //    Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
		//                //}
		//                //string enable = Convert.ToString ((object[])(fieldValues[i]))[2].ToString();
		//                    //string SqlQuery = "INSERT into Office365Tests (Id,ServerId,Tests,EnableSimulationTests,ResponseThreshold,Type)" +//somaraj
		//                    //       "VALUES(" + ((object[])(fieldValues[i]))[0] + "," + Convert.ToInt32(TestsDetails.Rows[0]["id"]) + ", '" + ((object[])(fieldValues[i]))[3] + "','" + ((object[])(fieldValues[i]))[1] + "','" + Convert.ToString( ((object[])(fieldValues[i]))[2]) + "','" + ((object[])(fieldValues[i]))[4] + "')";
		//                string SqlQuery = "INSERT INTO Office365Tests (Id,ServerId,Tests,EnableSimulationTests,ResponseThreshold,Type) VALUES(" + Convert.ToInt32(TestsDetails.Rows[0]["id"]) + "," + Convert.ToInt32(TestsDetails.Rows[0]["id"]) + ",'" + ((object[])(fieldValues[i]))[3] + "','" + ((object[])(fieldValues[i]))[1] + "'," + ((object[])(fieldValues[i]))[2] + ",'" + ((object[])(fieldValues[i]))[4] + "')";
		//                    Insert = objAdaptor.ExecuteNonQuery(SqlQuery);


		//            }
		//        }
		//    }
		//    catch
		//    {
		//        Insert = false;
		//    }
		//    finally
		//    {
		//    }
		//    return Insert;
		//}
		public bool InsertDatafortests(List<object> fieldValues, string servername)
		{
			string sqlQuery;
			bool Insert = false;
			try
			{
				DataTable TestsDetails = new DataTable();
				DataTable TestsData = new DataTable();

				sqlQuery = "Select id from O365Server where name='" + servername + "'";
				TestsDetails = objAdaptor.FetchData(sqlQuery);

				if (TestsDetails.Rows.Count > 0)
				{
					for (int i = 0; i < fieldValues.Count; i++)
					{
						//sqlQuery = "Select Id from Office365Tests where Tests='" + ((object[])(fieldValues[i]))[3] + "' and ServerId="+ Convert.ToInt32(TestsDetails.Rows[0]["id"]+"");
						//TestsData = objAdaptor.FetchData(sqlQuery);
						//if (TestsData.Rows.Count > 0)
						//{
						//    string SqlQuery = "UPDATE Office365Tests set EnableSimulationTests='" + ((object[])(fieldValues[i]))[1] + "', ResponseThreshold='" + ((object[])(fieldValues[i]))[2] + "' WHERE Id=" + ((object[])(fieldValues[i]))[0];
						//    Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
						//}

						string SqlQuery = "INSERT into Office365Tests (Id,ServerId,Tests,EnableSimulationTests,ResponseThreshold,Type)" +//somaraj
							   "VALUES(" + ((object[])(fieldValues[i]))[0] + "," + Convert.ToInt32(TestsDetails.Rows[0]["id"]) + ", '" + ((object[])(fieldValues[i]))[3] + "','" + ((object[])(fieldValues[i]))[1] + "','" + ((object[])(fieldValues[i]))[2] + "','" + ((object[])(fieldValues[i]))[4] + "')";

						Insert = objAdaptor.ExecuteNonQuery(SqlQuery);


					}
				}
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
		public bool UpdateDatafortestsnew(Office365Tests o365tests)
		{
			string sqlQuery;
			bool Update = false;
			DataTable testdatatable = new DataTable();
			try
			{
				sqlQuery = "select Tests from Office365Tests where ServerId='" + o365tests.ServerId + "' and Tests= '" + o365tests.Tests + "' ";
				testdatatable = objAdaptor.FetchData(sqlQuery);
				if (testdatatable.Rows.Count > 0)
				{
					sqlQuery = "UPDATE Office365Tests set  ServerId='" + o365tests.ServerId + "', EnableSimulationTests= '" + o365tests.EnableSimulationTests + "',ResponseThreshold=" + o365tests.ResponseThreshold + " ,Type='" + o365tests.Type + "',Tests='" + o365tests.Tests + "' WHERE ServerId='" + o365tests.ServerId + "' and Tests='" + o365tests.Tests + "'";

					Update = objAdaptor.ExecuteNonQuery(sqlQuery);
				}
				else
				{
					string SqlQuery = "INSERT into Office365Tests (ServerId,Tests,EnableSimulationTests,ResponseThreshold,Type)" +//somaraj
							   "VALUES(" + o365tests.ServerId + ",'" + o365tests.Tests + "', '" + o365tests.EnableSimulationTests + "','" + o365tests.ResponseThreshold + "','" + o365tests.Type + "')";
					Update = objAdaptor.ExecuteNonQuery(SqlQuery);
				}
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
		public bool InsertDatafortestsnew(Office365Tests o365tests)
		{
			string sqlQuery;
			bool Insert = false;
			try
			{
				DataTable TestsDetails = new DataTable();
				DataTable TestsData = new DataTable();

				sqlQuery = "Select * from O365Server where ID='" + o365tests.ServerId + "'";
				TestsDetails = objAdaptor.FetchData(sqlQuery);

				if (TestsDetails.Rows.Count > 0)
				{
					//for (int i = 0; i < fieldValues.Count; i++)
					//{
					//sqlQuery = "Select Id from Office365Tests where Tests='" + ((object[])(fieldValues[i]))[3] + "' and ServerId="+ Convert.ToInt32(TestsDetails.Rows[0]["id"]+"");
					//TestsData = objAdaptor.FetchData(sqlQuery);
					//if (TestsData.Rows.Count > 0)
					//{
					//    string SqlQuery = "UPDATE Office365Tests set EnableSimulationTests='" + ((object[])(fieldValues[i]))[1] + "', ResponseThreshold='" + ((object[])(fieldValues[i]))[2] + "' WHERE Id=" + ((object[])(fieldValues[i]))[0];
					//    Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
					//}

					//string SqlQuery = "INSERT into Office365Tests (Id,ServerId,Tests,EnableSimulationTests,ResponseThreshold,Type)" +//somaraj
					//       "VALUES(" + ((object[])(fieldValues[i]))[0] + "," + Convert.ToInt32(TestsDetails.Rows[0]["id"]) + ", '" + ((object[])(fieldValues[i]))[3] + "','" + ((object[])(fieldValues[i]))[1] + "','" + ((object[])(fieldValues[i]))[2] + "','" + ((object[])(fieldValues[i]))[4] + "')";
					string SqlQuery = "INSERT into Office365Tests (ServerId,Tests,EnableSimulationTests,ResponseThreshold,Type)" +//somaraj
							   "VALUES(" + o365tests.ServerId + ",'" + o365tests.Tests + "', '" + o365tests.EnableSimulationTests + "','" + o365tests.ResponseThreshold + "','" + o365tests.Type + "')";
					Insert = objAdaptor.ExecuteNonQuery(SqlQuery);


					//}
				}
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
		public bool InsertDataforNodes(List<object> fieldValues, string servername)
		{
			bool Insert = false;
			try
			{
				DataTable NodesDetails = new DataTable();
				string sqlQuery;
				sqlQuery = "Select id from O365Server where name='" + servername + "'";
				NodesDetails = objAdaptor.FetchData(sqlQuery);
				//int NodeID = 6;
				if (NodesDetails.Rows.Count > 0)
				{
					for (int i = 0; i < fieldValues.Count; i++)
					{

						string SqlQuery = "INSERT INTO O365Nodes (O365ServerID,NodeID)" +
							"VALUES(" + Convert.ToInt32(NodesDetails.Rows[0]["id"]) + "," + ((object[])(fieldValues[i]))[0] + ")";



						Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
					}
				}
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
	}
}
