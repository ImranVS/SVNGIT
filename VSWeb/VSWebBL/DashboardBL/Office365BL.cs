using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDAL;
namespace VSWebBL.DashboardBL
{
	public class Office365BL
	{
		private static Office365BL _self = new Office365BL();

		/// <summary>
		/// Used to call the functions using class name instead of object
		/// </summary>
		public static Office365BL Ins
		{
			get { return _self; }
		}

		public DataTable FillMailboxChart()
		{
			try
			{
				return VSWebDAL.DashboardDAL.Office365DAL.Ins.FillMailboxChart();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable GetOffice365Userlicensesstatus()
		{
			try
			{
				return VSWebDAL.DashboardDAL.Office365DAL.Ins.GetOffice365Userlicensesstatus();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable Groupgrid()
		{
			try
			{
				return VSWebDAL.DashboardDAL.Office365DAL.Ins.Groupgrid();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable Usersettingsgrid()
		{
			try
			{
				return VSWebDAL.DashboardDAL.Office365DAL.Ins.Usersettingsgrid();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public DataTable PasswordsChart()
		{
			try
			{
				return VSWebDAL.DashboardDAL.Office365DAL.Ins.PasswordsChart();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable PasswordsNeverexpiresChart()
		{
			try
			{
				return VSWebDAL.DashboardDAL.Office365DAL.Ins.PasswordsNeverexpiresChart();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		
		public DataTable FillDevicesChart()
		{
			try
			{
				return VSWebDAL.DashboardDAL.Office365DAL.Ins.FillDevicesChart();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable FillMailboxUsage()
		{
			try
			{
				return VSWebDAL.DashboardDAL.Office365DAL.Ins.FillMailboxUsage();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable FillStatListView(string Type,string serverName )
		{
			try
			{
				return VSWebDAL.DashboardDAL.Office365DAL.Ins.FillStatListView(Type,serverName );
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable SetGraphForMailFiles(string ServerName)
		{
			try
			{
	           return VSWebDAL.DashboardDAL.Office365DAL.Ins.SetGraphForMailFiles(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
		
		}
		public DataTable SetGraphDeviceTypes(string ServerName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.Office365DAL.Ins.SetGraphDeviceTypes(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable SetGraphLastLogonUsers(string Name)
		{
			try
			{
				return VSWebDAL.DashboardDAL.Office365DAL.Ins.SetGraphLastLogonUsers(Name);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
		
		}
		public DataTable SetGraphForSyncType(string Name)
		{

			return VSWebDAL.DashboardDAL.Office365DAL.Ins.SetGraphForSyncType(Name);
		}
		public DataTable SetGraphP2PSessions(string Name)
		{
			try
			{
				return VSWebDAL.DashboardDAL.Office365DAL.Ins.SetGraphP2PSessions(Name);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable SetGraphAVSessions(string Name)
		{
			try
			{
				return VSWebDAL.DashboardDAL.Office365DAL.Ins.SetGraphAVSessions(Name);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable SetGraphConfReport(string Name)
		{
			try
			{
				return VSWebDAL.DashboardDAL.Office365DAL.Ins.SetGraphConfReport(Name);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
        //1/15/2015 NS added for VSPLUS-1316
		public DataTable GetHealthAssessmentStatusDetails(string TypeAndName, string servername)
        {
			try
			{
				return VSWebDAL.DashboardDAL.Office365DAL.Ins.GetHealthAssessmentStatusDetails(TypeAndName,servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable GetNodeName(int ID,string Accountname2)
        {
            try
            {
                return VSWebDAL.DashboardDAL.Office365DAL.Ins.GetNodeName(ID,Accountname2);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }




        public DataTable get0635grid(string Name)
        {
            try
            {
                return VSWebDAL.DashboardDAL.Office365DAL.Ins.get0635grid(Name);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        //1/16/2015 NS added for VSPLUS-1316
        public DataTable GetMailTestsResponseTimes(string nodename,string servername)
        {
			try
			{
                return VSWebDAL.DashboardDAL.Office365DAL.Ins.GetMailTestsResponseTimes(nodename,servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }
		public DataTable GetDirSyncStats(string nodename, string accountName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.Office365DAL.Ins.GetDirSyncStats(nodename,accountName);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
        //1/16/2015 NS added for VSPLUS-1316
        public DataTable GetMailScenarioTestsResponseTimes(string nodename,string servername)
        {
			try
			{
                return VSWebDAL.DashboardDAL.Office365DAL.Ins.GetMailScenarioTestsResponseTimes(nodename,servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        //1/20/2015 NS added for VSPLUS-1316
        public DataTable GetOneDriveStats(string nodename,string servername)
        {
			try
			{
                return VSWebDAL.DashboardDAL.Office365DAL.Ins.GetOneDriveStats(nodename,servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        //1/20/2015 NS added for VSPLUS-1316
        public DataTable GetSiteTestsResponseTimes(string nodename,string servername)
        {
			try
			{
                return VSWebDAL.DashboardDAL.Office365DAL.Ins.GetSiteTestsResponseTimes(nodename,servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
		public DataTable ResponseThreshold(string ServerName, string NodeName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.Office365DAL.Ins.ResponseThreshold(ServerName, NodeName);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
        //1/20/2015 NS added for VSPLUS-1316
        public DataTable GetTaskFolderTestsResponseTimes(string nodename, string servername)
        {
			try
			{
                return VSWebDAL.DashboardDAL.Office365DAL.Ins.GetTaskFolderTestsResponseTimes(nodename,servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        //2/25/2016 Durga Modified for  VSPLUS-2611
		public DataTable GetOffice365Mails()
		{
			return VSWebDAL.DashboardDAL.Office365DAL.Ins.GetOffice365Mails();
		}
        //2/26/2016 NS added for VSPLUS-2648
        public DataTable GetOffice365Tests(string aname)
        {
            return VSWebDAL.DashboardDAL.Office365DAL.Ins.GetOffice365Tests(aname);
        }
        //3/9/2016 NS added for VSPLUS2648
        public DataTable GetMailBoxTypes(string servername)
        {
            return VSWebDAL.DashboardDAL.Office365DAL.Ins.GetMailBoxTypes(servername);
        }

        public DataTable GetUsersDailyCount(string statname, string servername, string fromdate="", string todate="")
        {
            return VSWebDAL.DashboardDAL.Office365DAL.Ins.GetUsersDailyCount(statname, servername,fromdate,todate);
        }

        public DataTable GetInactiveUsers(string servername)
        {
            return VSWebDAL.DashboardDAL.Office365DAL.Ins.GetInactiveUsers(servername);
        }

        public DataTable GetActiveInactiveUsers(string servername)
        {
            return VSWebDAL.DashboardDAL.Office365DAL.Ins.GetActiveInactiveUsers(servername);
        }

        public DataTable GetInactiveMailboxes(string servername)
        {
            return VSWebDAL.DashboardDAL.Office365DAL.Ins.GetInactiveMailboxes(servername);
        }

        //3/21/2016 NS added for VSPLUS-2652
        public DataTable GetOffice365Servers()
        {
            return VSWebDAL.DashboardDAL.Office365DAL.Ins.GetOffice365Servers();
        }

        public DataTable GetOffice365PwdExpSettings(string servername)
        {
            return VSWebDAL.DashboardDAL.Office365DAL.Ins.GetOffice365PwdExpSettings(servername);
        }

        public DataTable GetOffice365PwdStrongSettings(string servername)
        {
            return VSWebDAL.DashboardDAL.Office365DAL.Ins.GetOffice365PwdStrongSettings(servername);
        }

        public DataTable GetOffice365UserSettings(string servername)
        {
            return VSWebDAL.DashboardDAL.Office365DAL.Ins.GetOffice365UserSettings(servername);
        }
		public DataTable GetOffice365UserServicedetails(string servername)
		{
			return VSWebDAL.DashboardDAL.Office365DAL.Ins.GetOffice365UserServicedetails(servername);
		}

        public DataTable GetOffice365GroupCount()
        {
            return VSWebDAL.DashboardDAL.Office365DAL.Ins.GetOffice365GroupCount();
        }

        public DataTable GetLicensesInfo(string ServerName)
        {
            return VSWebDAL.DashboardDAL.Office365DAL.Ins.GetLicensesInfo(ServerName);
        }

        public DataTable GetInactiveUsersCount(string ServerName)
        {
            return VSWebDAL.DashboardDAL.Office365DAL.Ins.GetInactiveUsersCount(ServerName);
        }
        public DataTable GetO365Users()
        {
            try
            {
                return VSWebDAL.DashboardDAL.Office365DAL.Ins.GetO365Users();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
       
        public DataTable GetUserCommonGroups(string UserName1, string UserName2)
        {
            try
            {
                return VSWebDAL.DashboardDAL.Office365DAL.Ins.GetUserCommonGroups(UserName1, UserName2);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
	}
}
