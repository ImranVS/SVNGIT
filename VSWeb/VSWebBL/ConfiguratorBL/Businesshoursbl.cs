using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.ConfiguratorBL
{
	public class Businesshoursbl
	{
		private static Businesshoursbl _self = new Businesshoursbl();
		public static Businesshoursbl Ins
		{
			get { return _self; }
		}

		public bool InsertData(HoursIndicator BusinesshoursObject)

		{


			//return VSWebDAL.ConfiguratorDAL.FeedbackDAL.Ins.InsertData(StatusObj);
			return VSWebDAL.ConfiguratorDAL.Businesshoursdal.Ins.InsertData(BusinesshoursObject);
		}
		//public DataTable GetDefaultGMT()
		//{


		//    return VSWebDAL.ConfiguratorDAL.Businesshoursdal.Ins.GetDefaultGMT();


		//}
		public DataTable GetBusinessHours()
		{


			return VSWebDAL.ConfiguratorDAL.Businesshoursdal.Ins.GetBusinessHours();


		}
		public HoursIndicator GetData(HoursIndicator BusinessObjects)
		{
	
			return VSWebDAL.ConfiguratorDAL.Businesshoursdal.Ins.GetData(BusinessObjects);
		}
		public Object DeleteBusinessHoursDetails(HoursIndicator Business)

		{
			//return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.DeleteBusinessHoursDetails(Business);
			return VSWebDAL.ConfiguratorDAL.Businesshoursdal.Ins.DeleteBusinessHoursDetails(Business);

		}
		public bool UpdateBusinesshours(HoursIndicator obj)
		{

			return VSWebDAL.ConfiguratorDAL.Businesshoursdal.Ins.UpdateBusinesshours(obj);
		}
		public bool UpdateAlertDetails(HoursIndicator obj)
		{

			return VSWebDAL.ConfiguratorDAL.Businesshoursdal.Ins.UpdateAlertDetails(obj);
		}
		public DataTable GetBusinesshoursNames()
		{
		
			return VSWebDAL.ConfiguratorDAL.Businesshoursdal.Ins.GetBusinesshoursNames();
		}
		public HoursIndicator GetDataForBUsinesshrs(HoursIndicator Busibject)
		{
		
			return VSWebDAL.ConfiguratorDAL.Businesshoursdal.Ins.GetDataForBUsinesshrs(Busibject);
		}
		public DataTable GetName(HoursIndicator Busibject)

		{

			return VSWebDAL.ConfiguratorDAL.Businesshoursdal.Ins.GetName(Busibject);
		}
		public DataTable GetBusiandOffhoursName(HoursIndicator Busibject)
		{

			return VSWebDAL.ConfiguratorDAL.Businesshoursdal.Ins.GetBusiandOffhoursName(Busibject);
		}
		public DataTable GetNamebydropdown(HoursIndicator Busibject)
		{

			return VSWebDAL.ConfiguratorDAL.Businesshoursdal.Ins.GetNamebydropdown(Busibject);
		}
	}
}
