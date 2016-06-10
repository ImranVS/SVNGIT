using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebBL;
using VSWebDAL;

namespace VSWebBL.ConfiguratorBL
{
	public class IBMConnectionsServersBL
	{
		private static IBMConnectionsServersBL _self = new IBMConnectionsServersBL();

		public static IBMConnectionsServersBL Ins
		{
			get { return _self; }
		}

		public DataTable GetdataforIBMConnectionsServersGridbyUser(int UserID)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.IBMConnectionsServersDAL.Ins.GetdataforIBMConnectionsServersGridbyUser(UserID);
			}
			catch (Exception ex)
			{

				throw ex;
			}


		}

		public Object DeleteIBMConnectionsServers(IBMConnectionsServers IBMConnectionsObject)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.IBMConnectionsServersDAL.Ins.DeleteIBMConnectionsServers(IBMConnectionsObject);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}


		public  IBMConnectionsServers GetdatawithId(IBMConnectionsServers IBMConnectionsObject)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.IBMConnectionsServersDAL.Ins.GetdatawithId(IBMConnectionsObject);
			}
			catch (Exception ex)
			{

				throw ex;
			}


		}

		public DataTable GetServerType(string ServerName)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.IBMConnectionsServersDAL.Ins.GetServerType(ServerName);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public Object UpdateIBMConnectionsServers(IBMConnectionsServers StSObject)
		{
			Object returnval = ValidateSametimeServerUpdate(StSObject);
			try
			{
				if (returnval.ToString() == "")
				{

					return VSWebDAL.ConfiguratorDAL.IBMConnectionsServersDAL.Ins.UpdateIBMConnectionsServers(StSObject);
				}
				else return returnval;
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public Object ValidateSametimeServerUpdate(IBMConnectionsServers UpdateObject)
		{

			Object ReturnValue = "";
			try
			{
				// if (UpdateObject.Name == null || UpdateObject.Name == "")
				//{
				//  return "ER#Please enter the  SametimeServers Name";

				//}
				if (UpdateObject.Category == null || UpdateObject.Category == "")
				{
					return "ER#Please enter the category of the device";
				}
				// if (UpdateObject.Location == null || UpdateObject.Location == "")
				// {
				//  return "ER#Please enter the location of the device";
				// }
				// if (UpdateObject.Description == null || UpdateObject.Description == "")
				// {
				// return "ER#Please enter a description of the device";
				//}
				if (UpdateObject.ResponseThreshold == null)
				{
					return "ER#Please enter a Response Threshold, in milliseconds, over which the device will be considered 'slow'. Validation Failure";

				}

				if (UpdateObject.ScanInterval == null)
				{
					return "ER#Please enter a Scan Interval";
				}

				if (UpdateObject.RetryInterval == null)
				{
					return "ER#Please enter a Retry Interval";
				}
				// if (UpdateObject.IPAddress == null)
				// {
				//   return "ER#Please enter the IP Address device, such as '127.0.0.1'";
				//}
				if (UpdateObject.OffHoursScanInterval == null)
				{
					return "ER#Please enter an off-hours Scan Interval that is a number, in minutes.";
				}
				//if ((UpdateObject.RetryInterval) > (UpdateObject.ScanInterval))
				//{
				//    return "ER#Please enter a Retry Interval that is less than the Scan Interval";

				//}


			}
			catch (Exception t)
			{

				throw t;
			}
			return "";




		}

		public DataTable GetIPAddress(IBMConnectionsServers StObj)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.IBMConnectionsServersDAL.Ins.GetIPAddress(StObj);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public Object insertdetail(IBMConnectionsServers insertObject)
		{
			//bool insert=false;
			Object returnVal = ValidateSametimeServerUpdate(insertObject);
			try
			{
				if (returnVal.ToString() == "")
				{
					return VSWebDAL.ConfiguratorDAL.IBMConnectionsServersDAL.Ins.insertdetails(insertObject);
				}
				else return returnVal;
			}
			catch (Exception ex)
			{

				throw ex;
			}



		}

		public DataTable GetIBMConnectionsCredentials(int ServerTypeID)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.IBMConnectionsServersDAL.Ins.GetIBMConnectionsCredentials(ServerTypeID);
			}
			catch (Exception ex)
			{

				throw ex;
			}


		}

		public Object UpdateDatafortestsnew(IBMConnectionTests IBMConnectiondata)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.IBMConnectionsServersDAL.Ins.UpdateDatafortestsnew(IBMConnectiondata);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public int GetIBMConnectionTestsId(string tests)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.IBMConnectionsServersDAL.Ins.GetIBMConnectionTestsId(tests);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public DataTable GetIBMConnectionTestsData(int ID)
		{

			return VSWebDAL.ConfiguratorDAL.IBMConnectionsServersDAL.Ins.GetIBMConnectionTestsData(ID);
		}

		public DataTable GetwebspherecellforIBMC(IBMConnectionsServers Stobj)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.IBMConnectionsServersDAL.Ins.GetwebspherecellforIBMC(Stobj);

			}
			catch (Exception ex)
			{

				throw ex;
			}
		}

		public bool InsertData(WebsphereCell STSettingsObject, int key)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.IBMConnectionsServersDAL.Ins.InsertData1(STSettingsObject, key);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public DataTable GetwebspherecellforIBMCS(IBMConnectionsServers Stobj, int key )
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.IBMConnectionsServersDAL.Ins.GetwebspherecellforIBMCS(Stobj, key);

			}
			catch (Exception ex)
			{

				throw ex;
			}
		}

		public DataTable GetcellID(WebsphereCell Stobj)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.IBMConnectionsServersDAL.Ins.GetcellID(Stobj);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public bool InsertwebsphereSametimenodesandservers(VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.Cell cells, int id, int Sid)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.IBMConnectionsServersDAL.Ins.InsertwebsphereSametimenodesandservers(cells, id, Sid);
			}
			catch (Exception ex)
			{

				throw ex;
			}
			//return VSWebDAL.SecurityDAL.ServersDAL.Ins.UpdateData(ServerObject);

		}

		public DataTable FetsametimeserversbycellID(int cellID)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.IBMConnectionsServersDAL.Ins.FetsametimeserversbycellID(cellID);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public DataTable GetCredentialID(string name)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.IBMConnectionsServersDAL.Ins.GetCredentialID(name);
			}
			catch (Exception ex)
			{

				throw ex;
			}


		}


		
	}
}
