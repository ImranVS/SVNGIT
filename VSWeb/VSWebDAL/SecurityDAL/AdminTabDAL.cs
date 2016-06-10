using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebDAL.SecurityDAL
{
    public class AdminTabDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static AdminTabDAL _self = new AdminTabDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static AdminTabDAL Ins
        {
            get { return _self; }
        }

        // public DataTable ServerAllowUpdateGrid(string locationID )
        public DataTable ServerAllowUpdateGrid(string ServerID)
        {
            DataTable ServerAllowDataTable = new DataTable();
            try
            {
                string SqlQuery = "SELECT t1.ID, ServerName, Location, ServerType FROM Servers t1 " +
                    "INNER JOIN Locations t2 ON LocationID = t2.ID INNER JOIN ServerTypes t3 ON ServerTypeID = t3.ID ";
                if (!ServerID.Equals(""))
                {
                    SqlQuery += "WHERE t1.ID NOT IN (" + ServerID + ") ";
                }
                SqlQuery += "ORDER BY ServerName";
                ServerAllowDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
			
            finally
            {
            }
            return ServerAllowDataTable;
        }

        public DataTable ServerRestrictUpdateGrid(string ServerID)
        {
            DataTable ServerRestrictDataTable = new DataTable();
            try
            {
                string SqlQuery = "SELECT t1.ID, ServerName, Location, ServerType FROM Servers t1 " +
                    "INNER JOIN Locations t2 ON LocationID = t2.ID INNER JOIN ServerTypes t3 ON ServerTypeID = t3.ID ";
                if (!ServerID.Equals(""))
                {
                    SqlQuery += "WHERE ID IN (" + ServerID + ") ";
                }
                SqlQuery += "ORDER BY ServerName";
                ServerRestrictDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
			
            finally
            {
            }
            return ServerRestrictDataTable;
        }

        public void AssignServerTypeRestrictions(string ServerNameGrid, string ServerTypeGrid, string ServerType, string FullName)
        {
            string[] ServerNamesGrid = ServerNameGrid.Split(';');
            string[] ServerTypesGrid = ServerTypeGrid.Split(';');
            string[] ServerTypes = ServerType.Split(';');
            string SqlQuery = "";
            DataTable ServerTypeRestrictionsDataTable = new DataTable();
            try
            {
                SqlQuery = "SELECT * FROM UserServerTypeRestrictions WHERE FullName='" + FullName + "'";
                ServerTypeRestrictionsDataTable = objAdaptor.FetchData(SqlQuery);
                if (ServerTypeRestrictionsDataTable.Rows.Count > 0)
                {
                    SqlQuery = "DELETE FROM UserServerTypeRestrictions WHERE FullName='" + FullName + "'";
                    objAdaptor.ExecuteNonQuery(SqlQuery);
                    SqlQuery = "DELETE FROM UserServerRestrictions WHERE FullName='" + FullName + "'";
                    objAdaptor.ExecuteNonQuery(SqlQuery);
                }
                for (int i = 0; i < ServerTypes.Length; i++)
                {
                    SqlQuery = "INSERT INTO UserServerTypeRestrictions (FullName,ServerType) VALUES('" + FullName + "','" + ServerTypes[i] + "')";
                    objAdaptor.ExecuteNonQuery(SqlQuery);
                }
                for (int i = 0; i < ServerNamesGrid.Length; i++)
                {
                    SqlQuery = "INSERT INTO UserServerRestrictions (FullName,ServerName,ServerType) VALUES('" + FullName + "','" + ServerNamesGrid[i] + "','" + ServerTypesGrid[i] + "')";
                    objAdaptor.ExecuteNonQuery(SqlQuery);
                }

            }
			catch (Exception ex)
			{
				throw ex;
			}
			
            finally
            {
            }

        }

        public DataTable LocVisibleUpdateListBox()
        {
            DataTable LocationsDataTable = new DataTable();
            try
            {
                string SqlQuery = "SELECT ID, Location FROM Locations ORDER BY Location";
                LocationsDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return LocationsDataTable;
        }

        public DataTable ServerTypeVisibleUpdateListBox()
        {
            DataTable ServerTypesDataTable = new DataTable();
            try
            {
                string SqlQuery = "SELECT ServerType FROM ServerTypes ORDER BY ServerType";
                ServerTypesDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return ServerTypesDataTable;
        }

        public DataTable ServersVisibleUpdateGrid(string UserName)
        {
            DataTable UserServerRestrictionsDataTable = new DataTable();
            try
            {
                //string SqlQuery = "SELECT t1.ID, ServerName, Location, ServerType FROM Servers t1 " +
                //    "INNER JOIN Locations t2 ON LocationID = t2.ID INNER JOIN ServerTypes t3 ON ServerTypeID = t3.ID " +
                //    "WHERE t1.ID NOT IN " +
                //        "(SELECT ServerID FROM UserServerRestrictions INNER JOIN Users ON UserID = ID " +
                //        "WHERE FullName='" + UserName + "') " +
                //    "AND t2.ID NOT IN " +
                //        "(SELECT LocationID FROM UserLocationRestrictions INNER JOIN Users ON UserID = ID " +
                //        "WHERE FullName='" + UserName + "') " +
                //    "AND t3.ID NOT IN " +
                //        "(SELECT ServerTypeID FROM UserServerTypeRestrictions INNER JOIN Users ON UserID = ID " +
                //        "WHERE FullName='" + UserName + "') " +
                //    "ORDER BY ServerName ";
                //UserServerRestrictionsDataTable = objAdaptor.FetchData(SqlQuery);


                //9/22/2015 NS modified
                string sqlQuery = "select t1.ID, t2.ID LocId, ServerName Name, Description, Location, ServerType from servers t1 " +
                  "inner join locations t2 on locationid = t2.id inner join servertypes t3 on servertypeid = t3.id " +
                  "where t1.id   not in " +
                      "(select serverid from userserverrestrictions inner join users on userid = id " +
                      "where fullname='" + UserName + "') " +
                    //"and t2.id  in " +
                    //    "(select locationid from userlocationrestrictions inner join users on userid = id " +
                    //    "where fullname='" + UserName + "') " +
                    //"and t3.id  in " +
                    //    "(select servertypeid from userservertyperestrictions inner join users on userid = id " +
                    //    "where fullname='" + UserName + "') " +
                  "order by servername ";




                UserServerRestrictionsDataTable = objAdaptor.FetchData(sqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return UserServerRestrictionsDataTable;
        }

        public DataTable ServersNotVisibleUpdateGrid(string UserName)
        {
            DataTable UserServerRestrictionsDataTable = new DataTable();
            try
            {
                /* string SqlQuery = "SELECT t1.ID, ServerName, Location, ServerType FROM Servers t1 " + 
                     "INNER JOIN Locations t2 ON LocationID = t2.ID INNER JOIN ServerTypes t3 ON ServerTypeID = t3.ID " + 
                     "WHERE t1.ID IN " + 
                         "(SELECT ServerID FROM UserServerRestrictions INNER JOIN Users ON UserID = ID " + 
                         "WHERE FullName='" + UserName + "') " + 
                     "ORDER BY ServerName ";*/

                string SqlQuery = "SELECT t1.ID, ServerName, Location, ServerType FROM Servers t1 " +
                   "INNER JOIN Locations t2 ON LocationID = t2.ID INNER JOIN ServerTypes t3 ON ServerTypeID = t3.ID " +
                   "WHERE t1.ID  IN " +
                       "(SELECT ServerID FROM UserServerRestrictions INNER JOIN Users ON UserID = ID " +
                       "WHERE FullName='" + UserName + "') " +
                    //"AND t2.ID  IN " +
                    //    "(SELECT LocationID FROM UserLocationRestrictions INNER JOIN Users ON UserID = ID " +
                    //    "WHERE FullName='" + UserName + "') " +
                    //"AND t3.ID  IN " +
                    //    "(SELECT ServerTypeID FROM UserServerTypeRestrictions INNER JOIN Users ON UserID = ID " +
                    //    "WHERE FullName='" + UserName + "') " +
                   "ORDER BY ServerName ";




                UserServerRestrictionsDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return UserServerRestrictionsDataTable;
        }

        public DataTable UserLocationVisibleUpdateListBox(string UserName)
        {
            DataTable LocationDataTable = new DataTable();
            try
            {
                string SqlQuery = "SELECT Location FROM Locations " +
                    "WHERE ID NOT IN " +
                    "(SELECT LocationID FROM UserLocationRestrictions " +
                    "WHERE UserID=(SELECT ID FROM Users where FullName= '" + UserName + "'))";
                LocationDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return LocationDataTable;
        }

        public DataTable UserLocationNotVisibleUpdateListBox(string UserName)
        {
            DataTable LocationDataTable = new DataTable();
            try
            {
                //string SqlQuery = "SELECT Location FROM  UserLocationRestrictions " +
                //    "WHERE LocationID=(SELECT ID FROM Users Where FullName='" + UserName + "') ";

                string SqlQuery = "SELECT Location FROM Locations " +
                   "WHERE ID IN " +
                   "(SELECT LocationID FROM UserLocationRestrictions " +
                   "WHERE UserID=(SELECT ID FROM Users where FullName= '" + UserName + "'))";
                LocationDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return LocationDataTable;
        }

        public DataTable UserServerTypeVisibleUpdateListBox(string UserName)
        {
            DataTable ServerTypeDataTable = new DataTable();
            try
            {
                string SqlQuery = "SELECT ServerType FROM ServerTypes " +
                    "WHERE ID NOT IN " +
                    "(SELECT ServerTypeID FROM UserServerTypeRestrictions " +
                    "WHERE UserID=(SELECT ID from Users WHERE FullName='" + UserName + "') )";
                ServerTypeDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return ServerTypeDataTable;
        }

        public DataTable UserServerTypeNotVisibleUpdateListBox(string UserName)
        {
            DataTable ServerTypeDataTable = new DataTable();
            try
            {
                string SqlQuery = "SELECT ServerType FROM ServerTypes WHERE ID=(SELECT ServerTypeID FROM " +
                    " UserServerTypeRestrictions WHERE UserID=(SELECT ID FROM Users " +
                    "WHERE FullName='" + UserName + "' ))";
                ServerTypeDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return ServerTypeDataTable;
        }

        public DataTable NavigatorVisibleUpdateTree()
        {
            DataTable NavigatorDataTable = new DataTable();
            try
            {
                string SqlQuery = "SELECT t1.ID, DisplayText, OrderNum, ParentID, PageLink FROM Menuitems t1 where [Level]<3 and Level<>0";
                SqlQuery += "ORDER BY t1.ID";
                NavigatorDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return NavigatorDataTable;
        }


        //added code june-13 by shilpa
        
        public bool InsertRestricted_Locations(string Uname, string Location)
        {
            bool Insert = false;
            string[] Locations = Location.Split(',');
            try
            {
                string SqlQuery = "Delete from UserLocationRestrictions where UserID=(SELECT ID from Users where FullName='" + Uname + "')";
                Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
                if (Location != "")
                {
                    for (int i = 0; i < Locations.Length; i++)
                    {
                        //9/25/2015 NS modified
                        //SqlQuery = "INSERT INTO UserLocationRestrictions(UserID,LocationID) VALUES ((SELECT ID from Users where FullName='" + Uname + "')" +
                        //",(SELECT ID from Locations where Location=" + Locations[i] + "))";
                        SqlQuery = "INSERT INTO UserLocationRestrictions(UserID,LocationID) VALUES ((SELECT ID from Users where FullName='" + Uname + "')" +
                            "," + Locations[i] + ")";
                        Insert = objAdaptor.ExecuteNonQuery(SqlQuery);

                    }
                }
                else
                {
                    Insert = true;
                }
            }
            catch
            {
                Insert = false;

            }
            return Insert;
        }


        public bool InsertRestricted_Location(string Uname, string Location, int i)
        {
            bool Insert = false;
            //string[] Locations = Location.Split(',');
            try
            {

                if (i == 0)
                {
                    string SqlQuery = "Delete from UserLocationRestrictions where UserID=(SELECT ID from Users where FullName='" + Uname + "')";
                    Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
                }
                if (Location.ToString() != "")
                {
                    string SqlQueryinsert = "INSERT INTO UserLocationRestrictions(UserID,LocationID) VALUES ((SELECT ID from Users where FullName='" + Uname + "')" +
                   ",(SELECT ID from Locations where Location='" + Location + "'))";

                    Insert = objAdaptor.ExecuteNonQuery(SqlQueryinsert);

                }
            }
            catch
            {
                Insert = false;

            }


            return Insert;
        }

        public bool InsertRestricted_Servers(string Uname, Dictionary<string, string> Servername)
        {
            bool InsertServer = false;
            //9/17/2015 NS modified
            //string[] Servers = Servername.Split(',');
            string separator = "###$$$###";
            string servertype = "";
            try
            {
                string sqlQuery = "Delete from UserServerRestrictions where UserID=(SELECT ID from Users where FullName='" + Uname + "')";
                InsertServer = objAdaptor.ExecuteNonQuery(sqlQuery);
                if (Servername.Count > 0)
                {
                    foreach (var item in Servername)
                    {
                        servertype = (item.Key).Substring(0, (item.Key).IndexOf(separator));
                        sqlQuery = "INSERT INTO UserServerRestrictions(UserID,ServerID) VALUES ((SELECT ID from Users where FullName='" + Uname + "')" +
                           ",(SELECT t1.ID FROM Servers t1 INNER JOIN ServerTypes t2 ON t1.ServerTypeID=t2.ID AND t2.ServerType=" +
                           servertype + " WHERE ServerName=" + item.Value + "))";
                        InsertServer = objAdaptor.ExecuteNonQuery(sqlQuery);
                    }
                }
                else
                {
                    InsertServer = true;
                }
            }
            catch
            {
                InsertServer = false;
            }

            return InsertServer;
        }
        public bool InsertRestricted_Menus(string Uname, string Menuname)
        {
            bool InsertServer = false;
            if (Menuname != "" && Menuname != null)
            {
                string[] Menus = Menuname.Split(',');
                try
                {
                    string sqlQuery = "Delete from UserMenuRestrictions where UserID=(SELECT ID from Users where FullName='" + Uname + "')";
                    InsertServer = objAdaptor.ExecuteNonQuery(sqlQuery);

                    for (int i = 0; i < Menus.Length; i++)
                    {
                        sqlQuery = "INSERT INTO UserMenuRestrictions(UserID,MenuID) VALUES ((SELECT ID from Users where FullName='" + Uname + "')" +
                        "," + Menus[i] + ")";
                        InsertServer = objAdaptor.ExecuteNonQuery(sqlQuery);
                    }
                }
                catch
                {

                    InsertServer = false;
                }
            }
            else
            {
                string sqlQuery = "Delete from UserMenuRestrictions where UserID=(SELECT ID from Users where FullName='" + Uname + "')";
                InsertServer = objAdaptor.ExecuteNonQuery(sqlQuery);

                InsertServer = true;
            }

            return InsertServer;
        }


		//public DataTable GetNavigatorByUserID(int UserId, string Level, string MenuArea)
		//{
		//    DataTable NavigatorDataTable = new DataTable();
		//    try
		//    {
		//        //1/20/2014 NS modified the query
		//        //string SqlQuery = "SELECT * FROM Menuitems t1 where [Level]" + Level + (UserId == 0 ? "" : " and ID not in(select MenuID from UserMenuRestrictions where UserId=" + UserId + ")");

		//        //26Feb14 MD, modified the query, not to show main menus whose all of the submenus are restricted
		//        //string SqlQuery = "SELECT * FROM Menuitems t1 where [Level]" + Level + 
		//        //    (UserId == 0 ? "" : " and ID not in " +
		//        //    "(select MenuID from UserMenuRestrictions where UserId=" + UserId + ") ") + 
		//        //    " ORDER BY ParentID, OrderNum";
		//        string SqlQuery = "SELECT distinct t1.* FROM Menuitems t1,SelectedFeatures sf,FeatureMenus fm where fm.FeatureID=sf.FeatureID and fm.MenuID=t1.ID and  MenuArea='" + MenuArea + "' and  [Level]" + Level +
		//            " and ID not in (select MenuID from UserMenuRestrictions where UserId=" + UserId + ")  ORDER BY ParentID, OrderNum";
		//        NavigatorDataTable = objAdaptor.FetchData(SqlQuery);
		//    }
		//    catch
		//    {
		//    }
		//    finally
		//    {
		//    }
		//    return NavigatorDataTable;
		//}
		public DataTable GetNavigatorByUserID(int UserId, string Level, string MenuArea)
		{
			DataTable NavigatorDataTable = new DataTable();
			try
			{
				NavigatorDataTable = objAdaptor.GetNavigatorByUserID("sp_MenuSorting", UserId, Level, MenuArea);
				
			}
			catch
			{
			}
			finally
			{
			}
			return NavigatorDataTable;
		}
        public DataTable GetLevel3Menus(string MenuArea)
        {
            DataTable NavigatorDataTable = new DataTable();
            try
            {
				string SqlQuery = "SELECT * FROM Menuitems t1 where [Level]=3 and  MenuArea='" + MenuArea + "' ORDER BY parentid,OverrideSort,OrderNum";
                NavigatorDataTable = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            finally
            {
            }
            return NavigatorDataTable;
        }

        public DataTable GetNavigatorChildsByRefName(string RefName)
        {
            DataTable NavigatorDataTable = new DataTable();
            try
            {
                string SqlQuery = "SELECT t1.* FROM Menuitems t1,Menuitems t2  where t1.ParentID=t2.ID and t2.RefName='" + RefName + "'";

                NavigatorDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return NavigatorDataTable;
        }
        public DataTable GetNavigatorByDisplayText(string DisplayText)
        {
            DataTable NavigatorDataTable = new DataTable();
            try
            {
                string SqlQuery = "SELECT t1.* FROM Menuitems t1 where t1.DisplayText='" + DisplayText + "'";

                NavigatorDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return NavigatorDataTable;
        }
        public DataTable GetRestrictedNavigatorByUserID(string UserName, string Level)
        {
            DataTable NavigatorDataTable = new DataTable();
            try
            {
                //string SqlQuery = "SELECT * FROM Menuitems t1 where [Level]" + Level + " and ID in(select MenuID from UserMenuRestrictions where UserId=" +
                //    "(select id from Users where FullName='" + UserName + "'))" +
                //    " union " +
                //    "SELECT * FROM Menuitems t1 where ParentID in(select MenuID from UserMenuRestrictions where UserId=" +
                //    "(select id from Users where FullName='" + UserName + "'))"
                //    ;
                string SqlQuery = "Select * from Menuitems where ID in(select MenuID from UserMenuRestrictions where UserId in (select id from users where FullName='"+UserName+"'))";
                NavigatorDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return NavigatorDataTable;
        }
        public DataTable GetServersByLocation(string LocName)
        {
            DataTable ServersByLocation = new DataTable();
            try
            {
                //2/5/2014 NS modified the query - added sort by server name
                string Query = "Select ID,ServerName,(Select Location from Locations where ID=Servers.LocationID) " +
                    "as Location,(select ServerType from ServerTypes where ID=Servers.ServerTypeID) as ServerType " +
                    "from Servers where LocationID in(Select ID from Locations where Location='" + LocName + "') " +
                    "order by ServerName";
                ServersByLocation = objAdaptor.FetchData(Query);

            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return ServersByLocation;
        }

        public DataTable GetServersByLocations(string LocNames)
        {
            DataTable ServersByLocation = new DataTable();
            try 
            {
                //2/5/2014 NS modified the query - added sort by server name
                string Query = "Select ID,ServerName,(Select Location from Locations where ID=Servers.LocationID) " +
                    "as Location,(select ServerType from ServerTypes where ID=Servers.ServerTypeID) as ServerType " +
                    "from Servers where LocationID in(Select ID from Locations where Location in (" + LocNames + ")) " +
                    "order by ServerName";
                ServersByLocation = objAdaptor.FetchData(Query);

            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return ServersByLocation;
        }
        /*
        public Object UpdateData(ServerTaskSettings STSettingsObject)
        {
            Object Update;
            try
            {
                //string SqlQuery = "Update ServerTaskSettings SET Enabled= " + STSettingsObject.Enabled + 
                //    ", RestartOffHours=" + STSettingsObject.RestartOffHours + ", SendLoadCommand=" +
                //    STSettingsObject.SendLoadCommand + ", SendExitCommand=" + STSettingsObject.SendExitCommand +
                //    ", SendRestartCommand=" + STSettingsObject.SendRestartCommand + " " +
                //                "Where MyID=" + STSettingsObject.MyID + " AND TaskID=" + STSettingsObject.TaskID;

                string SqlQuery = "Update ServerTaskSettings SET Enabled= '" + STSettingsObject.Enabled +
                    "', RestartOffHours='" + STSettingsObject.RestartOffHours + "', SendLoadCommand='" +
                    STSettingsObject.SendLoadCommand + "', SendExitCommand='" + STSettingsObject.SendExitCommand +
                    "', SendRestartCommand='" + STSettingsObject.SendRestartCommand + "',TaskID=" + STSettingsObject.TaskID +
                                " Where MyID=" + STSettingsObject.MyID;

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
        }*/

        public DataTable GetRPRAccessData()
        {

            DataTable ReportDataTable = new DataTable();
            try
            {
                string SqlQuery = "select * from RPRAccessPages";
                ReportDataTable = objAdaptor.FetchData(SqlQuery);

            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return ReportDataTable;
        }

        public DataTable GetServersByLocation1(string LocName)
        {
            DataTable ServersByLocation = new DataTable();
            try
            {
                //2/5/2014 NS modified the query - added sort by server name
                string Query = "Select ID,ServerName,(Select Location from Locations where ID=Servers.LocationID) " +
                    "as Location,(select ServerType from ServerTypes where ID=Servers.ServerTypeID) as ServerType " +
                    "from Servers where LocationID in (Select ID from Locations where Location='" + LocName + "') " +
                    "order by ServerName";
                ServersByLocation = objAdaptor.FetchData(Query);

            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return ServersByLocation;
        }

		public DataTable GetNavigatorForDashboardOnly()
		{
			DataTable NavigatorDataTable = new DataTable();
			try
			{
				string SqlQuery = "SELECT * FROM Menuitems t1 where [Level]<=2 and  MenuArea='Dashboard' and id in (73,74,75,76,77,78,110,113) ORDER BY ParentID, OrderNum";
				NavigatorDataTable = objAdaptor.FetchData(SqlQuery);
			}
			catch
			{
			}
			finally
			{
			}
			return NavigatorDataTable;
		}

        //9/23/2015 NS added
        public DataTable GetAllServersNotVisible(string UserName)
        {
            DataTable UserServerRestrictionsDataTable = new DataTable();
            try
            {
                string SqlQuery = "SELECT t1.ID, ServerName AS Name, LocationID LocId, Location, ServerTypeID srvtypeid, ServerType FROM Servers t1 " +
                   "INNER JOIN Locations t2 ON LocationID = t2.ID INNER JOIN ServerTypes t3 ON ServerTypeID = t3.ID " +
                   "WHERE t1.ID  IN " +
                    "(SELECT ServerID FROM UserServerRestrictions INNER JOIN Users ON UserID = ID " +
                    "WHERE FullName='" + UserName + "') " +
                    "UNION " +
                    "SELECT t1.ID,t1.Name,t1.LocationID LocId,Location,t3.ID srvtypeid,t3.ServerType " +
					"FROM URLs t1 INNER JOIN Locations t2 ON LocationID = t2.ID,ServerTypes t3 " + 
					"WHERE t1.ServerTypeId=t2.ID AND t1.ID  IN " +
					"(SELECT ServerID FROM UserServerRestrictions INNER JOIN Users ON UserID = ID " +
                    "WHERE FullName='" + UserName + "') " +
                    "UNION " +
                    "SELECT t1.ID, ServerName AS Name, LocationID LocId, Location, ServerTypeID srvtypeid, ServerType FROM Servers t1 " +
                    "INNER JOIN Locations t2 ON LocationID = t2.ID INNER JOIN ServerTypes t3 ON ServerTypeID = t3.ID " +
                    "WHERE t2.ID IN " +
                    "(SELECT LocationID FROM UserLocationRestrictions INNER JOIN Users ON UserID = ID " +
                    "WHERE FullName='" + UserName + "') " +
                    "UNION " +
                    "SELECT t1.ID,t1.Name,t1.LocationID LocId,Location,t3.ID srvtypeid,t3.ServerType " +
					"FROM URLs t1 INNER JOIN Locations t2 ON LocationID = t2.ID INNER JOIN ServerTypes t3 ON t3.ID=t1.ServerTypeId " +
					"WHERE t2.ID  IN " +
					"(SELECT LocationID FROM UserLocationRestrictions INNER JOIN Users ON UserID = ID " +
                    "WHERE FullName='" + UserName + "') " + 
                    "ORDER BY Name ";
                UserServerRestrictionsDataTable = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return UserServerRestrictionsDataTable;
        }

		public bool AnyNodeAlive()
		{
			DataTable dt = new DataTable();
			try
			{
				string SqlQuery = "exec PR_RefreshServerCollection 0";
				dt = objAdaptor.FetchData(SqlQuery);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
			finally
			{
			}
		}
    }

}