using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using VSWebDO;

namespace VSWebBL.ConfiguratorBL
{
    public class TravelerBL
    {
        private static TravelerBL _self = new TravelerBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static TravelerBL Ins
        {
            get { return _self; }
        }

        public DataTable GetTravelerHADataStore()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.TravelerDAL.Ins.GetTravelerHADataStore();
			}
			catch (Exception ex)
			{
				
				throw ex;
			} 
        }
        public Object UpdateTravelerDataStoreData(TravelerDS TravelerObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.TravelerDAL.Ins.UpdateTravelerDataStoreData(TravelerObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public Object InsertTravelerDataStoreData(TravelerDS TravelerObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.TravelerDAL.Ins.InsertTravelerDataStoreData(TravelerObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public Object DeleteTravelerDataStoreData(TravelerDS TravelerObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.TravelerDAL.Ins.DeleteTravelerDataStoreData(TravelerObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable GetTravelerTestWhenScan()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.TravelerDAL.Ins.GetTravelerTestWhenScan();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
		public Object UpdateTravelerPassword(TravelerDS TravelerObject)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.TravelerDAL.Ins.UpdateTravelerPassword(TravelerObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetValuebyID(TravelerDS TravelerObject)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.TravelerDAL.Ins.GetValuebyID(TravelerObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
		
		}
    }
}