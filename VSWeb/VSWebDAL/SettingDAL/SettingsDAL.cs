using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.SettingDAL
{
   public class SettingsDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static SettingsDAL _self = new SettingsDAL();

        public static SettingsDAL Ins
        {
            get { return _self; }
        }
        /// <summary>
        /// Get all Data from DominoCluster
        /// </summary>

        public Settings GetData(Settings StObject)
        {                   
            
            Settings ReturnStObject =new Settings();
            DataTable SettingsDataTable = new DataTable();
            try
            {
                string SqlQuery = "SELECT svalue FROM Settings WHERE sname='"+StObject.sname+"'";

                SettingsDataTable = objAdaptor.FetchData(SqlQuery);
                ReturnStObject.svalue = SettingsDataTable.Rows[0]["svalue"].ToString();

            }
            catch
            {
            }
            finally
            {
            }
            return ReturnStObject;
        }
	   public bool UpdateScanvalue(string sname, string svalue, string stype)
  {
   //26/11/2014 Sowjanya modified
   bool UpdateRet = false;
   int mode = 0;
   try
   {
    if (sname != null && sname != "")
    {
     int count = 0;
     string Query = "SELECT Count(*) FROM ScanSettings WHERE sname = '" + sname + "' and svalue='" + svalue + "'";
     count = objAdaptor.ExecuteScalar(Query);
     string SqlQuery = "";
     if (count == 0)
     {
      SqlQuery = "INSERT INTO ScanSettings(sname,svalue,stype) VALUES('" + sname + "','" + svalue + "','" + stype + "')";
	  mode = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
     }
     else
     {
      //SqlQuery = "UPDATE Settings SET svalue='" + svalue + "',stype ='" + stype + "' WHERE sname = '" + sname + "'";

     }

    
     if (mode == 1)
     {
      UpdateRet = true;
     }
    }
   }
   catch
   {
   }
   return UpdateRet;
  }
  public bool UpdateScanFirstvalue(string sname, string svalue, string stype)
  {
   //1/12/2014 Sowjanya modified
   bool UpdateRet = false;
   int mode = 0;
   try
   {
   string SqlQuery ="update scansettings set Priority=2";
    mode = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
    string SqlQuery1 = "update scansettings set Priority=1 where sname='" + sname + "' and svalue='" + svalue + "'";
                mode = objAdaptor.ExecuteNonQueryRetRows(SqlQuery1);
    return UpdateRet;
   }
   catch
   {
   }
   return UpdateRet;
  }
        public string Getvalue(string sname)
        {
            string svalue;
            DataTable settingsdatatable=new DataTable();
            try
            {
                string SqlQuery = "select svalue from Settings where sname='" + sname + "'";
                settingsdatatable = objAdaptor.FetchData(SqlQuery);
                if (settingsdatatable.Rows.Count > 0)
                {
                    svalue = settingsdatatable.Rows[0]["svalue"].ToString();

                }
                else
                {
                    svalue = "";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return svalue;
        }
		public bool Getcheckvalue(string sname)
		{
			bool checkvalue=false;
			DataTable settingsdatatable = new DataTable();
			try
			{
				string SqlQuery = "select svalue from Settings where sname='" + sname + "'";
				settingsdatatable = objAdaptor.FetchData(SqlQuery);
                if (settingsdatatable.Rows.Count > 0)
                {
                    if (settingsdatatable.Rows[0]["svalue"].ToString() != "")
                        checkvalue = Convert.ToBoolean(settingsdatatable.Rows[0]["svalue"].ToString());
                }
			}
			catch (Exception ex)
			{

				throw ex;
			}

			return checkvalue;
		}
        public bool UpdateSvalue(string sname, string svalue,string stype)
        {
            //2/14/2013 NS modified
            bool UpdateRet = false;
            int mode = 0;
            try
            {
                if (sname != null && sname != "")
                {
                    int count = 0;
                    string Query = "SELECT Count(*) FROM Settings WHERE sname = '" + sname + "'";
                    count = objAdaptor.ExecuteScalar(Query);
                    string SqlQuery = "";
                    if (count == 0)
                    {
                        SqlQuery = "INSERT INTO Settings VALUES('" + sname + "','" + svalue + "','" + stype + "')";
                    }
                    else
                    {
                        SqlQuery = "UPDATE Settings SET svalue='" + svalue + "',stype ='" + stype + "' WHERE sname = '" + sname + "'";
                    }

                    mode = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
                    if (mode == 1)
                    {
                        UpdateRet = true;
                    }
                }
            }
            catch
            {
            }
            return UpdateRet;            
        }

        public DataTable GetAllData()
        {

            Settings ReturnStObject = new Settings();
            DataTable SettingsDataTable = new DataTable();
            try
            {
                string SqlQuery = "SELECT * FROM Settings";

                SettingsDataTable = objAdaptor.FetchData(SqlQuery);
                //ReturnStObject.svalue = SettingsDataTable.Rows[0]["svalue"].ToString();
                //ReturnStObject.sname = SettingsDataTable.Rows[0]["sname"].ToString();
                //ReturnStObject.stype = SettingsDataTable.Rows[0]["stype"].ToString();

            }
            catch
            {
            }
            finally
            {
            }
            return SettingsDataTable;
        }

        public bool UpdateSettings(Settings StObject,string strsname)
        {
            bool UpdateRet = false;
            int Update = 0;
            try
            {
                if (strsname != "")
                {
                    string SqlQuery = "";
                    if (StObject.stype != "")
                    {
                        SqlQuery = "UPDATE Settings set svalue='" + StObject.svalue + "',stype='" + StObject.stype + "' where sname='" + strsname + "'";

                    }

                    else
                    {
                        SqlQuery = "UPDATE Settings set svalue='" + StObject.svalue + "' where sname='" + strsname + "'";


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
                    if (StObject.stype != "")
                    {
                        SqlQuery = "INSERT INTO Settings VALUES('" + StObject.sname + "','" + StObject.svalue + "','" + StObject.stype + "')";
                    }
                    else
                    {
                        SqlQuery = "INSERT INTO Settings VALUES('" + StObject.sname + "','" + StObject.svalue + "','System.String')";
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


        public object DeleteData(Settings StObject)
        {
            bool UpdateRet = false;
            int Update = 0;
            try
            {
                string SqlQuery = "delete Settings where sname='" + StObject.sname + "'";
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
    }
}
