using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
   public class AlertServers
    {
       public AlertServers()
       { }
       public int ID
       {
           get { return _ID; }
           set { _ID = value; }
       }
       private int _ID;
       public int AlertKey
       {
           get { return _AlertKey; }
           set { _AlertKey = value; }
       }
       private int _AlertKey;

       public int ServerID
       {
           get { return _ServerID; }
           set { _ServerID = value; }
       }
       private int _ServerID;

       public AlertServers(int ID,
           int AlertKey,
           int ServerID)
       {
           this._ID = ID;
           this._AlertKey = AlertKey;
           this._ServerID = ServerID;
       }

    }
}
