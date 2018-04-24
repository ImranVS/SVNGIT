using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using vsMobileContracts;
using System.Web;
using System.ServiceModel.Activation;
using System.Web.Security;
using System.Threading;
using System.Configuration;
using System.Data;
using VSFramework;
namespace vsMobileService
{
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class MobileService:IMobileSC
	{
		public List<ServerStatus> getServerStatus()
		{
			string returnStr = "";
			//try
			//{
			
			string conString = ConfigurationManager.ConnectionStrings["VitalSignsConnectionString"].ToString();
			List<ServerResults> lp = new List<ServerResults>();
			List<ServerStatus> lSS = new List<ServerStatus>();
			ServerStatus SS = new ServerStatus();
			VSAdaptor VSA = new VSAdaptor();
			

			StringBuilder s = new StringBuilder();
			s.Append("SELECT COUNT(*) CNT,StatusCode from Status where  StatusCode in ('Issue','Maintenance','Not Responding','OK')  group by StatusCode");
			DataTable DT= VSA.FetchData(conString, s.ToString());
			SS.ServerType = "All";
			foreach (DataRow dr in DT.Rows)
			{
				ServerResults sr = new ServerResults();
				sr.Total = Convert.ToInt32(dr[0].ToString());
				sr.Status = dr[1].ToString();
				lp.Add(sr);
			}
			SS.ServerResults = lp.ToArray();
			lSS.Add(SS);

			StringBuilder s1 = new StringBuilder();
			s1.Append("SELECT COUNT(*) CNT,StatusCode,Type from Status WHERE  StatusCode in ('Issue','Maintenance','Not Responding','OK')  group by StatusCode,Type union SELECT COUNT(*) CNT,StatusCode,SecondaryRole+' (*)'  from Status WHERE  StatusCode in ('Issue','Maintenance','Not Responding','OK')  group by StatusCode,SecondaryRole  order by Type");
			 DT = VSA.FetchData(conString, s1.ToString());
			List<ServerResults> lp1 = new List<ServerResults>();
			string serverType = "";
			ServerStatus SS2 = new ServerStatus();
			foreach (DataRow dr in DT.Rows)
			{
				ServerResults sr = new ServerResults();
				sr.Total = Convert.ToInt32(dr[0].ToString());
				sr.Status = dr[1].ToString();
				if ((serverType != "" && serverType != dr[2].ToString()))
				{
					ServerStatus SS1 = new ServerStatus();
					SS1.ServerType = serverType;
					SS1.ServerResults = lp1.ToArray();
					lSS.Add(SS1);
					lp1.Clear();
				}
				lp1.Add(sr);
				serverType = dr[2].ToString();
			}
			DT.Dispose();
			SS2.ServerType = serverType;
			SS2.ServerResults = lp1.ToArray();
			lSS.Add(SS2);
			
			return lSS;
			//}
			//catch (Exception ex)
			//{
			//    returnStr = ex.Message.ToString();
			//}

			
		}

		public List<ServerDetail> getServerInfo(string serverType,string status)
		{
			List<ServerDetail> lSd = new List<ServerDetail>();
			string conString = ConfigurationManager.ConnectionStrings["VitalSignsConnectionString"].ToString();
			VSAdaptor VSA = new VSAdaptor();
			StringBuilder s = new StringBuilder();
			serverType = serverType.Replace(" (*)", "");
			if (serverType =="All" && status != "All")
				s.Append("select Name,details,description,status from status where StatusCode='" + status + "'");
			//else if(serverType =="All" && status == "All" )
			//    s.Append("select Name,details,description from status");
			else
				s.Append("select Name,details,description,status from status where (TYPE='" + serverType + "' OR SecondaryRole='" + serverType + "')  and StatusCode='" + status +"'");
			DataTable DT = VSA.FetchData(conString, s.ToString());
			foreach (DataRow dr in DT.Rows)
			{
				ServerDetail sd = new ServerDetail();
				sd.ServerName =dr[0].ToString();
				sd.ServerDetails = dr[1].ToString();
				sd.Comment = dr[2].ToString();
				sd.Status = dr["Status"].ToString();
				lSd.Add(sd);
			}

			return lSd;
		}

		public List<ServerDetails> getExecutiveSummary()
		{
			List<ServerDetails> lSd = new List<ServerDetails>();
			string conString = ConfigurationManager.ConnectionStrings["VitalSignsConnectionString"].ToString();
			VSAdaptor VSA = new VSAdaptor();
			StringBuilder s = new StringBuilder();
			List<ServerDetail> lSd1 = new List<ServerDetail>();
			s.Append("select Name,details,description,statuscode,status from  status where  StatusCode in ('Issue','Maintenance','Not Responding','OK') order by statuscode");
				string statusCode = "";
			DataTable DT = VSA.FetchData(conString, s.ToString());
			foreach (DataRow dr in DT.Rows)
			{
				ServerDetail sr = new ServerDetail();

				sr.Status = dr["Status"].ToString();
				sr.Comment=dr[2].ToString();
				sr.ServerName=dr[0].ToString();
				sr.ServerDetails = dr[1].ToString();

				if ((statusCode != "" && statusCode != dr[3].ToString()))
				{
					ServerDetails SS1 = new ServerDetails();
					SS1.Status = statusCode;
					SS1.sd = lSd1.ToArray();
					
					lSd.Add(SS1);
					lSd1.Clear();
				}
				lSd1.Add(sr);
				statusCode = dr[3].ToString();
			}
			ServerDetails SS2 = new ServerDetails();
			SS2.Status = statusCode;
			SS2.sd = lSd1.ToArray();

			lSd.Add(SS2);
			return lSd;
		}

		public int registerDeviceInfo(string deviceId, string osType, string deviceType)
		{
			VSAdaptor VSA = new VSAdaptor();
			object recordsAffected=0;
			try
			{
				StringBuilder s = new StringBuilder();
				s.Append("select count(*) as CNT from  MobileAppUsers where  DeviceId='" + deviceId + "'");
				object  IsExist=VSA.ExecuteScalarAny("", "", s.ToString());
				if (Convert.ToInt16(IsExist) == 0)
				{
					string SqlQuery = "insert into MobileAppUsers(DeviceId,OsType,DeviceType) values('" + deviceId + "','" + osType + "','" + deviceType + "')";
					recordsAffected = VSA.ExecuteNonQueryAny("", "", SqlQuery);
				}
				else
				{
					string SqlQuery = "update MobileAppUsers set OsType='" + osType + "',DeviceType='" + deviceType + "' Where Deviceid='" + deviceId + "'";
					recordsAffected = VSA.ExecuteNonQueryAny("", "", SqlQuery);
				}
			}
			catch
			{
				recordsAffected = 0;
			}
			finally
			{
			}
			return Convert.ToInt32(recordsAffected);
		}
	}
}
