using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDAL;
using System.Data;
using VSWebDO;

namespace VSWebBL.ConfiguratorBL
{
    public class UserPreferencesBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private static UserPreferencesBL _self = new UserPreferencesBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static UserPreferencesBL Ins
        {
            get { return _self; }
        }
        public void UpdateUserPreferences(string PreferenceName, string PreferenceValue, int UserID)
        {
			try
			{
			   VSWebDAL.ConfiguratorDAL.UserPreferencesDAL.Ins.UpdateUserPreferences(PreferenceName, PreferenceValue, UserID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
		
        public DataTable GetUserRowPrefrenceDetails(int UserID)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.UserPreferencesDAL.Ins.GetUserRowPrefrenceDetails(UserID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
	
		public DataTable Getonpremiseswithuser( string loginname)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.UserPreferencesDAL.Ins.Getonpremiseswithuser(loginname);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
		
		}


		public DataTable getAssemblyVersionInfo()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.UserPreferencesDAL.Ins.getAssemblyVersionInfo();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

        //2/11/2016 NS modified for VSPLUS-2594
		public DataTable GetDatabaseVersionInfo(string category = "")
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.UserPreferencesDAL.Ins.GetDatabaseVersionInfo(category);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
        public DataTable filldropdown()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.UserPreferencesDAL.Ins.fillddldata();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable filldropdown_CredentialsComboBox()

		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.UserPreferencesDAL.Ins.fillddldata_CredentialsComboBox();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
        public DataTable getimagepath(string name)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.UserPreferencesDAL.Ins.getimagepath(name);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
		public DataTable getimagepathfornetwork(string name)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.UserPreferencesDAL.Ins.getimagepathfornetwork(name);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

        //Mukund VSPlus-1020,14Oct14
        public DataTable IsCloudSelected()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.UserPreferencesDAL.Ins.IsCloudSelected();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
        
        }
		
		public DataTable IsNetworkdeviceSelected()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.UserPreferencesDAL.Ins.IsNetworkdeviceSelected();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
        }
		public DataTable IsSNMPSelected()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.UserPreferencesDAL.Ins.IsSNMPSelected();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
        }


		public DataTable GetNetworkdeviceData()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.UserPreferencesDAL.Ins.GetNetworkdeviceData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
        }
		
        public DataTable GetCloudData()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.UserPreferencesDAL.Ins.GetCloudData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable GetDockData(int id)
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.UserPreferencesDAL.Ins.GetDockData(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public void savedock(int index1, int index2, int index3, int index4, int UserID,string cloudzone,string premiseszone,string networkzone,string dockzone,string s)
        {
            try
            {
                VSWebDAL.ConfiguratorDAL.UserPreferencesDAL.Ins.savedock(index1, index2,index3,index4, UserID,cloudzone,premiseszone,networkzone,dockzone,s);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
		//2/10/2016 Durga Added for VSPLUS-2595
		public void SaveOveralldock(Users Usersobj, string s, int UserID)
		{
			try
			{
				VSWebDAL.ConfiguratorDAL.UserPreferencesDAL.Ins.SaveOveralldock(Usersobj, s, UserID);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetSelctedData(int UserID)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.UserPreferencesDAL.Ins.GetSelctedData(UserID);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
	
	
	}
}