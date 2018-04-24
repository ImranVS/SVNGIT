using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.DashboardBL
{
   public class KeyMetricsBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
       private static KeyMetricsBL _self = new KeyMetricsBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
       public static KeyMetricsBL Ins
        {
            get { return _self; }
        }

       public DataTable GetUserCount(string param, string stype)
       {
		   try
		   {
			   return VSWebDAL.DashboardDAL.KeyMetricsDAL.Ins.GetUserCount(param, stype);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
          
       }

       public DataTable GetResponseTime(string param,string Sname,string stype, string location)
       {
		   try
		   {
			   return VSWebDAL.DashboardDAL.KeyMetricsDAL.Ins.GetResponseTime(param, Sname, stype, location);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
         
       }

     
       public DataTable GetKeyMetrics(string ServerLoc)
       {
		   try
		   {
			   return VSWebDAL.DashboardDAL.KeyMetricsDAL.Ins.GetKeyMetrics(ServerLoc);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
          
       }
	   public DataTable GetKeyMetricsvisible(string ServerLoc)
       {
		   try
		   {
			   return VSWebDAL.DashboardDAL.KeyMetricsDAL.Ins.GetKeyMetricsvisible(ServerLoc);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
		   
       }
	   
       //10/20/2015 NS modified for VSPLUS-2072
        public DataTable GetType(string includesrv = "")
        {
			try
			{
                return VSWebDAL.DashboardDAL.KeyMetricsDAL.Ins.GetType(includesrv);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
		public DataTable GetspecificServerType(string page, string control)
		{
			try
			{
				return VSWebDAL.DashboardDAL.KeyMetricsDAL.Ins.GetspecificServerType(page, control);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

       //2/13/2015 NS added for VSPLUS-1346
        public DataTable GetLocation()
        {
			try
			{
				return VSWebDAL.DashboardDAL.KeyMetricsDAL.Ins.GetLocation();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        //10/30/2013 NS added
        public DataTable GetServerDaysUp(string param,string stype)
        {
			try
			{
				return VSWebDAL.DashboardDAL.KeyMetricsDAL.Ins.GetServerDaysUp(param, stype);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

       //2/5/2014 NS added for VSPLUS-211
        public DataTable GetServerDownTime()
        {
			try
			{
				return VSWebDAL.DashboardDAL.KeyMetricsDAL.Ins.GetServerDownTime();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        //3/17/2016 Durga Added for VSPLUS-2696
        public DataTable GetCostperuserserveddata(string param, string Sname, string stype,string UserType)
        {
            try
            {
                return VSWebDAL.DashboardDAL.KeyMetricsDAL.Ins.GetCostperuserserveddata(param, Sname, stype, UserType);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        //3/22/2016 Durga Added for VSPLUS-2695
        public DataTable GetCostPerUserServerGrid(string fromdate, string todate, string statname)

        {
            try
            {
                return VSWebDAL.DashboardDAL.KeyMetricsDAL.Ins.GetCostPerUserServerGrid(fromdate, todate, statname);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataTable GetCostPerUserServersGridForDomino(string fromdate, string todate)
        {
            try
            {
                return VSWebDAL.DashboardDAL.KeyMetricsDAL.Ins.GetCostPerUserServersGridForDomino(fromdate, todate);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        //3/22/2016 Durga Added for VSPLUS-2695
        public DataTable GetCostPerdayforExchange(string ServerName, string date, int StatValue)
        {
            try
            {
                return VSWebDAL.DashboardDAL.KeyMetricsDAL.Ins.GetCostPerdayforExchange(ServerName, date, StatValue);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataTable GetCostperuserserveddataForDomino(string param, string Sname, string stype)
        {
            try
            {
                return VSWebDAL.DashboardDAL.KeyMetricsDAL.Ins.GetCostperuserserveddataForDomino(param, Sname, stype);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        // 3/18/2016 Durga Addded for VSPLUS-2696
        public DataTable GetServerTypeForCostperUserserved()
        {
            try
            {
                return VSWebDAL.DashboardDAL.KeyMetricsDAL.Ins.GetServerTypeForCostperUserserved();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        //18-04-2016 Durga Modified for VSPLUS-2866

        public DataTable GetMonthlyExpenditureDetails(string GroupBy)
        {
            try
            {
                return VSWebDAL.DashboardDAL.KeyMetricsDAL.Ins.GetMonthlyExpenditureDetails(GroupBy);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataTable GetMostUtilizedServers()
        {
            try
            {
                return VSWebDAL.DashboardDAL.KeyMetricsDAL.Ins.GetMostUtilizedServers();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataTable GetCostPerUserServedDetails()
        {
            try
            {
                return VSWebDAL.DashboardDAL.KeyMetricsDAL.Ins.GetCostPerUserServedDetails();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        //3-5-2016 Durga Added for VSPLUS-2883
        public DataTable GetServerTypeForDailyMailVolume()
        {
            try
            {
                return VSWebDAL.DashboardDAL.KeyMetricsDAL.Ins.GetServerTypeForDailyMailVolume();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        //6/3/2016 Sowjanya modified for VSPLUS-2999
        public DataTable GetCurrencySymbol(string CurrencySymbol)
        {
            try
            {
                return VSWebDAL.DashboardDAL.KeyMetricsDAL.Ins.GetCurrencySymbol(CurrencySymbol);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


       //5/26/2016 NS added for VSPLUS-2941
        public DataTable GetCPUMemoryHealth()
        {
            return VSWebDAL.DashboardDAL.KeyMetricsDAL.Ins.GetCPUMemoryHealth();
        }

    }
}
