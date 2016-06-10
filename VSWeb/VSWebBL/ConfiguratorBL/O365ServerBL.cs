using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDAL;
using System.Data;
using VSWebDO;

namespace VSWebBL.ConfiguratorBL
{
	public class O365ServerBL
	{
		/// <summary>
		/// Declarations
		/// </summary>
		private static O365ServerBL _self = new O365ServerBL();

		/// <summary>
		/// Used to call the functions using class name instead of object
		/// </summary>
		public static O365ServerBL Ins
		{
			get { return _self; }
		}
		#region "Validations"


		/// <param name="URLsObject"></param>
		/// <returns></returns>
		public Object ValidateUpdate(O365Server O365ServerObject)
		{
			Object ReturnValue = "";
			try
			{
				if (O365ServerObject.Name == null || O365ServerObject.Name == "")
				{
					return "ER#Please enter a value in the Name field.";
				}

				if (O365ServerObject.Category == null || O365ServerObject.Category == " ")
				{
					return "ER#Please enter a value in the Category field.";
				}

				if (O365ServerObject.ResponseThreshold == null)
				{
					return "ER#Please enter a value in the Response Threshold field.";

				}
				if (O365ServerObject.ScanInterval.ToString() == "")
				{
					return "ER#Please enter a value in the Scan Interval field.";
				}

				if (O365ServerObject.OffHoursScanInterval.ToString() == " ")
				{
					return "ER#Please enter a value in the Off-Hours Scan Interval field.";
				}
				if (O365ServerObject.RetryInterval.ToString() == "")
				{
					return "ER#Please enter a value in the Retry Interval field.";
				}
				//if (O365ServerObject.URL == "" || O365ServerObject.URL == null || O365ServerObject.URL == "http://")
				//{
				//    return "ER#Please enter a value in the Address field, such as 'http://www.IBM.com'.";
				//}

				//if ((O365ServerObject.RetryInterval) > (O365ServerObject.ScanInterval))
				//{

				//    return "ER#Please enter a value in the Retry Interval field that is less than the Scan Interval value.";
				//}
			}
			catch (Exception ex)
			{ throw ex; }
			finally
			{ }
			return "";
		}

		#endregion
		/// <summary>
		/// Call to Get Data from URLs based on Primary key
		/// </summary>
		/// <param name="URLsObject">URLsObject object</param>
		/// <returns></returns>
		public O365Server GetData(O365Server O365ServerObject)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.GetData(O365ServerObject);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetOffice365TestsData(int ID)
		{

			return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.GetOffice365TestsData(ID);
		}
		/// <summary>
		/// Call to Get Data from DominoServers
		/// </summary>
		/// <returns></returns>
		public DataTable GetAllData()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.GetAllData();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		/// <summary>
		/// Call to Insert Data into URLs
		///  </summary>
		/// <param name="URLsObject">URLs object</param>
		/// <returns></returns>
		public bool InsertData(O365Server O365ServerObject)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.InsertData(O365ServerObject);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		/// <summary>
		/// Call to Update Data of DominoServers based on Key
		/// </summary>
		/// <param name="URLsObject">DominoServers object</param>
		/// <returns>Object</returns>
		public Object UpdateData(O365Server O365ServerObject)
		{
			Object ReturnValue = ValidateUpdate(O365ServerObject);
			try
			{
				if (ReturnValue.ToString() == "")
				{
					return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.UpdateData(O365ServerObject);
				}
				else return ReturnValue;
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public Object UpdateTestsData(O365Server O365ServerObject)
		{
			Object ReturnValue = ValidateUpdate(O365ServerObject);
			try
			{
				if (ReturnValue.ToString() == "")
				{
					return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.UpdateTestsData(O365ServerObject);
				}
				else return ReturnValue;
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public Object DeleteData(O365Server O365ServerObject)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.DeleteData(O365ServerObject);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetIPAddress(O365Server UrlObj, string mode)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.GetIPAddress(UrlObj, mode);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable Get(string id)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.Get(id);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetIdFromName(string name)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.GetIdFromName(name);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public bool InsertCustomPageValue(string userID, string URLval, string titleval, bool isprivate, string ID, bool doinsert)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.InsertCustomPageValue(userID, URLval, titleval, isprivate, ID, doinsert);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetCustomPageValue(string userID)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.GetCustomPageValue(userID);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public Int32 GetServerIDbyServerName(string serverName)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.GetServerIDbyServerName(serverName);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetCloudData()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.GetCloudData();
			}
			catch (Exception ex)
			{

				throw ex;
			}


		}
		public DataTable GetCloudDatavisible()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.GetCloudDatavisible();
			}
			catch (Exception ex)
			{

				throw ex;
			}


		}
		public DataTable GetCloudStatuses()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.GetCloudStatuses();
			}
			catch (Exception ex)
			{

				throw ex;
			}


		}
		public DataTable GetTestsTab(int ID)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.GetTestsTab(ID);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetTestsTabadd()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.GetTestsTabadd();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetNodesTabadd(string id)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.GetNodesTabadd(id);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetO365Nodes()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.GetO365Nodes();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetNodes()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.GetNodes();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public Boolean UpdateTests(int ID, List<object> fieldValues)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.UpdateTests(ID, fieldValues);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public Boolean UpdateNodes(int ID, List<object> fieldValues)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.UpdateNodes(ID, fieldValues);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public bool InsertDatafortests(List<object> fieldValues, string servername)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.InsertDatafortests(fieldValues, servername);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public Object UpdateDatafortestsnew(Office365Tests o365testdata)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.UpdateDatafortestsnew(o365testdata);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public Object InsertDatafortestsnew(Office365Tests o365testdata)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.InsertDatafortestsnew(o365testdata);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public bool InsertDataforNodes(List<object> fieldValues, string servername)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.O365ServerDAL.Ins.InsertDataforNodes(fieldValues, servername);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
	}
}
