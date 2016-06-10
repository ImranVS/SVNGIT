using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DO;
using DAL;
using System.Data;

namespace BL
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
				Users ReturnUsersObject =DAL.UsersDAL.Ins.GetData(UsersObject);

				if (ReturnUsersObject.Status == "Inactive")//10/04/2013 MD modified to avoid error if username does not exist
				{
					return "Your login is disabled. Contact the Administrator.";
				}
				else
				{

					if (ReturnUsersObject.LoginName == null)//10/04/2013 MD modified to avoid error if username does not exist
					{
						return "The username or password you entered is incorrect.";
					}
					else
					{ //9/12/2013 NS modified to make sure login name is case insensitive
						if (ReturnUsersObject.LoginName.ToUpper() == UsersObject.LoginName.ToUpper() && ReturnUsersObject.Password == UsersObject.Password)
						{

						}
						else
						{
							return "The username or password you entered is incorrect.";
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
				return DAL.UsersDAL.Ins.GetData(UsersObject);
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
				return DAL.UsersDAL.Ins.VerifyAccount(ref UsersObject);
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
				return DAL.UsersDAL.Ins.GetAllData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
		public DataTable Getusers()
		{
			try
			{
				return DAL.UsersDAL.Ins.Getusers();
			}
			catch (Exception ex)
			{

				throw ex;
			}


		}
		public DataTable GetCompanyNames()
		{
			try
			{
				return DAL.UsersDAL.Ins.GetCompanyNames();
			}
			catch (Exception ex)
			{

				throw ex;
			}


		}
		public DataTable GetCompanyID(string name)
		{
			try
			{
				return DAL.UsersDAL.Ins.GetCompanyID(name);
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
                if (UsersObject.UserType == null || UsersObject.UserType == "")
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
				DataTable dt = DAL.UsersDAL.Ins.GetDataByLoginNmae(UsersObject);
				if (dt.Rows.Count == 0)
				{
					if (ReturnValue.ToString() == "")
					{
						return DAL.UsersDAL.Ins.UpdateAccount(UsersObject);
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
				DataTable dt = DAL.UsersDAL.Ins.GetDataByLoginNmae(UsersObject);
				if (dt.Rows.Count == 0)
				{
					if (ReturnValue.ToString() == "")
					{
						return DAL.UsersDAL.Ins.UpdateData(UsersObject);
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
				DataTable dt = DAL.UsersDAL.Ins.GetDataByLoginNmae(UsersObject);
				if (dt.Rows.Count == 0)
				{
					if (ReturnValue.ToString() == "")
					{
						return DAL.UsersDAL.Ins.CreateAccount(UsersObject);
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
				return DAL.UsersDAL.Ins.DeleteData(UsersObject);
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
				return DAL.UsersDAL.Ins.UpdateAccount(UsersAccObject);
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
				return DAL.UsersDAL.Ins.UpdateAccntPassword(UsersAccObject);
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
				return DAL.UsersDAL.Ins.Getdatabyid(UsersAccObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetIsAdmin(string userid)
        {
			try
			{
				return DAL.UsersDAL.Ins.GetIsAdmin(userid);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
		public DataTable GetIsAdmin1(string userid)
		{
			try
			{
				return DAL.UsersDAL.Ins.GetIsAdmin1(userid);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

        public DataTable GetUserSysMessages(string userid)
        {
            return DAL.UsersDAL.Ins.GetUserSysMessages(userid);
        }

        public bool UpdateUserSysMessageDate(string userid, string msgid, string dateupd)
        {
            return DAL.UsersDAL.Ins.UpdateUserSysMessageDate(userid,msgid,dateupd);
        }

		public object InsertCompanyName(LicenseCompanys UsersObject)
		{
			try
			{
				Object ReturnValue = ValidateCompanyUpdate(UsersObject);
				DataTable dt = DAL.UsersDAL.Ins.GetDataByCompanyName(UsersObject);
				if (dt.Rows.Count == 0)
				{
					if (ReturnValue.ToString() == "")
					{
						return DAL.UsersDAL.Ins.InsertCompanyName(UsersObject);
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
		public Object UpdateCompanyData(LicenseCompanys UsersObject)
		{
			try
			{
				Object ReturnValue = ValidateCompanyUpdate(UsersObject);
				DataTable dt = DAL.UsersDAL.Ins.GetDataByCompanyName(UsersObject);
				if (dt.Rows.Count == 0)
				{
					if (ReturnValue.ToString() == "")
					{
						return DAL.UsersDAL.Ins.UpdateCompanyData(UsersObject);
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

		public Object ValidateCompanyUpdate(LicenseCompanys UsersObject)
		{
			Object ReturnValue = "";
			try
			{
				if (UsersObject.CompanyName == null || UsersObject.CompanyName == "")
				{
					return "ER#Please enter the CompanyName";
				}
							}
			catch (Exception ex)
			{ throw ex; }
			finally
			{ }
			return "";
		}

		public Object DeleteCompanyData(Users UsersObject)
		{
			try
			{
				return DAL.UsersDAL.Ins.DeleteCompanyData(UsersObject);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public DataTable GetCompanyData()
		{
			try
			{
				return DAL.UsersDAL.Ins.GetCompanyData();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public LicenseKey GetData(LicenseKey Licensekeyobject,int id)
		{
			try
			{
				return DAL.UsersDAL.Ins.GetData(Licensekeyobject, id);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
    }
}
