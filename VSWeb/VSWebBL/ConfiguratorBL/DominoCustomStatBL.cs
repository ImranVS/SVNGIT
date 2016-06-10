using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.ConfiguratorBL
{
    public class DominoCustomStatBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private static DominoCustomStatBL _self = new DominoCustomStatBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static DominoCustomStatBL Ins
        {
            get { return _self; }
        }


        /// <summary>
        /// Call to Get Data from DominoCustomStatValues based on Primary key
        /// </summary>
        /// <param name="DominoServersObject">DominoServers object</param>
        /// <returns></returns>
        public DominoCustomStatValues GetData(DominoCustomStatValues DominoCustomstatObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoCustomStatisticsDAL.Ins.GetData(DominoCustomstatObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
         
       #region Validations
        public Object ValidateDCSUpdate(DominoCustomStatValues DominoCustomstatObject)
        {
            Object ReturnValue = "";
            try
            {
                if (DominoCustomstatObject.ServerName == null || DominoCustomstatObject.ServerName == "")
                {
                    return "ER#Please select the server name";
                }
                if (DominoCustomstatObject.StatName== null || DominoCustomstatObject.StatName == " ")
                {
                    return "ER#Please select the stat";
                }
                
                             
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { }
            return "";
        }

        #endregion
        /// <summary>
        /// Call to Get Data from DominoCustomStatValues
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllData()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoCustomStatisticsDAL.Ins.GetAllData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        /// <summary>
        /// Call to Insert Data into DominoCustomStatValues
        ///  </summary>
        /// <param name="DominoClusterObject">DominoCustomStatValues object</param>
        /// <returns></returns>
        public bool InsertData(DominoCustomStatValues DominoCustomstatObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoCustomStatisticsDAL.Ins.InsertData(DominoCustomstatObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        /// <summary>
        /// Call to Update Data of DominoCustomStatValues based on Key
        /// </summary>
        /// <param name="DominoCustomstatObject">DominoCustomStatValues object</param>
        /// <returns>Object</returns>
        public Object UpdateData(DominoCustomStatValues DominoCustomstatObject)
        {
			try
			{
				Object ReturnValue = ValidateDCSUpdate(DominoCustomstatObject);
				if (ReturnValue.ToString() == "")
				{
					return VSWebDAL.ConfiguratorDAL.DominoCustomStatisticsDAL.Ins.UpdateData(DominoCustomstatObject);
				}
				else return ReturnValue;
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        /// <summary>
        /// Call DAL Delete Data
        /// </summary>
        /// <param name="DCSObject"></param>
        /// <returns></returns>
        public Object DeleteData(DominoCustomStatValues DCSObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoCustomStatisticsDAL.Ins.DeleteData(DCSObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

    }
}
