using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL
{


   public class ExchangeBAL
    {
       private static ExchangeBAL _self = new ExchangeBAL();
       public static ExchangeBAL Ins
        {
            get { return _self; }
        }
       public DataTable GetAllData()
       {
           try
           {
               return VSWebDAL.ConfiguratorDAL.ExchangeDAL.Ins.GetAllData();
           }
           catch (Exception ex)
           {

               throw ex;
           }

       }

	   public DataTable GetAllDatabyUser(int UserID)
       {
           try
           {
			   return VSWebDAL.ConfiguratorDAL.ExchangeDAL.Ins.GetAllDatabyUser(UserID);
           }
           catch (Exception ex)
           {

               throw ex;
           }

       }

       public Object UpdateExchangeSettingsData(ExchangeSettings Mobj)
       {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.ExchangeDAL.Ins.UpdateExchangeSettingsData(Mobj);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
         
       }
       public DataTable GetExchangeSettings(ExchangeSettings Mobj)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ExchangeDAL.Ins.GetExchangeSettings(Mobj);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

       public DataTable GetExchangeServerDetails()
       {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.ExchangeDAL.Ins.GetExchangeServerDetails();
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
          
           //DataTable dt = VSWebDAL.ConfiguratorDAL.ExchangeDAL.Ins.GetExchangeServerDetails();
           //Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
           //List<string> myValues = new List<string>();

           //DataView dv = new DataView(dt);
           //if (dv.Count > 0)
           //{
           //    DataTable distinctServers = dv.ToTable(true, "ID");
           //    for (int i = 0; i < distinctServers.Rows.Count; i++)
           //    {
           //        int CAS = 0, HUB = 0, MBX = 0, EDGE = 0;
           //        for (int j = 0; j < dt.Rows.Count; j++)
           //        {                       
           //            if (distinctServers.Rows[i][0].ToString() == dt.Rows[j][0].ToString())
           //            {
           //                if (CAS == 1 || dt.Rows[j][3].ToString() == "1")
           //                {
           //                    CAS = 1;
           //                }
           //                if (EDGE == 1 || dt.Rows[j][4].ToString() == "1")
           //                {
           //                    EDGE = 1;
           //                }
           //                if (MBX == 1 || dt.Rows[j][2].ToString() == "1")
           //                {
           //                    MBX = 1;
           //                }
           //                if (HUB == 1 || dt.Rows[j][5].ToString() == "1")
           //                {
           //                    HUB = 1;
           //                }
           //            }
           //        }
           //        myValues.Add(MBX.ToString());
           //        myValues.Add(CAS.ToString());
           //        myValues.Add(EDGE.ToString());
           //        myValues.Add(HUB.ToString());
           //        dic.Add(distinctServers.Rows[i][0].ToString(), myValues);
           //    }
           //}
           //else
           //{
           //}

           //return dt;
       }
       public DataTable GetDAGStatus(string DagName)
       {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.ExchangeDAL.Ins.GetDAGStatus(DagName);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
          
       }
       public DataTable GetHubEdgeStatus()
       {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.ExchangeDAL.Ins.GetHubEdgeStatus();
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
           
       }
       public DataTable GetCASStatus()
       {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.ExchangeDAL.Ins.GetCASStatus();
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
           
       }
       public DataTable GetMailBoxStatus()
       {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.ExchangeDAL.Ins.GetMailBoxStatus();
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
          
       }

       public object UpdateServerRolesData(string ServerId, bool RoleHub, bool RoleMailBox, bool RoleCAS, bool RoleEdge, bool RoleUnified)
       {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.ExchangeDAL.Ins.UpdateServerRolesData(ServerId, RoleHub, RoleMailBox, RoleCAS, RoleEdge, RoleUnified);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
          
       }

       public string getServerVersion(string serverID)
       {
		   try
		   {
			    return VSWebDAL.ConfiguratorDAL.ExchangeDAL.Ins.getServerVersion(serverID);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
       }

	   public int GetMailboxRoleCount()
	   {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.ExchangeDAL.Ins.GetMailboxRoleCount();
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
		  
	   }
	   public DataTable GetAllData1()
	   {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.ExchangeDAL.Ins.GetAllData1();
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
		   
	   }
	   public object updateEnableLatencyTest(int id, int yellowthershold, int latency, bool checkedvalue)
	   {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.ExchangeDAL.Ins.updateEnableLatencyTest(id, yellowthershold, latency, checkedvalue);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
		  
	   }
    }
}
