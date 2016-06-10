using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.SecurityDAL
{
   public class UserStartupURLDAL
    {

        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
		private static UserStartupURLDAL _self = new UserStartupURLDAL();

		public static UserStartupURLDAL Ins
        {
            get { return _self; }
        }
        /// <summary>
        /// Get all Data from UserSecurityQuestions
        /// </summary>

        public DataTable GetAllData(Users u)
        {

            DataTable UserStartupURLs = new DataTable();
            ServerTypes ReturnUQobject = new ServerTypes();
            try
            {
                string SqlQuery = "SELECT * FROM [UsersStartupURLs] ";
				string whereClause  ="";
				if (u.Isdashboard)
					whereClause += " WHERE isDashboard=1 ";
				if (u.IsConfigurator)
				{
					if (whereClause != "")
						whereClause += " OR IsConfigurator=1 ";
					else
						whereClause += " WHERE IsConfigurator=1 ";
				}

				if (u.Isconsolecomm)
				{
					if (whereClause != "")
						whereClause += " OR Isconsolecomm=1 ";
					else
						whereClause += " WHERE Isconsolecomm=1 ";
				}

				if (whereClause != "")
					SqlQuery = SqlQuery + whereClause;
				UserStartupURLs = objAdaptor.FetchData(SqlQuery);

            }
            catch
            {
            }
            finally
            {
            }
			return UserStartupURLs;
        }
    }
}
