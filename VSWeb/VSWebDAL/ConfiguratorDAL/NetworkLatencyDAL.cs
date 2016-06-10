using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.ConfiguratorDAL
{
   public class NetworkLatencyDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static NetworkLatencyDAL _self = new NetworkLatencyDAL();

        public static NetworkLatencyDAL Ins
        {
            get { return _self; }
        }
        /// <summary>
        /// Get all Data from DominoCluster
        /// </summary>

        public DataTable GetAllData()
        {

            DataTable networklatencyDataTable = new DataTable();
            NetworkLatency ReturnDCObject = new NetworkLatency();
            try
            {
                string SqlQuery = "SELECT ID,Enabled,Name,ScanInterval,TestDuration FROM NetworkLatency";

                networklatencyDataTable = objAdaptor.FetchData(SqlQuery);

            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return networklatencyDataTable;
        }
        /// <summary>
        /// Get Data from DominoServers based on Key
        /// </summary>
        public NetworkLatency GetData(NetworkLatency NLObject)
        {
            DataTable networklatencyDataTable = new DataTable();
            NetworkLatency ReturnNLObject = new NetworkLatency();
            try
            {
                string SqlQuery = "Select * from NetworkLatency where [NetworkLatencyId]=" + NLObject.NetworkLatencyId.ToString();
                networklatencyDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object

                ReturnNLObject.TestName = networklatencyDataTable.Rows[0]["TestName"].ToString();
               
                if (networklatencyDataTable.Rows[0]["ScanInterval"].ToString() != "")
                    ReturnNLObject.ScanInterval = int.Parse(networklatencyDataTable.Rows[0]["ScanInterval"].ToString());
                if (networklatencyDataTable.Rows[0]["TestDuration"].ToString() != "")
                    ReturnNLObject.TestDuration = int.Parse(networklatencyDataTable.Rows[0]["TestDuration"].ToString());
                //ReturnNLObject.ServerType = networklatencyDataTable.Rows[0]["ServerType"].ToString();
               
              
                // AdvNtwrkConCheckBox.Checked = (AdvIPAddressTextBox.Text != "" ? true : false);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return ReturnNLObject;
        }
        /// <summary>
        /// Insert data into DominoCluster table
        /// </summary>
        /// <param name="DSObject">DominoServers object</param>
        /// <returns></returns>

        public bool InsertData(NetworkLatency NLObject)
        {
            bool Insert = false;
            try
            {
                string SqlQuery = "INSERT INTO NetworkLatency ([TestName] ,[ScanInterval],[TestDuration],Enable) " +
                    "VALUES('" + NLObject.TestName + "'," + NLObject.ScanInterval +
                    "," + NLObject.TestDuration + ",'"+NLObject.Enable+"')";


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


        public Object DeleteNetworkLatencyServers(string id)
        {
            Object Update;
            try
            {
                string SqlQuery = "Delete NetworkLatencyServers Where NetworkLatencyId=" + id;
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

        public Object DeleteData(string id)
        {
            Object Update;
            try
            {
                //10/20/2015 NS modified for VSPLUS-2222
                DeleteNetworkLatencyServers(id);
                string SqlQuery = "DELETE FROM NetworkLatency WHERE NetworkLatencyId=" + id;
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
        /// Update data into DominoCluster table
        /// </summary>
        /// <param name="DSObject">DominoServers object</param>
        /// <returns></returns>
        public Object UpdateData(NetworkLatency NLObject)
        {
            Object Update;
            try
            {
                string SqlQuery = "UPDATE NetworkLatency SET TestName='" + NLObject.TestName + "', " +
                    "ScanInterval='" + NLObject.ScanInterval + "', " +
                    "TestDuration='" + NLObject.TestDuration + "' ,Enable='"+NLObject.Enable+"'" +
                    "WHERE NetworkLatencyId = " + NLObject.NetworkLatencyId + "";

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

       
        public DataTable Getvalue()
        {
            
            DataTable settingsdatatable = new DataTable();
            try
            {
                string SqlQuery = "select * from NetworkLatency";
                settingsdatatable = objAdaptor.FetchData(SqlQuery);
               
            }
			catch (Exception ex)
			{
				throw ex;
			}

            return settingsdatatable;
        }


        public DataTable Getname(string name)
        {

            DataTable settingsdatatable = new DataTable();
            try
            {
                string SqlQuery = "select TestName from NetworkLatency where TestName='"+name+"'";
                settingsdatatable = objAdaptor.FetchData(SqlQuery);

            }
			catch (Exception ex)
			{
				throw ex;
			}
            return settingsdatatable;
        }
        public DataTable Getvalue1(int id)
        {

            DataTable settingsdatatable = new DataTable();
            try
            {
                string SqlQuery = "select * from NetworkLatency where NetworkLatencyId='" + id + "'";
                settingsdatatable = objAdaptor.FetchData(SqlQuery);

            }
			catch (Exception ex)
			{
				throw ex;
			}

            return settingsdatatable;
        }

        public DataTable GetAllData1(string servertype)
        {
            DataTable MSServersDataTable = new DataTable();

            try
            {
                //3/21/2014 NS modified the query - need to add locationid
                //string SqlQuery = "select Sr.ID,Sr.ServerName,S.ServerType,L.Location,sa.ScanInterval,sa.Enabled,sr.ipaddress,sa.category from Servers Sr inner join ServerTypes S on Sr.ServerTypeID=S.ID " +
                //             " inner join Locations L on Sr.LocationID =L.ID left outer join ServerAttributes sa on sr.ID=sa.serverid  where S.ServerType='Exchange' ";
                string SqlQuery = "select s.ID,s.ServerName,t2.Location,  n.Enabled,n.LatencyYellowThreshold,n.LatencyRedThreshold from servers s Left Outer Join NetworkLatencyServers n on s.ID=n.ServerID INNER JOIN [ServerTypes] t3 ON [ServerTypeID] = t3.[ID] INNER JOIN [Locations] t2 ON [LocationID] = t2.[ID]   where  t3.ServerType='" + servertype + "'";
                MSServersDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return MSServersDataTable;
        }


        public DataTable GetAllData2(int serverkey, string page, string Control)
        {
            DataTable MSServersDataTable = new DataTable();

            try
            {
                //10/21/2015 NS modified for VSPLUS-2223
                string SqlQuery1 = "select ID,ServerTypeId,ServerType,ServerName,Location,cast('false' as bit) as Enabled,LatencyRedThreshold,LatencyYellowThreshold from (select pm.ServerTypeId,s.ID,s.ServerName,t3.ServerType,t2.Location, pm.AttributeName,pm.DefaultValue from servers s, [ServerTypes] t3 ,[Locations] t2 ,ProfilesMaster pm where s.[ServerTypeID] = t3.[ID] and  s.[LocationID] = t2.[ID] and pm.ServerTypeId=t3.ID and t3.ServerType in (select ServerType from ServerTypes where id   in(select ServertypeID from  Servertypeexcludelist where Page='" + page + "'and Control='" + Control + "'))   and pm.AttributeName in ('LatencyYellowThreshold','LatencyRedThreshold')) as query PIVOT (MAX(DefaultValue) FOR AttributeName IN (LatencyYellowThreshold,LatencyRedThreshold)) AS Pivot0 GROUP BY ID,ServerTypeId,ServerType,ID,ServerName,Location,LatencyYellowThreshold,LatencyRedThreshold order by ServerName";
                MSServersDataTable = objAdaptor.FetchData(SqlQuery1);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return MSServersDataTable;
        }

       //10/21/2015 NS added for VSPLUS-2223
        public DataTable GetSelectedServers(int serverkey, string page, string Control)
        {
            DataTable MSServersDataTable = new DataTable();
            try
            {
                if (serverkey != 0)
                {
                    string SqlQuery = "select ID,ServerTypeId,ServerType,ServerName,Location,Enabled,LatencyYellowThreshold, LatencyRedThreshold from (select pm.ServerTypeId,s.ID,s.ServerName,n.Enabled,t3.ServerType,t2.Location, pm.AttributeName,pm.DefaultValue,n.LatencyYellowThreshold,n.LatencyRedThreshold  from servers s, [ServerTypes] t3 ,[Locations] t2 ,ProfilesMaster pm,NetworkLatencyServers n where s.[ServerTypeID] = t3.[ID] and  s.[LocationID] = t2.[ID] and pm.ServerTypeId=t3.ID and s.ID=n.ServerID and n.NetworkLatencyId='" + serverkey + "' and t3.ServerType in (select ServerType from ServerTypes where id   in(select ServertypeID from  Servertypeexcludelist where Page='" + page + "'and Control='" + Control + "'))  and pm.AttributeName in ('LatencyYellowThreshold','LatencyRedThreshold')) as query  GROUP BY ID,ServerTypeId,ServerType,ID,ServerName,enabled,Location,LatencyYellowThreshold,LatencyRedThreshold order by ServerName";
                    MSServersDataTable = objAdaptor.FetchData(SqlQuery);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return MSServersDataTable;
        }

        public object updatenwEnableLatencyTest(int id, int nlid, int latencyred, int yellowthershold, bool checkedvalue,string testname)
        {
            //ExchangeServers - ServerId, LatencyEnable, Red,Yellow
             bool UpdateRet = false;
            int Update = 0;
            try
            {
                DataTable dt = new DataTable();
                ServerTypes STypebject = new ServerTypes();
                ServerTypes RetnSTypebject = new ServerTypes();
               
               // RetnSTypebject = VSWebDAL.SecurityDAL.ServerTypesDAL.Ins.GetDataForServerType(STypebject);
              //  StObject.ServerTypeId = RetnSTypebject.ID;
                if (id != 0)
                {
                    string SqlQuery = "";
                    //if (StObject.RoleType == null)
                    //{
                    //    StObject.RoleType = "";
                    //}
                    SqlQuery = "select * from NetworkLatencyServers where ServerID='" + id + "' ";
                    dt = objAdaptor.FetchData(SqlQuery);
                    if (dt.Rows.Count > 0)
                    {
                        SqlQuery = "update NetworkLatencyServers set LatencyYellowThreshold=" + yellowthershold + ", LatencyRedThreshold=" + latencyred + ",Enabled='" + checkedvalue + "'  where ServerID=" + id;

                        Update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
                    }

                }
                //foreach (ExchangeSettings fieldValue in fieldValues)
                //{
              
                //}
            }
            catch
            {
                Update = 0;
            }
            if (Update == 0)
            {
                try
                {
                    string SqlQuery = "";
                    DataTable networklatency = new DataTable();
                    string sqlQuery;
                    sqlQuery = "select [NetworkLatencyId] from [NetworkLatency] where [TestName]='" + testname + "'";
                    networklatency = objAdaptor.FetchData(sqlQuery);

                    if (networklatency.Rows.Count > 0)
                    {
                        SqlQuery = "Insert into NetworkLatencyServers(ServerID,NetworkLatencyId,LatencyYellowThreshold , LatencyRedThreshold , Enabled) values" +
                            //"ActiveSyncCredentialsId) values" +
                           "(" + id + ", " + Convert.ToInt32(networklatency.Rows[0]["NetworkLatencyId"]) + ",'" +
                           yellowthershold + "','" + latencyred + "','" + checkedvalue + "')";
                        Update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
                    }
                }
                catch
                {
                    Update = 0;
                }
            }
            if (Update == 1)
            {
                UpdateRet = true;
            }
            return UpdateRet;
        }

        public bool Updatelatency(NetworkLatency StObject, string id,string name)
        {
            DataTable dt = new DataTable();
            bool UpdateRet = false;
            int Update = 0;
            try
            {

                if (id != "")
                {
                    string SqlQuery = "";
                    //if (StObject.RoleType == null)
                    //{
                    //    StObject.RoleType = "";
                    //}
                    //10/21/2015 NS modified for VSPLUS-2223
                    /*
                    SqlQuery = "select * from NetworkLatencyServers where ServerID='" + id +  "' and [NetworkLatencyId]='"+StObject.NetworkLatencyId+"' ";
                    dt = objAdaptor.FetchData(SqlQuery);
                    if (dt.Rows.Count > 0)
                    {
                        SqlQuery = "update NetworkLatencyServers set LatencyYellowThreshold=" + StObject.LatencyYellowThreshold + ", LatencyRedThreshold=" + StObject.LatencyRedThreshold + ",Enabled='" + StObject.Enabled + "'  where ServerID=" + id;

                        Update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
                    }
                     */
                    if (StObject.Enabled)
                    {
                        SqlQuery = "INSERT INTO NetworkLatencyServers(ServerID,NetworkLatencyId,LatencyYellowThreshold,LatencyRedThreshold,Enabled) VALUES" +
                            "(" + id + ", " + StObject.NetworkLatencyId + ",'" +
                            StObject.LatencyYellowThreshold + "','" + StObject.LatencyRedThreshold + "','" + StObject.Enabled + "')";
                        Update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
                    }
                }
            }
            catch
            {
                Update = 0;
            }
            if (Update == 0)
            {
                try
                {
                    string SqlQuery = "";
                    DataTable networklatency = new DataTable();
                    string sqlQuery;
                    sqlQuery = "select [NetworkLatencyId] from [NetworkLatency] where [TestName]='" + name + "'";
                    networklatency = objAdaptor.FetchData(sqlQuery);

                    if (networklatency.Rows.Count > 0)
                    {
                        //10/21/2015 NS modified for VSPLUS-2223
                        if (StObject.Enabled)
                        {
                            SqlQuery = "Insert into NetworkLatencyServers(ServerID,NetworkLatencyId,LatencyYellowThreshold , LatencyRedThreshold , Enabled) values" +
                                //"ActiveSyncCredentialsId) values" +
                           "(" + id + ", " + Convert.ToInt32(networklatency.Rows[0]["NetworkLatencyId"]) + ",'" +
                           StObject.LatencyYellowThreshold + "','" + StObject.LatencyRedThreshold + "','" + StObject.Enabled + "')";
                            Update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
                        }
                    }
                }
                catch
                {
                    Update = 0;
                }
            }
            if (Update == 1)
            {
                UpdateRet = true;
            }
            return UpdateRet;
        }



        //public Object UpdateExchangeSettingsData(NetworkLatency nl)
        //{
        //    Object returnval;

        //    DataTable dt = GetExchangeSettings(nl);
        //    if (dt.Rows.Count == 0)
        //    {
        //        returnval = InsertExchangeSettings(nl);
        //    }
        //    else
        //    {
        //        returnval = UpdateExchangeSettings(nl);

        //    }
        //    return returnval;
        //}
        //public DataTable GetExchangeSettings(NetworkLatency nl)
        //{
        //    DataTable MSServersDataTable = new DataTable();

        //    try
        //    {
        //        string SqlQuery = "select * from   NetworkLatencyServers where ID='" + nl.ID+"'";
        //        MSServersDataTable = objAdaptor.FetchData(SqlQuery);
        //    }
        //    catch
        //    {
        //    }
        //    finally
        //    {
        //    }
        //    return MSServersDataTable;
        //}

        public Object insertnetworklatency(int id, int nlid, int latencyred, int yellowthershold, bool checkedvalue, string testname)
        {
            bool Insert = false;
            try
                {
                    string SqlQuery = "";
                    DataTable networklatency = new DataTable();
                    string sqlQuery;
                    sqlQuery = "select [NetworkLatencyId] from [NetworkLatency] where [TestName]='" + testname + "'";
                    networklatency = objAdaptor.FetchData(sqlQuery);

                    if (networklatency.Rows.Count > 0)
                    {

                        SqlQuery = "Insert into NetworkLatencyServers(ServerID,NetworkLatencyId,LatencyYellowThreshold , LatencyRedThreshold , Enabled) values" +
                            //"ActiveSyncCredentialsId) values" +
                       "(" + id + ", " + Convert.ToInt32(networklatency.Rows[0]["NetworkLatencyId"]) + ",'" +
                       yellowthershold + "','" + latencyred + "','" + checkedvalue + "')";
                        Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
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
            

        

        //public Object UpdateExchangeSettings(NetworkLatency nl)
        //{
        //    Object returnval;
        //    try
        //    {

        //        string st = "update NetworkLatencyServers set LatencyYellowThreshold=" + nl.LatencyYellowThreshold + ", LatencyRedThreshold=" + nl.LatencyRedThreshold + ",Enabled='" + nl.Enabled + "'  where ID='"+nl.ID+"'";
        //        returnval = objAdaptor.ExecuteNonQuery(st);
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //    return returnval;

        //}

        public bool UpdateeditProfiles(NetworkLatency StObject, string strsname)
        {
            bool UpdateRet = false;
            int Update = 0;
            try
            {
                DataTable dt = new DataTable();
                ServerTypes STypebject = new ServerTypes();
               
               // RetnSTypebject = VSWebDAL.SecurityDAL.ServerTypesDAL.Ins.GetDataForServerType(STypebject);
              //  StObject.ServerTypeId = RetnSTypebject.ID;
                if (strsname != "")
                {
                    string SqlQuery = "";
                    //if (StObject.RoleType == null)
                    //{
                    //    StObject.RoleType = "";
                    //}
                    SqlQuery = "select * from NetworkLatencyServers where ServerID='" + strsname + "' ";
                    dt = objAdaptor.FetchData(SqlQuery);
                    if (dt.Rows.Count > 0)
                    {
                        SqlQuery = "UPDATE NetworkLatencyServers set Enabled='" + StObject.Enabled + "',LatencyRedThreshold='" + StObject.LatencyRedThreshold + "',LatencyYellowThreshold='" + StObject.LatencyYellowThreshold +
                            "'where ServerID='" + strsname + "'";
                        Update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
                    }
                }
            }
            catch
            {
                Update = 0;
            }
            if (Update == 0)
            {
                try
                {
                    string SqlQuery = "";

                    SqlQuery = "Insert into NetworkLatencyServers(ServerID,NetworkLatencyId,LatencyYellowThreshold , LatencyRedThreshold , Enabled) values" +
                    //"ActiveSyncCredentialsId) values" +
                   "(" + StObject.ServerID + ",'"+StObject.NetworkLatencyId+"','" +
                   StObject.LatencyYellowThreshold + "','" + StObject.LatencyRedThreshold + "','" + StObject.Enabled + "')";
                    Update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
                }
                catch
                {
                    Update = 0;
                }
            }
            if (Update == 1)
            {
                UpdateRet = true;
            }
            return UpdateRet;
        }
		public DataTable GetTest()
		{

			DataTable NetworkLatencyDataTable = new DataTable();
			ProfileNames ReturnLOCbject = new ProfileNames();
			try
			{
				//3/19/2014 NS modified for VSPLUS-484
				string SqlQuery = "SELECT * FROM NetworkLatency ORDER BY TestName";

				NetworkLatencyDataTable = objAdaptor.FetchData(SqlQuery);

			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return NetworkLatencyDataTable;
		}

		public DataTable GetName(NetworkLatency nlobj)
		{
			//SametimeServers SametimeObj = new SametimeServers();
			DataTable NetworkLatencyTable = new DataTable();
			try
			{
				if (nlobj.NetworkLatencyId == 0)
				{

					string sqlQuery = "Select * from NetworkLatency where TestName='" + nlobj.TestName + "' ";
					NetworkLatencyTable = objAdaptor.FetchData(sqlQuery);

				}
				else
				{
					string sqlQuery = "Select * from NetworkLatency where TestName='" + nlobj.TestName + "' and NetworkLatencyId <>" + nlobj.NetworkLatencyId + " ";
					NetworkLatencyTable = objAdaptor.FetchData(sqlQuery);
				}
			}

			catch (Exception ex)
			{

				throw ex;
			}
			return NetworkLatencyTable;

		}
    }
}

