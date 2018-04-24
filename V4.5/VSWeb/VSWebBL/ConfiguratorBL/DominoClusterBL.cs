using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.ConfiguratorBL
{
   public class DominoClusterBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private static DominoClusterBL _self = new DominoClusterBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static DominoClusterBL Ins
        {
            get { return _self; }
        }
        #region "Validations"

        /// <summary>
        /// Validation before submitting data for Server tab
        /// </summary>
        /// <param name="DominoClusterObject"></param>
        /// <returns></returns>
        public Object ValidateDCUpdate(DominoCluster DominoClusterObject)
        {
            Object ReturnValue = "";
            try
            {
				if (DominoClusterObject.Name == null || DominoClusterObject.Name == "")
				{
					return "Please enter a Cluster name.";
				}
                if (DominoClusterObject.ServerAName== null || DominoClusterObject.ServerAName== "")
                {
                    return "Please select the first server in the Cluster.";
                }
                if (DominoClusterObject.ServerBName== null || DominoClusterObject.ServerBName== "")
                {
                    return "Please select the second server in the Cluster.";
                }
                if ((DominoClusterObject.ServerID_C!= 0) && (DominoClusterObject.Server_C_Directory == ""))
                {
                    return "Please enter the name of the directory where user mail files are located, for example, 'mail'.";
                }
				if ((DominoClusterObject.ServerID_A) > 0 && (DominoClusterObject.ServerID_B > 0))
				{
					if (DominoClusterObject.ServerID_A == DominoClusterObject.ServerID_B)
					{
						return "You must select two different servers to be added as cluster members.";
					}
				}
				//if (DominoClusterObject.Server_A_Directory == "" || DominoClusterObject.Server_A_Directory == null)
				//{
				//    return "ER#Please enter the name of the directory where user mail files are located, for example, 'mail'.";
				//}
				//if (DominoClusterObject.Server_B_Directory == "" || DominoClusterObject.Server_B_Directory == null)
				//{
				//    return "ER#Please enter the name of the directory where user mail files are located, for example, 'mail'.";
				//}
                if (DominoClusterObject.Category == null || DominoClusterObject.Category == "")
                {
                    return "Please enter a Category.";
                }
                if (DominoClusterObject.ScanInterval.ToString() == "")
                {
                    return "Please enter a Scan Interval.";
                }
                if (DominoClusterObject.ScanInterval < 60)
                {
                    return "The value of the Scan Interval for clusters may not be less than 60 minutes.";
                }
                if (DominoClusterObject.OffHoursScanInterval < 120)
                {
                    return "The value of the Scan Interval for clusters during the off-hours may not be less than 120 minutes.";
                }
                if (DominoClusterObject.OffHoursScanInterval.ToString() == "")
                {
                    return "Please enter an Off-Hours Scan Interval.";
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
        /// Call to Get Data from DominoCluster based on Primary key
        /// </summary>
        /// <param name="DominoClusterObject">DominoClusterObject object</param>
        /// <returns></returns>
        public DominoCluster GetData(DominoCluster DominoClusterObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoClusterDAL.Ins.GetData(DominoClusterObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        /// <summary>
        /// Call to Get Data from DominoServers
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllData()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoClusterDAL.Ins.GetAllData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        /// <summary>
        /// Call to Insert Data into DominoCluster
        ///  </summary>
        /// <param name="DominoClusterObject">DominoCluster object</param>
        /// <returns></returns>
        public Object InsertData(DominoCluster DominoClusterObject)
        {
			try
			{
				Object ReturnValue = ValidateDCUpdate(DominoClusterObject);
				if (ReturnValue.ToString() == "")
				{
					return VSWebDAL.ConfiguratorDAL.DominoClusterDAL.Ins.InsertData(DominoClusterObject);
				}
				else return ReturnValue;
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
       }

        /// <summary>
        /// Call to Update Data of DominoServers based on Key
        /// </summary>
        /// <param name="DominoClusterObject">DominoServers object</param>
        /// <returns>Object</returns>
        public Object UpdateData(DominoCluster DominoClusterObject)
        {
			try
			{
				Object ReturnValue = ValidateDCUpdate(DominoClusterObject);
				if (ReturnValue.ToString() == "")
				{
					return VSWebDAL.ConfiguratorDAL.DominoClusterDAL.Ins.UpdateData(DominoClusterObject);
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
        /// <param name="DCObject"></param>
        /// <returns></returns>
        public Object DeleteData(DominoCluster DCObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoClusterDAL.Ins.DeleteData(DCObject);
			}
			catch (Exception ex)
			{
				
				throw ex ;
			}
           
        }
        public DataTable GetIPAddress(DominoCluster ClusterObj)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoClusterDAL.Ins.GetIPAddress(ClusterObj);
			}
			catch (Exception ex) 
			{
				
				throw ex;
			}
           
        }

        public DataTable GetServer()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoClusterDAL.Ins.GetServer();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        
        }
		public DataTable GetNameforStatus(DominoCluster ClusterObj)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoClusterDAL.Ins.GetNameforStatus(ClusterObj);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}


        //10/2/2016 Sowmya Added for VSPLUS 2455
        public bool Updateclusterscandata(DominoCluster DominoClusterObject)
        {
            try
            {
                
                    return VSWebDAL.ConfiguratorDAL.DominoClusterDAL.Ins.Updateclusterscandata(DominoClusterObject);
                
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


    }
}
