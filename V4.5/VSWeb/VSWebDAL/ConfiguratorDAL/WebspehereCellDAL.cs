using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
namespace VSWebDAL.ConfiguratorDAL
{
public	class WebspehereCellDAL
	{
		private Adaptor objAdaptor = new Adaptor();
		private static WebspehereCellDAL _self = new WebspehereCellDAL();

		public static WebspehereCellDAL Ins
		{
			get { return _self; }
		}
		public Object Deletecell(WebsphereCell cell)
		{
			Object Delete;
			try
			{
				string SqlQuery = "delete from Servers where ServerName in (select ServerName from WebSphereServer where CellId=" + cell.CellID + ") and ServerTypeID=22;" + 
					"delete from WebSphereServer where CellID=" + cell.CellID + ";" +
					"delete from WebSphereNode where CellID=" + cell.CellID + ";" +
					"delete from WebSphereCellStats where CellID=" + cell.CellID + ";" +
					"delete from WebSphereCell where CellID=" + cell.CellID + ";";


				//Delete = objAdaptor.ExecuteNonQuery(S);
				//string s1 = "Delete from AlertDeviceTypes where AlertKey=" + Alert.AlertKey;
				//Delete = objAdaptor.ExecuteNonQuery(s1);
				//string s2 = "Delete from AlertEvents where AlertKey=" + Alert.AlertKey;
				//Delete = objAdaptor.ExecuteNonQuery(s2);
				//string s3 = "Delete from AlertLocations where AlertKey=" + Alert.AlertKey;
				//Delete = objAdaptor.ExecuteNonQuery(s3);
				//string s4 = "Delete from AlertServers where AlertKey=" + Alert.AlertKey;
				//Delete = objAdaptor.ExecuteNonQuery(s4);
				//string SqlQuery = "Delete from AlertNames where AlertKey=" + Alert.AlertKey;
				Delete = objAdaptor.ExecuteNonQuery(SqlQuery);
			}
			catch
			{

				Delete = false;
			}
			finally
			{
			}

			return Delete;

		}

		public DataTable Fillcombobox()
        {
            DataTable WebsphereCellnames = new DataTable();
            try
            {

				string SqlQuery = "Select CellID,CellName from WebsphereCell";
				WebsphereCellnames = objAdaptor.FetchData(SqlQuery);
				
            }
			catch (Exception ex)
			{
				throw ex;
			}
			return WebsphereCellnames;
        }
		public DataTable GetWebsphereCellNames()
        {
            DataTable WebsphereNamestab = new DataTable();
            try
            {

				string SqlQuery = "select wc.CellID,wc.Name,wc.CredentialsID,wc.CellName,wc.HostName,wc.ConnectionType,wc.PortNo,wc.GlobalSecurity,wc.Realm,cd.AliasName as AliasName from WebsphereCell wc left outer join Credentials cd  on wc.CredentialsID=cd.ID";
				WebsphereNamestab = objAdaptor.FetchData(SqlQuery);
			
            }
			catch (Exception ex)
			{
				throw ex;
			}

			return WebsphereNamestab;
        }
	}
}
