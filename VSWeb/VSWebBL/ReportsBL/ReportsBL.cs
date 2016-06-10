using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.ReportsBL
{
    public  class ReportsBL
    {

        private static ReportsBL _self = new ReportsBL();
        public static ReportsBL Ins
        {
            get
            {
                return _self;
            }
        }

        public DataTable getDominoSummaryStats(string ServerType)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.getDominoSummaryStats(ServerType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable getDominoSummaryDiskStats(string ServerType)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.getDominoSummaryDiskStats(ServerType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable getDistinctDomino(string ServerType)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.getdistinctDominoSummary(ServerType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable getDominocluster()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.getDominoDailyStats();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }


        public DataTable getDailyMailVolume(string ServerType)
        {
			try
			{
                return VSWebDAL.ReportsDAL.ReportsDAL.Ins.getDailyMailVolume(ServerType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable getDailyMemoryUsedBL(string ServerType)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.getDailyMemoryUsed(ServerType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }

        public DataTable getDeviceHourlyOnTargetPctRptBL(string ServerType)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.getDeviceHourlyOnTargetPctRpt(ServerType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        //2/5/2015 NS added for VSPLUS-1370
        public DataTable getDeviceHourlyOnTargetPctServerTypes()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.getDeviceHourlyOnTargetPctServerTypes();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable getDeviceUptimeRptBL(string sType)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.DeviceUptimeRpt(sType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }


        public DataTable DominoDiskHealthLocRptBL()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.DominoDiskHealthLocRpt();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        //sowmya
        public DataTable DominoDiskHealthRptBL()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.DominoDiskHealthRpt();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable DominoResponseTimesMonthlyRptBL()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.DominoResponseTimesMonthlyRpt();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            

        }

        public DataTable DominoSrvCPUUtilRptBL()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.DominoSrvCPUUtilRpt();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable ServerAvailabilityRptBL(int month, int year, string ServerName, bool exactmatch, string downMin, string ServerType, int day)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.ServerAvailabilityRpt(month, year, ServerName, exactmatch, downMin, ServerType, day);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable SrvAvailabilityRptBL()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.SrvAvailabilityRpt();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable SrvDiskFreeSpaceTrendRptBL(string ServerType)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.SrvDiskFreeSpaceTrendRpt(ServerType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        public DataTable SrvAvailabilityRpt()
        {
            try
            {
                return VSWebDAL.ReportsDAL.ReportsDAL.Ins.SrvAvailabilityRpt();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataTable SrvDiskFreeSpaceServerTypes()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.SrvDiskFreeSpaceServerTypes();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
       
        public DataTable SrvTransPerMinRptBL()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.SrvTransPerMinRpt();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }


        public DataTable ServerListLocRptBL()
        {
            try
            {
                return VSWebDAL.ReportsDAL.ReportsDAL.Ins.ServerListLocRpt();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //1/06/2016 sowmya added for VSPLUS-2934
        public DataTable GetCommuniyList()
        {
            try
            {
                return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetCommuniyList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public DataTable ServerListTypeRptBL()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.ServerListTypeRpt();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           

        }

        public DataTable DeviceTypeComboBoxBL(string devicetype)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.DeviceTypeComboBoxforhistoricalresponse(devicetype);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable DeviceTypeComboBoxforhistoricalresponseelsepartBL()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.DeviceTypeComboBoxforhistoricalresponseelsepart();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable FillDeviceTypeComboforhistoricalresponseBL()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.FillDeviceTypeComboforhistoricalresponse();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable fillBlackBerryProbeStatsBL()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.fillBlackBerryProbeStats();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable fillDeviceDailyStatsBL()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.fillDeviceDailyStats();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }


        public DataTable fillNotesMailStatsBL()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.fillNotesMailStats();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable fillServerTypesBL()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.fillServerTypes();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }


        public DataTable fillTravelerInterval()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.fillTravelerInterval();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable fillTravelerServer()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.fillTravelerServer();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable fillMailServer()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.fillMailServer();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable fillMailbyServerName(string servername)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.fillMailbyServerName(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable DominoDiskSpaceBL(string servername,bool exactmatch,bool isSummary, string servertype)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.DominoDiskSpaceDAL(servername, exactmatch, isSummary, servertype);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public double GetDominoDiskConsumption(string ServerType, string ServerName, string DiskName, bool exactmatch, bool isSummary)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetDominoDiskConsumption(ServerType, ServerName, DiskName, exactmatch, isSummary);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable GetDominoDiskMonthlyConsumption(string ServerName, string DiskName, string Year, string ServerType)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetDominoDiskMonthlyConsumption(ServerName, DiskName, Year, ServerType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable fillDominoDiskAvgConsumption(string ServerType)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.fillDominoDiskAvgConsumption(ServerType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        //2/5/2015 NS added for VSPLUS-1370
        public DataTable fillDominoDiskAvgConsumptionServerTypes()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.fillDominoDiskAvgConsumptionServerTypes();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
        
        }

        public DataTable GetTravelerStats(string ServerName, string Interval)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetTravelerStats(ServerName, Interval);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetTravelerStatsSrv(string ServerName, string TravelerName)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetTravelerStatsSrv(ServerName, TravelerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable GetTravelerStatsDelta(string Interval, string ServerName, string StartDate, string EndDate, bool isSummary)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetTravelerStatsDelta(Interval, ServerName, StartDate, EndDate, isSummary);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }

        //4/22/2014 NS added
        public DataTable GetTravelerHTTPSessions(string TravelerName)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetTravelerHTTPSessions(TravelerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetUserNameList()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetUserNameList();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }

        public DataTable GetUserAccessList(string UserName)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetUserAccessList(UserName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }

        public DataTable GetLogFileData()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetLogFileData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetMailFileStats(string fileSize)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetMailFileStats(fileSize);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable GetMailThreshold()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetMailThreshold();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetMaintWin()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetMaintWin();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable GetNotesDBs()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetNotesDBs();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
        }

        public DataTable GetServerList(string param, string qtype)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetServerList(param, qtype);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        //1/06/2016 sowmya added for VSPLUS-2934
        public DataTable GetCommList(string param)
        {
            try
            {
                return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetCommList(param);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetServerTasks()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetServerTasks();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetDominoDBStats(string servername,string foldername)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetDominoDBStats(servername, foldername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetDominoDBStatsSrv()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetDominoDBStatsSrv();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetDominoDBStatsFolder(string servername)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetDominoDBStatsFolder(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable GetDominoServerHealth()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetDominoServerHealth();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable GetOverallServerHealth()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetOverallServerHealth();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetResponseTimes(string typeval)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetResponseTimes(typeval);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable GetMAXCpuUtil(string servername, DateTime starttime, DateTime endtime,string threshold)
        {
			try
			{
                return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetMAXCpuUtil(servername, starttime, endtime, threshold);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable GetConsoleCmdList()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetConsoleCmdList();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable fillDownMinutesServer(string sType)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.fillDownMinutesServer(sType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        //8/21/2014 NS added for VSPLUS-886
        public DataTable GetTravelerDeviceSyncs(string ServerName,string StartDate, string EndDate)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetTravelerDeviceSyncs(ServerName, StartDate, EndDate);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }
        //9/30/2014 NS added for VSPLUS-953
        public DataTable GetTravelerDevices()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetTravelerDevices();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        //9/10/2014 NS added for VSPLUS-921
        public DataTable GetDBClusterInfo(string ClusterName)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetDBClusterInfo(ClusterName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        //9/11/2014 NS added for VSPLUS-921
        public DataTable GetDBClusterServers(string ClusterName)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetDBClusterServers(ClusterName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        //9/11/2014 NS added for VSPLUS-921
        public DataTable GetDBClusterNames()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetDBClusterNames();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        //9/17/2014 NS added for VSPLUS-456
        public DataTable GetDeviceUptimePctServerNames(string ServerType)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetDeviceUptimePctServerNames(ServerType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        //2/4/2015 NS added for VSPLUS-1370
        public DataTable GetDeviceUptimePctServerTypes()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetDeviceUptimePctServerTypes();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        //11/3/2014 NS added for VSPLUS-648
        public DataTable GetDominoStatList(string stattype,string statname="")
		{
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetDominoStatList(stattype,statname);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }
        //11/3/2014 NS added for VSPLUS-648
        public DataTable GetDominoStatValues(string statname,string stattype,string startdate,string enddate,string servername="")
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetDominoStatValues(statname, stattype, startdate, enddate,servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        //1/27/2015 NS added for VSPLUS-1324
        public DataTable GetSametimeStatNames()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetSametimeStatNames();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        //1/30/2015 NS added for VSPLUS-1370 
        public DataTable fillServerTypeList()
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.fillServerTypeList();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        //2/2/2015 NS added for VSPLUS
        public DataTable fillServerTypeList2(string statname)
        {
			try
			{
				return VSWebDAL.ReportsDAL.ReportsDAL.Ins.fillServerTypeList2(statname);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        //3/12/2015 NS added for VSPLUS-1534
        public DataTable GetExchangeServerList(string statname)
        {
            return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetExchangeServerList(statname);
        }

        //3/12/2015 NS added for VSPLUS-1534
        public DataTable GetExchangeMailSentCount(string servername, string statname, string datefrom, string dateto, string rpttype)
        {
            return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetExchangeMailSentCount(servername, statname, datefrom, dateto, rpttype);
        }

        //3/13/2015 NS added for VSPLUS-1534
        public DataTable ResponseTimeTrendRpt(string datefrom, string dateto, string ServerName, bool exactmatch, string ServerType, string rpttype)
        {
            return VSWebDAL.ReportsDAL.ReportsDAL.Ins.ResponseTimeTrendRpt(datefrom, dateto, ServerName, exactmatch, ServerType,rpttype);
        }

        //3/13/2015 NS added for VSPLUS-1534
        public DataTable fillServerListByType(string sType)
        {
            return VSWebDAL.ReportsDAL.ReportsDAL.Ins.fillServerListByType(sType);
        }

        //4/14/2015 NS added for VSPLUS-1635
        public DataTable GetDominoDBAvgSize(string servername)
        {
            return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetDominoDBAvgSize(servername);
        }

        //6/15/2015 NS added for VSPLUS-1841
        //1/5/2016 NS modified for VSPLUS-1534
        public DataTable GetUserCountTrend(string typeval, string startdt, string enddt, string servername, string fromtableParam = "", string statnameParam = "", bool exactmatch = true)
        {
            return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetUserCountTrend(typeval, startdt, enddt, servername, fromtableParam, statnameParam, exactmatch);
        }

        //7/29/2015 NS added for VSPLUS-2023
        //1/5/2016 NS modified for VSPLUS-1534
        public DataTable GetUserCountServers(string typeval, string fromtableParam="", string statnameParam="",bool exactmatch=true)
        {
            return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetUserCountServers(typeval, fromtableParam, statnameParam,exactmatch);
        }

        //12/1/2015 NS added for VSPLUS-2140
        public DataTable GetMSUserCountTypes(string stype)
        {
            return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetMSUserCountTypes(stype);
        }

        //12/1/2015 NS added for VSPLUS-2140
        public DataTable GetMSUserCounts(string stype, string utype, string servername, string sdate, string edate)
        {
            return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetMSUserCounts(stype, utype, servername, sdate, edate);
        }

        //12/4/2015 NS added for VSPLUS-2140
        public DataTable GetMSServers(string stype, string utype)
        {
            return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetMSServers(stype, utype);
        }
        //3/17/2016 Durga added for VSPLUS-2702
        public DataTable GetStalemailboxesInfo(string server)
        {
            try
            {
                return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetStalemailboxesInfo(server);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        //3/28/2016 Durga Added for VSPLUS-2698
        public DataTable GetServerUtiliZationOfDomino(string Name)
        {
            try
            {
                return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetServerUtiliZationOfDomino(Name);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataTable GetServerUtiliZationOfExchange(string Name, string StatName)
        {
            try
            {
                return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetServerUtiliZationOfExchange(Name, StatName);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        //17/3/2016 sowmya added for VSPLUS 2455
        public DataTable GetclusterinfoBL()
        {
            try
            {
                return VSWebDAL.ReportsDAL.ReportsDAL.Ins.Getclusterinfo();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        //12-04-2016 Sowjanya Modified for VSPLUS-2831
        public DataTable GetMSForumTypes()
        {
            return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetMSForumTypes();
        }

       
        public DataTable GetIBMConnectionsServerlist(string statname1)
        {
            return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetIBMConnectionsServerlist(statname1);
        }
        //11-05-2016 Durga Modified for VSPLUS-2836
        public DataTable GetonnectionsStatsNames(string StatName1, string StatName2)
        {
            return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetonnectionsStatsNames(StatName1, StatName2);
        }
        //26/4/2016 Durga Modified for VSPLUS-2878
        public DataTable GetonnectionsStatsNamesofWikis(string StatName1, string StatName2)
        {
            return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetonnectionsStatsNamesofWikis(StatName1, StatName2);
        }
        public DataTable GetIBMConnectionstatsInfo(string servername, string statname, string datefrom, string dateto)
        {
            return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetIBMConnectionstatsInfo(servername, statname, datefrom, dateto);
        }
        //12-05-2016 Sowmya Modified for VSPLUS-2830
        public DataTable Getfiletypes(string statname1, string statname2, string statname3, string statname4)
        {
            return VSWebDAL.ReportsDAL.ReportsDAL.Ins.Getfiletypes(statname1, statname2, statname3, statname4);
        }
        //13/4/2016 Durga Modified for VSPLUS-2832
        public DataTable GetActivityStatNames()
        {
            return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetActivityStatNames();
        }
        //5/9/2016 Sowjanya modified for VSPLUS-2931
        public DataTable GetDBClusterC(string ClusterName)
        {
            try
            {
                return VSWebDAL.ReportsDAL.ReportsDAL.Ins.GetDBClusterC(ClusterName);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        //6/3/2016 Sowjanya modified for VSPLUS-2895

        public DataTable CommunityNames()
        {
            try
            {
                return VSWebDAL.ReportsDAL.ReportsDAL.Ins.CommunityNames();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public DataTable SetGraphForTags(string serverName)
        {
            try
            {
                return VSWebDAL.ReportsDAL.ReportsDAL.Ins.SetGraphForTags(serverName);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
