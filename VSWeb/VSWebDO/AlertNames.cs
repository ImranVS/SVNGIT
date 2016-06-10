using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
   public class AlertNames
    {
       public AlertNames()
        {}

       public int AlertKey
       {
           get {return _AlertKey; }
           set { _AlertKey = value; }
       
       }
       private int _AlertKey;

       public string AlertName
       {
           get { return _AlertName; }
           set { _AlertName = value; }
       }
       private string _AlertName;

	   public int Templateid
	   {
		   get { return _Templateid; }
		   set { _Templateid = value; }
	   }
	   private int _Templateid;

	   public AlertNames(int AlertKey, string AlertName, int Templateid)
       {
           this._AlertKey = AlertKey;
           this._AlertName = AlertName;
		   this._Templateid = Templateid;
       
       }
    }

}
