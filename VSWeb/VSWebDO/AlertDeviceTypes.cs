using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
   public class AlertDeviceTypes
    {
       public AlertDeviceTypes()
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
       public int ServerTypeID
       {
           get { return _ServerTypeID; }
           set { _ServerTypeID = value; }
       }
       private int _ServerTypeID;


       public AlertDeviceTypes(int ID,
           int AlertKey,
           int ServerTypeID)
       {
           this._ID = ID;
           this._AlertKey = AlertKey;
           this._ServerTypeID = ServerTypeID;
       }
    }
}
