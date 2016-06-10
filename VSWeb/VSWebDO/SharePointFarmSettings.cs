
namespace VSWebDO
{
	public class SharePointFarmSettings
	{
		/// <summary>
		/// Default Contructor
		/// <summary>
		public SharePointFarmSettings()
		{ }


		public string FarmName
		{
			get { return _FarmName; }
			set { _FarmName = value; }
		}
		private string _FarmName;


		public string TestAppURL
		{
			get { return _TestAppURL; }
			set { _TestAppURL = value; }
		}
		private string _TestAppURL;


		public bool SiteCreationTest
		{
			get { return _SiteCreationTest; }
			set { _SiteCreationTest = value; }
		}
		private bool _SiteCreationTest;


		public bool FileUploadTest
		{
			get { return _FileUploadTest; }
			set { _FileUploadTest = value; }
		}
		private bool _FileUploadTest;

		public bool LogOnTest
		{
			get { return _LogOnTest; }
			set { _LogOnTest = value; }
		}
		private bool _LogOnTest;

		/// <summary>
		/// User defined Contructor
		/// <summary>

	}
}
