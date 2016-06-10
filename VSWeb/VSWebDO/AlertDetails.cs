using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
   public class AlertDetails
    {
       public AlertDetails()
       {
       }

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

       public string StartTime
       {
           get { return _StartTime; }
           set { _StartTime = value; }
       
       }
       private string _StartTime;

       public string EndTime
       {
           get { return _EndTime; }
           set { _EndTime = value; }
       
       }
       private string _EndTime;
       public int HoursIndicator
       {
           get { return _HoursIndicator; }
           set { _HoursIndicator = value; }
       }
       private int _HoursIndicator;

       public string SendTo
       {
           get { return _SendTo; }
           set { _SendTo = value; }
       
       }
       private string _SendTo;
       public string CopyTo
       {
           get { return _CopyTo; }
           set { _CopyTo = value; }
       
       }
       private string _CopyTo;

       public string BlindCopyTo
       {
           get { return _BlindCopyTo; }
           set { _BlindCopyTo = value; }
       
       }
       private string _BlindCopyTo;

       //12/1/2014 NS added for VSPLUS-946
       public string SMSTo
       {
           get { return _SMSTo; }
           set { _SMSTo = value; }

       }
       private string _SMSTo;

       public string Day
       {
           get { return _Day; }
           set { _Day = value; }
       }

       private string _Day;

       private Boolean _SendSNMPTrap;

       public Boolean SendSNMPTrap
       {
           get { return _SendSNMPTrap; }
           set { _SendSNMPTrap = value; }
       }

       private int _Duration;

       public int Duration
       {
           get { return _Duration; }
           set{ _Duration = value;}
       }

       //4/4/2014 NS added for VSPLUS-519
       private Boolean _EnablePersistentAlert;

       public Boolean EnablePersistentAlert
       {
           get { return _EnablePersistentAlert; }
           set { _EnablePersistentAlert = value; }
       }

       //12/4/2014 NS added for VSPLUS-1229
       private int _ScriptID;
       public int ScriptID
       {
           get { return _ScriptID; }
           set { _ScriptID = value; }
       }

       //public string EscalateTo
       //{

       //    get { return _EscalateTo; }
       //    set { _EscalateTo = value; }
       //}
       //private string _EscalateTo;

       public AlertDetails(int ID,
           int AlertKey,
           string Starttime,
          string EndTime,
           int HoursIndicator,
           string SendTo,
           string CopyTo,
           string BlindCopyTo,
           string Day,
           bool SendSNMPTrap,
           int Duration,
           bool EnablePersistentAlert,
           string SMSTo,
           int ScriptID)

       {

           this._ID = ID;
           this._AlertKey = AlertKey;
           this._HoursIndicator = HoursIndicator;
           this._SendTo = SendTo;
           //this._EscalateTo = EscalateTo;
           this._BlindCopyTo = BlindCopyTo;
           this._StartTime = Starttime;
           this._EndTime = EndTime;
           this._Day = Day;
           this._SendSNMPTrap = SendSNMPTrap;
           this._Duration = Duration;
           //4/4/2014 NS added for VSPLUS-519
           this._EnablePersistentAlert = EnablePersistentAlert;
           //12/4/2014 NS added for VSPLUS-1229
           this._SMSTo = SMSTo;
           this._ScriptID = ScriptID;
       }



   }
}
