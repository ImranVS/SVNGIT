using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDO;
using VSWebDAL;
using System.Data;
using VitalSignsWebSphereDLL;


namespace VSWebBL.SecurityBL
{
	public class webspehereImportBL
	{
		private static webspehereImportBL _self = new webspehereImportBL();

		/// <summary>
		/// Used to call the functions using class name instead of object
		/// </summary>
		public static webspehereImportBL Ins
		{
			get { return _self; }
		}

		public bool InsertData(WebsphereCell STSettingsObject)
        {
			try
			{
				return VSWebDAL.SecurityDAL.webspehereimportDAL.Ins.InsertData1(STSettingsObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            //return VSWebDAL.SecurityDAL.ServersDAL.Ins.UpdateData(ServerObject);
			
        }
		public DataTable GetnodesserversFromProcedure()
		{
			try
			{
				return VSWebDAL.SecurityDAL.webspehereimportDAL.Ins.GetServersFromProcedure();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public object InsertwebsphereData(Servers ServerObject)
		{

			try
			{
				return VSWebDAL.SecurityDAL.webspehereimportDAL.Ins.InsertwebsphereData(ServerObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
	
		}
        public object InsertwebsphereData1(Servers ServerObject)
        {
			try
			{
				return VSWebDAL.SecurityDAL.webspehereimportDAL.Ins.InsertwebsphereData1(ServerObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}

            

        }
		public bool Insertpwd(string AliasName, string UserID, string Password, int ServerType)
		{
			try
			{
				return VSWebDAL.SecurityDAL.webspehereimportDAL.Ins.Insertpwd(AliasName, UserID, Password, ServerType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable FetserversbycellID( int cellID)
		{
			try
			{
				return VSWebDAL.SecurityDAL.webspehereimportDAL.Ins.FetserversbycellID(cellID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public bool updatedata(int cellID,string enable)
		{
			try
			{
				return VSWebDAL.SecurityDAL.webspehereimportDAL.Ins.updatedata(cellID, enable);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable Getcelldata(int id)
		{
			try
			{
				return VSWebDAL.SecurityDAL.webspehereimportDAL.Ins.Getcelldata(id);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetCrentials(int id)
		{
			try
			{
				return VSWebDAL.SecurityDAL.webspehereimportDAL.Ins.GetCrentials(id);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public bool Insertpwd(string AliasName, string UserID, string Password)
		{
			try
			{
				return VSWebDAL.SecurityDAL.webspehereimportDAL.Ins.Insertpwd(AliasName, UserID, Password);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetAliasName(Credentials Csibject)
		{
			try
			{
				return VSWebDAL.SecurityDAL.webspehereimportDAL.Ins.GetAliasName(Csibject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public bool updatedataweb(int cellID, string sname, string enable)
		{
			try
			{
				return VSWebDAL.SecurityDAL.webspehereimportDAL.Ins.updatedataweb(cellID, sname, enable);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public bool updatewebservers(int cellID)
		{
			try
			{
				return VSWebDAL.SecurityDAL.webspehereimportDAL.Ins.updatewebservers(cellID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetServerName(Servers Csibject)
		{
			try
			{
				return VSWebDAL.SecurityDAL.webspehereimportDAL.Ins.GetServerName(Csibject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable FetsametimeserversbycellID(int cellID)
		{
			try
			{
				return VSWebDAL.SecurityDAL.webspehereimportDAL.Ins.FetsametimeserversbycellID(cellID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable Fetsametimeservers(int cellID)
		{
			try
			{
				return VSWebDAL.SecurityDAL.webspehereimportDAL.Ins.Fetsametimeservers(cellID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable GetCredentialValue(string Name)
		{
			try
			{
				return VSWebDAL.SecurityDAL.webspehereimportDAL.Ins.GetCredentialValue(Name);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			

		}
		

		public DataTable GetSpecificCellTypes()
		{
			try
			{
				return VSWebDAL.SecurityDAL.webspehereimportDAL.Ins.GetSpecificCellTypes();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetSpecificCellData(int id)
		{
			try
			{
				return VSWebDAL.SecurityDAL.webspehereimportDAL.Ins.GetSpecificCellData(id);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetNodeName(int CellID)
		{
			try
			{
				return VSWebDAL.SecurityDAL.webspehereimportDAL.Ins.GetNodeName(CellID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public bool Insertwebspherenodesandservers(VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.Cell cells, int id)
		{
			try
			{
				return VSWebDAL.SecurityDAL.webspehereimportDAL.Ins.Insertwebspherenodesandservers(cells, id);
			}
			catch (Exception ex)
			{

				throw ex;
			}
			//return VSWebDAL.SecurityDAL.ServersDAL.Ins.UpdateData(ServerObject);

		}
		public bool InsertwebsphereSametimenodesandservers(VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.Cell cells, int id,int Sid)
		{
			try
			{
				return VSWebDAL.SecurityDAL.webspehereimportDAL.Ins.InsertwebsphereSametimenodesandservers(cells, id,Sid);
			}
			catch (Exception ex)
			{

				throw ex;
			}
			//return VSWebDAL.SecurityDAL.ServersDAL.Ins.UpdateData(ServerObject);

		}
			public DataTable GetCellIdBynmae(string Name)
		{
			try
			{
				return VSWebDAL.SecurityDAL.webspehereimportDAL.Ins.GetCellIdBynmae(Name);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
			public DataTable GetNmae(string Name)

			{
				try
				{
					return VSWebDAL.SecurityDAL.webspehereimportDAL.Ins.GetNmae(Name);
				}
				catch (Exception ex)
				{

					throw ex;
				}

			}
			public DataTable GetcellinfobyIDandport(string IP, int portno)
			{
				try
				{
					return VSWebDAL.SecurityDAL.webspehereimportDAL.Ins.GetcellinfobyIDandport(IP, portno);
				}
				catch (Exception ex)
				{

					throw ex;
				}

			}
			public DataTable GetAllHostNmaes()
			{
				try
				{
					return VSWebDAL.SecurityDAL.webspehereimportDAL.Ins.GetAllHostNmaes();
				}
				catch (Exception ex)
				{

					throw ex;
				}

			}
	}

}
