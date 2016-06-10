using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
    public class Nodes
    {
          /// <summary>
	/// Default Contructor
	/// <summary>
        public Nodes()
	{}

        public int ID
        { 
            get { return _ID; }
            set { _ID = value; }  
        }
        private int _ID;
		public string Name
        {
			get { return _Name; }
			set { _Name = value; }
        }
		private string _Name;

		public string HostName
		{
			get { return _HostName; }
			set { _HostName = value; }
		}
		private string _HostName;

		public int Alive
		{
			get { return _Alive; }
			set { _Alive = value; }
		}
		private int _Alive;

		public string Version
		{
			get { return _Version; }
			set { _Version = value; }
		}
		private string _Version;

		public int CredentialsID
		{
			get { return _CredentialsID; }
			set { _CredentialsID = value; }
		}
		private int _CredentialsID;

		public string NodeType
		{
			get { return _NodeType; }
			set { _NodeType = value; }
		}
		private string _NodeType;

		public float LoadFactor
		{
			get { return _LoadFactor; }
			set { _LoadFactor = value; }
		}
		private float _LoadFactor;

		public string NodeTime
		{
			get { return _NodeTime; }
			set { _NodeTime = value; }
		}
		private string _NodeTime;

		public string Pulse
		{
			get { return _Pulse; }
			set { _Pulse = value; }
		}
		private string _Pulse;

		public	bool IsPrimaryNode
		{
			get { return _IsPrimaryNode; }
			set { _IsPrimaryNode = value; }
		}
		private bool _IsPrimaryNode;

		public int LocationID
		{
			get { return _LocationID; }
			set { _LocationID = value; }
		}
		private int _LocationID;

		public bool IsConfiguredPrimaryNode
		{
			get { return _IsConfiguredPrimaryNode; }
			set { _IsConfiguredPrimaryNode = value; }
		}
		private bool _IsConfiguredPrimaryNode;
		

        /// <summary>
        /// User defined Contructor
        /// <summary>
		public Nodes(int ID, string Name,

		 string HostName, int Alive, string Version,
			int CredentialsID, string NodeType, float LoadFactor, string NodeTime, string Pulse, bool IsPrimaryNode, bool IsConfiguredPrimaryNode)
            {
        this._ID = ID;
		this._Name = Name;
		this._HostName = HostName;
		this._Alive = Alive;
		this._Version = Version;
		this._CredentialsID = CredentialsID;
		this._NodeType = NodeType;
		this._LoadFactor = LoadFactor;
		this._NodeTime = NodeTime;
		this._Pulse = Pulse;
		this._IsPrimaryNode = IsPrimaryNode;
		this._IsConfiguredPrimaryNode = IsConfiguredPrimaryNode;


        }
           
        
    }
}
