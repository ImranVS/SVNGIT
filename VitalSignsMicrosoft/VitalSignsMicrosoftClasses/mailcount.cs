using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VitalSignsMicrosoftClasses
{
	public class MailCount
	{
		public string RecipientName
		{
			set;
			get;
		}	
		
		public int Inbound
		{
			get;
			set;
		}
		public int Outbound
		{
			get;
			set;
		}
		public int InboundSize
		{
			get;
			set;
		}
		public int OutboundSize
		{
			get;
			set;
		}
	}
}
