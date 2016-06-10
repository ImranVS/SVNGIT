using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.ConfiguratorDAL
{
	public class WebSpherepropertiesDAL
    {
        private Adaptor objAdaptor = new Adaptor();
		private static WebSpherepropertiesDAL _self = new WebSpherepropertiesDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
		public static WebSpherepropertiesDAL Ins
        {
            get { return _self; }
        }

        public DataTable GetAllData()
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT Sr.ID,Sr.ServerName,S.ServerType,L.Location,Sr.LocationID,ls.ScanInterval," +
                    "ls.Enabled,ls.Category FROM Servers Sr INNER JOIN ServerTypes S on Sr.ServerTypeID=S.ID " +
					"INNER JOIN Locations L ON Sr.LocationID=L.ID LEFT OUTER JOIN WebsphereServer ls ON sr.ID=ls.serverid " +
					"WHERE S.ServerType='WebSphere' ";
                dt = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }

		public DataTable GetAllDataByNames(WebSpherePropertie ServerObject)

        {

            DataTable ServersDataTable = new DataTable();
            //Servers ReturnSerevrbject = new Servers();
            try
            {
				if (ServerObject.ServerID == 0)
                {
					//string SqlQuery = "select Sr.ID,Sr.ServerName,sr.Description,S.ServerType,sa.CredentialsId,"+
					//    "L.Location,sa.Enabled,sa.ScanInterval,sa.RetryInterval,sa.OffHoursScanInterval, "+
					//    "sa.category,sa.CPUThreshold,sa.MemoryThreshold,sa.ResponseThreshold,sa.FailureThreshold "+
					//    "from Servers Sr inner join ServerTypes S "+
					//    "on Sr.ServerTypeID=S.ID  inner join Locations L on Sr.LocationID =L.ID  "+
					//    "inner join WebsphereServer sa on sr.ID=sa.serverid " +
					//    "where sr.ServerName='" + ServerObject.ServerName + "'";
					//ServersDataTable = objAdaptor.FetchData(SqlQuery);
                }
                else
                {
					string SqlQuery = "select * from WebsphereServer where ServerID="+	ServerObject.ServerID   +"";
                    ServersDataTable = objAdaptor.FetchData(SqlQuery);
                }
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return ServersDataTable;
        }

		public Object UpdateData(WebSpherePropertie WebSphereobj)
        {
            Object Update;
            try
            {
				DataTable dt = GetData(WebSphereobj.ServerID);
                if (dt.Rows.Count > 0)
                {
					string SqlQuery = "UPDATE WebsphereServer SET NodeID=" + WebSphereobj.NodeID + ",CellID =" + WebSphereobj.CellID + ", AvgThreadPool='" + WebSphereobj.AvgThreadPool +
						"',ActiveThreadCount=" + WebSphereobj.ActiveThreadCount + ",CurrentHeap=" + WebSphereobj.CurrentHeap +
						",MaxHeap=" + WebSphereobj.MaxHeap + ",Uptime='" + WebSphereobj.Uptime +
						"',HungThreadCount=" + WebSphereobj.HungThreadCount +
						",DumpGenerated =" + WebSphereobj.DumpGenerated +
						" where ServerId=" + WebSphereobj.ServerID + "";

                    Update = objAdaptor.ExecuteNonQuery(SqlQuery);
                }
                else
                {
					Update = InsertData(WebSphereobj);
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

		public bool InsertData(WebSpherePropertie WebSphereobj)
        {
            bool Insert = false;
            try
            {
				string SqlQuery = "INSERT INTO [WebsphereServer] (ServerID,NodeID,CellID,ServerName,AvgThreadPool,ActiveThreadCount,CurrentHeap," +
					"MaxHeap,Uptime,HungThreadCount,DumpGenerated,Enabled) " +
					   " VALUES(" + WebSphereobj.ServerID + "," + WebSphereobj.NodeID + "," + WebSphereobj.CellID + ",'" + WebSphereobj.ServerName + "','" + WebSphereobj.AvgThreadPool + "'," +
					   WebSphereobj.ActiveThreadCount + "," + WebSphereobj.CurrentHeap + "," + WebSphereobj.MaxHeap + "," +
					   WebSphereobj.Uptime + "," + WebSphereobj.HungThreadCount + ",'" + WebSphereobj.DumpGenerated + "','" + WebSphereobj.Enabled + "')";

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
		public bool InsertDataforservers(Servers ServerObject)

		{
			bool Insert = false;
			try
			{
				string SqlQuery = "INSERT INTO [Servers] ([ServerName],[ServerTypeID],[Description],[LocationID],[IPAddress]) " +
					   "VALUES('" + ServerObject.ServerName + "', " + ServerObject.ServerTypeID + ", '" + ServerObject.Description + "'," + ServerObject.LocationID +
				",'" + ServerObject.IPAddress + "')";

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
		public Object UpdateDataforservers(Servers ServerObject)

		{
			Object Update;
			try
			{
				string SqlQuery = "UPDATE Servers SET ServerName='" + ServerObject.ServerName +
					"',ServerTypeID=" + ServerObject.ServerTypeID + ",Description='" + ServerObject.Description +
					"',LocationID=" + ServerObject.LocationID + ",IPAddress='" + ServerObject.IPAddress + "'  where ID=" + ServerObject.ID + "";
		
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

        public DataTable GetData(int ServerId)
        {
            DataTable ServersDataTable = new DataTable();
			try
			{
				string SqlQuery = "select * from WebsphereServer where serverid =" + ServerId;
				ServersDataTable = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
            return ServersDataTable;
        }

        public WebSpherePropertie GetData(WebSpherePropertie DSObject)
        {
            DataTable DominoServersDataTable = new DataTable();
            WebSpherePropertie ReturnDSObject = new WebSpherePropertie();
            ServerAttributes sr = new ServerAttributes();

            try
            {
                //string SqlQuery = "select *,ISNULL(RequireSSL,0) RS,ServerName as Name,IPAddress,LocationID,Description,servers.ID,Location from Servers left join dominoservers on Servers.ID=DominoServers.ServerID inner join Locations on servers.LocationID=Locations.ID where servers.ID=" + DSObject.Key + "";
                //8/7/2014 NS modified for VSPLUS-853
                string SqlQuery = "select Sr.ID,Sr.ServerName,sr.Description,S.ServerType,L.Location,Sr.LocationID,ws.ActiveThreadCount,ws.AvgThreadPool,ws.CurrentHeap,ws.DumpGenerated,ws.HungThreadCount,ws.MaxHeap,ws.Uptime,sa.ScanInterval,sa.Enabled,sa.ResponseTime,sa.RetryInterval,sa.ConsFailuresBefAlert,sa.OffHourInterval,sa.OffHourInterval,sa.CPU_Threshold,sa.MemThreshold,sr.ipaddress,sa.category,wc.CellName, wn.NodeName,ws.CellID,ws.NodeID from Servers Sr inner join ServerTypes S on Sr.ServerTypeID=S.ID inner Join WebsphereServer ws on ws.ServerName=Sr.ServerName Inner join WebsphereCell wc on wc.CellID=ws.CellID inner join WebsphereNode wn on wn.NodeID=ws.NodeID inner join Locations L on Sr.LocationID =L.ID left outer join ServerAttributes sa on sr.ID=sa.serverid  where  sr.ID='" + DSObject.ServerID + "'";
                DominoServersDataTable = objAdaptor.FetchData(SqlQuery);
                if (DominoServersDataTable.Rows.Count > 0)
                {

                    bool value;
                    //Commented by Mukund 06Jun14:Error this parameter is not taken now. Disk space is separately dealt
                    //ReturnDSObject.DiskSpaceThreshold = float.Parse(DominoServersDataTable.Rows[0]["DiskSpaceThreshold"].ToString());

                    ReturnDSObject.Category = DominoServersDataTable.Rows[0]["category"].ToString();

                    // ReturnDSObject. = DominoServersDataTable.Rows[0]["Description"].ToString();
                    //sr.Enabled = bool.Parse(DominoServersDataTable.Rows[0]["Enabled"].ToString());



                    ReturnDSObject.FailureThreshold = int.Parse(DominoServersDataTable.Rows[0]["ConsFailuresBefAlert"].ToString());

                    ReturnDSObject.ServerName = DominoServersDataTable.Rows[0]["ServerName"].ToString();
                    ReturnDSObject.OffHoursScanInterval = int.Parse(DominoServersDataTable.Rows[0]["OffHourInterval"].ToString());

                    ReturnDSObject.ResponseThreshold = int.Parse(DominoServersDataTable.Rows[0]["ResponseTime"].ToString());
                    ReturnDSObject.RetryInterval = int.Parse(DominoServersDataTable.Rows[0]["RetryInterval"].ToString());
                    ReturnDSObject.ScanInterval = int.Parse(DominoServersDataTable.Rows[0]["ScanInterval"].ToString());

                  //ReturnDSObject.IPAddress = DominoServersDataTable.Rows[0]["IPAddress"].ToString();
					//Sowjanya modified for VSPLUS-2647 ticket
					  ReturnDSObject.Memory_Threshold =int.Parse( string.IsNullOrEmpty(DominoServersDataTable.Rows[0]["MemThreshold"].ToString()) ? "0":DominoServersDataTable.Rows[0]["MemThreshold"].ToString());
					  ReturnDSObject.CPU_Threshold = int.Parse(string.IsNullOrEmpty(DominoServersDataTable.Rows[0]["CPU_Threshold"].ToString()) ? "0" : DominoServersDataTable.Rows[0]["CPU_Threshold"].ToString());
                  
                    ReturnDSObject.ServerID = int.Parse(DominoServersDataTable.Rows[0]["ID"].ToString());
                    ReturnDSObject.ServerID = Convert.ToInt16(DominoServersDataTable.Rows[0]["ID"].ToString());



					ReturnDSObject.AvgThreadPool = int.Parse(string.IsNullOrEmpty(DominoServersDataTable.Rows[0]["AvgThreadPool"].ToString()) ? "0" : DominoServersDataTable.Rows[0]["AvgThreadPool"].ToString());
                    ReturnDSObject.ActiveThreadCount = int.Parse(string.IsNullOrEmpty(DominoServersDataTable.Rows[0]["ActiveThreadCount"].ToString())?"0":DominoServersDataTable.Rows[0]["ActiveThreadCount"].ToString());
					ReturnDSObject.CurrentHeap = DominoServersDataTable.Rows[0]["CurrentHeap"].ToString();
                    ReturnDSObject.MaxHeap = DominoServersDataTable.Rows[0]["MaxHeap"].ToString();
					ReturnDSObject.Uptime = int.Parse(string.IsNullOrEmpty(DominoServersDataTable.Rows[0]["Uptime"].ToString()) ? "0" : DominoServersDataTable.Rows[0]["Uptime"].ToString());
					ReturnDSObject.HungThreadCount = int.Parse(string.IsNullOrEmpty(DominoServersDataTable.Rows[0]["HungThreadCount"].ToString()) ? "0" : DominoServersDataTable.Rows[0]["HungThreadCount"].ToString());
                    ReturnDSObject.DumpGenerated = DominoServersDataTable.Rows[0]["DumpGenerated"].ToString();

                    //ReturnDSObject.Modified_By = Convert.ToInt16(DominoServersDataTable.Rows[0]["Modified_By"].ToString());
                    //ReturnDSObject.Modified_On = DominoServersDataTable.Rows[0]["Modified_On"].ToString();
                    //8/7/2014 NS added for VSPLUS-853
                   // ReturnDSObject.CredentialsID = int.Parse(DominoServersDataTable.Rows[0]["CredentialsID"].ToString());
                }
                //else
                //{
                //    ReturnDSObject.Location = DominoServersDataTable.Rows[0]["Location"].ToString();
                //    ReturnDSObject.Name = DominoServersDataTable.Rows[0]["Name"].ToString();
                //    ReturnDSObject.IPAddress = DominoServersDataTable.Rows[0]["IPAddress"].ToString();
                //    ReturnDSObject.Description = DominoServersDataTable.Rows[0]["Description"].ToString();
                //    ReturnDSObject.ServerID = Convert.ToInt16(DominoServersDataTable.Rows[0]["ID"].ToString());
                //    ReturnDSObject.LocationID = int.Parse(DominoServersDataTable.Rows[0]["LocationID"].ToString());
                //}

            }


                //populate & return data object


            catch(Exception ex)
            {
				throw ex;
            }
            finally
            {
            }
            return ReturnDSObject;
        }
        public Object UpdatesData(WebSpherePropertie WebSphereobj)
        {
            Object Updates=false;
            try
            {
           
               // DataTable dt = GetData(WebSphereobj.ServerID);
               
                    string SqlQuery = "UPDATE WebsphereServer SET AvgThreadPool='" + WebSphereobj.AvgThreadPool +
                        "',ActiveThreadCount='" + WebSphereobj.ActiveThreadCount + "',CurrentHeap='" + WebSphereobj.CurrentHeap +
                        "',MaxHeap='" + WebSphereobj.MaxHeap + "',Uptime='" + WebSphereobj.Uptime +
                        "',HungThreadCount='" + WebSphereobj.HungThreadCount +
                        "',DumpGenerated ='" + WebSphereobj.DumpGenerated +
                        "' where ServerId=" + WebSphereobj.ServerID + "";

                     Updates = objAdaptor.ExecuteNonQuery(SqlQuery);
                    SqlQuery = "UPDATE ServerAttributes SET ScanInterval='" + WebSphereobj.ScanInterval +
                       "',RetryInterval='" + WebSphereobj.RetryInterval + "',OffHourInterval='" + WebSphereobj.OffHoursScanInterval +
                       "',ResponseTime='" + WebSphereobj.ResponseThreshold + "',ConsFailuresBefAlert='" + WebSphereobj.FailureThreshold +

                       "' where ServerId=" + WebSphereobj.ServerID + "";

                     Updates = objAdaptor.ExecuteNonQuery(SqlQuery);

                
               
            }
           
            catch(Exception ex)
            {
                 Updates = false;
            }
            finally
            {
            }
            return Updates;
        }
       


		public DataTable GetDataForServerID(string ServerName)
        {
            DataTable ServerDataTable = new DataTable();
            
            try
            {
				string SqlQuery = "Select * from Servers where ServerName='" + ServerName + "'";
                ServerDataTable = objAdaptor.FetchData(SqlQuery);
                
            }
            catch(Exception ex)
            {
				throw ex;
            }
            finally
            {
            }
            return ServerDataTable;
        }
		public DataTable GetServerNameinwebsphereservers(string Name)
		{
			//SametimeServers SametimeObj = new SametimeServers();
			DataTable CsTable = new DataTable();
			try
			{

				string sqlQuery = "Select * from WebsphereServer where ServerName='" + Name + "' ";
				CsTable = objAdaptor.FetchData(sqlQuery);

			}
			catch (Exception ex)
			{
				throw ex;
			}
			return CsTable;

		}
		public DataTable GetNodeID(string NodeName)
        {
			DataTable ServersDataTable = new DataTable();
			try
			{
				string SqlQuery = "select * from WebsphereNode where NodeName ='" + NodeName + "'";
				ServersDataTable = objAdaptor.FetchData(SqlQuery);
				
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return ServersDataTable;
		
		}

    }

}
