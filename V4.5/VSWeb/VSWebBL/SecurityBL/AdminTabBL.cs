using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.SecurityBL
{
    public class AdminTabBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private static AdminTabBL _self = new AdminTabBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static AdminTabBL Ins
        {
            get { return _self; }
        }

        public DataTable ServerAllowUpdateGrid(string ServerID)
        {
			try
			{
				return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.ServerAllowUpdateGrid(ServerID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        public DataTable ServerRestrictUpdateGrid(string ServerID)
        {
			try
			{
				return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.ServerRestrictUpdateGrid(ServerID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public void AssignServerTypeRestrictions(string ServerNameGrid, string ServerTypeGrid, string ServerType, string FullName)
        {
			try
			{
				VSWebDAL.SecurityDAL.AdminTabDAL.Ins.AssignServerTypeRestrictions(ServerNameGrid, ServerTypeGrid, ServerType, FullName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable ServerTypeVisibleUpdateListBox()
        {
			try
			{
				return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.ServerTypeVisibleUpdateListBox();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        public DataTable LocVisibleUpdateListBox()
        {
			try
			{
				return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.LocVisibleUpdateListBox();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable ServersVisibleUpdateGrid(string UserName)
        {
			try
			{
				return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.ServersVisibleUpdateGrid(UserName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable ServersNotVisibleUpdateGrid(string UserName)
        {
			try
			{
				return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.ServersNotVisibleUpdateGrid(UserName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
        
        }
        public DataTable UserLocationVisibleUpdateListBox(string UserName)
		{
			try
			{
				return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.UserLocationVisibleUpdateListBox(UserName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        public DataTable UserLocationNotVisibleUpdateListBox(string UserName)
        {
			try
			{
				return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.UserLocationNotVisibleUpdateListBox(UserName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable UserServerTypeVisibleUpdateListBox(string UserName)
        {
			try
			{
				return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.UserServerTypeVisibleUpdateListBox(UserName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        public DataTable UserServerTypeNotVisibleUpdateListBox(string UserName)
        {
			try
			{
				return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.UserServerTypeNotVisibleUpdateListBox(UserName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        public DataTable NavigatorVisibleUpdateTree()
        {
			try
			{
				return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.NavigatorVisibleUpdateTree();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        /*
        public Object UpdateData(ServerTaskSettings STSettingsObject)
        {
            return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.UpdateData(STSettingsObject);
        }*/

        public bool InsertRestricted_Locations(string Uname, string Location)
        {
			try
			{
				return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.InsertRestricted_Locations(Uname, Location);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           

        }
        public bool InsertRestricted_Location(string Uname, string Location, int i)
        {
			try
			{
				return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.InsertRestricted_Location(Uname, Location, i);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            

        }
        public bool InsertRestricted_Servers(string Uname, Dictionary<string, string> Servername)
        {
			try
			{
				return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.InsertRestricted_Servers(Uname, Servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public bool InsertRestricted_Menus(string Uname, string Menuname)
        {
			try
			{
				return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.InsertRestricted_Menus(Uname, Menuname);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
		public DataTable GetNavigatorByUserID(int UserId, string Level, string MenuArea)
		{
			try
			{
				return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.GetNavigatorByUserID(UserId, Level, MenuArea);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
        public DataTable GetLevel3Menus(string MenuArea)
        {
			try
			{
				return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.GetLevel3Menus(MenuArea);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable GetRestrictedNavigatorByUserID(string UserName, string Level)
        {
			try
			{
				return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.GetRestrictedNavigatorByUserID(UserName, Level);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable GetNavigatorChildsByRefName(string RefName)
        {
			try
			{
				return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.GetNavigatorChildsByRefName(RefName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable GetNavigatorByDisplayText(string DisplayText)
        {
			try
			{
				return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.GetNavigatorByDisplayText(DisplayText);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable Getserversbyloction(string LocName)
        {
			try
			{
				return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.GetServersByLocation(LocName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }
        public DataTable GetServersByLocations(string LocNames)
        {
			try
			{
				return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.GetServersByLocations(LocNames);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable GetRPRAccessData()
        {
			try
			{
				return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.GetRPRAccessData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable Getserversbyloction1(string LocName)
        {
			try
			{
				return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.GetServersByLocation1(LocName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
		public DataTable GetNavigatorForDashboardOnly()
		{
			try
			{


				DataTable dt = VSWebDAL.SecurityDAL.AdminTabDAL.Ins.GetNavigatorForDashboardOnly();

				for (int i = 0; i < dt.Rows.Count; i++)
				{
					DataRow row = dt.Rows[i];
					if (row["DisplayText"].ToString() == "Logout" || row["DisplayText"].ToString() == "My Account")
						row["DisplayText"] = "Login";

					if (row["DisplayText"].ToString() == "EXJournal Summary")
					{
						string ExJournalEnabled = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Enable ExJournal");
						if (ExJournalEnabled != "true")
						{
							dt.Rows.Remove(row);
							i--;
						}

					}
				}

				return dt;
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

        //9/23/2015 NS added
        public DataTable GetAllServersNotVisible(string UserName)
        {
            return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.GetAllServersNotVisible(UserName);
        }

		public bool AnyNodeAlive()
		{
			return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.AnyNodeAlive();
		}
    }
}
