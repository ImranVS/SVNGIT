using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL
{
    public class SharePointSettingsDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static SharePointSettingsDAL _self = new SharePointSettingsDAL();

        public static SharePointSettingsDAL Ins
        {
            get { return _self; }
        }
        /// <summary>
        /// Get all Data from DominoCluster
        /// </summary>

        public SharePointSettings GetData(SharePointSettings StObject)
        {

            SharePointSettings ReturnStObject = new SharePointSettings();
            DataTable SharePointSettingsDataTable = new DataTable();
            try
            {
                string SqlQuery = "SELECT Svalue FROM SharePointSettings WHERE Sname='" + StObject.Sname + "'";

                SharePointSettingsDataTable = objAdaptor.FetchData(SqlQuery);
                ReturnStObject.Svalue = SharePointSettingsDataTable.Rows[0]["Svalue"].ToString();

            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return ReturnStObject;
        }
        public string Getvalue(string Sname)
        {
            string Svalue;
            DataTable SharePointsettingsdatatable = new DataTable();
            try
            {
                string SqlQuery = "select Svalue from SharePointSettings where Sname='" + Sname + "'";
                SharePointsettingsdatatable = objAdaptor.FetchData(SqlQuery);
                if (SharePointsettingsdatatable.Rows.Count > 0)
                {
                    Svalue = SharePointsettingsdatatable.Rows[0]["Svalue"].ToString();

                }
                else
                {
                    Svalue = "";
                }
            }
			catch (Exception ex)
			{
				throw ex;
			}

            return Svalue;
        }

        public bool UpdateSvalue(string Sname, string Svalue,int ServerId)
        {
            //2/14/2013 NS modified
            bool UpdateRet = false;
            int mode = 0;
            try
            {
                if (Sname != null && Sname != "")
                {
                    int count = 0;
                    string Query = "SELECT Count(*) FROM SharePointSettings WHERE Sname = '" + Sname + "'";
                    count = objAdaptor.ExecuteScalar(Query);
                    string SqlQuery = "";
                    if (count == 0)
                    {
                        SqlQuery = "INSERT INTO SharePointSettings VALUES('" + ServerId + "','" + Sname + "','" + Svalue + "')";
                    }
                    else
                    {
                        SqlQuery = "UPDATE SharePointSettings SET svalue='" + Svalue + "',ServerId ='" + ServerId + "' WHERE Sname = '" + Sname + "'";
                    }

                    mode = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
                    if (mode == 1)
                    {
                        UpdateRet = true;
                    }
                }
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return UpdateRet;            
        }

        public DataTable GetAllData()
        {

            SharePointSettings ReturnStObject = new SharePointSettings();
            DataTable SharePointSettingsDataTable = new DataTable();
            try
            {
                string SqlQuery = "SELECT * FROM SharePointSettings";

                SharePointSettingsDataTable = objAdaptor.FetchData(SqlQuery);
                //ReturnStObject.svalue = SettingsDataTable.Rows[0]["svalue"].ToString();
                //ReturnStObject.sname = SettingsDataTable.Rows[0]["sname"].ToString();
                //ReturnStObject.stype = SettingsDataTable.Rows[0]["stype"].ToString();

            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return SharePointSettingsDataTable;
        }

        public bool UpdateSharePointSettings(SharePointSettings StObject, string strSname)
        {
            bool UpdateRet = false;
            int Update = 0;
            try
            {
                if (strSname != "")
                {
                    string SqlQuery = "";
                    if (StObject.ServerId.ToString() != "")
                    {
                        SqlQuery = "UPDATE SharePointSettings set svalue='" + StObject.Svalue + "',ServerId='" + StObject.ServerId + "' where Sname='" + strSname + "'";

                    }

                    else
                    {
                        SqlQuery = "UPDATE SharePointSettings set svalue='" + StObject.Svalue + "' where Sname='" + strSname + "'";


                    }

                    Update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
                }
            }
            catch
            {
                Update = 0;
            }
           if (Update == 0)
            {
                try
                {
                    string SqlQuery = "";
                    if (StObject.ServerId.ToString() != "")
                    {
                        SqlQuery = "INSERT INTO SharePointSettings VALUES('" + StObject.ServerId + "','" + StObject.Sname + "','" + StObject.Svalue + "')";
                    }
                    else
                    {
                        SqlQuery = "INSERT INTO SharePointSettings VALUES('" + StObject.Sname + "','" + StObject.Svalue + "','System.String')";
                    }
                    Update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
                }
                catch
                {
                    Update = 0;
                }
            }
            if (Update == 1)
            {
                UpdateRet = true;
            }
            return UpdateRet;
        }


        public object DeleteData(SharePointSettings StObject)
        {
            bool UpdateRet = false;
            int Update = 0;
            try
            {
                string SqlQuery = "delete SharePointSettings where Sname='" + StObject.Sname + "'";
                Update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
            }
            catch
            {
                Update = 0;
            }

            if (Update == 1)
            {
                UpdateRet = true;
            }
            return UpdateRet;
        }

		public DataTable GetFarmSettings(string farm)
		{

			SharePointSettings ReturnStObject = new SharePointSettings();
			DataTable SharePointSettingsDataTable = new DataTable();
			try
			{
				string SqlQuery = "SELECT * FROM SharePointFarmSettings WHERE FarmName='" + farm + "'";

				SharePointSettingsDataTable = objAdaptor.FetchData(SqlQuery);


			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return SharePointSettingsDataTable;
		}

		public bool UpdateFarmSettings(SharePointFarmSettings settings)
		{

			SharePointSettings ReturnStObject = new SharePointSettings();
			try
			{
				string SqlQuery = "IF EXISTS(Select * from SharePointFarmSettings where FarmName='" + settings.FarmName +"') BEGIN ";
				SqlQuery += " UPDATE SharePointFarmSettings set LogOnTest='" + settings.LogOnTest + "', SiteCreationTest='" + settings.SiteCreationTest + "', ";
				SqlQuery += " FileUploadTest='" + settings.FileUploadTest + "', TestApplicationURL='" + settings.TestAppURL + "' where FarmName='" + settings.FarmName + "' ";
				SqlQuery += " END ELSE BEGIN ";
				SqlQuery += " Insert into SharePointFarmSettings (FarmName, LogOnTest, SiteCreationTest, FileUploadTest, TestApplicationURL) VALUES ";
				SqlQuery += " ('" + settings.FarmName + "', '" + settings.LogOnTest + "', '" + settings.SiteCreationTest + "', '" + settings.FileUploadTest + "', '" + settings.TestAppURL + "') END";

				return objAdaptor.ExecuteNonQuery(SqlQuery);


			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return false;
		}


    }
}
