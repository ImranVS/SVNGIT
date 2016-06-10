using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
   public class AlertEvents
    {
       public AlertEvents()
       {}
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
       public int EventID
       {
           get {return _EventID; }
           set { _EventID = value; }
       }
       private int _EventID;
       public AlertEvents(int ID,
           int AlertKey,
           int EventID)
       {
           this._ID = ID;
           this._AlertKey = AlertKey;
           this._EventID = EventID;
       }
    }
}
