using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomerTrackingDO;
using CustomerTrackingDL;
using System.Data;

namespace CustomerServiceBL
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
            Users ReturnUsersObject = CustomerTrackingDL.UsersDAL.Ins.GetData(UsersObject);
           
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
      

        //5/17/2012 NS modified the pass by type to be able to get a handle on the UsersObject values
        public Users GetData(ref Users UsersObject)
        {
            return CustomerTrackingDL.UsersDAL.Ins.GetData(UsersObject);
        }

        //5/17/2012 NS added new function to verify account info
        public bool VerifyAccount(ref Users UsersObject)
        {
            return CustomerTrackingDL.UsersDAL.Ins.VerifyAccount(ref UsersObject);
        }


        //added on 29/5/2012
        public DataTable GetAllData()
        {
            return CustomerTrackingDL.UsersDAL.Ins.GetAllData();
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
            catch (Exception ex)
            { throw ex; }
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
            Object ReturnValue = ValidateUpdate(UsersObject);
            DataTable dt = CustomerTrackingDL.UsersDAL.Ins.GetDataByLoginNmae(UsersObject);
            if (dt.Rows.Count == 0)
            {
                if (ReturnValue.ToString() == "")
                {
                    return CustomerTrackingDL.UsersDAL.Ins.UpdateAccount(UsersObject);
                }
                else return ReturnValue;
            }
            else return "LoginName Is Not Available.";
        }

        /// <summary>
        /// Call to Update Data of Users based on Key
        /// </summary>
        /// <param name="UsersObject">Users object</param>
        /// <returns>Object</returns>
        public Object UpdateData(Users UsersObject)
        {
            Object ReturnValue = ValidateUpdate(UsersObject);
            DataTable dt = CustomerTrackingDL.UsersDAL.Ins.GetDataByLoginNmae(UsersObject);
            if (dt.Rows.Count == 0)
            {
                if (ReturnValue.ToString() == "")
                {
                    return CustomerTrackingDL.UsersDAL.Ins.UpdateData(UsersObject);
                }
                else return ReturnValue;
            }
            else return "LoginName Is Not Available.";
        }
        public Object CreateAccount(Users UsersObject)
        {
            Object ReturnValue = ValidateUpdate(UsersObject);
            DataTable dt = CustomerTrackingDL.UsersDAL.Ins.GetDataByLoginNmae(UsersObject);
            if (dt.Rows.Count == 0)
            {
                if (ReturnValue.ToString() == "")
                {
                    return CustomerTrackingDL.UsersDAL.Ins.CreateAccount(UsersObject);
                }
                else return ReturnValue;
            }
            else return "LoginName Is Not Available.";
        }
        /// <summary>
        /// Call DAL Delete Data
        /// </summary>
        /// <param name="UsersObject"></param>
        /// <returns></returns>
        public Object DeleteData(Users UsersObject)
        {
            return CustomerTrackingDL.UsersDAL.Ins.DeleteData(UsersObject);
        }
        public Object UpdateAccount(Users UsersAccObject)
        {
            return CustomerTrackingDL.UsersDAL.Ins.UpdateAccount(UsersAccObject);
            //Object ReturnValue = ValidateUpdate(UsersAccObject);
            //if (ReturnValue.ToString() == "")
            //{
            //    return VSWebDAL.SecurityDAL.UsersDAL.Ins.InsertAccount(UsersAccObject);
            //}
            //else return ReturnValue;

        }


        public bool UpdateAccntPassword(Users UsersAccObject)
        {
            return CustomerTrackingDL.UsersDAL.Ins.UpdateAccntPassword(UsersAccObject);
            

        }

        public DataTable GetUserbyID(Users UsersAccObject)
        {
            return CustomerTrackingDL.UsersDAL.Ins.Getdatabyid(UsersAccObject);
        }

        public DataTable GetIsAdmin(string userid)
        {
            return CustomerTrackingDL.UsersDAL.Ins.GetIsAdmin(userid);
        }

        public DataTable GetIsConsoleComm(string userid)
        {
            return CustomerTrackingDL.UsersDAL.Ins.GetIsConsoleComm(userid);
        }

        //1/2/2014 NS added
        public DataTable GetIsConfig(string userid)
        {
            return CustomerTrackingDL.UsersDAL.Ins.GetIsConfig(userid);
        }
    }
}
