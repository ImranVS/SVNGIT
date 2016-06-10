using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.ConfiguratorBL
{
   public  class BlackBerryServersBL
    {
        private static BlackBerryServersBL _self = new BlackBerryServersBL();
        public static BlackBerryServersBL Ins
        {
            get
            {
                return _self;
            }
        }
        public Object ValidateBlackBerryServers(BlackBerryServers BlackBerryServerObject)
        {
            Object returnvalue = "";
            try
            {
               // if (BlackBerryServerObject.Name == null || BlackBerryServerObject.Name == "")
               // {
                //    return "ER#Please enter the BlackBerryServers Name";
               // }
               // if (BlackBerryServerObject.Description == null || BlackBerryServerObject.Description == "")
               // {
               //     return "ER#Please enter a description of the device";
               // }
                if (BlackBerryServerObject.Category == null || BlackBerryServerObject.Category == "")
                {
                    return "ER#Please enter a category";
                }
                if (BlackBerryServerObject.ScanInterval == null)
                {
                    return "ER#Please enter a Scan Interval";
                }
                if (BlackBerryServerObject.OffHoursScanInterval == null)
                {
                    return "ER#Please enter a OffHoursScanInterval";
                }
                if (BlackBerryServerObject.RetryInterval == null)
                {
                    return "ER#Please enter a RetryInterval";
                }
			//    if (BlackBerryServerObject.RetryInterval > BlackBerryServerObject.ScanInterval)
			//    {
			//        return "ER#Please enter a ScanInterval greater than RetryInterval";
			//    }
			}
            catch (Exception ex)
            {
                throw ex;
            }
            return returnvalue;



        }





       public DataTable fillgrid()
       {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.BlackBerryServersDAL.Ins.getfillgrid();
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
           
       }

	   public DataTable getfillgridbyUser(int userid)
       {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.BlackBerryServersDAL.Ins.getfillgridbyUser(userid);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
           
       }
       public Object deleteBlackBerryServer(BlackBerryServers BlackBerryServersObject)
       {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.BlackBerryServersDAL.Ins.deletefromgrid(BlackBerryServersObject);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
           
       }

       public Object insertBlackBerryServer(BlackBerryServers BlackBerryServersObject)
       {
           //bool insert = false;
           Object returnvalue = ValidateBlackBerryServers(BlackBerryServersObject);
		   try
		   {
			   if (returnvalue.ToString() == "")
			   {
				   return VSWebDAL.ConfiguratorDAL.BlackBerryServersDAL.Ins.insertBlackBerryService(BlackBerryServersObject);
			   }
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
          
           return returnvalue;
       }

       public DataTable getdatawithid(BlackBerryServers BlackBerryServerObject)
       {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.BlackBerryServersDAL.Ins.filldatabyid(BlackBerryServerObject);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
         
       }
       public DataTable getServerID(string servername)
       {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.BlackBerryServersDAL.Ins.getServerID(servername);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
          
       }
       public Object updatedetails(BlackBerryServers BlackBerryServerObject)
       {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.BlackBerryServersDAL.Ins.updateBlackBerrySever(BlackBerryServerObject);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
         

       }

       public DataTable finddatawithname(BlackBerryServers BlackBerryServerObject,string name)
       {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.BlackBerryServersDAL.Ins.getthevaluewithname(BlackBerryServerObject, name);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
        
       }

       public DataTable fillcombo(BlackBerryServers BlackBerryServerObject)
       {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.BlackBerryServersDAL.Ins.fillcombo1(BlackBerryServerObject);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
           
       }
       //public DataTable getkey(string name)
       //{
       //    return VSWebDAL.ConfiguratorDAL.BlackBerryServersDAL.Ins.getkey(name);
       //}

       //public object Insert(BlackBerryServers BlackBerryServerObject)
       //{
       //    return VSWebDAL.ConfiguratorDAL.BlackBerryServersDAL.Ins.Insert(BlackBerryServerObject);
       //}
      
       public DataTable getHAName(string Id)
       {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.BlackBerryServersDAL.Ins.getHAName(Id);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
           
       }
	   public DataTable GetBESName(string Name)
       {
		   return VSWebDAL.ConfiguratorDAL.BlackBerryServersDAL.Ins.GetBESName(Name);
	   }
    }

}
