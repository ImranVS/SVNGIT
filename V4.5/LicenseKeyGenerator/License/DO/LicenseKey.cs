using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DO
{
	public class LicenseKey
	{
public  LicenseKey()
	{}


		public string Key
		{
			get
			{
				return _Key;
			}
			set
			{
				_Key = value;
			}
		}
		private string _Key;
		public int Units
		{
			get { return _Units; }
			set { _Units = value; }
		}
		private int _Units;
		public string InstallType
		{
			get
			{
				return _InstallType;
			}
			set
			{
				_InstallType = value;
			}
		}
		private string _InstallType;
		public string CompanyName
		{
			get
			{
				return _CompanyName;
			}
			set
			{
				_CompanyName = value;
			}
		}
		private string _CompanyName;
		public string LicenseType
		{
			get
			{
				return _LicenseType;
			}
			set
			{
				_LicenseType = value;
			}
		}
		private string _LicenseType;
		public string ExpirationDate
		{
			get
			{
				return _ExpirationDate;
			}
			set
			{
				_ExpirationDate = value;
			}
		}
		private string _ExpirationDate;
		public string EncUnits
		{
			get
			{
				return _EncUnits;
			}
			set
			{
				_EncUnits = value;
			}
		}
		private string _EncUnits;
		public int CreateBy
		{
			get { return _CreateBy; }
			set { _CreateBy = value; }
		}
		private int _CreateBy;
		public DateTime CreatedOn
		{
			get { return _CreatedOn; }
			set { _CreatedOn = value; }
		}
		private DateTime _CreatedOn;
		public int ID
		{
			get { return _ID; }
			set { _ID = value; }
		}
		private int _ID;
		
		public int CompanyID
		{
			get { return CompanyID; }
			set { _CompanyID = value; }
		}
		private int _CompanyID;


		public LicenseKey(String Key,
			int Units,
			string InstallType,
			string CompanyName,
			string LicenseType,
			string ExpirationDate,
			string EncUnits,
			int CreateBy,
 DateTime CreatedOn, int ID, int CompanyID)
		{
			this._Key = Key;
			this._Units = Units;
			this._InstallType = InstallType;
			this._CompanyName = CompanyName;
			this._LicenseType = LicenseType;
			this._ExpirationDate = ExpirationDate;
			this._EncUnits = EncUnits;
			this._CreateBy = CreateBy;
			this._CreatedOn = CreatedOn;
			this._Units = Units;
			this._ID = ID;

		}

	}
}
