using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDAL;
namespace VSWebBL.DashboardBL
{
    public class mailFileBL
    {
        private static mailFileBL _self = new mailFileBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static mailFileBL Ins
        {
            get { return _self; }
        }

        public DataTable GetMails(string tab)
        {
			try
			{
				return VSWebDAL.DashboardDAL.MailFilesDAL.Ins.GetMails(tab);
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
				return VSWebDAL.DashboardDAL.MailFilesDAL.Ins.FillProblemsGrid();
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
				return VSWebDAL.DashboardDAL.MailFilesDAL.Ins.FillReplicationGrid();
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
				return VSWebDAL.DashboardDAL.MailFilesDAL.Ins.FillServerCombobox();
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
				return VSWebDAL.DashboardDAL.MailFilesDAL.Ins.FillDBByTemplateGrid();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
		public DataTable GetServerNames()
		{
			try
			{
				return VSWebDAL.DashboardDAL.MailFilesDAL.Ins.GetServerNames();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
    }
}
