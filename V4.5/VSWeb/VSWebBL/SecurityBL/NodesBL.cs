using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.SecurityBL
{
   public class NodesBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
	   private static NodesBL _self = new NodesBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
	   public static NodesBL Ins
        {
            get { return _self; }
        }
        public DataTable GetAllData()
        {
			try
			{
				return VSWebDAL.SecurityDAL.NodesDAL.Ins.GetAllData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

		//public DataTable AssigntoserverUpdateTree()
		//{
		//    try
		//    {
		//        return VSWebDAL.SecurityDAL.NodesDAL.Ins.AssigntoserverUpdateTree();
		//    }
		//    catch (Exception ex)
		//    {

		//        throw ex;
		//    }

		//}
		//public DataTable GetDataFromNodes()
		//{
		//    try
		//    {
		//        return VSWebDAL.SecurityDAL.NodesDAL.Ins.GetDataFromNodes();
		//    }
		//    catch (Exception ex)
		//    {

		//        throw ex;
		//    }

		//}
	

		public Boolean Updatenodes(int AssignedNodeId, List<object> fieldValues)
		{
			try
			{
				return VSWebDAL.SecurityDAL.NodesDAL.Ins.Updatenodes(AssignedNodeId, fieldValues);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

	
		public DataTable GetAllDatafromNodes()
		{
			try
			{
				return VSWebDAL.SecurityDAL.NodesDAL.Ins.GetAllDatafromNodes();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public DataTable GetAllDataByNames(Nodes NodesObject)
		{
			try
			{
				return VSWebDAL.SecurityDAL.NodesDAL.Ins.GetAllDataByNames(NodesObject);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public Object InsertData(Nodes NodesObject)
		{
			try
			{
				return VSWebDAL.SecurityDAL.NodesDAL.Ins.InsertData(NodesObject);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public Object UpdateDataforservers(Nodes NodesObject)
		{
			try
			{
				return VSWebDAL.SecurityDAL.NodesDAL.Ins.UpdateDataforservers(NodesObject);
			}
			catch (Exception ex)
			{

				throw ex;
			}



		}

		public DataTable GetforAssignNodes()
		{
			try
			{
				return VSWebDAL.SecurityDAL.NodesDAL.Ins.GetforAssignNodes();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public Object DeleteData(Nodes NodesObject)
		{
			try
			{
				return VSWebDAL.SecurityDAL.NodesDAL.Ins.DeleteData(NodesObject);
			}
			catch (Exception)
			{

				throw;
			}

		}
		public DataTable GetCredentialsBynameid(Credentials LOCbject)
		{
			try
			{
				return VSWebDAL.SecurityDAL.NodesDAL.Ins.GetCredentialsBynameid(LOCbject);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public DataTable GetAllNodeServicesDetails()
		{
			try
			{
				return VSWebDAL.SecurityDAL.NodesDAL.Ins.GetAllNodeServicesDetails();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public DataTable GetAllNodeStatus()
		{
			try
			{
				return VSWebDAL.SecurityDAL.NodesDAL.Ins.GetAllNodeStatus();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public DataTable GetNodeServices(string NodeName)
		{
			try
			{
				return VSWebDAL.SecurityDAL.NodesDAL.Ins.GetNodeServices(NodeName);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public void forceCollectionRefresh()
		{
			VSWebDAL.SecurityDAL.NodesDAL.Ins.forceCollectionRefresh();
		}

		public Object SetDisableState(Boolean isDisabled, string NodeID)
		{
			try
			{
				return VSWebDAL.SecurityDAL.NodesDAL.Ins.SetDisableState(isDisabled, NodeID);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
	   
    }
}
 