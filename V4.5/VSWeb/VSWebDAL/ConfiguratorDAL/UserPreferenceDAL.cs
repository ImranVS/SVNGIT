using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using VSWebDO;

namespace VSWebDAL.ConfiguratorDAL
{
    public class UserPreferencesDAL
    {
        ///<summary>
        ///Declarations
        ///</summary>
        private Adaptor objAdaptor = new Adaptor();
        private static UserPreferencesDAL _self = new UserPreferencesDAL();

        public static UserPreferencesDAL Ins
        {

            get { return _self; }
        }

        public void UpdateUserPreferences(string PreferenceName, string PreferenceValue, int UserID)
        {
            if(PreferenceName == "FilterBy")
                PreferenceValue = PreferenceValue.Replace("'", "''");
            //2/14/2013 NS modified
            int mode = 0;
			try
			{
				int count = 0;
				string Query = "SELECT Count(*) FROM UserPreferences WHERE PreferenceName = '" + PreferenceName + "' and UserID =" + UserID;
				count = objAdaptor.ExecuteScalar(Query);
				string SqlQuery = "";
				if (count == 0)
				{
					SqlQuery = "INSERT INTO UserPreferences VALUES('" + PreferenceName + "','" + PreferenceValue + "'," + UserID + ")";
				}
				else
				{
					SqlQuery = "UPDATE UserPreferences SET PreferenceValue='" + PreferenceValue + "' WHERE PreferenceName = '" + PreferenceName + "' and UserID =" + UserID;
				}

				mode = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
				//SqlQuery = "SELECT * FROM UserPreferences WHERE UserID =" + UserID;
				//return objAdaptor.FetchData(SqlQuery);

			}



			catch (Exception ex)
			{
				throw ex;
			}           
        }
		
        public DataTable getimagepath(string name)
        {
            DataTable dt=new DataTable();
            try
            {
                string Query = "SELECT * FROM CloudMaster WHERE  Name='" + name + "'";
                 dt = objAdaptor.FetchData(Query);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }
		public DataTable getimagepathfornetwork(string name)
		{
			DataTable dt = new DataTable();
			try
			{
				string Query = "SELECT * FROM NetworkMaster WHERE  Name='" + name + "'";
				dt = objAdaptor.FetchData(Query);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}
      

        public DataTable GetUserRowPrefrenceDetails(int UserID)
        {
            DataTable dt = new DataTable();
            try
            {
                string Query = "SELECT * FROM UserPreferences WHERE UserID =" + UserID;
                dt = objAdaptor.FetchData(Query);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }
	
        public DataTable fillddldata()
        {
            DataTable dt = new DataTable();
            try
            {
                //10/7/2014 NS modified
                string Query = "SELECT Name FROM CloudMaster " +
                    "ORDER BY Name ";
                dt = objAdaptor.FetchData(Query);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }
		public DataTable fillddldata_CredentialsComboBox()


		{
			DataTable dt = new DataTable();
			try
			{
				//10/7/2014 NS modified
				string Query = "SELECT Name,Image FROM NetworkMaster " +
					"ORDER BY Name ";
				dt = objAdaptor.FetchData(Query);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}


		public DataTable Getonpremiseswithuser(string loginname)
		{
			DataTable dt = new DataTable();
			try
			{
				string Query = "select * from Users where LoginName='" + loginname + "'" ;
				dt = objAdaptor.FetchData(Query);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}

        //2/11/2016 NS modified for VSPLUS-2594
		public DataTable GetDatabaseVersionInfo(string category = "")
		{
			DataTable dt = new DataTable();
			try
			{
				string Query = "SELECT CATEGORY,VALUE,LAST_UPDATE,DESCRIPTION,UPDATE_BY FROM VS_MANAGEMENT ";
                if (category != "")
                {
                    Query += "WHERE Category='" + category + "' ";
                }
				dt = objAdaptor.FetchData(Query);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			
			return dt;
		}

		public DataTable getAssemblyVersionInfo()
		{
			DataTable dt = new DataTable();
			try
			{
                string Query = "SELECT  NodeName as [Node Name], AssemblyName as [Assembly Name],AssemblyVersion as [Assembly Version],ProductVersion as [Product Version], FileArea as [File Area],BuildDate as [Build Date] FROM VS_AssemblyVersionInfo order by NodeName, AssemblyName";
				dt = objAdaptor.FetchData(Query);
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return dt;
		}

        //VSPLUS-1020,16Oct14, Swathi, Checking if Cloud is selected
        public DataTable IsCloudSelected()
        {
            DataTable dt = new DataTable();
            try
            {
                string Query = "select st.id,null,st.ServerType,st.id,'ServerTypes' from ServerTypes st,SelectedFeatures ft where ft.FeatureId=st.FeatureId and servertype='Cloud'";
                dt = objAdaptor.FetchData(Query);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }

		public DataTable IsNetworkdeviceSelected()
        {
            DataTable dt = new DataTable();
            try
            {
                string Query = "select st.id,null,st.ServerType,st.id,'ServerTypes' from ServerTypes st,SelectedFeatures ft where ft.FeatureId=st.FeatureId and servertype='Network Device'";
                dt = objAdaptor.FetchData(Query);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }

		public DataTable IsSNMPSelected()
        {
            DataTable dt = new DataTable();
            try
            {
                string Query = "select st.id,null,st.ServerType,st.id,'ServerTypes' from ServerTypes st,SelectedFeatures ft where ft.FeatureId=st.FeatureId and servertype='SNMP Devices'";
                dt = objAdaptor.FetchData(Query);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }

		public DataTable GetNetworkdeviceData()
        {
            DataTable dt = new DataTable();
            try
            {
                //5/5/2016 NS modified
                //Changed -ND to -Network Device
                string Query = "select nd.Name, nd.Imageurl, st.StatusCode,st.Lastupdate from [Network Devices] nd, Status st where st.TypeANDName= nd.Name+'-Network Device'";
                dt = objAdaptor.FetchData(Query);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }
        public DataTable GetCloudData()
        {
            DataTable dt = new DataTable();
            try
            {
                string Query = "select cd.Name, cd.Imageurl, cd.URL,st.Status,cd.LastChecked from CloudDetails cd, Status st where st.TypeANDName= cd.Name+'-Cloud'";
                dt = objAdaptor.FetchData(Query);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }
        public DataTable GetDockData(int id)
        {
            DataTable dt = new DataTable();
            try
            {
                string Query = "select * from users where ID='"+id+"'";
                dt = objAdaptor.FetchData(Query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public void savedock(int index1, int index2,int index3,int index4, int UserID,string cloudzone,string premiseszone,string networkzone,string dockzone,string s)
        {
            
            //2/14/2013 NS modified
            int mode = 0;
            try
            {
                int count = 0;
                string Query = "SELECT * FROM Users WHERE   ID =" + UserID;
                count = objAdaptor.ExecuteScalar(Query);
                string SqlQuery = "";
                if (count == 0)
                {
                    SqlQuery = "INSERT INTO Users([cloudindex],[premisesindex],[networkindex],[dockindex],[cloudzone],[premisesZone],[networkZone],[DockZone],[Seesion]) VALUES('" + index1 + "','" + index2 + "','" + index3 + "','" + index4 + "','" + cloudzone + "','" + premiseszone + "','" + networkzone + "','" + dockzone + "','"+s+"')";
                }
                else
                {
                    SqlQuery = "UPDATE Users SET cloudindex='" + index1 + "',premisesindex='" + index2 + "',networkindex='" + index3 + "',dockindex='" + index4 + "',cloudzone='" + cloudzone + "' ,premisesZone='" + premiseszone + "',networkZone='" + networkzone + "',DockZone='" + dockzone + "',Seesion='"+s+"' WHERE   ID =" + UserID;
                }

                mode = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
                //SqlQuery = "SELECT * FROM UserPreferences WHERE UserID =" + UserID;
                //return objAdaptor.FetchData(SqlQuery);

            }



            catch (Exception ex)
            {
                throw ex;
            }
        }
		public DataTable GetSelctedData(int id)
		{
			DataTable dt = new DataTable();
			try
			{
				string Query = "select * from UserPreferences where UserID='" + id + "' and PreferenceName='FilterBy'";
				dt = objAdaptor.FetchData(Query);
			
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}
		//2/10/2016 Durga Added for VSPLUS-2595
		public void SaveOveralldock(Users Usersobj, string s, int UserID)
		{

			int mode = 0;
			try
			{
			
				
				string SqlQuery = "";


				SqlQuery = "UPDATE Users SET cloudindex='" + Usersobj.cloudindex + "',premisesindex='" + Usersobj.premisesindex + "',networkindex='" + Usersobj.networkindex + "',dockindex='" + Usersobj.dockindex + "',TravelerIndex='" + Usersobj.TravelerIndex + "',KeyUserDevicesIndex='" + Usersobj.KeyUserDevicesIndex + "',StatusIndex='" + Usersobj.StatusIndex + "',cloudzone='" + Usersobj.cloudZone + "' ,premisesZone='" + Usersobj.premisesZone + "',networkZone='" + Usersobj.networkZone + "',DockZone='" + Usersobj.DockZone + "', "
				+ "TravelerZone='" + Usersobj.TravelerZone + "',KeyUserDevicesZone='" + Usersobj.KeyUserDevicesZone + "',StatusZone='" + Usersobj.StatusZone + "' WHERE   ID =" + UserID;
				
				mode = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
				
			}



			catch (Exception ex)
			{
				throw ex;
			}
		}
    }
}