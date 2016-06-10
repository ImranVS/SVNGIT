//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace VSWebDO
//{
public class WebSphereNodes
{

    public int ID
    {
        get { return _ID; }
        set { _ID = value; }
    }
    private int _ID;
    public int ServerID
    {
        get { return _ServerID; }
        set { _ServerID = value; }
    }
    private int _ServerID;
    public bool Enabled
    {
        get { return _Enabled; }
        set { _Enabled = value; }
    }
    private bool _Enabled;
    public string Type
    {
        get { return _Type; }
        set { _Type = value; }
    }
    private string _Type;
    public string LoginName
    {
        get { return _LoginName; }
        set { _LoginName = value; }
    }
    private string _LoginName;

    public string Password
    {
        get { return _Password; }
        set { _Password = value; }
    }
    private string _Password;

    public string Connector
    {
        get { return _Connector; }
        set { _Connector = value; }
    }
    private string _Connector;

    public string NodeName
    {
        get { return _NodeName; }
        set { _NodeName = value; }
    }
    private string _NodeName;
    public int Port
    {
        get { return _Port; }
        set { _Port = value; }
    }
    private int _Port;
    /// <summary>
    public WebSphereNodes(string Name,
        int ID,
        int ServerID,
        bool Enabled,
        string Type,

           string LoginName,

        string Password,
         string Connector,
        string NodeName,
        int Port)
    {
        this.ID = _ID;
        this.ServerID = _ServerID;
        this.Enabled = _Enabled;
        this.Type = _Type;
        this.LoginName = _LoginName;
        this.Password = _Password;
        this.Connector = _Connector;
        this.NodeName = _NodeName;
        this.Port = _Port;
       
    }

    public WebSphereNodes()
    {
        // TODO: Complete member initialization
    }
}
//}
