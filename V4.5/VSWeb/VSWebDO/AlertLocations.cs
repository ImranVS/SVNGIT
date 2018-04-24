using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace VSWebDO
{
   public class AlertLocations
    {
       public AlertLocations()
       {
         
       }
       public int ID
       {

           get { return _ID; }
           set { _ID = value; }

       }
       private int _ID;
       public int LocationID
       {
           get { return _LocationID; }
           set { _LocationID = value; }
       }
       private int _LocationID;
       public int AlertKey
       {
           get { return _AlertKey; }
           set { _AlertKey = value; }
       }
       private int _AlertKey;
       public AlertLocations(int ID,
           int LocationID,
           int AlertKey)
       {

           this._ID = ID;
           this._LocationID = LocationID;
           this._AlertKey = AlertKey;
       }
    }
}
