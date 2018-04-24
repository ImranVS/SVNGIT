using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.ConfiguratorBL
{
   public class NetworkLatencyBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
       private static NetworkLatencyBL _self = new NetworkLatencyBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
       public static NetworkLatencyBL Ins
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
                    return "ER#Please enter a Cluster name.";
                }
                if (DominoClusterObject.ServerID_A == null || DominoClusterObject.ServerID_A.ToString()== "")
                {
                    return "ER#Please select the first server in the Cluster.";
                }
                if (DominoClusterObject.ServerID_B== null || DominoClusterObject.ServerID_B.ToString()== "")
                {
                    return "ER#Please select the second server in the Cluster.";
                }
                if ((DominoClusterObject.ServerID_C!= 0) && (DominoClusterObject.Server_C_Directory == ""))
                {
                    return "ER#Please enter the name of the directory where user mail files are located, for example, 'mail'.";
                }
                if (DominoClusterObject.ServerID_A== DominoClusterObject.ServerID_B)
                {
                    return "ER#You must select two different servers to be added as cluster members.";
                }
                if (DominoClusterObject.Server_A_Directory == "" || DominoClusterObject.Server_A_Directory == null)
                {
                    return "ER#Please enter the name of the directory where user mail files are located, for example, 'mail'.";
                }
                if (DominoClusterObject.Server_B_Directory == "" || DominoClusterObject.Server_B_Directory == null)
                {
                    return "ER#Please enter the name of the directory where user mail files are located, for example, 'mail'.";
                }
                if (DominoClusterObject.Category == null || DominoClusterObject.Category == "")
                {
                    return "ER#Please enter a Category.";
                }
                if (DominoClusterObject.ScanInterval.ToString() == "")
                {
                    return "ER#Please enter a Scan Interval.";
                }
                if (DominoClusterObject.ScanInterval < 60)
                {
                    return "ER#The value of the Scan Interval for clusters may not be less than 60 minutes.";
                }
                if (DominoClusterObject.OffHoursScanInterval < 120)
                {
                    return "ER#The value of the Scan Interval for clusters during the off-hours may not be less than 120 minutes.";
                }
                if (DominoClusterObject.OffHoursScanInterval.ToString() == "")
                {
                    return "ER#Please enter an Off-Hours Scan Interval.";
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
        public NetworkLatency GetData(NetworkLatency nwlatency)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.NetworkLatencyDAL.Ins.GetData(nwlatency);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}

           
        }
        public bool UpdateeditProfiles(NetworkLatency StObject, string strsname)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.NetworkLatencyDAL.Ins.UpdateeditProfiles(StObject, strsname);
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
				return VSWebDAL.ConfiguratorDAL.NetworkLatencyDAL.Ins.GetAllData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
		public DataTable GetTest()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.NetworkLatencyDAL.Ins.GetTest();
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
        public Object InsertData(NetworkLatency NetworkLatencyObject)
        {
			try
			{
				{
					return VSWebDAL.ConfiguratorDAL.NetworkLatencyDAL.Ins.InsertData(NetworkLatencyObject);
				}
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
        public Object UpdateData(NetworkLatency NetworkLatencyObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.NetworkLatencyDAL.Ins.UpdateData(NetworkLatencyObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
       

        public Object DeleteData(string id)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.NetworkLatencyDAL.Ins.DeleteData(id);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        public Object DeleteNetworkLatencyServers(string id)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.NetworkLatencyDAL.Ins.DeleteNetworkLatencyServers(id);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable Getvalue()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.NetworkLatencyDAL.Ins.Getvalue();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        public DataTable getname(string name)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.NetworkLatencyDAL.Ins.Getname(name);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        public DataTable Getvalue1(int id)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.NetworkLatencyDAL.Ins.Getvalue1(id);
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
        public DataTable GetAllData1(string servertype)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.NetworkLatencyDAL.Ins.GetAllData1(servertype);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetAllData2(int serverkey, string page, string Control)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.NetworkLatencyDAL.Ins.GetAllData2(serverkey,page,Control);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public object updateEnableLatencyTest(int id, int nlid, int latencyred, int yellowthershold, bool checkedvalue,string testname)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.NetworkLatencyDAL.Ins.updatenwEnableLatencyTest(id, nlid, latencyred, yellowthershold, checkedvalue, testname);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }
        public object insertnetworklatency(int id, int nlid, int latencyred, int yellowthershold, bool checkedvalue, string testname)
        {
			try
			{
              return VSWebDAL.ConfiguratorDAL.NetworkLatencyDAL.Ins.insertnetworklatency(id, nlid, latencyred, yellowthershold, checkedvalue, testname);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
		}
        public Object UpdateExchangeSettingsData(ExchangeSettings Mobj)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ExchangeDAL.Ins.UpdateExchangeSettingsData(Mobj);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        public bool Updatelatency(NetworkLatency StObject, string id,string name)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.NetworkLatencyDAL.Ins.Updatelatency(StObject, id, name);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

       //10/21/2015 NS added for VSPLUS-2223
        public DataTable GetSelectedServers(int serverkey, string page, string Control)
        {
            return VSWebDAL.ConfiguratorDAL.NetworkLatencyDAL.Ins.GetSelectedServers(serverkey, page, Control);
        }

		public DataTable GetName(NetworkLatency nlobj)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.NetworkLatencyDAL.Ins.GetName(nlobj);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
        
    }
}
