using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
     public class Maintenance
    {
        /// <summary>
        /// Default Contructor
        /// <summary>
       public Maintenance()
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
         public System.DateTime StartDate
         {
             get { return _StartDate; }
             set { _StartDate = value; }
         }
         private System.DateTime _StartDate;
         public System.DateTime StartTime
         {
             get { return _StartTime; }
             set { _StartTime = value; }
         
         }
         private System.DateTime _StartTime;
         public int Duration
         {
             get { return _Duration; }
             set { _Duration = value; }

         }

         private int _Duration;

         public System.DateTime EndDate
         {
             get { return _EndDate; }
             set { _EndDate = value; }
         }

         private System.DateTime _EndDate;
         public string MaintType
         {
             get { return _MaintType; }
             set { _MaintType = value; }
         
         }
         private string _MaintType;

         public string MaintDaysList
         {
             get { return _MaintDaysList; }
             set { _MaintDaysList = value; }
         
         }
         private string _MaintDaysList;

         public string EndDateIndicator
         {
             get { return _EndDateIndicator; }
             set { _EndDateIndicator = value; }         
         }
         private string _EndDateIndicator;

        /// <summary>
        /// User defined Contructor
        /// <summary>

         public Maintenance(int ID,
             string Name,
             System.DateTime StartDate,
             System.DateTime StartTime,
             int Duration,
             System.DateTime EndDate,
              string MaintType,
             string MaintDaysList,
             string EndDateIndicator)
         {

             this._ID = ID;
             this._Name = Name;
             this._StartDate = StartDate;
             this._StartTime = StartTime;
             this._MaintType = MaintType;
             this._Duration = Duration;
             this._EndDate = EndDate;
             this._EndDateIndicator = EndDateIndicator;
             this._MaintDaysList = MaintDaysList;

         
         }

           
       
     
     }
}
