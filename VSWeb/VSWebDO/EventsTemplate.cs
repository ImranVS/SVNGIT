using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
	   public class EventsTemplate
	{
		   public EventsTemplate()
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
		   public string EventID
		   {
			   get { return _EventID; }
			   set { _EventID = value; }

		   }
		   private string _EventID;
		   public EventsTemplate(int ID, string Name, string EventID)
       {
           this._ID = ID;
           this._Name = Name;
		   this._EventID = EventID;
       
       }

	}
}
