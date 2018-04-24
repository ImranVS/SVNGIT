using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using System.Text.RegularExpressions;

namespace VSWebDAL.SecurityDAL
{
   public class UserProfileMasterDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static UserProfileMasterDAL _self = new UserProfileMasterDAL();

        public static UserProfileMasterDAL Ins
        {
            get { return _self; }
        }
        /// <summary>
        /// Get all Data from UserProfileMasterDAL
        /// </summary>

        public DataTable GetAllData()
        {

            DataTable UserProfileMasterDataTable = new DataTable();
            UserProfileMaster ReturnLOCbject = new UserProfileMaster();
            try
            {
                string SqlQuery = "SELECT * FROM [UserProfileMaster]";

                UserProfileMasterDataTable = objAdaptor.FetchData(SqlQuery);

            }
            catch
            {
            }
            finally
            {
            }
            return UserProfileMasterDataTable;
        }
        /// <summary>
        /// Get Data from DominoServers based on Key
        /// </summary>
        public UserProfileMaster GetData(UserProfileMaster LOCbject)
        {
            DataTable UserProfileMasterDataTable = new DataTable();
            UserProfileMaster ReturnLOCbject = new UserProfileMaster();
            try
            {
                string SqlQuery = "Select * from UserProfileMaster where [ID]=" + LOCbject.ID.ToString();
                UserProfileMasterDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
                ReturnLOCbject.Name = UserProfileMasterDataTable.Rows[0]["Name"].ToString();
              
            }
            catch
            {
            }
            finally
            {
            }
            return ReturnLOCbject;
        }
        public UserProfileMaster GetDataForName(UserProfileMaster LOCbject)
        {
            DataTable UserProfileMasterDataTable = new DataTable();
            UserProfileMaster ReturnLOCbject = new UserProfileMaster();
            object response = "";
            try
            {
                string SqlQuery = "Select * from UserProfileMaster where Name='" + LOCbject.Name + "'";
                UserProfileMasterDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
                if (UserProfileMasterDataTable.Rows.Count > 0)
                {
                    if (UserProfileMasterDataTable.Rows[0]["ID"].ToString() != "")
                    {
                        ReturnLOCbject.ID = int.Parse(UserProfileMasterDataTable.Rows[0]["ID"].ToString());
                        
                    }


                }
                else
                {
                    response = 0;
                }
            }
            catch
            {
            }
            finally
            {
            }
            return ReturnLOCbject;
        }

        /// <summary>
        /// Insert data into UserProfileMaster table
        /// </summary>
        /// <param name="DSObject">UserProfileMaster object</param>
        /// <returns></returns>

        public bool InsertData(UserProfileMaster LOCbject)
        {

            bool Insert = false;
            try
            {
                string SqlQuery = "INSERT INTO UserProfileMaster(Name) VALUES('" + LOCbject.Name + "')";
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
        /// <summary>
        /// Update data into UserProfileMaster table
        /// </summary>
        /// <param name="LOCbject">UserProfileMaster object</param>
        /// <returns></returns>
        public Object UpdateData(UserProfileMaster LOCbject)
        {
            Object Update;
            try
            {
                string SqlQuery = "UPDATE UserProfileMaster SET Name='" + LOCbject.Name + "'WHERE ID = " + LOCbject.ID + "";
                    
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
        //delete Data from UserProfileMaster Table

        public Object DeleteData(UserProfileMaster LOCbject)
        {
            Object Update;
            try
            {

                string SqlQuery = "Delete UserProfileMaster Where ID=" + LOCbject.ID;

                Update = objAdaptor.ExecuteNonQuery(SqlQuery);

                SqlQuery = "Delete UserProfileDetailed Where UserProfileMasterID=" + LOCbject.ID;

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

        public DataTable GetDataForUserProfileByname(UserProfileMaster LOCbject)
        {
            DataTable UserProfileMasterDataTable = new DataTable();
            
            
            try
            {
                string SqlQuery = "Select * from UserProfileMaster where Name='" + LOCbject.Name + "'";
                UserProfileMasterDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
               
               
            }
            catch
            {
            }
            finally
            {
            }
            return UserProfileMasterDataTable;
        }
        public int UpdateProfileDetails(List<UserProfileDetailed> list)
        {

            int Update = 0;
            try
            {
                string SqlQuery = "delete UserProfileDetailed where UserProfileMasterID=" + list[0].UserProfileMasterID;
                Update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);

                for (int i = 0; i < list.Count; i++)
                {
                    SqlQuery = "INSERT INTO UserProfileDetailed(UserProfileMasterID,ProfilesMasterID,Value) values(" + list[i].UserProfileMasterID + "," + list[i].ProfilesMasterID + ",'" + list[i].Value + "')";
                    Update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
                }
            }
            catch
            {
                Update = 0;
            }

            return Update;
        }

        public DataTable GetUserProfileDetailedData(UserProfileMaster LOCbject)
        {
            DataTable UserProfileMasterDataTable = new DataTable();
           // UserProfileDetailed ReturnLOCbject = new UserProfileDetailed();
            try
            {
                string SqlQuery = "  select up.*,pm.ServerTypeId,st.ServerType from UserProfileDetailed up,ProfilesMaster pm,ServerTypes st " +
                " where pm.id=up.ProfilesMasterID and st.id=pm.servertypeid and UserProfileMasterID=" + LOCbject.ID.ToString();
                
                UserProfileMasterDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
               // ReturnLOCbject. = UserProfileMasterDataTable.Rows[0]["Name"].ToString();

            }
            catch
            {
            }
            finally
            {
            }
            return UserProfileMasterDataTable;
        }
        public int UpdateServerSettings(int serverID, List<ProfilesMaster> fieldValues)
        {
            string SqlQuery ="";
            string parameters ="";
            string tableName ="";
            int Update = 0;
            int count = 0; 
			string secure1="";
			string secure2="";
  
            try
            {
				foreach(string tbl in fieldValues.Select(s => s.RelatedTable).Distinct())
				{
					count = 0;
					parameters = "";
					secure1 = "";
					 secure2="";
					foreach (ProfilesMaster fieldValue in fieldValues.Where(s => s.RelatedTable == tbl).ToList())
					{
						//Get table name
						if (count == 0)
						{
							if (fieldValue.UnitOfMeasurement.ToLower() == "string")
							{
								parameters += "[" + fieldValue.RelatedField + "]= '" + fieldValue.DefaultValue + "'";
							}
							else
							{
								parameters += "[" + fieldValue.RelatedField + "]= " + fieldValue.DefaultValue;
							}
							count++;
							//tableName = tbl;
						}
						else
						{
							if (fieldValue.UnitOfMeasurement.ToLower() == "string")
							{
								parameters += ",[" + fieldValue.RelatedField + "]= '" + fieldValue.DefaultValue + "'";
							}
							else
							{
								parameters += ",[" + fieldValue.RelatedField + "]= " + fieldValue.DefaultValue;
							}
						}
						if (fieldValue.RelatedField.ToLower() == "ipaddress")
						{
							DataTable dt = new DataTable();
							SqlQuery = "select IPAddress from servers WHERE Id =" + serverID;
							dt = objAdaptor.FetchData(SqlQuery);
							string ipaddress = dt.Rows[0]["IPAddress"].ToString();
							 Regex Splitter = new Regex("!|@|//");
                             String[] Parts = Splitter.Split(ipaddress);

							//string[] QueryArray = ipaddress.Split('//');
							 secure1 = Parts[0].ToString();
							 secure2 = Parts[1].ToString();
						}
					}
					if (tbl != "URLs" && tbl != "Network Devices" && tbl != "Servers")
					{
						SqlQuery = "UPDATE " + tbl + " SET " + parameters + " WHERE ServerId =" + serverID;
					}
					else if ((tbl == "Servers") &&( parameters == "[IPAddress]= 'http://'"||parameters == "[IPAddress]= 'https://'"))
					{
						SqlQuery = "UPDATE " + tbl + " SET " + parameters + " + '" + secure2 + "'  WHERE Id =" + serverID;
					}
					else
					{
						SqlQuery = "UPDATE " + tbl + " SET " + parameters + " WHERE Id =" + serverID;
					}
					Update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);   
				}
            }
            catch
            {
                Update = 0;
            }
            return Update;
        }
    }
}
