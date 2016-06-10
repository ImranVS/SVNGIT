using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.SecurityBL
{
   public class UserProfileMasterBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private static UserProfileMasterBL _self = new UserProfileMasterBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static UserProfileMasterBL Ins
        {
            get { return _self; }
        }
        public DataTable GetAllData()
        {
			try
			{
				return VSWebDAL.SecurityDAL.UserProfileMasterDAL.Ins.GetAllData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        #region "Validations"

        /// <summary>
        /// Validation before submitting data for Server tab
        /// </summary>
        /// <param name="LocObject"></param>
        /// <returns></returns>
        public Object ValidateUpdate(UserProfileMaster LocObject)
        {
            Object ReturnValue = "";
            try
            {
                if (LocObject.Name == null || LocObject.Name == "")
                {
                    return "ER#Please enter the Profile name";
                }
               
            }
            catch (Exception ex )
            { throw ex ; }
            finally
            { }
            return "";
        }

        #endregion

        /// <summary>
        /// Call to Get Data from UserProfileMaster based on Primary key
        /// </summary>
        /// <param name="LocObject">UserProfileMaster object</param>
        /// <returns></returns>
        public UserProfileMaster GetData(UserProfileMaster LocObject)
        {
			try
			{
				return VSWebDAL.SecurityDAL.UserProfileMasterDAL.Ins.GetData(LocObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
       // get data based on location

        public UserProfileMaster GetDataForName(UserProfileMaster LocObject)
        {
			try
			{
				return VSWebDAL.SecurityDAL.UserProfileMasterDAL.Ins.GetDataForName(LocObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }


        /// <summary>
        /// Call to Insert Data into UserProfileMaster
        ///  </summary>
        /// <param name="LocObject">UserProfileMaster object</param>
        /// <returns></returns>
        public object InsertData(UserProfileMaster LocObject)
        {
			try
			{
				Object ReturnValue = ValidateUpdate(LocObject);
				DataTable value = VSWebDAL.SecurityDAL.UserProfileMasterDAL.Ins.GetDataForUserProfileByname(LocObject);
				if (value.Rows.Count == 0)
				{
					if (ReturnValue.ToString() == "")
					{
						return VSWebDAL.SecurityDAL.UserProfileMasterDAL.Ins.InsertData(LocObject);
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
        public Object UpdateData(UserProfileMaster LocObject)
        {
			try
			{
				Object ReturnValue = ValidateUpdate(LocObject);
				DataTable value = VSWebDAL.SecurityDAL.UserProfileMasterDAL.Ins.GetDataForUserProfileByname(LocObject);
				if (value.Rows.Count == 0)
				{
					if (ReturnValue.ToString() == "")
					{
						return VSWebDAL.SecurityDAL.UserProfileMasterDAL.Ins.UpdateData(LocObject);
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
        /// Call DAL Delete Data
        /// </summary>
        /// <param name="LocObject"></param>
        /// <returns></returns>
        public Object DeleteData(UserProfileMaster LocObject)
        {
			try
			{
				return VSWebDAL.SecurityDAL.UserProfileMasterDAL.Ins.DeleteData(LocObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public int UpdateProfileDetails(List<UserProfileDetailed> list)
        {
			try
			{
				return VSWebDAL.SecurityDAL.UserProfileMasterDAL.Ins.UpdateProfileDetails(list);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          

        }
        public DataTable GetUserProfileDetailedData(UserProfileMaster LOCbject)
        {
			try
			{
				return VSWebDAL.SecurityDAL.UserProfileMasterDAL.Ins.GetUserProfileDetailedData(LOCbject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        public int UpdateServerSettings(int serverId, List<ProfilesMaster> fieldValues)
        {
			try
			{
				return VSWebDAL.SecurityDAL.UserProfileMasterDAL.Ins.UpdateServerSettings(serverId, fieldValues);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           // return
            //int ID = VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(serverId);
            
        }
    }
}
