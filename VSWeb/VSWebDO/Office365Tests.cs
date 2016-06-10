using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
	public class Office365Tests
			 
	{
		public Office365Tests()
		{
		}
		public int ServerId
		{
			get { return _ServerId; }
			set { _ServerId = value; }
		}
		private int _ServerId;
		public string Type
		{
			get { return _Type; }
			set { _Type = value; }
		}
		private string _Type;

		public string Tests
		{
			get { return _Tests; }
			set { _Tests = value; }
		}
		private string _Tests;
		public bool EnableSimulationTests
		{
			get { return _EnableSimulationTests; }
			set { _EnableSimulationTests = value; }
		}
		private bool _EnableSimulationTests;
		public bool SubscribedAlerts
		{
			get { return _SubscribedAlerts; }
			set { _SubscribedAlerts = value; }
		}
		private bool _SubscribedAlerts;

		public int ResponseThreshold
		{

			get { return _ResponseThreshold; }
			set { _ResponseThreshold = value; }
		}
		private int _ResponseThreshold;
		public int Id
		{
			get { return _Id; }
			set { _Id = value; }
		}
		private int _Id;

	}

 
}
