using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.SecurityDAL
{
    public class ServerTypesDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static ServerTypesDAL _self = new ServerTypesDAL();

        public static ServerTypesDAL Ins
        {
            get { return _self; }
        }
        /// <summary>
        /// Get all Data from LocationsDAL
        /// </summary>

        public DataTable GetAllData()
        {

            DataTable ServerTypesDataTable = new DataTable();
            ServerTypes ReturnLOCbject = new ServerTypes();
            try
            {
                string SqlQuery = "SELECT * FROM [ServerTypes] order by ServerType";

                ServerTypesDataTable = objAdaptor.FetchData(SqlQuery);

            }
            catch
            {
            }
            finally
            {
            }
            return ServerTypesDataTable;
        }

		public DataTable GetAllDatawithprofileid( int profilevalue)
        {

            DataTable ServerTypesDataTable = new DataTable();
            ServerTypes ReturnLOCbject = new ServerTypes();
            try
            {
               
				

				//string SqlQuery = "SELECT distinct pm.ServerTypeId,st.serverType FROM ProfilesMaster pm Inner Join ProfileNames Pn  on pn.ID=pm.ProfileId inner join ServerTypes st on st.ID=pm.ServerTypeId inner join SelectedFeatures sf on sf.FeatureID=st.FeatureId  where Pn.ID=" + profilevalue + " order by ServerType";
                //2/23/2016 NS modified for VSPLUS-2568
                string SqlQuery = "select distinct CASE WHEN st.serverType='Office365' THEN 'Office 365' ELSE st.serverType END serverType,pm.ServerTypeId from servertypes st inner join ProfilesMaster pm on pm.servertypeid=st.id inner join SelectedFeatures sf on sf.FeatureID=st.FeatureId  order by  servertype";
                ServerTypesDataTable = objAdaptor.FetchData(SqlQuery);

            }
            catch
            {
            }
            finally
            {
            }
            return ServerTypesDataTable;
        }
        /// <summary>
        /// Get Data from ServerTypes based on Key
        /// </summary>
        public ServerTypes GetData(ServerTypes STypebject)
        {
            DataTable ServerTypesDataTable = new DataTable();
            ServerTypes ReturnSTypeobject = new ServerTypes();
            try
            {
                string SqlQuery = "Select * from ServerTypes where [ID]=" + STypebject.ID.ToString();
                ServerTypesDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
                ReturnSTypeobject.ServerType = ServerTypesDataTable.Rows[0]["ServerType"].ToString();

            }
            catch
            {
            }
            finally
            {
            }
            return ReturnSTypeobject;
        }

        public ServerTypes GetDataForServerType(ServerTypes STypebject)
        {
            DataTable ServerTypesDataTable = new DataTable();
            ServerTypes ReturnSTypeobject = new ServerTypes();
            try
            {
                string SqlQuery = "Select * from ServerTypes where ServerType='" + STypebject.ServerType + "'";
                ServerTypesDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
                if (ServerTypesDataTable.Rows.Count > 0)
                {
                    if (ServerTypesDataTable.Rows[0]["ID"].ToString() != "")
                        ReturnSTypeobject.ID = int.Parse(ServerTypesDataTable.Rows[0]["ID"].ToString());
                }
                else
                {
                    ReturnSTypeobject.ID = 0;
                }
            }
            catch
            {
            }
            finally
            {
            }
            return ReturnSTypeobject;
        }

        public Object insertServertypes(ServerTypes StypeObject)
        {
            Object returnval;
            try
            {
                string s = "insert into ServerTypes(ServerType) values('" + StypeObject.ServerType + "')";
                 returnval = objAdaptor.ExecuteNonQuery(s);

            }
            catch (Exception e)
            {
                throw e;
            }
            return returnval;
        }

        public Object updateServertype(ServerTypes StypeObject)
        {
            Object updateObject;
            try
            {
                string update = "update ServerTypes set ServerType='" + StypeObject.ServerType + "' where ID=" + StypeObject.ID + "";
                updateObject = objAdaptor.ExecuteNonQuery(update);

            }
            catch (Exception x)
            {
                throw x;
            }
            return updateObject;
        }

        public Object deleteservertype(ServerTypes StypeObject)
        {
            Object deleteobject;
            try
            {
                string delete = "delete from ServerTypes where ID=" + StypeObject.ID + "";
                deleteobject = objAdaptor.ExecuteNonQuery(delete);

            }
            catch (Exception r)
            {
                throw r;
            }
            return deleteobject;
        }

        public DataTable GetDataForServerTypeByname(ServerTypes STypebject)
        {
            DataTable ServerTypesDataTable = new DataTable();
            
            try
            {
                string SqlQuery = "Select * from ServerTypes where ServerType='" + STypebject.ServerType + "'";
                ServerTypesDataTable = objAdaptor.FetchData(SqlQuery);
                
            }
            catch
            {
            }
            finally
            {
            }
            return ServerTypesDataTable;
        }

        //MD 06-Jan-14
        public DataTable GetSpecificServerTypes()
        {

            DataTable ServerTypesDataTable = new DataTable();
            ServerTypes ReturnLOCbject = new ServerTypes();
            try
            {
                //Mukund removed Sharepoint from 'not in'
                //5/20/2016 Sowjanya modified for VSPLUS-2987

                string SqlQuery = "SELECT * FROM [ServerTypes] where ServerType not in ('Mail','URL','Network Device','Notes Database','Office365','Cloud','Network Latency','Mobile Users','Domino Cluster','Domino Cluster Database') order by servertype ";

                ServerTypesDataTable = objAdaptor.FetchData(SqlQuery);

            }
            catch
            {
            }
            finally
            {
            }
            return ServerTypesDataTable;
        }

        //CY 03/27
        public DataTable GetExchangeRoles()
        {
            DataTable ExchangeRolesDataTable = new DataTable();
            try
            {
                string SqlQuery = "SELECT * FROM [RolesMaster] where servertypeid = 5";
                ExchangeRolesDataTable = objAdaptor.FetchData(SqlQuery);

            }
            catch
            {
            }
            finally
            {
            }
            return ExchangeRolesDataTable;
        }

		public DataTable GetExchangeRoleswithprofile(int servertypeid)
        {
            DataTable ExchangeRolesDataTable = new DataTable();
            try
            {
				string SqlQuery = "Select distinct rm.Id,rm.RoleName,rm.ServerTypeId from ProfilesMaster pm inner join RolesMaster rm on pm.RoleType=rm.RoleName where rm.servertypeid = " + servertypeid + " order by RoleName";
                ExchangeRolesDataTable = objAdaptor.FetchData(SqlQuery);

            }
            catch
            {
            }
            finally
            {
            }
            return ExchangeRolesDataTable;
        }
        //2/29/2016 Durga Modified for VSPLUS-2668
        public DataTable GetSpecifiServertypesforSSE(int profilevalue, string Page, string control)
        {

            DataTable ServerTypesDataTable = new DataTable();
            ServerTypes ReturnLOCbject = new ServerTypes();
            try
            {

                string SqlQuery = "select distinct CASE WHEN st.serverType='Office365' THEN 'Office 365' ELSE st.serverType END serverType,pm.ServerTypeId from servertypes st inner join ProfilesMaster pm on pm.servertypeid=st.id inner join SelectedFeatures sf on sf.FeatureID=st.FeatureId where st.id not  in(select ServertypeID from  Servertypeexcludelist where Page='" + Page + "' and Control='" + control + "')  order by  servertype";
                ServerTypesDataTable = objAdaptor.FetchData(SqlQuery);

            }
            catch
            {
            }
            finally
            {
            }
            return ServerTypesDataTable;
        }
    }

}
