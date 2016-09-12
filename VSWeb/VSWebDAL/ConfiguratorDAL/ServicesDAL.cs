using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.ConfiguratorDAL
{
    public class ServicesDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static ServicesDAL _self = new ServicesDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static ServicesDAL Ins
        {
            get { return _self; }
        }

        public DataTable GetAllServices()
        {
            DataTable ServicesDataTable = new DataTable();

            try
            {
                string SqlQuery = "select rm.ServerTypeId, sm.ServiceId, sm.ServiceName, sm.ServiceShortName, sm.SecurityContext, sm.ServiceDescription, sm.DefaultStartupType, sm.Required, sm.Custom,rm.RoleName,svr.VersionNo from ServiceMaster sm, ServiceVersionRole svr, RolesMaster rm where sm.ServiceId=svr.ServiceId and rm.Id=svr.RoleId ";
                ServicesDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return ServicesDataTable;
        }
        /// <summary>
        /// GetAllServicesByServerType, left outer join on services and servers
        /// </summary>
        /// <param name="ServerTypeId"></param>
        /// <returns></returns>
        public DataTable GetAllServicesByServerType(int ServerTypeId)
        {
            DataTable ServicesDataTable = new DataTable();

            try
            {
                string SqlQuery = "SELECT  svr.id,  case when ss.ServerId is NULL then 'False' else 'True' end Monitored,ss.ServerId ,  rm.ServerTypeId, sm.ServiceId, sm.ServiceName, sm.ServiceShortName, sm.SecurityContext, sm.ServiceDescription, sm.DefaultStartupType, sm.Required, " +
                                "sm.Custom, rm.RoleName, svr.VersionNo"+
                                "FROM dbo.ServiceMaster AS sm INNER JOIN "+
                                "dbo.ServiceVersionRole AS svr ON sm.ServiceId = svr.ServiceId INNER JOIN "+
                                "dbo.RolesMaster AS rm ON svr.RoleId = rm.Id	"+
                                "left outer join ServerServices ss on svr.Id=ss.SVRId  where rm.ServerTypeId=" + ServerTypeId.ToString();
                ServicesDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return ServicesDataTable;
        }

        /// <summary>
        /// GetAllServicesByServerIdType;left outer join on services and servers
        /// </summary>
        /// <param name="ServerTypeId"></param>
        /// <param name="ServerId"></param>
        /// <returns></returns>
		public DataTable GetWebsphereserverCredentials(int ServerType)

		{
			DataTable CredentialsDataTable = new DataTable();

			try
			{
				string SqlQuery = "SELECT * from Credentials where ServerTypeID=" + ServerType + "";
				CredentialsDataTable = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return CredentialsDataTable;
		}
        public DataTable GetAllServicesByServerIdType(int ServerTypeId, int ServerId, string VersionNo)
        {
            DataTable ServicesDataTable = new DataTable();

            try
            {
                string SqlQuery = "(select null as id,null as Monitored,null as SeverId,null as ServerTypeId, null as ServiceId, RoleName as ServiceName, null as ServiceShortName, null as SecurityContext, " +
								"null as ServiceDescription, null as DefaultStartupType, null as Required,  null as Custom, null as RoleName, null as VersionNo, ID as RoleId, id as AlteredID from rolesmaster) " +
					
								"union( " +
					
					
								"SELECT   svr.id, case when ss.ServerId is NULL then 'False' else 'True' end Monitored,ss.ServerId,  rm.ServerTypeId, sm.ServiceId, sm.ServiceName, sm.ServiceShortName, sm.SecurityContext, sm.ServiceDescription, sm.DefaultStartupType, sm.Required, " +
                                " sm.Custom, rm.RoleName, svr.VersionNo, rm.ID as RoleId, svr.id + (select Max(id) from RolesMaster) as AlteredId" +
                                " FROM dbo.ServiceMaster AS sm INNER JOIN " +
                                " dbo.ServiceVersionRole AS svr ON sm.ServiceId = svr.ServiceId INNER JOIN " +
                                " dbo.RolesMaster AS rm ON svr.RoleId = rm.Id " +
                                " left outer join ServerServices ss on svr.Id=ss.SVRId and ss.serverid=" + ServerId + "  where rm.ServerTypeId=" + ServerTypeId.ToString() +
                                " and svr.VersionNo='" + VersionNo + "')";
                
				
				ServicesDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return ServicesDataTable;
        }


        //public DataTable GetAllServicesByServerIdType(int ServerTypeId, int ServerId, string VersionNo,string role)
        //{
        //    DataTable ServicesDataTable = new DataTable();

        //    try
        //    {
        //        string SqlQuery = "SELECT   svr.id, case when ss.ServerId is NULL then 'False' else 'True' end Monitored,ss.ServerId,  rm.ServerTypeId, sm.ServiceId, sm.ServiceName, sm.ServiceShortName, sm.SecurityContext, sm.ServiceDescription, sm.DefaultStartupType, sm.Required, " +
        //                        " sm.Custom, rm.RoleName, svr.VersionNo" +
        //                        " FROM dbo.ServiceMaster AS sm INNER JOIN " +
        //                        " dbo.ServiceVersionRole AS svr ON sm.ServiceId = svr.ServiceId INNER JOIN " +
        //                        " dbo.RolesMaster AS rm ON svr.RoleId = rm.Id	" +
        //                        " left outer join ServerServices ss on svr.Id=ss.SVRId and ss.serverid=" + ServerId + "  where rm.ServerTypeId=" + ServerTypeId.ToString() +
        //                        " and svr.VersionNo='" + VersionNo + "' " + (role != "" ? " and rm.RoleName in (" + role + ")" : "");
        //        ServicesDataTable = objAdaptor.FetchData(SqlQuery);
        //    }
        //    catch
        //    {
        //    }
        //    finally
        //    {
        //    }
        //    return ServicesDataTable;
        //}

        public DataTable GetVersions()
        {
            DataTable VersionsDataTable = new DataTable();

            try
            {
                string SqlQuery = "SELECT * from ServerVersions";
                VersionsDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return VersionsDataTable;
        }
		public DataTable GetCredentials()
		{
			DataTable CredentialsDataTable = new DataTable();

			try
			{
				string SqlQuery = "SELECT * from Credentials";
				CredentialsDataTable = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return CredentialsDataTable;
		}
        // 8/7/2016 Durga Addded for VSPLUS-2877
        public DataTable GetCredentialsForSSE()
        {
            DataTable CredentialsDataTable = new DataTable();

            try
            {
                string SqlQuery = "SELECT * from Credentials where servertypeid is not null and servertypeid not in(3,22) ";
                CredentialsDataTable = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return CredentialsDataTable;
        }
		public DataTable GetSametimeCredentials()
		{
			DataTable CredentialsDataTable = new DataTable();

			try
			{
				string SqlQuery = "SELECT * from Credentials where ServerTypeId=3";
				CredentialsDataTable = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return CredentialsDataTable;
		}

		public DataTable GetExchangeServers()
		{
			DataTable ServersDataTable = new DataTable();

			try
			{
				string SqlQuery = "SELECT * from Servers where ServerTypeID=5";
				ServersDataTable = objAdaptor.FetchData(SqlQuery);
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
        public DataTable GetSelectedRoles(int ServerTypeId, int ServerId, string VersionNo)
        {
            DataTable RolesDataTable = new DataTable();

            try
            {
                string SqlQuery = "SELECT distinct rm.RoleName  FROM dbo.ServiceMaster AS sm INNER JOIN dbo.ServiceVersionRole AS svr "+
                    " ON sm.ServiceId = svr.ServiceId INNER JOIN dbo.RolesMaster AS rm ON svr.RoleId = rm.Id inner join ServerServices ss "+
                    " on svr.Id=ss.SVRId and ss.serverid=" + ServerId + "  where rm.ServerTypeId=" + ServerTypeId + " and svr.VersionNo='" + VersionNo  + "'";
                RolesDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
			finally
            {
            }
            return RolesDataTable;
        }
        public DataTable GetRoles(int ServerId)
        {
            DataTable RolesDataTable = new DataTable();

            try
            {
                string SqlQuery = "SELECT rm.Id,rm.RoleName  from RolesMaster rm,ServerRoles sr where rm.Id=sr.RoleId and sr.serverid=" + ServerId;
                RolesDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return RolesDataTable;
        }

        public DataTable GetRolesbyName(string ServerName)
        {
            DataTable RolesDataTable = new DataTable();

            try
            {
                string SqlQuery = "SELECT distinct rm.Id,rm.RoleName,sr.ServerId,exgset.VersionNo " +
									"from RolesMaster rm,ServerRoles sr,servers srv " +
									"inner join ExchangeSettings exgset on exgset.ServerId = srv.ID " +
									"where rm.Id=sr.RoleId and sr.serverid=srv.ID and srv.ServerName='" + ServerName + "'";
                RolesDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return RolesDataTable;
        }

        public bool InsertSVRData(List<object> fieldValues, string ServerId)
        {
            bool Insert = false;
            try
            {
                string SqlQuery = "DELETE FROM ServerServices WHERE ServerId=" + ServerId + "";
                Insert = objAdaptor.ExecuteNonQuery(SqlQuery);

                for (int i = 0; i < fieldValues.Count; i++)
                {
                    SqlQuery = "INSERT INTO ServerServices(ServerId,SVRId) values(" + ServerId + "," + fieldValues[i] + ")";
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

        public DataTable GetSelectedVersions(int ServerTypeId, int ServerId )
        {
            DataTable VersionsDataTable = new DataTable();

            try
            {
              string SqlQuery = "SELECT distinct svr.VersionNo  FROM dbo.ServiceMaster AS sm INNER JOIN dbo.ServiceVersionRole AS svr "+
                    " ON sm.ServiceId = svr.ServiceId INNER JOIN dbo.RolesMaster AS rm ON svr.RoleId = rm.Id inner join ServerServices ss "+
                    " on svr.Id=ss.SVRId and ss.serverid=" + ServerId + "  where rm.ServerTypeId=" + ServerTypeId + "";
               
                VersionsDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return VersionsDataTable;
        }

        public DataTable GetAllServicesByServerIdTypeAndRoles(int ServerTypeId, int ServerId, string VersionNo, string Roles)
        {
            DataTable ServicesDataTable = new DataTable();

            try
            {
                if (Roles == "") Roles = "''";

                string SqlQuery ="(select id as svID,null as Monitored,null as SeverId,null as ServerTypeId, null as ServiceId, RoleName as ServiceName, null as ServiceShortName, null as SecurityContext, " +
								"null as ServiceDescription, null as DefaultStartupType, null as Required,  null as Custom, null as RoleName, null as VersionNo, ID as RoleId, id as AlteredID from rolesmaster) " +

								"union( " +
								"SELECT   svr.id, case when ss.ServerId is NULL then 'False' else 'True' end Monitored,ss.ServerId,  rm.ServerTypeId, sm.ServiceId, sm.ServiceName, sm.ServiceShortName, sm.SecurityContext, sm.ServiceDescription, sm.DefaultStartupType, sm.Required, " +
                                " sm.Custom, rm.RoleName, svr.VersionNo" +
                                " FROM dbo.ServiceMaster AS sm INNER JOIN " +
                                " dbo.ServiceVersionRole AS svr ON sm.ServiceId = svr.ServiceId INNER JOIN " +
                                " dbo.RolesMaster AS rm ON svr.RoleId = rm.Id " +
                                " left outer join ServerServices ss on svr.Id=ss.SVRId and ss.serverid=" + ServerId + "  where rm.ServerTypeId=" + ServerTypeId.ToString() +
                                " and svr.VersionNo='" + VersionNo + "' and rm.RoleName in (" + Roles + ")  order by sm.ServiceName ";
                ServicesDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return ServicesDataTable;
        }

		public DataTable GetWindowsServices(string servername)
		{
			DataTable ServicesDataTable = new DataTable();

			try
			{
                //11/18/2014 NS modifid the value of Type for better UI presence/readability
				string SqlQuery = "select ID,DisplayName as ServiceName, [StartMode], [Status] as Result, DateStamp,Monitored, Monitored as isSelected, 'Type' = case WHEN ServerRequired='1' THEN 'Required for Windows' else 'NOT Required for Windows' end from  WindowsServices where ServerName =  '" + servername + "' ORDER BY Monitored DESC,DisplayName";
				ServicesDataTable = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return ServicesDataTable;
		}
        public DataTable GetWindowsServices1(string servername)
        {
            DataTable ServicesDataTable = new DataTable();

            try
            {
                //11/19/2014 NS modified the query for better readability
				//1/28/2016 Sowjanya modified the query for VSPLUS-2526,VSPLUS-2524
				string SqlQuery = "select ID,DisplayName as ServiceName, [StartMode], [Status] as Result, DateStamp,Monitored, Monitored as isSelected, 'Type' = case WHEN ServerRequired='1' THEN 'Required for Exchange' else 'NOT Required for Exchange' end from  WindowsServices where ServerName =  '" + servername + "' ORDER BY Monitored ASC,DisplayName DESC";
                ServicesDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return ServicesDataTable;
        }

		public DataTable GetWindowsServicesForSP(string servername)
		{
			DataTable ServicesDataTable = new DataTable();

			try
			{
				//11/19/2014 NS modified the query for better readability
				string SqlQuery = "select ID,DisplayName as ServiceName, [StartMode], [Status] as Result, DateStamp,Monitored, Monitored as isSelected, 'Type' = case WHEN ServerRequired='1' THEN 'Suggested for SharePoint' else 'Windows Services' end from  WindowsServices where ServerName =  '" + servername + "' ORDER BY Monitored DESC,DisplayName";
				ServicesDataTable = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return ServicesDataTable;
		}


		public Boolean UpdateWindowsServices(string servername, List<object> fieldValues)
		{
			bool Update = false;

			try
			{
				string SqlQuery = "UPDATE WindowsServices set Monitored=0 WHERE ServerName='" + servername + "'";
				//select ID,DisplayName as ServiceName,[StartMode],DateStamp,Monitored  from  WindowsServices where ServerName =  '" + servername + "' ORDER BY Monitored DESC";
				Update = objAdaptor.ExecuteNonQuery(SqlQuery);

				for (int i = 0; i < fieldValues.Count; i++)
				{
					SqlQuery = "UPDATE WindowsServices set Monitored=1 WHERE ServerName='" + servername + "' AND ID=" + fieldValues[i];
					Update = objAdaptor.ExecuteNonQuery(SqlQuery);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return Update;
		}

		public DataTable GetWindowsServicesWS(string servername)
		{
			DataTable ServicesDataTable = new DataTable();

			try
			{
				//11/19/2014 NS modified the query for better readability
				string SqlQuery = "select ID,DisplayName as ServiceName, [StartMode], [Status] as Result, DateStamp,Monitored, Monitored as isSelected, 'Type' = case WHEN ServerRequired='1' THEN 'Required for Applications' else 'NOT Required for WebSphere' end from  WindowsServices where ServerName =  '" + servername + "' ORDER BY Monitored DESC,DisplayName";
				ServicesDataTable = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return ServicesDataTable;
		}

		public DataTable GetWindowsServicesWS()
		{
			DataTable ServicesDataTable = new DataTable();

			try
			{
				//11/19/2014 NS modified the query for better readability
				string SqlQuery = " select distinct ws.ID,DisplayName as ServiceName, [StartMode], [Status] as Result, DateStamp,Monitored, Monitored as isSelected ,sr.ServerName, sr.ServerTypeID,st.ServerType from  WindowsServices ws inner join servers sr on sr.ServerName=ws.ServerName  inner join ServerTypes st on st.ID=sr.ServerTypeID where st.ServerType='WebSphere' ORDER BY Monitored DESC,DisplayName ";

				ServicesDataTable = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return ServicesDataTable;
		}
		public Boolean UpdateWindowsServicesWS(string servername, List<object> fieldValues)
		{
			bool Update = false;

			try
			{
				string SqlQuery = "UPDATE WindowsServices set Monitored=0 WHERE ServerName='" + servername + "'";
				//select ID,DisplayName as ServiceName,[StartMode],DateStamp,Monitored  from  WindowsServices where ServerName =  '" + servername + "' ORDER BY Monitored DESC";
				Update = objAdaptor.ExecuteNonQuery(SqlQuery);

				for (int i = 0; i < fieldValues.Count; i++)
				{
					SqlQuery = "UPDATE WindowsServices set Monitored=1 WHERE ServerName='" + servername + "' AND ID=" + fieldValues[i];
					Update = objAdaptor.ExecuteNonQuery(SqlQuery);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return Update;
		}

        //14/07/2016 sowmya added for VSPLUS-3097
        public DataTable GetTestNames()
        {
            DataTable ExchangeTestDataTable = new DataTable();

            try
            {
                string SqlQuery = "select TestId,TestName from ExchangeTestNames order by TestName ";
                ExchangeTestDataTable = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return ExchangeTestDataTable;
        }
    }
}
