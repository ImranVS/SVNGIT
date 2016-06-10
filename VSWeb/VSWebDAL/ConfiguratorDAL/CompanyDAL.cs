using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDO;
using System.Data;
using System.Data.SqlClient;

namespace VSWebDAL.ConfiguratorDAL
{
   public class CompanyDAL
    {
        private Adaptor objAdaptor = new Adaptor();
        private static CompanyDAL _self = new CompanyDAL();

        public static CompanyDAL Ins
        {
            get { return _self; }
        }

        public bool UpdateLogo(Company cobj)
        {
            bool update = false;
            DataTable dt;
            string sqlQuery;
            try
            {
                //5/23/2013 NS modified
                sqlQuery = "SELECT * FROM Company";
                dt = objAdaptor.FetchData(sqlQuery);
                if (dt.Rows.Count > 0)
                {
					sqlQuery = "UPDATE Company SET LogoPath = @LogoPath,CompanyName = @CompanyName";
					SqlCommand cmd = new SqlCommand(sqlQuery);
					cmd.Parameters.AddWithValue("@LogoPath", (object)cobj.LogoPath ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@CompanyName", (object)cobj.CompanyName ?? DBNull.Value);
					update = objAdaptor.ExecuteNonQuerywithcmd(cmd);
                }
                else
                {
					sqlQuery = "INSERT INTO Company VALUES (@CompanyName,@LogoPath)";
					SqlCommand cmd = new SqlCommand(sqlQuery);
					cmd.Parameters.AddWithValue("@LogoPath", (object)cobj.LogoPath ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@CompanyName", (object)cobj.CompanyName ?? DBNull.Value);
					update = objAdaptor.ExecuteNonQuerywithcmd(cmd);
                }
                //update= objAdaptor.ExecuteNonQuery(sqlQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return update;
        }


        public Company GetLogo()
        {
            Company cmpobj=new Company();
           // string logopath;
            DataTable logo = new DataTable();
            try
            {
                string sqlQuery = "Select * from Company";
                logo = objAdaptor.FetchData(sqlQuery);
                //5/23/2013 NS modified
                if (logo.Rows.Count > 0)
                {
                    cmpobj.LogoPath = logo.Rows[0]["LogoPath"].ToString();
                    cmpobj.CompanyName = logo.Rows[0]["CompanyName"].ToString();
                }
                else
                {
                    cmpobj.LogoPath = "/images/RPR Wyatt Logo.gif";
                    cmpobj.CompanyName = "RPR Wyatt";
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            return cmpobj;//logopath;
        
        }

    }
}
