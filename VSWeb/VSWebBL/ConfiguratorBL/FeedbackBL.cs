using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.ConfiguratorBL
{
	public class FeedbackBL
    {
      
		private static FeedbackBL _self = new FeedbackBL();

       
		public static FeedbackBL Ins
        {
            get { return _self; }
        }


		public bool InsertData(FeedBack StatusObj)
		{
			

			return VSWebDAL.ConfiguratorDAL.FeedbackDAL.Ins.InsertData(StatusObj);
		}
		public DataTable GetFeedback()
		{


			return VSWebDAL.ConfiguratorDAL.FeedbackDAL.Ins.GetFeedback();
			

		}
		public bool UpdateFeedback(FeedBack fgd)
		{
			
			return VSWebDAL.ConfiguratorDAL.FeedbackDAL.Ins.UpdateFeedback(fgd);
		}
		public DataTable GetCompanyName()
		{


			return VSWebDAL.ConfiguratorDAL.FeedbackDAL.Ins.GetCompanyName();
	
			
		}

    }
}
