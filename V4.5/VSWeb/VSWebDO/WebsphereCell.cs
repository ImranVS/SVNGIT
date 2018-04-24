using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
public	class WebsphereCell
	{
	public WebsphereCell()
	{}
	public int CellID
	{
		get { return _CellID; }
		set { _CellID = value; }
	}
	private int _CellID;
	public int SametimeId
	{
		get { return _SametimeId; }
		set { _SametimeId = value; }
	}
	private int _SametimeId;

	public string CellName
	{

		get { return _CellName; }
		set { _CellName = value; }

	}
	private string _CellName;
	public string HostName
	{

		get { return _HostName; }
		set { _HostName = value; }

	}
	private string _HostName;
	public string ConnectionType
	{

		get { return _ConnectionType; }
		set { _ConnectionType = value; }

	}
	private string _ConnectionType;
	public string Name
	{

		get { return _Name; }
		set { _Name = value; }

	}
	private string _Name;



	public int PortNo
	{
		get { return _PortNo; }
		set { _PortNo = value; }
	}
	private int _PortNo;

	public bool GlobalSecurity
	{
		get { return _GlobalSecurity; }
		set { _GlobalSecurity = value; }
	}
	private bool _GlobalSecurity;

	public int CredentialsID
	{
		get { return _CredentialsID; }
		set { _CredentialsID = value; }
	}
	private int _CredentialsID;


	public string Realm
	{

		get { return _Realm; }
		set { _Realm = value; }

	}
	private string _Realm;
	public int IBMConnectionSID
	{
		get { return _IBMConnectionSID; }
		set { _IBMConnectionSID = value; }
	}
	private int _IBMConnectionSID;

	public WebsphereCell(int CellID, string CellName, string HostName, string ConnectionType, int PortNo, bool GlobalSecurity, int CredentialsID, string Realm, string Name, int IBMConnectionSID)
    {
		this._CellID = CellID;
		this._CellName = CellName;
		this._HostName = HostName;
		this._ConnectionType = ConnectionType;
		this._PortNo = PortNo;
		this._GlobalSecurity = GlobalSecurity;

		this._CredentialsID = CredentialsID;
		this._Realm = Realm;
		this._Name = Name;
		this._IBMConnectionSID = IBMConnectionSID;
		
	}
		
		


	}
}
