using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VitalSignsMicrosoftClasses
{
 public	class O365ServiceDetails
	{
	 public int ServerId
		{
			set;
			get;
		}

	 public string ServiceName
		{
			get;
			set;
		}
	 public string ServiceID
		{
			get;
			set;
		}
	 public DateTime StartTime
		{
			get;
			set;
		}
	 public DateTime EndTime
		{
			get;
			set;
		}
	 public string Status
	 {
		 get;
		 set;
	 }
	 public string EventType
	 {
		 get;
		 set;
	 }
	 public string Message
	 {
		 get;
		 set;
	 }
	}
}
