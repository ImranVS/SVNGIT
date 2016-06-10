using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDO;
using VSWebDAL;
using System.Data;

namespace VSWebBL.SecurityBL
{
    public class UsersBL
    {
        /// <summary>
        /// Declarations
        /// </summary>

        private static UsersBL _self = new UsersBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static UsersBL Ins
        {
            get { return _self; }
        }
        public object VerifyUser(ref Users UsersObject)
        {
			try
			{
				Users ReturnUsersObject = VSWebDAL.SecurityDAL.UsersDAL.Ins.GetData(UsersObject);

				if (ReturnUsersObject.Status == "Inactive")//10/04/2013 MD modified to avoid error if username does not exist
				{
					return "Your login is disabled. Contact the Administrator.";
				}
				else
				{

					if (ReturnUsersObject.LoginName == null)//10/04/2013 MD modified to avoid error if username does not exist
					{
						return "The Username or password you entered is incorrect.";
					}
					else
					{ //9/12/2013 NS modified to make sure login name is case insensitive
						if (ReturnUsersObject.LoginName.ToUpper() == UsersObject.LoginName.ToUpper() && ReturnUsersObject.Password == UsersObject.Password)
						{

						}
						else
						{
							return "The Username or password you entered is incorrect.";
						}
					}
					return ReturnUsersObject;
				}
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
      

        //5/17/2012 NS modified the pass by type to be able to get a handle on the UsersObject values
        public Users GetData(ref Users UsersObject)
        {
			try
			{
				return VSWebDAL.SecurityDAL.UsersDAL.Ins.GetData(UsersObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        //5/17/2012 NS added new function to verify account info
        public bool VerifyAccount(ref Users UsersObject)
        {
			try
			{
				return VSWebDAL.SecurityDAL.UsersDAL.Ins.VerifyAccount(ref UsersObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
		public bool GetIsFirstTimeLogin(int id )
		{
			try
			{
				return VSWebDAL.SecurityDAL.UsersDAL.Ins.GetIsFirstTimeLogin(id);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public Object UpdateIsFirstTimeLogin(int UserID)
		{
			try
			{
				return VSWebDAL.SecurityDAL.UsersDAL.Ins.UpdateIsFirstTimeLogin(UserID);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}


        //added on 29/5/2012
        public DataTable GetAllData()
        {
			try
			{
				return VSWebDAL.SecurityDAL.UsersDAL.Ins.GetAllData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        #region "Validations"

        /// <summary>
        /// Validation before submitting data for Users Grid
        /// </summary>
        /// <param name="UsersObject"></param>
        /// <returns></returns>
        public Object ValidateUpdate(Users UsersObject)
        {
            Object ReturnValue = "";
            try
            {
                if (UsersObject.LoginName == null || UsersObject.LoginName == "")
                {
                    return "ER#Please enter the LoginName";
                }
                //if (UsersObject.Password == null || UsersObject.Password == "")
                //{
                //    return "#ERPlease enter the Password";
                //}
                if (UsersObject.FullName == null || UsersObject.FullName== "")
                {
                    return "#ERPlease enter the FullName";
                }
                if (UsersObject.Status == null || UsersObject.Status == "") 
                {
                    return "#ERPlease select the Status";
                }
                if (UsersObject.SuperAdmin == null || UsersObject.SuperAdmin == "")
                {
                    return "#ERPlease select the SuperAdmin";
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
        /// Call to Insert Data into Users
        ///  </summary>
        /// <param name="UsersObject">Users object</param>
        /// <returns></returns>
        public object InsertData(Users UsersObject)
        {
			try
			{
				Object ReturnValue = ValidateUpdate(UsersObject);
				DataTable dt = VSWebDAL.SecurityDAL.UsersDAL.Ins.GetDataByLoginNmae(UsersObject);
				if (dt.Rows.Count == 0)
				{
					if (ReturnValue.ToString() == "")
					{
						return VSWebDAL.SecurityDAL.UsersDAL.Ins.UpdateAccount(UsersObject);
					}
					else return ReturnValue;
				}
				else return "LoginName Is Not Available.";
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        /// <summary>
        /// Call to Update Data of Users based on Key
        /// </summary>
        /// <param name="UsersObject">Users object</param>
        /// <returns>Object</returns>
        public Object UpdateData(Users UsersObject)
        {
			try
			{
				Object ReturnValue = ValidateUpdate(UsersObject);
				DataTable dt = VSWebDAL.SecurityDAL.UsersDAL.Ins.GetDataByLoginNmae(UsersObject);
				if (dt.Rows.Count == 0)
				{
					if (ReturnValue.ToString() == "")
					{
						return VSWebDAL.SecurityDAL.UsersDAL.Ins.UpdateData(UsersObject);
					}
					else return ReturnValue;
				}
				else return "LoginName Is Not Available.";
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public Object CreateAccount(Users UsersObject)
        {
			try
			{
				Object ReturnValue = ValidateUpdate(UsersObject);
				DataTable dt = VSWebDAL.SecurityDAL.UsersDAL.Ins.GetDataByLoginNmae(UsersObject);
				if (dt.Rows.Count == 0)
				{
					if (ReturnValue.ToString() == "")
					{
						return VSWebDAL.SecurityDAL.UsersDAL.Ins.CreateAccount(UsersObject);
					}
					else return ReturnValue;
				}
				else return "LoginName Is Not Available.";
			}
			catch (Exception ex)
			{
				throw ex;
			}
           
        }
        /// <summary>
        /// Call DAL Delete Data
        /// </summary>
        /// <param name="UsersObject"></param>
        /// <returns></returns>
        public Object DeleteData(Users UsersObject)
        {
			try
			{
				return VSWebDAL.SecurityDAL.UsersDAL.Ins.DeleteData(UsersObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        public Object UpdateAccount(Users UsersAccObject)
        {
			try
			{
				return VSWebDAL.SecurityDAL.UsersDAL.Ins.UpdateAccount(UsersAccObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
            //Object ReturnValue = ValidateUpdate(UsersAccObject);
            //if (ReturnValue.ToString() == "")
            //{
            //    return VSWebDAL.SecurityDAL.UsersDAL.Ins.InsertAccount(UsersAccObject);
            //}
            //else return ReturnValue;
        }
		public Object UpdateAccount1(Users UsersAccObject)
		{
			try
			{
				return VSWebDAL.SecurityDAL.UsersDAL.Ins.UpdateAccount1(UsersAccObject);
			}
			catch (Exception ex)
			{

				throw ex;
			}

			//Object ReturnValue = ValidateUpdate(UsersAccObject);
			//if (ReturnValue.ToString() == "")
			//{
			//    return VSWebDAL.SecurityDAL.UsersDAL.Ins.InsertAccount(UsersAccObject);
			//}
			//else return ReturnValue;
		}

        public bool UpdateAccntPassword(Users UsersAccObject)
        {
			try
			{
				return VSWebDAL.SecurityDAL.UsersDAL.Ins.UpdateAccntPassword(UsersAccObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
        }

        public DataTable GetUserbyID(Users UsersAccObject)
        {
			try
			{
				return VSWebDAL.SecurityDAL.UsersDAL.Ins.Getdatabyid(UsersAccObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

		public DataTable GetIsAdmin1(string id)
		{
			try
			{
				return VSWebDAL.SecurityDAL.UsersDAL.Ins.GetIsAdmin1(id);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
        public DataTable GetIsAdmin(string name)
        {
			try
			{
				return VSWebDAL.SecurityDAL.UsersDAL.Ins.GetIsAdmin(name);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
		public DataTable GetIsnotLoggedIn(string name)
		{
			try
			{
				return VSWebDAL.SecurityDAL.UsersDAL.Ins.GetIsnotLoggedIn(name);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

        public DataTable GetIsConsoleComm(string userid)
        {
			try
			{
				return VSWebDAL.SecurityDAL.UsersDAL.Ins.GetIsConsoleComm(userid);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        //1/2/2014 NS added
        public DataTable GetIsConfig(string userid)
        {
			try
			{
				return VSWebDAL.SecurityDAL.UsersDAL.Ins.GetIsConfig(userid);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        //2/27/2015 NS added
        public DataTable GetUserSysMessages(string userid)
        {
            return VSWebDAL.SecurityDAL.UsersDAL.Ins.GetUserSysMessages(userid);
        }

        public bool UpdateUserSysMessageDate(string userid, string msgid, string dateupd)
        {
            return VSWebDAL.SecurityDAL.UsersDAL.Ins.UpdateUserSysMessageDate(userid,msgid,dateupd);
        }

        //9/28/2015 NS added for VSPLUS-2170
        public DataTable GetUserAccessData()
        {
            return VSWebDAL.SecurityDAL.UsersDAL.Ins.GetUserAccessData();
        }
    }
}
