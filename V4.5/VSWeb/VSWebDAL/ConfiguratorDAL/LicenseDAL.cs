using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.ConfiguratorDAL
{
	public class LicenseDAL
    {
        private Adaptor objAdaptor = new Adaptor();
		private static LicenseDAL _self = new LicenseDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
		public static LicenseDAL Ins
        {
            get { return _self; }
        }

		public bool InsertLicense(License keyvalue)
		{
			DataTable licensekey = new DataTable();
			bool UpdateRet = false;
			int mode = 0;
			try
			{
                int count = 0;
                /* 4/27/2015 NS modified for VSPLUS-1700 */
                /*
                string Query = "SELECT Count(*) FROM License WHERE LicenseKey = '" + keyvalue.LicenseKey + "'";
                count = objAdaptor.ExecuteScalar(Query);
                string SqlQuery = "";
                if (count == 0)
                {
                    SqlQuery = "INSERT INTO License([LicenseKey],[Units],[InstallType],[CompanyName],[LicenseType],[ExpirationDate]) VALUES('" + keyvalue.LicenseKey + "'," +
                    " " + keyvalue.Units + ",'" + keyvalue.InstallType + "','" + keyvalue.CompanyName + "','" + keyvalue.LicenseType + "','" + keyvalue.ExpirationDate + "')";
                }
                else
                {
                    SqlQuery = "UPDATE License SET LicenseKey='" + keyvalue.LicenseKey + "',Units=" + keyvalue.Units + ",InstallType='" + keyvalue.InstallType + "',CompanyName='" + keyvalue.CompanyName + "'," +
                        "LicenseType='" + keyvalue.LicenseType + "',ExpirationDate='" + keyvalue.ExpirationDate + "'";
                }
                */
                string Query = "SELECT Count(*) FROM License";
                count = objAdaptor.ExecuteScalar(Query);
                string SqlQuery = "";
                if (count != 0)
                {
                    SqlQuery = "DELETE FROM License";
                    objAdaptor.ExecuteNonQuery(SqlQuery);
                }
                SqlQuery = "INSERT INTO License([LicenseKey],[Units],[InstallType],[CompanyName],[LicenseType],[ExpirationDate]) VALUES('" + keyvalue.LicenseKey + "'," +
                    " " + keyvalue.Units + ",'" + keyvalue.InstallType + "','" + keyvalue.CompanyName + "','" + keyvalue.LicenseType + "','" + keyvalue.ExpirationDate + "')";
                mode = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
                if (mode == 1)
                {
                    licensekey = objAdaptor.FetchLicense("PR_EncDecLicense", false);
                    UpdateRet = true;
                }
			}
			catch
			{
			}
			return UpdateRet;
		}
		public DataTable GetLicensedetails()
		{
			
			DataTable BsTable = new DataTable();
			try
			{

				string sqlQuery = "Select * from License";
				BsTable = objAdaptor.FetchData(sqlQuery);
				
			}
			catch (Exception)
			{

				throw;
			}
			return BsTable;

		}
		public DataTable Getlicenseusage()

		{
			DataTable licenseusage = new DataTable();
			try
			{
				//string sqlQuery = "select ST.ServerType, DeviceTypeID from DeviceInventory DI,ServerTypeLicenses STL ,ServerTypes ST"+
                  //                "where (DI.CurrentNodeId IS NOT NULL AND DI.CurrentNodeId >0) AND DI.DeviceTypeID = STL.ServerTypeId and STL.ServerTypeId=ST.ID group by ST.ServerType,DI.DeviceTypeID , STL.UnitCost";
				//string sqlQuery = "select ST.ServerType as ServerType,STL.UnitsUsed as UnitsUsed,di.DeviceTypeID,COUNT(DeviceTypeID) * STL.UnitCost,count(DeviceTypeID) as noofsevers  from DeviceInventory di inner join ServerTypeLicenses  STL on DI.DeviceTypeID = STL.ServerTypeId inner join ServerTypes st on STL.ServerTypeId=ST.ID where DI.CurrentNodeId IS NOT NULL AND DI.CurrentNodeId >0 " +

				//     "group by ST.ServerType,DI.DeviceTypeID , STL.UnitCost,STL.UnitsUsed";
                //4/23/2015 NS modified for VSPLUS-1681 - added UnitCost
                //20/05/2016 sowmya added for VSPLUS-2989
			string sqlQuery =
    "			 select ST.ServerType, DeviceTypeID,COUNT(DeviceTypeID) as noofservers, STL.UnitCost UnitCost, " +
 "( "+
 "select COUNT(DeviceTypeID)  from DeviceInventory DI1,ServerTypeLicenses STL1 ,ServerTypes ST1 "+
"where (DI1.CurrentNodeId IS NOT NULL AND DI1.CurrentNodeId >0) AND DI1.DeviceTypeID = STL1.ServerTypeId and  "+
"STL1.ServerTypeId=ST1.ID and DI1.DeviceTypeID=DI.DeviceTypeID "+
" ) as noofunits, "+
"  ( "+
" select COUNT(DeviceTypeID) * STL.UnitCost  from DeviceInventory DI1,ServerTypeLicenses STL1 ,ServerTypes ST1 "+
"where (DI1.CurrentNodeId IS NOT NULL AND DI1.CurrentNodeId >0) AND DI1.DeviceTypeID = STL1.ServerTypeId and  "+
"STL1.ServerTypeId=ST1.ID and DI1.DeviceTypeID=DI.DeviceTypeID "+
" )  as totalcost "+
"  from DeviceInventory DI,ServerTypeLicenses STL ,ServerTypes ST " +
"where   DI.DeviceTypeID = STL.ServerTypeId and STL.ServerTypeId=ST.ID and ServerType not in('Domino Cluster Database','Domino Cluster') group by ST.ServerType,DI.DeviceTypeID , STL.UnitCost ";

				licenseusage = objAdaptor.FetchData(sqlQuery);

			}
			catch (Exception)
			{

				throw;
			}

			return licenseusage;
		}
		public DataTable Getlicenseunitsinfo()
		{
			DataTable licenseusage = new DataTable();
			try
			{
				string sqlQuery = "select sum(UnitsPurchased) as UnitsPurchased,sum(UnitsUsed) as UnitsUsed from ServerTypeLicenses";
				licenseusage = objAdaptor.FetchData(sqlQuery);

			}
			catch (Exception)
			{

				throw;
			}

			return licenseusage;
		}
		public DataTable Gettotalunitsused()

		{
			DataTable licenseusage = new DataTable();
			try
			{
//                string sqlQuery = "select COUNT(DeviceTypeID) * STL1.UnitCost as totalunitsused from DeviceInventory DI1,ServerTypeLicenses STL1 ,ServerTypes ST1 " +
//"where (DI1.CurrentNodeId IS NOT NULL AND DI1.CurrentNodeId >0) AND DI1.DeviceTypeID = STL1.ServerTypeId and "+
//"STL1.ServerTypeId=ST1.ID group by ST1.ServerType,DI1.DeviceTypeID , STL1.UnitCost";

                string sqlQuery = "select SUM(totalcost) as totalunitsused from " +
"(select ( "+
" select COUNT(DeviceTypeID) * STL.UnitCost  from DeviceInventory DI1,ServerTypeLicenses STL1 ,ServerTypes ST1 "+
"where (DI1.CurrentNodeId IS NOT NULL AND DI1.CurrentNodeId >0) AND DI1.DeviceTypeID = STL1.ServerTypeId and  "+
"STL1.ServerTypeId=ST1.ID and DI1.DeviceTypeID=DI.DeviceTypeID "+
" )  as totalcost "+
" from DeviceInventory DI,ServerTypeLicenses STL ,ServerTypes ST "+
"where   DI.DeviceTypeID = STL.ServerTypeId and STL.ServerTypeId=ST.ID group by ST.ServerType,DI.DeviceTypeID , STL.UnitCost " +
") a";
				licenseusage = objAdaptor.FetchData(sqlQuery);

			}
			catch (Exception)
			{

				throw;
			}

			return licenseusage;
		}
    }
}
