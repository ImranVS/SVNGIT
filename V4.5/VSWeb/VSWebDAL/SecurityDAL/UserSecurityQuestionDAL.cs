using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.SecurityDAL
{
   public class UserSecurityQuestionDAL
    {

        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static UserSecurityQuestionDAL _self = new UserSecurityQuestionDAL();

        public static UserSecurityQuestionDAL Ins
        {
            get { return _self; }
        }
        /// <summary>
        /// Get all Data from UserSecurityQuestions
        /// </summary>

        public DataTable Getimagepath(string loginname)
        {
            DataTable usersecuritydt = new DataTable();
            Users retobj = new Users();
            try
            {
                string sqlquery = "SELECT * from Users where LoginName ='"+loginname+"'";
                usersecuritydt = objAdaptor.FetchData(sqlquery);


            }
            catch
            {


            }
            finally
            {
            }
            return usersecuritydt;
        
        }
        public DataTable GetAllData()
        {

            DataTable UserQuestionsDataTable = new DataTable();
            ServerTypes ReturnUQobject = new ServerTypes();
            try
            {
                string SqlQuery = "SELECT * FROM [UserSecurityQuestions]";

                UserQuestionsDataTable = objAdaptor.FetchData(SqlQuery);

            }
            catch
            {
            }
            finally
            {
            }
            return UserQuestionsDataTable;
        }
    }
}
