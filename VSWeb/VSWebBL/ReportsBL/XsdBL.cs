using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;
using VSWebDAL.ReportsDAL;
using System.Web.UI.WebControls;

namespace VSWebBL.ReportsBL
{
  public class XsdBL
    {
        private static XsdBL _Self = new XsdBL();
        public static XsdBL Ins
        {
            get
            {
                return _Self;
            }
        }

      //2/11/2015 NS modified for VSPLUS-1428
        public DataTable getdataBL(string servername,DateTime startime,DateTime endtime, string threshold, string servertype)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.getdata(servername, startime, endtime, threshold, servertype);               
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        //12/17/2013 NS added
        public DataTable getServersForCPUUtil(string ServerType)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.getServersForCPUUtil(ServerType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable getAvgDailyResponseBL(string devicename, DateTime startime, DateTime endtime, string servertype)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.getdataforAvgDailyResponse(devicename, startime, endtime, servertype);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable getBlackBerryBL(string Devicename, DateTime starttime, DateTime endtime)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.getdataBlackBerry(Devicename, starttime, endtime);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           

        }

        public DataTable getClusterSecOnQBL(string servername, string startdate)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.getClusterSeconQ(servername, startdate);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        //26/4/2016 Durga Modified for VSPLUS-2883
        public DataTable getDailyMailVolumeBL(string ServerName,string ServerType)
        {
			try
			{
                return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.DailyMailVolumeRptDS(ServerName, ServerType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable getDailyMemoryUsedBL(string servername,int datey,int month, string servertype)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.DailyMemoryUsed(datey, month, servername, servertype);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable DeviceHourlyonTargetBL(string servername,string servertype)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.DeviceHourlyonTarget(servername, servertype);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable DeviceuptimeBL(string Servername, DateTime dateval, string ServerType)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.DeviceUptime(dateval, Servername, ServerType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }

        public DataTable DominoDBDailyBL()
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.DominoDBDaily();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable DominoDiskSpaceBL(string Servername)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.DominoDiskSpace(Servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        public DataTable DominoDiskSpaceLocBL(string Servername)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.DominoDiskSpaceLoc(Servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable DominoDiskSpaceSrcBL(string Servername)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.DominoDiskSpaceSrc(Servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable DominoResponseTimesMonthlyBL(int month, int year, string servername)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.DominoResponseTimesMonthly(month, year, servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable DominoServerHealthBL()
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.DominoServerHealth();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable DominoSrvCpuutilBL(string ServerName)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.DominoSrvCpuutil(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable hr10AvarageDeliveryTimeinSecondsBL(string ServerName,DateTime starttime,DateTime endtime)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.hr10AvarageDeliveryTimeinSeconds(ServerName, starttime, endtime);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable hr11Monthly(DateTime starttime, DateTime endtime)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.hr11Monthly(starttime, endtime);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable hr2BL(string mydevice, DateTime starttime, DateTime endtime)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.hr2Rpt(mydevice, starttime, endtime);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable hr4BL(string mydevice, DateTime starttime, DateTime endtime)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.hr4Rpt(mydevice, starttime, endtime);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }


        public DataTable hr5BL(string Mydevice, DateTime startime, DateTime endtime)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.hr5Rpt(Mydevice, startime, endtime);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable hr6BL(string mydevice, DateTime starttime, DateTime endtime)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.hr6Rpt(mydevice, starttime, endtime);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

      //  public DataTable hr7BL(string 

        public DataTable hr7BL(string mydevice, DateTime starttime, DateTime endtime)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.hr7Rpt(mydevice, starttime, endtime);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable hr8BL(string mydevice, DateTime starttime, DateTime endtime)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.hr8Rpt(mydevice, starttime, endtime);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable hr9BL(string mydevice, DateTime starttime, DateTime endtime)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.hr9Rpt(mydevice, starttime, endtime);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable HRDeviceDailyStatusBL(string mydevice, DateTime starttime, DateTime endtime)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.HRDeviceDailyStatus(mydevice, starttime, endtime);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable HRMonthlyDSBL(DateTime startime, DateTime endtime)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.HRMonthlyDS(startime, endtime);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable OverallSrvStatusHealthRptDSBL()
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.OverallSrvStatusHealthRptDS();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable ResponseTimeXtraRptBL()
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.ResponseTimeXtraRpt();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable SrvAvailabilityRptBL(int month,int year,string ServerName)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.SrvAvailabilityRpt(month, year, ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable SrvDiskFreeSpaceTrendRptDSBL(int month, int year, string Servername, string ServerType)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.SrvDiskFreeSpaceTrendRptDS(month, year, Servername, ServerType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable SrvTransPerMinRptDSBL(int month, int year, string Servername)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.SrvTransPerMinRptDS(month, year, Servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable ConfigUserListRptBL()
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.ConfigUserListRpt();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }

        public DataTable LicenseCountServerTypesBL()
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.LicenseCountServerTypes();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable LicenseCountServerBL()
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.LicenseCountServer();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable LicenseCountSettingBL()
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.LicenseCountSetting();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable LogFileScanRptDSBL()
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.LogFileScanRptDS();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public SqlDataSource  Hr1RptBL(string mydevice, DateTime starttime, DateTime endtime)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.Hr1Rpt(mydevice, starttime, endtime);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable Hr3RptBL(string mydevice,DateTime starttime,DateTime endtime)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.Hr3Rpt(mydevice, starttime, endtime);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        //9/16/2014 NS added for VSPLUS-456
        public DataTable DeviceUptimePctBL(string servername, DateTime startdt, DateTime enddt, string servertype)
        {
			try
			{
				return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.DeviceUptimePct(servername, startdt, enddt, servertype);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

      //12/18/2015 NS added for VSPLUS-2291
        //26/4/2016 Durga Modified for VSPLUS-2883
        public DataTable MonthlyMailVolumeRptDS(string ServerName, string ServerType)
        {
            return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.MonthlyMailVolumeRptDS(ServerName, ServerType);
        }
		//2/1/2016 Durga Modified for VSPLUS 2174

		public DataTable GetTravlerData(string servername, DateTime startime, DateTime endtime, string threshold, string servertype,string StatName)
		{
			try
			{
				return VSWebDAL.ReportsDAL.TravelerReportsDAL.Ins.GetTravlerData(servername, startime, endtime, threshold, servertype, StatName);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
        //2/19/2016 Durga Modified for VSPLUS 2174
        public DataTable getServersForTraveler(string ServerType, string StatName)
        {
            try
            {
                return VSWebDAL.ReportsDAL.TravelerReportsDAL.Ins.getServersForTraveler(ServerType, StatName);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // 3/17/2016 Durga Addded for VSPLUS-2702
        public DataTable GetO365Server()
        {
            try
            {
                return VSWebDAL.ReportsDAL.TravelerReportsDAL.Ins.GetO365Server();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //22/4/2016 Durga added for  VSPLUS-2806

        public DataTable GetServersListFromDominoDailyStats(string StatName)
        {
            try
            {
                return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.GetServersListFromDominoDailyStats(StatName);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public DataTable GetDominoDailyStatsInfo(string servername, string Threshold, string StatName)
        {
            try
            {
                return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.GetDominoDailyStatsInfo(servername, Threshold, StatName);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



     //Durga Added for VSPLUS-2993 24/05/2016
        public DataTable getExchangeMailboxNames(string Type)
        {
            try
            {
                return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.getExchangeMailboxNames(Type);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataTable GetServerNames(string Type)
        {
            try
            {
                return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.GetServerNames(Type);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataTable GetExchangeMailboxData(string Name, DateTime startime, DateTime endtime, string threshold)
        {
            try
            {
                return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.GetExchangeMailboxData(Name, startime, endtime, threshold);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataTable GetO365MailboxesInfo()
        {
            try
            {
                return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.GetO365MailboxesInfo();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataTable GetO365MailboxData( DateTime startime, DateTime endtime, string threshold)
        {
            try
            {
                return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.GetO365MailboxData(startime, endtime, threshold);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public DataTable GetExchangeServerData(string Name,DateTime startime, DateTime endtime, string threshold)
        {
            try
            {
                return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.GetExchangeServerData(Name,startime, endtime, threshold);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        //6/3/2016 Sowjanya modified for VSPLUS-2895
        public DataTable ConnectionTags(string Name, string ServerName)
        {
            try
            {
                return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.ConnectionTags(Name, ServerName);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        //6/3/2016 Sowjanya modified for VSPLUS-2895
        public DataTable GetTagsCount(string Name,string ServerName)
        {
            try
            {
                return VSWebDAL.ReportsDAL.AvgCpuUtilDAL.Ins.GetTagsCount(Name,ServerName);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

  }
}
