using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDAL;
namespace VSWebBL.DashboardBL
{
    public class ExchangeMailFileBL
    {
		private static ExchangeMailFileBL _self = new ExchangeMailFileBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
		public static ExchangeMailFileBL Ins
        {

            get { return _self; }
        }

        public DataTable GetMails()
        {
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeMailFilesDAL.Ins.GetMails();
			}
			catch (Exception ex)
			{ 
				
				throw ex;
			}
           
        }

        public DataTable FillProblemsGrid()
        {
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeMailFilesDAL.Ins.FillProblemsGrid();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
        }

        public DataTable FillReplicationGrid()
        {
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeMailFilesDAL.Ins.FillReplicationGrid();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
        }

        public DataTable FillServerCombobox()
        {
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeMailFilesDAL.Ins.FillServerCombobox();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
        }

		public DataTable FillDBCombobox()
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeMailFilesDAL.Ins.FillDBCombobox();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

        public DataTable FillDBByTemplateGrid()
        {
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeMailFilesDAL.Ins.FillDBByTemplateGrid();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
        }

    }
}
