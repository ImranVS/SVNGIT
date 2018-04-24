using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Security;
using System.Web;
using System.Web.Security;
namespace vsMobileContracts
{
	[ServiceContract]
	public interface IMobileSC
	{
		[OperationContract]
		[WebInvoke(Method = "GET",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		List<ServerStatus> getServerStatus();

		[OperationContract]
		[WebInvoke(Method = "GET",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		List<ServerDetail> getServerInfo(string serverType, string status);

		[OperationContract]
		[WebInvoke(Method = "GET",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		List<ServerDetails> getExecutiveSummary();

		[OperationContract]
		[WebInvoke(Method = "GET",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		int registerDeviceInfo(string deviceId, string OsType,string deviceType);
	}
}
