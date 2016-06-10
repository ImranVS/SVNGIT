using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
	public class IBMConnectionTests
			 
	{
		public IBMConnectionTests()
		{
		}
		public bool EnableSimulationTests
		{
			get { return _EnableSimulationTests; }
			set { _EnableSimulationTests = value; }
		}
		private bool _EnableSimulationTests;
	

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

		public int ServerId
		{
			get { return _ServerId; }
			set { _ServerId = value; }
		}
		private int _ServerId;


	}

 
}
