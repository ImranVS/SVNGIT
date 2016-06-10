using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace VSWebDAL.ConfiguratorDAL
{
   public class PwrShelDAL
    {
        private Adaptor objAdaptor = new Adaptor();
        private static PwrShelDAL _self = new PwrShelDAL();

        public static PwrShelDAL Ins
        {
            get { return _self; }
        }
        public DataTable GetPwrData()
        {
            DataTable Pwrsheltab = new DataTable();
            try
            {

                string SqlQuery = "SELECT Replace(ScriptName,'&#39;', '''') as ScriptName,Replace(ScriptDetails,'&#39;', '''') as ScriptDetails,Replace(Category,'&#39;', '''') as Category,Replace(Description,'&#39;', '''') as Description FROM PowershellScripts";
                Pwrsheltab = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return Pwrsheltab;

        }

        public DataTable fillcombo()
        {
            DataTable dt = new DataTable();
            try
            {
                string str = "Select distinct Category from PowershellScripts";
               dt=objAdaptor.FetchData(str);
               
                

            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;

        }



        public Object DeleteData(PowershellScripts pwrObject)
        {
            Object Update;
            try
            {

                string SqlQuery = "Delete PowershellScripts Where ScriptName='" + pwrObject.ScriptName+"'";

                Update = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Update = false;
            }
            finally
            {
            }
            return Update;
        }

        public PowershellScripts GetPwrData(PowershellScripts pwrObject)
        {
            DataTable pwrDataTable = new DataTable();
            PowershellScripts ReturnObject = new PowershellScripts();
            try
            {
                string SqlQuery = "Select * from PowershellScripts where [ScriptName]='" + pwrObject.ScriptName+"'";
                pwrDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
                ReturnObject.ScriptName = pwrDataTable.Rows[0]["ScriptName"].ToString();
                ReturnObject.ScriptDetails = pwrDataTable.Rows[0]["ScriptDetails"].ToString();
                ReturnObject.Category = pwrDataTable.Rows[0]["Category"].ToString();
                ReturnObject.Description = pwrDataTable.Rows[0]["Description"].ToString();
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return ReturnObject;
        }
        public bool InsertData(PowershellScripts pwrObject)
        {
            bool Insert = false;
            try
            {
                string SqlQuery = "INSERT INTO PowershellScripts (ScriptName ,ScriptDetails,Category,Description)" +                   
                    "VALUES('" + pwrObject.ScriptName + "','" + pwrObject.ScriptDetails +"','"+pwrObject.Category+"','"+pwrObject.Description+"')";
                  
                Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Insert = false;
            }
            finally
            {
            }
            return Insert;
        }

        public bool UpdateData(PowershellScripts pwrObject)
        {
            bool Update;
            try
            {
                string SqlQuery = "UPDATE PowershellScripts SET ScriptDetails='" + pwrObject.ScriptDetails + "' ,Category='" + pwrObject.Category + "',Description='"+pwrObject.Description+"' WHERE ScriptName = '" + pwrObject.ScriptName + "'";

                Update = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Update = false;
            }
            finally
            {
            }
            return Update;
        }
       public DataTable GetScriptName(string ScriptName)
       {
           DataTable Snametab = new DataTable();
           try
           {

               string SqlQry = "Select     * from PowershellScripts where ScriptName='" + ScriptName + "'";
               Snametab = objAdaptor.FetchData(SqlQry);
           }
		   catch (Exception ex)
		   {
			   throw ex;
		   }
           return Snametab;
       }

    }
}
