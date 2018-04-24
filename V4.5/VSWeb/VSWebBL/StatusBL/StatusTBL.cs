using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDAL;
using System.Data;

namespace VSWebBL.StatusBL
{
    public class StatusTBL
    {

        /// <summary>
        /// Declarations
        /// </summary>
        private static StatusTBL _self = new StatusTBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static StatusTBL Ins
        {
            get { return _self; }
        }

        public bool InsertData(Status StatusObj)
        {
			try
			{
				return VSWebDAL.StatusDAL.StatusTDAL.Ins.InsertData(StatusObj);
			}
			catch (Exception)
			{
				
				throw;
			}
        }
        public DataTable DistinctServerTypes()
        {
			try
			{
				return VSWebDAL.StatusDAL.StatusTDAL.Ins.DistinctServerTypes();
			}
			catch (Exception)
			{
				
				throw;
			}
          
        }
        public DataTable DistinctLocations()
        {
			try
			{
				return VSWebDAL.StatusDAL.StatusTDAL.Ins.DistinctLocations();
			}
			catch (Exception)
			{
				
				throw;
			}
           
        }
        public bool UpdateforScan(Status StatusObj)
        {
			try
			{
				return VSWebDAL.StatusDAL.StatusTDAL.Ins.UpdateforScan(StatusObj);
			}
			catch (Exception)
			{
				
				throw;
			}
          

        }
    }
}
