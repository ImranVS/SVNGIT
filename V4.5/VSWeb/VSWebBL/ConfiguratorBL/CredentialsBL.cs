using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.ConfiguratorBL
{
   public class CredentialsBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private static CredentialsBL _self = new CredentialsBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static CredentialsBL Ins
        {
            get { return _self; }
        }
        public DataTable GetAllData()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.CredentialsDAL.Ins.GetAllData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public Object ValidateUpdate(Credentials LocObject)
        {
            Object ReturnValue = "";
            try
            {
                if (LocObject.AliasName == null || LocObject.AliasName == "")
                {
                    return "ER#Please enter the Alias name";
                }

            }
            catch (Exception ex)
            { throw ex; }
            finally
            { }
            return "";
        }

        /// <summary>
        /// Call to Insert Data into Alias
        ///  </summary>
        /// <param name="LocObject">Alias object</param>
        /// <returns></returns>
        public object InsertData(Credentials LocObject)
        {
			try
			{
				Object ReturnValue = ValidateUpdate(LocObject);
				DataTable value = VSWebDAL.ConfiguratorDAL.CredentialsDAL.Ins.GetDataForCredentialsByname(LocObject);
				if (value.Rows.Count == 0)
				{
					if (ReturnValue.ToString() == "")
					{
						return VSWebDAL.ConfiguratorDAL.CredentialsDAL.Ins.InsertData(LocObject);
					}
					else return ReturnValue;
				}
				else return "";
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
               
        }
        /// <summary>
        /// Call to Update Data of DominoServers based on Key
        /// </summary>
        /// <param name="LocObject">DominoServers object</param>
        /// <returns>Object</returns>
        public bool UpdateData(Credentials LocObject)
		{
			bool update = false;
			try
			{
			
				Object ReturnValue = ValidateUpdate(LocObject);
				DataTable value = VSWebDAL.ConfiguratorDAL.CredentialsDAL.Ins.GetDataForCredentialsById(LocObject);
				if (value.Rows.Count > 0)
				{
					if (ReturnValue.ToString() == "")
					{
						update = VSWebDAL.ConfiguratorDAL.CredentialsDAL.Ins.UpdateData(LocObject);

					}
					else
						return update;
				}
				else return update;
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			return update;
        }
        /// <summary>
        /// Call DAL Delete Data
        /// </summary>
        /// <param name="LocObject"></param>
        /// <returns></returns>
        public string DeleteData(Credentials LocObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.CredentialsDAL.Ins.DeleteData(LocObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }

        public DataTable GetpwrshlCredentials(string cred, int serverid)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.CredentialsDAL.Ins.GetpwrshlCredentials(cred, serverid);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }
		public DataTable getCredentialsById(Credentials LocObject)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.CredentialsDAL.Ins.GetDataForCredentialsById(LocObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
        public DataTable GetDataForCredentialsByname(Credentials LocObject)
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.CredentialsDAL.Ins.GetDataForCredentialsByname(LocObject);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

    }
}
