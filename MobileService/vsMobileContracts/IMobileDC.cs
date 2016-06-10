using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace vsMobileContracts
{
	[DataContract]
	public class ServerResults
	{

		[DataMember]
		public string Status { get; set; }
		[DataMember]
		public int Total { get; set; }
	}

	[DataContract]
	public class ServerStatus
	{
		[DataMember]
		public ServerResults[] ServerResults { get; set; }
		[DataMember]
		public string ServerType { get; set; }
	}

	[DataContract]
	public class ServerDetail
	{
		[DataMember]
		public string ServerName { get; set; }

		[DataMember]
		public string ServerDetails { get; set; }

		[DataMember]
		public string Comment { get; set; }

		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public string Status { get; set; }
	}

	[DataContract]
	public class QueryStatus
	{
		[DataMember]
		public string StatusName { get; set; }
	}

	[DataContract]
	public class ServerDetails
	{
		[DataMember]
		public string Status { get; set; }
		[DataMember]
		public ServerDetail[] sd { get; set; }
	}
}
